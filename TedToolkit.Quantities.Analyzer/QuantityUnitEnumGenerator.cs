// -----------------------------------------------------------------------
// <copyright file="QuantityUnitEnumGenerator.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

using Microsoft.CodeAnalysis;

using TedToolkit.Quantities.Data;
using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

using static TedToolkit.RoslynHelper.Generators.SourceComposer;
using static TedToolkit.RoslynHelper.Generators.SourceComposer<
    TedToolkit.Quantities.Analyzer.QuantityUnitEnumGenerator>;

namespace TedToolkit.Quantities.Analyzer;

/// <summary>
/// The quantity unit enum generator.
/// </summary>
/// <param name="data">the data collection.</param>
/// <param name="quantity">the quantity.</param>
/// <param name="unitSystem">unit system.</param>
internal sealed class QuantityUnitEnumGenerator(DataCollection data, Quantity quantity, UnitSystem unitSystem)
{
    /// <summary>
    /// Generate the To String method.
    /// </summary>
    /// <returns>method.</returns>
    public Method GenerateToString()
    {
        var dimension = data.Dimensions[quantity.Dimension];
        IExpression? result = null;
        AddOne(dimension.AmountOfSubstance, nameof(Dimension.AmountOfSubstance));
        AddOne(dimension.ElectricCurrent, nameof(Dimension.ElectricCurrent));
        AddOne(dimension.Length, nameof(Dimension.Length));
        AddOne(dimension.LuminousIntensity, nameof(Dimension.LuminousIntensity));
        AddOne(dimension.Mass, nameof(Dimension.Mass));
        AddOne(dimension.ThermodynamicTemperature, nameof(Dimension.ThermodynamicTemperature));
        AddOne(dimension.Time, nameof(Dimension.Time));

        var switchStatement = new SwitchStatement("unit".ToSimpleName())
            .AddSection(new SwitchSection()
                .AddLabel(new SwitchLabel(0.ToLiteral()))
                .AddStatement((result ?? "".ToLiteral()).Return));

        foreach (var quantityUnit in quantity.Units)
        {
            var unit = data.Units[quantityUnit];
            var unitName = unit.GetUnitName(data.Units.Values);

            var invocation = "global::TedToolkit.Quantities.Internals.GetUnitString".ToSimpleName().Invoke()
                .AddArgument(Argument("isSymbol".ToSimpleName()))
                .AddArgument(Argument("formatProvider".ToSimpleName()))
                .AddArgument(Argument(unit.Symbol.Replace("\"", "\\\"").ToLiteral()))
                .AddArgument(Argument(unit.Name.ToLiteral()));

            foreach (var keyValuePair in unit.Labels)
            {
                invocation
                    .AddArgument(Argument(new TupleExpression()
                        .AddItem(keyValuePair.Key.ToLiteral())
                        .AddItem(keyValuePair.Value.ToLiteral())));
            }

            switchStatement.AddSection(new SwitchSection()
                .AddLabel(new SwitchLabel(quantity.UnitName.ToSimpleName().Sub(unitName)))
                .AddStatement(invocation.Return));
        }

        return Method("ToString", new(DataType.String)).Public.Static
            .AddParameter(Parameter(new DataType(quantity.UnitName.ToSimpleName()), "unit").This)
            .AddParameter(Parameter(DataType.Bool, "isSymbol"))
            .AddParameter(Parameter(DataType.FromType<IFormatProvider>().Null, "formatProvider"))
            .AddStatement(switchStatement
                .AddSection(new SwitchSection()
                    .AddLabel(new SwitchLabel())
                    .AddStatement("unit.ToString".ToSimpleName().Invoke().Return)));

        void AddOne(int count, string key)
        {
            if (count is 0)
            {
                return;
            }

            var unit = unitSystem.GetUnit(key);

            var member = ZString.Concat("global::TedToolkit.Quantities.", key, "Unit.",
                    unit.GetUnitName(data.Units.Values), ".ToString")
                .ToSimpleName().Invoke()
                .AddArgument(Argument("isSymbol".ToSimpleName()))
                .AddArgument(Argument("formatProvider".ToSimpleName()))
                .Operator("+", count.ToSuperscript().ToLiteral());

            if (result is null)
            {
                result = member;
            }
            else
            {
                result = result.Operator("+", "·".ToLiteral()).Operator("+", member);
            }
        }
    }

    /// <summary>
    /// Generate the code.
    /// </summary>
    /// <param name="context">the context.</param>
    public void GenerateCode(in SourceProductionContext context)
    {
        var enumDeclaration = Enum(quantity.UnitName).Public
            .AddEnumMember(new EnumMember("None"));

        Helpers.AddSummary(enumDeclaration, quantity.Description, quantity.Links, quantity.Dimension);

        foreach (var quantityUnit in quantity.Units)
        {
            var unit = data.Units[quantityUnit];
            var unitName = unit.GetUnitName(data.Units.Values);

            var allUnitName = ZString.Concat("AllUnit.", unitName).ToSimpleName();
            enumDeclaration.AddEnumMember(
                new EnumMember(unitName, allUnitName)
                    .AddRootDescription(new DescriptionInheritDoc(allUnitName)));
        }

        File()
            .AddNameSpace(NameSpace("TedToolkit.Quantities")
                .AddMember(enumDeclaration))
            .Generate(context, quantity.UnitName);
    }
}