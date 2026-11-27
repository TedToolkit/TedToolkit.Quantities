// -----------------------------------------------------------------------
// <copyright file="QuantityCatalogBuilder.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Newtonsoft.Json;

using TedToolkit.FileSystem;
using TedToolkit.Quantities.Data;

namespace TedToolkit.Quantities.SI.Double.Build;

/// <summary>
/// Builds a source generator catalog from already generated QUDT entries.
/// </summary>
internal static class QuantityCatalogBuilder
{
    /// <summary>
    /// Writes the selected quantities and units using the generated catalogs.
    /// </summary>
    /// <param name="basePath">The catalog to extend.</param>
    /// <param name="catalogPath">The catalog from which to select entries.</param>
    /// <param name="outputPath">The destination additional file.</param>
    /// <param name="selections">Quantity names or quantity and unit keys separated by a colon.</param>
    internal static void Generate(string basePath, string catalogPath, string outputPath, string[] selections)
    {
        var primary = Read(basePath);
        var catalog = Read(catalogPath);
        var quantities = primary.Quantities.ToDictionary(pair => pair.Key, pair => pair.Value);
        var units = primary.Units.ToDictionary(pair => pair.Key, pair => pair.Value);
        var dimensions = primary.Dimensions.ToDictionary(pair => pair.Key, pair => pair.Value);
        foreach (var selection in selections)
        {
            var parts = selection.Split(':');
            var name = parts[0];
            if (parts.Length == 1 && quantities.ContainsKey(name))
            {
                continue;
            }

            var quantity = quantities.TryGetValue(name, out var existing) ? existing : catalog.Quantities[name];
            var unitKeys = parts.Length == 1 ? quantity.Units : quantity.Units.Append(parts[1]).Distinct().ToArray();
            foreach (var key in unitKeys)
            {
                if (!units.ContainsKey(key))
                {
                    units.Add(key, catalog.Units[key]);
                }
            }

            quantities[name] = quantity with { Units = unitKeys, };
            dimensions[quantity.Dimension] = catalog.Dimensions[quantity.Dimension];
        }

        var convertible = units.Where(pair =>
        {
            var conversion = pair.Value.Conversion;
            return conversion.Multiplier.IsFinite && !conversion.Multiplier.IsZero && conversion.Offset.IsFinite;
        }).ToDictionary(pair => pair.Key, pair => pair.Value);
        var exposed = quantities.ToDictionary(pair => pair.Key, pair => pair.Value with
        {
            Units = pair.Value.Units.Where(convertible.ContainsKey).ToArray(),
        });
        var file = new FilePath(outputPath);
        var json = JsonConvert.SerializeObject(new DataCollection(exposed, convertible, dimensions));
        if (file.Exists && file.ReadAllText() == json)
        {
            return;
        }

        file.WriteAllText(json);
    }

    /// <summary>
    /// Reads the QUDT catalog used by the generated types.
    /// </summary>
    /// <param name="path">The path of the catalog.</param>
    /// <returns>The catalog data.</returns>
    internal static DataCollection Read(string path)
    {
        return JsonConvert.DeserializeObject<DataCollection>(new FilePath(path).ReadAllText());
    }
}