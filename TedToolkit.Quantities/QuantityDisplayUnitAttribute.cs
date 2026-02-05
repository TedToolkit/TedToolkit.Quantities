// -----------------------------------------------------------------------
// <copyright file="QuantityDisplayUnitAttribute.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;

namespace TedToolkit.Quantities;

/// <summary>
/// The Quantity Display Unit Attribute.
/// </summary>
/// <typeparam name="TEnum">enum type.</typeparam>
/// <param name="enum">the enum.</param>
[AttributeUsage(AttributeTargets.Struct)]
[Conditional("CODE_ANALYSIS")]
#pragma warning disable CA1019, CS9113
public sealed class QuantityDisplayUnitAttribute<TEnum>(TEnum @enum) : Attribute
#pragma warning restore CA1019, CS9113
    where TEnum : struct, Enum;