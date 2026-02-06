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
/// <param name="Name">name.</param>
/// <param name="Description">description.</param>
/// <param name="Links">links.</param>
/// <param name="IsBasic">is basic.</param>
/// <param name="Dimension">dimension.</param>
/// <param name="IsDimensionDefault">is the dimension default.</param>
/// <param name="Units">units.</param>
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
    /// Gets denominator.
    /// </summary>
    public required string Denominator { get; init; }

    /// <summary>
    /// Gets numerator.
    /// </summary>
    public required string Numerator { get; init; }

    /// <summary>
    /// Gets exactMatch.
    /// </summary>
    public required IReadOnlyList<string> ExactMatch { get; init; }

    /// <summary>
    /// Gets unitName.
    /// </summary>
    [JsonIgnore]
    public string UnitName
        => Name + "Unit";

    /// <summary>
    /// Gets a value indicating whether isNoDimensions.
    /// </summary>
    [JsonIgnore]
    public bool IsNoDimensions
        => Dimension.Contains("A0E0L0I0M0H0T0");
}