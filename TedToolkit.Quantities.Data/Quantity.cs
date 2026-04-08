// -----------------------------------------------------------------------
// <copyright file="Quantity.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Newtonsoft.Json;

namespace TedToolkit.Quantities.Data;

/// <summary>
/// The quantity.
/// </summary>
/// <param name="Name">The name.</param>
/// <param name="Description">The description.</param>
/// <param name="Links">The links.</param>
/// <param name="IsBasic">Whether this is a basic quantity.</param>
/// <param name="Dimension">The dimension.</param>
/// <param name="IsDimensionDefault">Whether this is the dimension default.</param>
/// <param name="Units">The units.</param>
public readonly record struct Quantity(
    string Name,
    string Description,
    IReadOnlyList<Link> Links,
    bool IsBasic,
    string Dimension,
    bool IsDimensionDefault,
    IReadOnlyList<string> Units)
{
    /// <summary>
    /// Gets the denominator.
    /// </summary>
    public required string Denominator { get; init; }

    /// <summary>
    /// Gets the numerator.
    /// </summary>
    public required string Numerator { get; init; }

    /// <summary>
    /// Gets the exact match list.
    /// </summary>
    public required IReadOnlyList<string> ExactMatch { get; init; }

    /// <summary>
    /// Gets the unit name.
    /// </summary>
    [JsonIgnore]
    public string UnitName
    {
        get
        {
            return Name + "Unit";
        }
    }

    /// <summary>
    /// Gets a value indicating whether this quantity is dimensionless.
    /// </summary>
    [JsonIgnore]
    public bool IsNoDimensions
    {
        get
        {
            return Dimension.Contains("A0E0L0I0M0H0T0");
        }
    }
}