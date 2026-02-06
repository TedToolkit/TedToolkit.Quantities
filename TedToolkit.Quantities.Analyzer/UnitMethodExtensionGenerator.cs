// -----------------------------------------------------------------------
// <copyright file="UnitMethodExtensionGenerator.cs" company="TedToolkit">
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
    TedToolkit.Quantities.Analyzer.UnitMethodExtensionGenerator>;

namespace TedToolkit.Quantities.Analyzer;

/// <summary>
/// The unit method extension generator.
/// </summary>
/// <param name="isPublic">is public.</param>
/// <param name="typeName">the type symbol.</param>
/// <param name="data">the data set.</param>
public sealed class UnitMethodExtensionGenerator(bool isPublic, ITypeSymbol typeName, DataCollection data)
    : UnitExtensionGenerator(isPublic, data)
{
    /// <inheritdoc />
    protected override string FileName
        => "_UnitMethodExtension";

    /// <inheritdoc />
    protected override TypeDeclaration ModifyClass(TypeDeclaration classDeclaration)
    {
        foreach (var (quantity, unit, memberName) in CreateMembers())
        {
            var methodExpression = ZString.Concat("global::TedToolkit.Quantities.", quantity, ".From", unit)
                .ToSimpleName();
            classDeclaration.AddMember(Method(memberName, new ReturnType(new DataType(quantity.ToSimpleName()))).Public
                .Static
                .AddRootDescription(new DescriptionInheritDoc(methodExpression))
                .AddParameter(Parameter(DataType.FromSymbol(typeName), "value").This)
                .AddStatement(methodExpression.Invoke().AddArgument(Argument("@value".ToSimpleName())).Return));
        }

        return classDeclaration;
    }
}