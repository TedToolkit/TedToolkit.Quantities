// -----------------------------------------------------------------------
// <copyright file="Unit.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Newtonsoft.Json;
using PeterO.Numbers;

namespace TedToolkit.Quantities.Data;

/// <summary>
/// The Unit.
/// </summary>
/// <param name="Key">key.</param>
/// <param name="Name">name.</param>
/// <param name="Description">description.</param>
/// <param name="Links">links.</param>
/// <param name="Symbol">symbol.</param>
/// <param name="Labels">labels.</param>
/// <param name="Multiplier">multiplier.</param>
/// <param name="Offset">offset.</param>
/// <param name="FactorUnits">factor units.</param>
/// <param name="ApplicableSystem">applicable system.</param>
public readonly record struct Unit(
    string Key,
    string Name,
    string Description,
    IReadOnlyList<Link> Links,
    string Symbol,
    Dictionary<string, string> Labels,
    string Multiplier,
    string Offset,
    IReadOnlyList<FactorUnit> FactorUnits,
    int ApplicableSystem)
{
    /// <summary>
    /// Get the unit Name.
    /// </summary>
    /// <param name="allUnits">all units.</param>
    /// <returns>name.</returns>
    public string GetUnitName(IEnumerable<Unit> allUnits)
    {
        var name = Name;
        return MakeSafe(allUnits.Count(u => u.Name == name) is not 1
            ? name + "_" + Key
            : name);

        static string MakeSafe(string value)
        {
            return value
                .Replace("(", "_")
                .Replace("-", "_")
                .Replace(")", "_")
                .Replace(",", "")
                .Replace(".", "")
                .Replace("°", "");
        }
    }

    /// <summary>
    /// Gets conversion.
    /// </summary>
    [JsonIgnore]
    public Conversion Conversion
        => new(EDecimal.FromString(Multiplier), EDecimal.FromString(Offset));

    /// <summary>
    /// Gets distance to default.
    /// </summary>
    [JsonIgnore]
    public double DistanceToDefault
    {
        get
        {
            var result = 0.0;
            if (!string.IsNullOrEmpty(Multiplier))
            {
                if (double.TryParse(Multiplier, out var value))
                    result += Math.Abs(value - 1);
                else
                    return double.MaxValue;
            }

            if (!string.IsNullOrEmpty(Offset))
            {
                if (double.TryParse(Offset, out var value))
                    result += Math.Abs(value);
                else
                    return double.MaxValue;
            }

            return result;
        }
    }
}