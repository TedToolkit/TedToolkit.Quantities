// -----------------------------------------------------------------------
// <copyright file="QuantityOperatorAttribute.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;

#pragma warning disable CS9113 // Parameter is unread.

namespace TedToolkit.Quantities;

/// <summary>
/// It can generate the operators for you.
/// </summary>
/// <param name="operator">operator.</param>
/// <typeparam name="TLeft">left type.</typeparam>
/// <typeparam name="TRight">right type.</typeparam>
/// <typeparam name="TResult">result type.</typeparam>
[Conditional("CODE_ANALYSIS")]
[AttributeUsage(AttributeTargets.Struct, AllowMultiple = true)]
#pragma warning disable S2326, CS9113, CA1019
public sealed class QuantityOperatorAttribute<TLeft, TRight, TResult>(Operator @operator) : Attribute
#pragma warning restore S2326, CS9113, CA1019
    where TLeft : struct
    where TRight : struct
    where TResult : struct;