// -----------------------------------------------------------------------
// <copyright file="QuantityOperatorDeclaration.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

using TedToolkit.RoslynHelper;

namespace TedToolkit.Quantities.Analyzer;

/// <summary>
/// Represents a quantity operator resolved from its attribute.
/// </summary>
/// <param name="Left">The left operand type.</param>
/// <param name="Right">The right operand type.</param>
/// <param name="Result">The result type.</param>
/// <param name="Token">The C# operator token.</param>
internal readonly record struct QuantityOperatorDeclaration(
    ITypeSymbol Left, ITypeSymbol Right, ITypeSymbol Result, string Token)
{
    /// <summary>
    /// Reads the operator declarations on a quantity.
    /// </summary>
    /// <param name="symbol">The quantity symbol from source.</param>
    /// <returns>The operators declared on the quantity.</returns>
    internal static IEnumerable<QuantityOperatorDeclaration> Read(INamedTypeSymbol? symbol)
    {
        if (symbol is null)
        {
            yield break;
        }

        foreach (var attribute in symbol.GetAttributes())
        {
            if (attribute.AttributeClass is not { IsGenericType: true, TypeArguments.Length: 3, } attributeClass
                || attributeClass.ConstructUnboundGenericType().FullName
                    is not "TedToolkit.Quantities.QuantityOperatorAttribute<,,>"
                || attribute.ConstructorArguments.FirstOrDefault().Value is not int value
                || value > 3)
            {
                continue;
            }

            var token = value switch
            {
                0 => "+",
                1 => "-",
                2 => "*",
                _ => "/",
            };
            var types = attributeClass.TypeArguments;
            yield return new(types[0], types[1], types[2], token);
        }
    }
}