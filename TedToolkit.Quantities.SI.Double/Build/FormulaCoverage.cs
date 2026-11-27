// -----------------------------------------------------------------------
// <copyright file="FormulaCoverage.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using TedToolkit.FileSystem;
using TedToolkit.Quantities.Data;

namespace TedToolkit.Quantities.SI.Double.Build;

/// <summary>
/// Builds the formula inventory from comments beside the quantity declarations.
/// </summary>
internal static class FormulaCoverage
{
    private static readonly (string Id, string Heading)[] _domains =
    [
        ("geometry", "Geometry and algebra"),
        ("mechanics", "Mechanics, rotation, and waves"),
        ("fluids", "Fluids and mass flow"),
        ("electromagnetism", "Electricity and magnetism"),
        ("thermal", "Heat and temperature"),
        ("optics", "Photometry and radiometry"),
        ("chemistry", "Chemistry and radiation dose"),
    ];

    /// <summary>
    /// Validates formula comments and produces the README inventory.
    /// </summary>
    /// <param name="sourcePaths">The package's C# source paths.</param>
    /// <param name="data">The catalog used for the generated quantity types.</param>
    /// <returns>The Markdown for the formula coverage section.</returns>
    internal static string Generate(IEnumerable<string> sourcePaths, in DataCollection data)
    {
        var parseOptions = new CSharpParseOptions(LanguageVersion.Preview);
        var declarations = sourcePaths.Select(path => new FilePath(path))
            .SelectMany(file => CSharpSyntaxTree.ParseText(file.ReadAllText(),
                    parseOptions, file.FullName)
                .GetRoot().DescendantNodes().OfType<StructDeclarationSyntax>())
            .ToArray();

        var formulas = new Dictionary<string, Formula>(StringComparer.Ordinal);
        AddOperators(formulas, declarations.SelectMany(ReadOperators), data);
        AddMethods(formulas, declarations, data.Quantities);
        Validate(formulas.Values);
        return Render(formulas.Values);
    }

    private static IEnumerable<OperatorDeclaration> ReadOperators(StructDeclarationSyntax declaration)
    {
        foreach (var list in declaration.AttributeLists)
        {
            foreach (var attribute in list.Attributes)
            {
                var attributeName = attribute.Name switch
                {
                    QualifiedNameSyntax qualified => qualified.Right,
                    AliasQualifiedNameSyntax aliased => aliased.Name,
                    _ => attribute.Name,
                };
                if (attributeName is not GenericNameSyntax
                    { Identifier.ValueText: "QuantityOperator" or "QuantityOperatorAttribute", } name)
                {
                    continue;
                }

                var types = name.TypeArgumentList.Arguments;
                if (types.Count != 3 || attribute.ArgumentList?.Arguments is not { Count: 1, } arguments
                    || arguments[0].Expression is not MemberAccessExpressionSyntax operation)
                {
                    throw new InvalidDataException($"Invalid quantity operator declaration on {declaration.Identifier.ValueText}.");
                }

                var token = operation.Name.Identifier.ValueText switch
                {
                    "ADD" => "+",
                    "SUBTRACT" => "-",
                    "MULTIPLY" => "*",
                    "DIVIDE" => "/",
                    _ => throw new InvalidDataException($"Unknown quantity operator {operation}."),
                };
                yield return new(TypeName(types[0]), TypeName(types[1]), TypeName(types[2]), token, FormulaComment.Read(list));
            }
        }

        foreach (var operation in declaration.Members.OfType<OperatorDeclarationSyntax>())
        {
            var token = operation.OperatorToken.Text;
            var parameters = operation.ParameterList.Parameters;
            if (parameters.Count == 2 && token is "+" or "-" or "*" or "/")
            {
                yield return new(TypeName(parameters[0].Type!), TypeName(parameters[1].Type!),
                    TypeName(operation.ReturnType), token, FormulaComment.Read(operation));
            }
        }
    }

    private static void AddOperators(Dictionary<string, Formula> formulas,
        IEnumerable<OperatorDeclaration> operators, in DataCollection data)
    {
        foreach (var group in operators.GroupBy(GetEquationShape))
        {
            var ids = group.Select(item => item.Comment.Id)
                .OfType<string>().Distinct(StringComparer.Ordinal).ToArray();
            if (ids.Length != 1)
            {
                throw new InvalidOperationException(
                    $"Operator family '{group.Key}' requires one formula identifier. Found {ids.Length.ToString(CultureInfo.InvariantCulture)}.");
            }

            var formula = GetFormula(formulas, ids[0]);
            foreach (var item in group)
            {
                ValidateDimensions(item, data);
                formula.AddDocumentation(item.Comment);
                formula.Results.Add(item.Result);
            }
        }
    }

