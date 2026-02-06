// -----------------------------------------------------------------------
// <copyright file="UnitEnumGenerator.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

using TedToolkit.Quantities.Data;
using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

using static TedToolkit.RoslynHelper.Generators.SourceComposer;
using static TedToolkit.RoslynHelper.Generators.SourceComposer<
    TedToolkit.Quantities.Analyzer.UnitEnumGenerator>;

namespace TedToolkit.Quantities.Analyzer;

/// <summary>
/// The unit enum  generator.
/// </summary>
/// <param name="units">all units.</param>
public sealed class UnitEnumGenerator(IReadOnlyList<Unit> units)
{
    /// <summary>
    /// Generate the code of the unit enum.
    /// </summary>
    /// <param name="context">context.</param>
    public void GenerateCode(scoped in SourceProductionContext context)
    {
        var enumDeclaration = Enum("AllUnit").Public
            .AddEnumMember(new EnumMember("None"));

        foreach (var unit in units)
        {
            var enumMember = new EnumMember(unit.GetUnitName(units));
            Helpers.AddSummary(enumMember, unit.Description, unit.Links, "");
            enumDeclaration.AddEnumMember(enumMember);
        }

        File()
            .AddNameSpace(NameSpace("TedToolkit.Quantities")
                .AddMember(enumDeclaration))
            .Generate(context, "_AllUnits");
    }
}