// -----------------------------------------------------------------------
// <copyright file="Internal.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;

namespace TedToolkit.Quantities;

/// <summary>
/// Some internal methods.
/// </summary>
public static class Internal
{
    /// <summary>
    /// Get the unit string.
    /// </summary>
    /// <param name="isSymbol">is a symbol.</param>
    /// <param name="formatProvider">provider.</param>
    /// <param name="symbol">symbol.</param>
    /// <param name="defaultLabel">default label.</param>
    /// <param name="labels">labels.</param>
    /// <returns>result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="labels"/> is <c>null</c>.</exception>
    public static string GetUnitString(bool isSymbol, IFormatProvider? formatProvider,
        string symbol,
        string defaultLabel, params (string, string)[] labels)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(labels);
#else
        if (labels is null)
            throw new ArgumentNullException(nameof(labels));
#endif

        if (isSymbol)
            return symbol;

        var culture = formatProvider as CultureInfo ?? CultureInfo.CurrentCulture;
        var letter = culture.Name;
        foreach (var (key, value) in labels)
        {
            if (key.Equals(letter, StringComparison.OrdinalIgnoreCase))
                return value;
        }

        letter = culture.TwoLetterISOLanguageName;
        foreach (var (key, value) in labels)
        {
            if (key.Equals(letter, StringComparison.OrdinalIgnoreCase))
                return value;
        }

        return defaultLabel;
    }

    /// <summary>
    /// Parse the format.
    /// </summary>
    /// <param name="format">format.</param>
    /// <param name="isSymbol">is symbol.</param>
    /// <returns>result.</returns>
    public static string? ParseFormat(string? format, out bool isSymbol)
    {
        if (string.IsNullOrEmpty(format))
        {
            isSymbol = true;
            return null;
        }

#if NET6_0_OR_GREATER
        var splitFormat = format.Split('|');
        isSymbol = splitFormat[0].Contains('s', StringComparison.InvariantCultureIgnoreCase);
        return splitFormat[^1];
#else
        var splitFormat = format!.Split('|');
        isSymbol = splitFormat[0].Contains('s') || splitFormat[0].Contains('S');
        return splitFormat[splitFormat.Length - 1];
#endif
    }
}