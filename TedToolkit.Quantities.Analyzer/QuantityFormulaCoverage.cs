// -----------------------------------------------------------------------
// <copyright file="QuantityFormulaCoverage.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using System.Text;

using Microsoft.CodeAnalysis;

using TedToolkit.Quantities.Data;
using TedToolkit.RoslynHelper;

namespace TedToolkit.Quantities.Analyzer;

/// <summary>
/// Builds the formula inventory from the same symbols used to generate quantity operators.
/// </summary>
internal static class QuantityFormulaCoverage
{
    private const string FormulaAttribute = "TedToolkit.Quantities.QuantityFormulaAttribute";

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
    /// Validates formula annotations and emits the Markdown as generated source comments.
    /// </summary>
    /// <param name="context">The current source generation context.</param>
    /// <param name="quantities">The handwritten quantity declarations.</param>
    /// <param name="data">The catalog used for the generated quantity types.</param>
    internal static void Generate(in SourceProductionContext context, IReadOnlyCollection<INamedTypeSymbol> quantities,
        in DataCollection data)
    {
        var operators = quantities.SelectMany(QuantityOperatorDeclaration.Read).ToArray();
        var methods = quantities.SelectMany(type => type.GetMembers().OfType<IMethodSymbol>()).ToArray();
        if (!operators.Any(item => ReadString(item.Attribute, "FormulaId") is not null)
            && !methods.Any(method => method.GetAttributes().Any(attr => attr.AttributeClass?.FullName == FormulaAttribute)))
        {
            return;
        }

        var formulas = new Dictionary<string, Formula>(StringComparer.Ordinal);
        AddOperators(formulas, operators, data);
        AddMethods(formulas, methods);
        Validate(formulas.Values);

        // A comment-only source artifact gives MSBuild the Markdown without adding metadata to the library.
        var markdown = Render(formulas.Values);
        context.AddSource("FormulaCoverage.g.cs", "// " + markdown.Replace("\n", "\n// "));
    }

    private static void AddOperators(Dictionary<string, Formula> formulas,
        IEnumerable<QuantityOperatorDeclaration> operators, in DataCollection data)
    {
        foreach (var group in operators.GroupBy(GetEquationShape))
        {
            var ids = group.Select(item => ReadString(item.Attribute, "FormulaId"))
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
                formula.AddDocumentation(item.Attribute);
                formula.Results.Add(item.Result.Name);
            }
        }
    }

    private static void AddMethods(Dictionary<string, Formula> formulas, IEnumerable<IMethodSymbol> methods)
    {
        foreach (var method in methods.Where(method =>
                     method.DeclaredAccessibility == Microsoft.CodeAnalysis.Accessibility.Public
                     && method.MethodKind == MethodKind.Ordinary && !method.IsImplicitlyDeclared))
        {
            var attribute = method.GetAttributes()
                .FirstOrDefault(attr => attr.AttributeClass?.FullName == FormulaAttribute);
            if (attribute?.ConstructorArguments.FirstOrDefault().Value is not string id)
            {
                throw new InvalidOperationException($"Named formula '{method}' requires a QuantityFormula attribute.");
            }

            var formula = GetFormula(formulas, id);
            formula.AddDocumentation(attribute);
            formula.Results.Add(method.ReturnType.Name);
            formula.Methods.Add($"{method.ContainingType.Name}.{method.Name}");
        }
    }

    private static void ValidateDimensions(in QuantityOperatorDeclaration item, in DataCollection data)
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
            $"The result of {item.Left.Name} {item.Token} {item.Right.Name} has incompatible dimensions: {item.Result.Name}.");

        static Dimension GetDimension(ITypeSymbol type, in DataCollection data)
        {
            return type.SpecialType == SpecialType.System_Double
                ? default
                : data.Dimensions[data.Quantities[type.Name].Dimension] with { Dimensionless = 0, };
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

    private static string GetEquationShape(QuantityOperatorDeclaration item)
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
        static string Name(ITypeSymbol type)
        {
            return type.SpecialType == SpecialType.System_Double ? "DimensionlessRatio" : type.Name;
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
        return value.Replace("|", "\\|").Replace('\r', ' ').Replace('\n', ' ');
    }

    private static string? ReadString(AttributeData attribute, string name)
    {
        return attribute.NamedArguments.FirstOrDefault(argument => argument.Key == name).Value.Value as string;
    }

    private sealed class Formula(string id)
    {
        internal string Id { get; } = id;

        internal string? Equation { get; private set; }

        internal string? Applicability { get; private set; }

        internal string[] Sources { get; private set; } = [];

        internal SortedSet<string> Results { get; } = new(StringComparer.Ordinal);

        internal SortedSet<string> Methods { get; } = new(StringComparer.Ordinal);

        internal void AddDocumentation(AttributeData attribute)
        {
            var equation = ReadString(attribute, "Equation");
            var conditions = ReadString(attribute, "Applicability");
            var sourceArgument = attribute.NamedArguments.FirstOrDefault(argument => argument.Key == "Sources").Value;
            if (equation is null && conditions is null && sourceArgument.Kind != TypedConstantKind.Array)
            {
                return;
            }

            var sources = sourceArgument.Kind == TypedConstantKind.Array && !sourceArgument.IsNull
                ? sourceArgument.Values.Select(value => value.Value as string ?? "").ToArray()
                : [];
            if (string.IsNullOrWhiteSpace(equation) || string.IsNullOrWhiteSpace(conditions)
                || sources.Length == 0 || sources.Any(source => !IsSourceLink(source)))
            {
                throw new InvalidOperationException(
                    $"Formula '{Id}' requires an equation, applicability, and at least one Markdown source link.");
            }

            if (Equation is not null)
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
            return source.StartsWith("[", StringComparison.Ordinal) && start > 1
                   && source.EndsWith(")", StringComparison.Ordinal)
                   && Uri.TryCreate(source.Substring(start + 2, source.Length - start - 3),
                       UriKind.Absolute, out var url)
                   && url.Scheme is "https" or "http";
        }
    }
}