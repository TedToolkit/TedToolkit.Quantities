// -----------------------------------------------------------------------
// <copyright file="IQuantityQuantity{TQuantity}.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Quantities;

/// <summary>
/// Quantity.
/// </summary>
/// <typeparam name="TQuantity">The quantity type.</typeparam>
public interface IQuantityQuantity<TQuantity> :
    IQuantity,
    IEquatable<TQuantity>,
    IComparable<TQuantity>
    where TQuantity : struct, IQuantityQuantity<TQuantity>
{
#if NET7_0_OR_GREATER
    /// <summary>
    /// Gets zero value.
    /// </summary>
    static abstract TQuantity Zero { get; }

    /// <summary>
    /// Gets one value.
    /// </summary>
    static abstract TQuantity One { get; }
#endif

    /// <inheritdoc cref="Math.Abs(double)"/>
    TQuantity Abs();

    /// <inheritdoc cref="Math.Min(double, double)"/>
    TQuantity Min(TQuantity val2);

    /// <inheritdoc cref="Math.Max(double, double)"/>
    TQuantity Max(TQuantity val2);

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="Math.Clamp(double, double, double)"/>
#else
    /// <summary>
    /// Returns this value. clamped to the inclusive range of <paramref name="min" /> and <paramref name="max" />.
    /// </summary>
    /// <param name="min">The lower bound of the result.</param>
    /// <param name="max">The upper bound of the result.</param>
    /// <returns>
    ///        this value if <paramref name="min" /> ≤ this value ≤ <paramref name="max" />.
    /// -or-
    /// <paramref name="min" /> if this value &lt; <paramref name="min" />.
    /// -or-
    /// <paramref name="max" /> if <paramref name="max" /> &lt; this value.
    /// -or-
    ///  <see cref="double.NaN" /> if this value equals <see cref="double.NaN" />.</returns>
#endif
    TQuantity Clamp(TQuantity min, TQuantity max);
}