// -----------------------------------------------------------------------
// <copyright file="IQuantity{TQuantity,TValue,TUnit}.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Numerics;

namespace TedToolkit.Quantities;

/// <summary>
/// Quantity.
/// </summary>
/// <typeparam name="TQuantity">The quantity type.</typeparam>
/// <typeparam name="TValue">The value type.</typeparam>
/// <typeparam name="TUnit">The unit type.</typeparam>
public interface IQuantity<TQuantity,
#if NET7_0_OR_GREATER
    TValue,
#else
    out TValue,
#endif
    in TUnit> :
    IQuantityQuantity<TQuantity>,
    IQuantityValue<TValue>
    where TValue : struct,
#if NET7_0_OR_GREATER
    INumber<TValue>,
#endif
    IConvertible
    where TQuantity : struct, IQuantity<TQuantity, TValue, TUnit>
    where TUnit : struct, Enum
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// Create from the value and unit.
    /// </summary>
    /// <param name="value">the value.</param>
    /// <param name="unit">the unit.</param>
    /// <returns>quantity.</returns>
    static abstract TQuantity From(TValue value, TUnit unit);
#endif

    /// <summary>
    /// Get the value as the specific unit.
    /// </summary>
    /// <param name="unit">unit.</param>
    /// <returns>value.</returns>
    TValue As(TUnit unit);

    /// <summary>
    /// To string.
    /// </summary>
    /// <param name="unit">unit.</param>
    /// <param name="format">format.</param>
    /// <param name="formatProvider">format provider.</param>
    /// <returns>string.</returns>
    string ToString(TUnit unit, string? format = null, IFormatProvider? formatProvider = null);
}