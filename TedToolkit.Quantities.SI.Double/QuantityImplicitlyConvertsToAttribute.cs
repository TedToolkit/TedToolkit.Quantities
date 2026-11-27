// -----------------------------------------------------------------------
// <copyright file="QuantityImplicitlyConvertsToAttribute.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;

namespace TedToolkit.Quantities;

/// <summary>
/// Declares a widening conversion from a specialized precompiled SI quantity to a general one.
/// </summary>
/// <typeparam name="TTarget">The general quantity type with the same SI dimensions.</typeparam>
/// <remarks>The generated conversion preserves the coherent SI value.</remarks>
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
[Conditional("CODE_ANALYSIS")]
internal sealed class QuantityImplicitlyConvertsToAttribute<TTarget> : Attribute
    where TTarget : struct;
