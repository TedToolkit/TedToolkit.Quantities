// -----------------------------------------------------------------------
// <copyright file="QuantityImplicitToValueTypeAttribute.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;

namespace TedToolkit.Quantities;

/// <summary>
/// Implicit convert to the value type.
/// </summary>
[AttributeUsage(AttributeTargets.Struct)]
[Conditional("CODE_ANALYSIS")]
public sealed class QuantityImplicitToValueTypeAttribute : Attribute;