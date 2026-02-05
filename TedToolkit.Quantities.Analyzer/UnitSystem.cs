// -----------------------------------------------------------------------
// <copyright file="UnitSystem.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.Quantities.Data;

namespace TedToolkit.Quantities.Analyzer;

/// <summary>
/// Unit system.
/// </summary>
/// <param name="unitDictionary">unit dictionary.</param>
/// <param name="collection">collections.</param>
internal readonly struct UnitSystem(Dictionary<string, string> unitDictionary, DataCollection collection)
{
    /// <summary>
    /// Gets keys.
    /// </summary>
    public IReadOnlyCollection<string> Keys { get; } = unitDictionary.Keys;

    /// <summary>
    /// Get the quantity.
    /// </summary>
    /// <param name="key">key.</param>
    /// <returns>quantity.</returns>
    public Quantity GetQuantity(string key)
        => collection.Quantities.Values.First(q => q.Name == key);

    /// <summary>
    /// Get the unit.
    /// </summary>
    /// <param name="key">key.</param>
    /// <returns>unit.</returns>
    public Unit GetUnit(string key)
    {
        var quantity = GetQuantity(key);

        var data = collection;
        var allUnits = data.Units.Values.ToArray();
        var quantityUnits = quantity.Units
            .Select(u => data.Units[u]);
        if (unitDictionary.TryGetValue(key, out var unitKey))
            return quantityUnits.First(q => q.GetUnitName(allUnits) == unitKey);

        return quantityUnits
            .OrderBy(i => i.DistanceToDefault)
            .ThenByDescending(i => i.ApplicableSystem)
            .First();
    }
}