// -----------------------------------------------------------------------
// <copyright file="QuantityFormulaAttribute.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics;

namespace TedToolkit.Quantities;

/// <summary>
/// Documents a named calculation in the quantity formula inventory.
/// </summary>
/// <param name="id">The formula identifier in domain/name form.</param>
[Conditional("CODE_ANALYSIS")]
[AttributeUsage(AttributeTargets.Method)]
public sealed class QuantityFormulaAttribute(string id) : Attribute
{
    /// <summary>
    /// Gets the formula identifier shared with equivalent operators or methods.
    /// </summary>
    public string Id { get; } = id;

    /// <summary>
    /// Gets or sets the equation when this method is its documentation source.
    /// </summary>
    public string? Equation { get; set; }

    /// <summary>
    /// Gets or sets the physical conditions required for the equation to apply.
    /// </summary>
    public string? Applicability { get; set; }

    /// <summary>
    /// Gets or sets Markdown links to the formula sources.
    /// </summary>
    public string[] Sources { get; set; } = [];
}