    private static void AddMethods(Dictionary<string, Formula> formulas,
        IEnumerable<StructDeclarationSyntax> declarations, IReadOnlyDictionary<string, Quantity> quantities)
    {
        foreach (var declaration in declarations.Where(item => quantities.ContainsKey(item.Identifier.ValueText)))
        {
            foreach (var method in declaration.Members.OfType<MethodDeclarationSyntax>()
                         .Where(method => method.Modifiers.Any(SyntaxKind.PublicKeyword)))
            {
                var comment = FormulaComment.Read(method);
                var name = $"{declaration.Identifier.ValueText}.{method.Identifier.ValueText}";
                var id = comment.Id
                    ?? throw new InvalidDataException($"Named formula '{name}' requires a Formula comment.");

                var formula = GetFormula(formulas, id);
                formula.AddDocumentation(comment);
                formula.Results.Add(TypeName(method.ReturnType));
                formula.Methods.Add(name);
            }
        }
    }

    private static string TypeName(TypeSyntax type)
    {
        return type switch
        {
            PredefinedTypeSyntax predefined => predefined.Keyword.ValueText,
            QualifiedNameSyntax qualified => qualified.Right.Identifier.ValueText,
            AliasQualifiedNameSyntax aliased => aliased.Name.Identifier.ValueText,
            SimpleNameSyntax simple => simple.Identifier.ValueText,
            _ => throw new InvalidDataException($"Unsupported quantity type {type}."),
        };
    }

    private static void ValidateDimensions(OperatorDeclaration item, in DataCollection data)
    {
        var left = GetDimension(item.Left, data);
        var right = GetDimension(item.Right, data);
        var result = GetDimension(item.Result, data);
        var valid = item.Token switch
        {
            "+" or "-" => left == right && left == result,
            "*" => CombinesDimensions(left, right, result),
            "/" => CombinesDimensions(right, result, left),
            _ => false,
        };
        if (valid)
        {
            return;
        }

        throw new InvalidOperationException(
            $"The result of {item.Left} {item.Token} {item.Right} has incompatible dimensions: {item.Result}.");

        static Dimension GetDimension(string type, in DataCollection data)
        {
            return type is "double" or "Double"
                ? default
                : data.Dimensions[data.Quantities[type].Dimension] with { Dimensionless = 0, };
        }

        static bool CombinesDimensions(in Dimension left, in Dimension right, in Dimension result)
        {
            return left.AmountOfSubstance + right.AmountOfSubstance == result.AmountOfSubstance
                   && left.ElectricCurrent + right.ElectricCurrent == result.ElectricCurrent
                   && left.Length + right.Length == result.Length
                   && left.LuminousIntensity + right.LuminousIntensity == result.LuminousIntensity
                   && left.Mass + right.Mass == result.Mass
                   && left.ThermodynamicTemperature + right.ThermodynamicTemperature == result.ThermodynamicTemperature
                   && left.Time + right.Time == result.Time;
        }
    }

    private static string GetEquationShape(OperatorDeclaration item)
    {
        var inverse = item.Token is "/" or "-";
        var token = item.Token is "+" or "-" ? "+" : "*";
        var left = Name(inverse ? item.Right : item.Left);
        var right = Name(inverse ? item.Result : item.Right);
        var result = Name(inverse ? item.Left : item.Result);
        return string.CompareOrdinal(left, right) <= 0
            ? $"{left} {token} {right} = {result}"
            : $"{right} {token} {left} = {result}";

        // Reciprocal operators use double for the unitless numerator.
        static string Name(string type)
        {
            return type is "double" or "Double" ? "DimensionlessRatio" : type;
        }
    }

    private static Formula GetFormula(Dictionary<string, Formula> formulas, string id)
    {
        if (!formulas.TryGetValue(id, out var formula))
        {
            formula = new(id);
            formulas.Add(id, formula);
        }

        return formula;
    }

    private static void Validate(IEnumerable<Formula> formulas)
    {
        foreach (var formula in formulas)
        {
            var parts = formula.Id.Split('/');
            if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[1])
                || !_domains.Any(domain => domain.Id == parts[0]))
            {
                throw new InvalidOperationException($"Formula identifier '{formula.Id}' must use a known domain and a name.");
            }

