// -----------------------------------------------------------------------
// <copyright file="IQuantityValue{TValue}.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Numerics;

namespace TedToolkit.Quantities;

/// <summary>
/// The quantity value.
/// </summary>
/// <typeparam name="TValue">the value type.</typeparam>
public interface IQuantityValue<out TValue> :
    IQuantity
    where TValue : struct,
#if NET7_0_OR_GREATER
    INumber<TValue>,
#endif
    IConvertible
{
    /// <summary>
    /// Gets the Value. In the most case, you don't need it.
    /// </summary>
    TValue Value { get; }
}