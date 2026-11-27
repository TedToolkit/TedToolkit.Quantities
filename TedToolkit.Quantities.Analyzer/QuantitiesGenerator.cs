// -----------------------------------------------------------------------
// <copyright file="QuantitiesGenerator.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Immutable;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using TedToolkit.RoslynHelper;

using static TedToolkit.RoslynHelper.SourceComposer;
using static TedToolkit.RoslynHelper.SourceComposer<
    TedToolkit.Quantities.Analyzer.QuantitiesGenerator>;

namespace TedToolkit.Quantities.Analyzer;

/// <summary>
/// The generator for quantities.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class QuantitiesGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var additionalJsonFiles = context.AdditionalTextsProvider
            .Where(file => Path.GetFileName(file.Path).StartsWith("Quantity", StringComparison.OrdinalIgnoreCase)
                           && Path.GetExtension(file.Path).Equals(".json", StringComparison.OrdinalIgnoreCase));
        var compilationProvider = context.CompilationProvider;

        var combined = compilationProvider.Combine(additionalJsonFiles.Collect());

        context.RegisterSourceOutput(combined, Generate);
    }

    private static (ITypeSymbol TDataType,
        int Flag,
        Dictionary<string, string> Units,
        string QuantitySystem,
        string[] Quantities)?
        ReadUnit(Compilation compilations)
    {
        if (compilations.Assembly.GetAttributes()
                .FirstOrDefault(a =>
                    a.AttributeClass is { IsGenericType: true, } attributeClass
                    && attributeClass.ConstructUnboundGenericType().ToString().Contains("Quantities")) is not
                    { } attrData)
        {
            return null;
        }

        var tDataType = attrData.AttributeClass?.TypeArguments.FirstOrDefault();
        if (tDataType is null)
        {
            return null;
        }

        if (attrData.ApplicationSyntaxReference?.GetSyntax() is not AttributeSyntax syntax)
        {
            return null;
        }

        var flag = 0;
        var quantitySystem = "";
        var quantities = new List<string>();
        var quantityTypes = new Dictionary<string, string>();
        if (syntax.ArgumentList?.Arguments is { } arguments)
        {
            var isFirst = true;
            foreach (var attributeArgumentSyntax in arguments)
            {
                var name = attributeArgumentSyntax.NameEquals?.Name.Identifier.ValueText;

                var expr = attributeArgumentSyntax.Expression;
                if (name == "Options")
                {
                    GetData<int>(v => flag = v);
                }
                else if (name is null)
                {
                    GetData<string>(v =>
                    {
                        if (isFirst)
                        {
                            quantitySystem = v;
                            isFirst = false;
                        }
                        else
                        {
                            quantities.Add(v);
                        }
                    });
                }
                else
                {
                    quantityTypes[name] = expr.ToString().Split('.')[^1];
                }

                void GetData<TData>(Action<TData> action)
                {
                    var semanticModel = compilations.GetSemanticModel(expr.SyntaxTree);
                    var constant = semanticModel.GetConstantValue(expr);

                    if (constant is not { HasValue: true, Value: TData v, })
                    {
                        return;
                    }

                    action(v);
                }
            }
        }

        return (tDataType, flag, quantityTypes, quantitySystem, quantities.ToArray());
    }

    private static void Generate(SourceProductionContext context,
        (Compilation Compilation, ImmutableArray<AdditionalText> Texts) arg)
    {
        try
        {
            var (compilations, texts) = arg;
            if (compilations.GetTypeByMetadataName("TedToolkit.Quantities.QuantitiesAttribute`1")
                ?.BaseType?.FullName
                is "System.Attribute")
            {
                return;
            }

            var unitAttribute = ReadUnit(compilations);
            var tDataType = unitAttribute?.TDataType;
            var flag = unitAttribute?.Flag ?? 0;
            var units = unitAttribute?.Units;
            var quantitySystem = unitAttribute?.QuantitySystem;
            var catalog = texts.SingleOrDefault(text => quantitySystem is not null
                && string.Equals(Path.GetFileName(text.Path), $"Quantity.{quantitySystem}.Catalog.json",
                    StringComparison.OrdinalIgnoreCase));
            var data = Helpers.GetData(quantitySystem,
                texts.Where(text => text != catalog).Select(text => text.GetText(context.CancellationToken)!.ToString()),
                unitAttribute?.Quantities ?? Array.Empty<string>(),
                catalog?.GetText(context.CancellationToken)?.ToString());

            var unit = new UnitSystem(units, data);
            {
                new UnitEnumGenerator([.. data.Units.Values,]).GenerateCode(context);

                var toStringExtensions = Class("UnitToStringExtensions").Public.Static;
                foreach (var quantity in data.Quantities)
                {
                    var enumGenerator = new QuantityUnitEnumGenerator(data, quantity.Value, unit);
                    enumGenerator.GenerateCode(context);
                    if (tDataType is not null)
                    {
                        toStringExtensions = toStringExtensions.AddMember(enumGenerator.GenerateToString());
                    }
                }

                File()
                    .AddNameSpace(NameSpace("TedToolkit.Quantities")
                        .AddMember(toStringExtensions))
                    .Generate(context, "_UnitToStringExtensions");

                new QuantitiesAttributeGenerator(data).Generate(context);
            }

            if (tDataType is null)
            {
                return;
            }

            var isPublic = (flag & 1 << 0) is 0;
            var generateMethods = (flag & 1 << 1) is not 0;
            var generateProperties = (flag & 1 << 2) is not 0;
            var declaredQuantities = data.Quantities.Values
                .Select(quantity => compilations.Assembly.GetTypeByMetadataName("TedToolkit.Quantities." + quantity.Name))
                .OfType<INamedTypeSymbol>()
                .ToDictionary(symbol => symbol.Name);
            var hasDimensionConversions = declaredQuantities.Values.Any(QuantityStructGenerator.HasWidening);

            foreach (var quantity in data.Quantities)
            {
                declaredQuantities.TryGetValue(quantity.Value.Name, out var quantitySymbol);
                new QuantityStructGenerator(data, quantity.Value, tDataType, unit,
                        isPublic, quantitySymbol, hasDimensionConversions)
                    .GenerateCode(context);
            }

            QuantityFormulaCoverage.Generate(context, declaredQuantities.Values, data);
            new ToleranceGenerator(data.Quantities.Values.ToArray(), isPublic, tDataType)
                .Generate(context, compilations);

            if (generateProperties)
            {
                new UnitPropertyExtensionGenerator(isPublic, tDataType, data).Generate(context);
            }
            else if (generateMethods)
            {
                new UnitMethodExtensionGenerator(isPublic, tDataType, data).Generate(context);
            }
        }
#pragma warning disable CA1031
        catch (Exception e)
#pragma warning restore CA1031
        {
            var msg = e.GetType().Name + ": " + e.Message + "\n" + e.StackTrace;
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor("ERR", msg, msg, "Error", DiagnosticSeverity.Error, true), Location.None));
        }
    }
}