            if (formula.Equation is null)
            {
                throw new InvalidOperationException($"Formula '{formula.Id}' is missing its documentation declaration.");
            }
        }
    }

    private static string Render(ICollection<Formula> formulas)
    {
        var markdown = new StringBuilder("## Formula coverage\n\n");
        markdown.Append("The inventory has **")
            .Append(formulas.Count.ToString(CultureInfo.InvariantCulture))
            .Append(" rows**. Swapped products, solved forms, and typed variants share a row. ")
            .Append("Result types include the supported operators and named calculations.\n\n");
        foreach (var (domain, heading) in _domains)
        {
            var rows = formulas.Where(formula => formula.Id.StartsWith(domain + "/", StringComparison.Ordinal))
                .OrderBy(formula => formula.Id, StringComparer.Ordinal);
            markdown.Append("### ").Append(heading).Append("\n\n")
                .Append("| Formula | Result types | Applicability and named entry points | Source |\n")
                .Append("| --- | --- | --- | --- |\n");
            foreach (var formula in rows)
            {
                WriteRow(markdown, formula);
            }

            markdown.Append('\n');
        }

        markdown.Append("## Formula references\n\n");
        foreach (var source in formulas.SelectMany(formula => formula.Sources)
                     .Distinct(StringComparer.Ordinal).OrderBy(source => source, StringComparer.Ordinal))
        {
            markdown.Append("- ").Append(source).Append('\n');
        }

        return markdown.ToString().TrimEnd('\n');
    }

    private static void WriteRow(StringBuilder markdown, Formula formula)
    {
        markdown.Append("| ").Append(EscapeCell(formula.Equation!)).Append(" | ")
            .Append(CodeList(formula.Results)).Append(" | ").Append(EscapeCell(formula.Applicability!));
        if (formula.Methods.Count > 0)
        {
            markdown.Append(" Supported methods include ").Append(CodeList(formula.Methods)).Append('.');
        }

        markdown.Append(" | ").Append(EscapeCell(string.Join(" · ", formula.Sources))).Append(" |\n");
    }

    private static string CodeList(IEnumerable<string> values)
    {
        return string.Join(", ", values.Select(name => "`" + name + "`"));
    }

    private static string EscapeCell(string value)
    {
        return value.Replace("|", "\\|", StringComparison.Ordinal).Replace('\r', ' ').Replace('\n', ' ');
    }

    private sealed record OperatorDeclaration(
        string Left, string Right, string Result, string Token, FormulaComment Comment);

    private sealed record FormulaComment(string? Id, string? Equation, string? Applicability, string[] Sources)
    {
        internal static FormulaComment Read(SyntaxNode declaration)
        {
            var lines = declaration.GetLeadingTrivia()
                .Where(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
                .Select(trivia => trivia.ToString()[2..].Trim())
                .ToArray();
            var id = Values("Formula ").SingleOrDefault();
            var equation = Values("Equation ").SingleOrDefault();
            var applicability = Values("Applies ").SingleOrDefault();
            var sources = Values("Source ").ToArray();
            if (id is null && (equation is not null || applicability is not null || sources.Length != 0))
            {
                throw new InvalidDataException("Formula metadata requires a Formula identifier comment.");
            }

            return new(id, equation, applicability, sources);

            IEnumerable<string> Values(string prefix)
            {
                return lines.Where(line => line.StartsWith(prefix, StringComparison.Ordinal))
                    .Select(line => line[prefix.Length..].Trim());
            }
        }
    }

    private sealed class Formula(string id)
    {
        internal string Id { get; } = id;

        internal string? Equation { get; private set; }

        internal string? Applicability { get; private set; }

        internal string[] Sources { get; private set; } = [];

        internal SortedSet<string> Results { get; } = new(StringComparer.Ordinal);

        internal SortedSet<string> Methods { get; } = new(StringComparer.Ordinal);

        internal void AddDocumentation(FormulaComment comment)
        {
            var (id, equation, conditions, sources) = comment;
            if (equation is null && conditions is null && sources.Length == 0)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(equation) || string.IsNullOrWhiteSpace(conditions)
                || sources.Length == 0 || sources.Any(source => !IsSourceLink(source)))
            {
                throw new InvalidOperationException(
                    $"Formula '{Id}' requires an equation, applicability, and at least one Markdown source link.");
            }

            if (id != Id || Equation is not null)
            {
                throw new InvalidOperationException($"Formula '{Id}' has more than one documentation declaration.");
            }

            Equation = equation;
            Applicability = conditions;
            Sources = sources;
        }

        private static bool IsSourceLink(string source)
        {
            var start = source.IndexOf("](", StringComparison.Ordinal);
            return start > 1 && source[0] == '[' && source[^1] == ')'
                   && Uri.TryCreate(source.Substring(start + 2, source.Length - start - 3),
                       UriKind.Absolute, out var url)
                   && url.Scheme is "https" or "http";
        }
    }
}