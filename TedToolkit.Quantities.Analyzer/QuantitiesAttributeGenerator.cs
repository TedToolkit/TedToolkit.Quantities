// -----------------------------------------------------------------------
// <copyright file="QuantitiesAttributeGenerator.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using TedToolkit.Quantities.Data;
using TedToolkit.RoslynHelper.Extensions;
using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

using static TedToolkit.RoslynHelper.Generators.SourceComposer;
using static TedToolkit.RoslynHelper.Generators.SourceComposer<
    TedToolkit.Quantities.Analyzer.QuantitiesAttributeGenerator>;

namespace TedToolkit.Quantities.Analyzer;

/// <summary>
/// The generator for the quantities attribute.
/// </summary>
/// <param name="data">the data collection.</param>
internal sealed class QuantitiesAttributeGenerator(DataCollection data)
{
    /// <summary>
    /// The quantity unit.
    /// </summary>
    /// <param name="Quantity">quantity.</param>
    /// <param name="Unit">the unit.</param>
    public readonly record struct QuantityUnit(Quantity Quantity, string Unit)
    {
        /// <summary>
        /// Gets unit name.
        /// </summary>
        public string UnitName
            => Quantity.UnitName;

        /// <summary>
        /// Gets name.
        /// </summary>
        public string Name
            => Quantity.Name;
    }

    /// <summary>
    /// Gets the quantity units.
    /// </summary>
    public IEnumerable<QuantityUnit> QuantityUnits
    {
        get
        {
            return data.Quantities.Values.Where(q => q.IsBasic).Select(q =>
            {
                var unit = q.Units
                    .Select(u => data.Units[u])
                    .OrderBy(u => u.DistanceToDefault)
                    .ThenByDescending(u => u.ApplicableSystem)
                    .First().GetUnitName(data.Units.Values);
                return new QuantityUnit(q, unit);
            });
        }
    }

    /// <summary>
    /// Generate the code.
    /// </summary>
    /// <param name="context">context.</param>
    public void Generate(scoped in SourceProductionContext context)
    {
        var quantityAttribute = Class("QuantitiesAttribute").Internal.Sealed
            .AddBaseType<System.Attribute>()
            .AddAttribute(Attribute<AttributeUsageAttribute>()
                .AddArgument(Argument(AttributeTargets.Assembly.ToExpression())))
            .AddAttribute(Attribute<ConditionalAttribute>()
                .AddArgument(Argument("CODE_ANALYSIS".ToLiteral())))
            .AddTypeParameter(TypeParameter("TData")
                .AddStructConstraint()
                .AddConstraint<IConvertible>())
            .AddParameter(Parameter(DataType.String, "quantitySystem"))
            .AddParameter(Parameter<string[]>("quantities").Params);

        foreach (var quantityUnit in QuantityUnits)
        {
            quantityAttribute.AddMember(
                Property(new(quantityUnit.UnitName), quantityUnit.Name).Public
                    .AddAccessor(Accessor(AccessorType.GET))
                    .AddAccessor(Accessor(AccessorType.INIT))
                    .AddDefault(quantityUnit.UnitName.ToSimpleName().Sub(quantityUnit.Unit)));
        }

        quantityAttribute.AddMember(Property(new("global::TedToolkit.Quantities.UnitOptions"), "Options").Public
            .AddAccessor(Accessor(AccessorType.GET))
            .AddAccessor(Accessor(AccessorType.INIT))
            .AddDefault(0.ToLiteral()));

        File()
            .AddNameSpace(NameSpace("TedToolkit.Quantities")
                .AddMember(quantityAttribute))
            .Generate(context, "_QuantitiesAttribute");
    }
}