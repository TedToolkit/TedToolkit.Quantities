// -----------------------------------------------------------------------
// <copyright file="UnitExtensionGenerator.cs" company="TedToolkit">
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
    TedToolkit.Quantities.Analyzer.UnitExtensionGenerator>;

namespace TedToolkit.Quantities.Analyzer;

/// <summary>
/// The unit extension generator.
/// </summary>
/// <param name="isPublic">is public.</param>
/// <param name="data">data collection.</param>
public abstract class UnitExtensionGenerator(bool isPublic, DataCollection data)
{
    /// <summary>
    /// Create the members.
    /// </summary>
    /// <returns>the members.</returns>
    protected IEnumerable<(string Quantity, string Unit, string MemberName)> CreateMembers()
    {
        return CreateUnits().GroupBy(i => i.Unit).SelectMany(g =>
        {
            return g.Count() < 2
                ? g.Select(pair => (pair.Quantity, pair.Unit, pair.Unit))
                : g.Select(pair => (pair.Quantity, pair.Unit, ZString.Join('_', pair.Unit, pair.Quantity)));
        });
    }

    private IEnumerable<(string Quantity, string Unit)> CreateUnits()
    {
        return data.Quantities.Values.SelectMany(q =>
            q.Units.Select(u => (q.Name, data.Units[u].GetUnitName(data.Units.Values))));
    }

    /// <summary>
    /// Gets the file name.
    /// </summary>
    protected abstract string FileName { get; }

    /// <summary>
    /// Modify the class.
    /// </summary>
    /// <param name="classDeclaration">the type declaration.</param>
    /// <returns>modified type declaration.</returns>
    protected abstract TypeDeclaration ModifyClass(TypeDeclaration classDeclaration);

    /// <summary>
    /// Generate the codes.
    /// </summary>
    /// <param name="context">context.</param>
    public void Generate(scoped in SourceProductionContext context)
    {
        var typeDeclaration = Class("UnitsExtension").Static.Partial;

        typeDeclaration = isPublic ? typeDeclaration.Public : typeDeclaration.Internal;

        File()
            .AddNameSpace(NameSpace("TedToolkit.Quantities")
                .AddMember(ModifyClass(typeDeclaration)))
            .Generate(context, FileName);
    }
}