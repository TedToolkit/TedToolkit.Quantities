// -----------------------------------------------------------------------
// <copyright file="UnitPropertyExtensionGenerator.cs" company="TedToolkit">
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
    TedToolkit.Quantities.Analyzer.UnitPropertyExtensionGenerator>;

namespace TedToolkit.Quantities.Analyzer;

/// <summary>
/// The unit property extension generator.
/// </summary>
/// <param name="isPublic">is public.</param>
/// <param name="typeName">the type.</param>
/// <param name="data">data.</param>
public class UnitPropertyExtensionGenerator(bool isPublic, ITypeSymbol typeName, DataCollection data)
    : UnitExtensionGenerator(isPublic, data)
{
    /// <inheritdoc />
    protected override string FileName
        => "_UnitPropertyExtension";

    /// <inheritdoc />
    protected override TypeDeclaration ModifyClass(TypeDeclaration classDeclaration)
    {
        var extension = Extension(Parameter(DataType.FromSymbol(typeName), "value"));

        foreach (var (quantity, unit, memberName) in CreateMembers())
        {
            var methodExpression = ZString.Concat("global::TedToolkit.Quantities.", quantity, ".From", unit)
                .ToSimpleName();
            extension.AddMember(Property(new DataType(quantity.ToSimpleName()), memberName).Public
                .AddRootDescription(new DescriptionInheritDoc(methodExpression))
                .AddAccessor(Accessor(AccessorType.GET)
                    .AddStatement(methodExpression.Invoke().AddArgument(Argument("@value".ToSimpleName())).Return)));
        }

#pragma warning disable CA1062
        return classDeclaration.AddMember(extension);
#pragma warning restore CA1062
    }
}