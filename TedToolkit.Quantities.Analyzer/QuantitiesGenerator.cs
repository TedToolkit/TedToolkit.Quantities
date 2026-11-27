// -----------------------------------------------------------------------
// <copyright file="QuantitiesGenerator.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Immutable;

using Cysharp.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using TedToolkit.Quantities.Data;
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
    private const string DefaultNamespace = "TedToolkit.Quantities";

    private const string PrecompiledNamespace = "TedToolkit.Quantities.SI.Double";

    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var additionalJsonFiles = context.AdditionalTextsProvider
            .Where(file => Path.GetFileName(file.Path).StartsWith("Quantity", StringComparison.OrdinalIgnoreCase)
                           && Path.GetExtension(file.Path).Equals(".json", StringComparison.OrdinalIgnoreCase));
        var compilationProvider = context.CompilationProvider;

        var precompiled = context.AnalyzerConfigOptionsProvider.Select((provider, _) =>
            provider.GlobalOptions.TryGetValue("build_property.PrecompileSiQuantities", out var value)
            && string.Equals(value, "true", StringComparison.OrdinalIgnoreCase));
        var combined = compilationProvider.Combine(additionalJsonFiles.Collect()).Combine(precompiled);

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
        ((Compilation Compilation, ImmutableArray<AdditionalText> Texts) Input, bool Precompiled) arg)
    {
        try
        {
            var ((compilations, texts), precompiled) = arg;
            var generatedNamespace = precompiled ? PrecompiledNamespace : DefaultNamespace;

            if (!precompiled && compilations.GetTypeByMetadataName("TedToolkit.Quantities.QuantitiesAttribute`1")
                ?.BaseType?.FullName
                is "System.Attribute")
            {
                return;
            }

            var unitAttribute = precompiled ? null : ReadUnit(compilations);
            var tDataType = precompiled
                ? compilations.GetSpecialType(SpecialType.System_Double)
                : unitAttribute?.TDataType;
            var flag = unitAttribute?.Flag ?? 0;
            var units = unitAttribute?.Units;
            var data = precompiled
                ? GetPrecompiledData()
                : Helpers.GetData(unitAttribute?.QuantitySystem,
                    texts.Select(t => t.GetText(context.CancellationToken)!.ToString()),
                    unitAttribute?.Quantities ?? Array.Empty<string>());

            var unit = new UnitSystem(units, data);
            {
                new UnitEnumGenerator([.. data.Units.Values,]).GenerateCode(context, generatedNamespace);

                var toStringExtensions = Class("UnitToStringExtensions").Public.Static;
                foreach (var quantity in data.Quantities)
                {
                    var enumGenerator = new QuantityUnitEnumGenerator(data, quantity.Value, unit);
                    enumGenerator.GenerateCode(context, generatedNamespace);
                    if (tDataType is not null)
                    {
                        toStringExtensions = toStringExtensions.AddMember(enumGenerator.GenerateToString());
                    }
                }

                File()
                    .AddNameSpace(NameSpace(generatedNamespace)
                        .AddMember(toStringExtensions))
                    .Generate(context, "_UnitToStringExtensions");

                if (!precompiled)
                {
                    new QuantitiesAttributeGenerator(data).Generate(context);
                }
            }

            if (tDataType is null)
            {
                return;
            }

            var isPublic = (flag & 1 << 0) is 0;
            var generateMethods = (flag & 1 << 1) is not 0;
            var generateProperties = (flag & 1 << 2) is not 0;
            var declaredQuantities = new List<INamedTypeSymbol>();

            foreach (var quantity in data.Quantities)
            {
                var quantitySymbol =
                    compilations.Assembly.GetTypeByMetadataName(ZString.Concat(generatedNamespace, ".",
                        quantity.Value.Name));
                if (quantitySymbol is not null)
                {
                    declaredQuantities.Add(quantitySymbol);
                }

                new QuantityStructGenerator(data, quantity.Value, tDataType, unit,
                        isPublic, quantitySymbol, generatedNamespace, precompiled)
                    .GenerateCode(context);
            }

            if (precompiled)
            {
                QuantityFormulaCoverage.Generate(context, declaredQuantities);
            }
            else
            {
                new ToleranceGenerator(data.Quantities.Values.ToArray(), isPublic, tDataType)
                    .Generate(context, compilations);
            }

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

    private static DataCollection GetPrecompiledData()
    {
        var si = Helpers.GetData("SI", [], [], requireCatalog: true);
        string[] originalSupplemental = ["DimensionlessRatio", "TemperatureDifference",];
        string[] additionalSupplemental =
        [
            "Pressure", "Conductance", "Resistivity", "TemperatureGradient",
            "Thickness", "Width", "Height", "Depth", "Radius", "Diameter", "Distance", "Wavelength", "PathLength",
            "Impulse", "ThermalConductance", "ReynoldsNumber",
        ];
        var catalog = Helpers.GetData("ALL", [], [.. originalSupplemental, .. additionalSupplemental,],
            requireCatalog: true);
        var quantities = si.Quantities.ToDictionary(pair => pair.Key, pair => pair.Value);
        var units = si.Units.ToDictionary(pair => pair.Key, pair => pair.Value);
        var dimensions = si.Dimensions.ToDictionary(pair => pair.Key, pair => pair.Value);
        AddQuantities(originalSupplemental);

        const string Celsius = "DEGC";

        const string Temperature = "ThermodynamicTemperature";
        units[Celsius] = catalog.Units[Celsius];
        var temperature = quantities[Temperature];
        quantities[Temperature] = temperature with { Units = temperature.Units.Union([Celsius,]).ToArray(), };

        // Append new units after Celsius to keep the existing public enum values stable.
        AddQuantities(additionalSupplemental);

        var convertibleUnits = units.Where(pair =>
        {
            var conversion = pair.Value.Conversion;
            return conversion.Multiplier.IsFinite && !conversion.Multiplier.IsZero && conversion.Offset.IsFinite;
        }).ToDictionary(pair => pair.Key, pair => pair.Value);
        var convertibleQuantities = quantities.ToDictionary(pair => pair.Key, pair => pair.Value with
        {
            Units = pair.Value.Units.Where(convertibleUnits.ContainsKey).ToArray(),
        });
        return new(convertibleQuantities, convertibleUnits, dimensions);

        void AddQuantities(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                if (quantities.ContainsKey(name))
                {
                    continue;
                }

                var quantity = catalog.Quantities[name];
                quantities.Add(name, quantity);
                dimensions[quantity.Dimension] = catalog.Dimensions[quantity.Dimension];
                foreach (var unit in quantity.Units)
                {
                    units[unit] = catalog.Units[unit];
                }
            }
        }
    }
}