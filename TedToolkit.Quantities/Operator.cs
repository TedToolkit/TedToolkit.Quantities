// -----------------------------------------------------------------------
// <copyright file="Operator.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Quantities;
#pragma warning disable SA1629

/// <summary>
/// Operator types.
/// </summary>
public enum Operator
{
    /// <summary>
    /// +
    /// </summary>
    ADD = 0,

    /// <summary>
    /// -
    /// </summary>
    SUBTRACT = 1,

    /// <summary>
    /// *
    /// </summary>
    MULTIPLY = 2,

    /// <summary>
    ///  /
    /// </summary>
    DIVIDE = 3,
}