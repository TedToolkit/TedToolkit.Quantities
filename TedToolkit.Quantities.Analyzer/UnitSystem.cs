// -----------------------------------------------------------------------
// <copyright file="UnitSystem.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.Quantities.Data;

namespace TedToolkit.Quantities.Analyzer;

/// <summary>
/// The unit system.
/// </summary>
/// <param name="unitDictionary">The unit dictionary mapping quantity names to unit names.</param>
/// <param name="collection">The data collection.</param>
internal readonly struct UnitSystem(Dictionary<string, string>? unitDictionary, DataCollection collection)
{
    /// <summary>
    /// Gets the keys.
    /// </summary>
    public IReadOnlyCollection<string> Keys { get; } =
        (IReadOnlyCollection<string>?)unitDictionary?.Keys ?? Array.Empty<string>();

    /// <summary>
    /// Gets the quantity by name.
    /// </summary>
    /// <param name="key">The quantity name.</param>
    /// <returns>The matching quantity.</returns>
    public Quantity GetQuantity(string key)
    {
        return collection.Quantities.Values.First(q => q.Name == key);
    }

    /// <summary>
    /// Gets the unit for a quantity.
    /// </summary>
    /// <param name="key">The quantity name.</param>
    /// <returns>The matching unit.</returns>
    public Unit GetUnit(string key)
    {
        var quantity = GetQuantity(key);

        var data = collection;
        var allUnits = data.Units.Values.ToArray();
        var quantityUnits = quantity.Units
            .Select(u => data.Units[u]);
        if (unitDictionary?.TryGetValue(key, out var unitKey) ?? false)
        {
            return quantityUnits.First(q => q.GetUnitName(allUnits) == unitKey);
        }

        return quantityUnits
            .OrderBy(i => i.DistanceToDefault)
            .ThenByDescending(i => i.ApplicableSystem)
            .First();
    }
}