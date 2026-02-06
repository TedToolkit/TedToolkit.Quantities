// -----------------------------------------------------------------------
// <copyright file="QuantitySystemGenerator.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

using static TedToolkit.RoslynHelper.Generators.SourceComposer;
using static TedToolkit.RoslynHelper.Generators.SourceComposer<
    TedToolkit.Quantities.Generator.QuantitySystemGenerator>;

namespace TedToolkit.Quantities.Generator;

/// <summary>
/// The quantity system generator.
/// </summary>
internal abstract class QuantitySystemGenerator
{
    /// <summary>
    /// Generate the quantity system.
    /// </summary>
    /// <param name="folder">folder.</param>
    /// <param name="systems">systems.</param>
    public static void GenerateQuantitySystem(string folder, IEnumerable<(string Name, string Description)> systems)
    {
        var classDeclaration = Class("QuantitySystems").Public.Static;

        foreach (var (name, description) in systems)
        {
            classDeclaration.AddMember(Field(DataType.String, name.Replace('-', '_'))
                .AddRootDescription(new DescriptionSummary(new DescriptionText(description)))
                .Public.Const.AddDefault(name.ToLiteral()));
        }

        var code = File()
            .AddNameSpace(NameSpace("TedToolkit.Quantities")
                .AddMember(classDeclaration))
            .ToCode();

        System.IO.File.WriteAllText(Path.Combine(folder, "QuantitySystems.g.cs"), code);
    }
}