// -----------------------------------------------------------------------
// <copyright file="UnitOption.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Quantities;

/// <summary>
/// The Generated Type access.
/// </summary>
[Flags]
public enum UnitOption
{
    /// <summary>
    /// Nothing.
    /// </summary>
    NONE = 0,

    /// <summary>
    /// Generate the unit with <see langword="internal"/> accessibility.
    /// </summary>
    INTERNAL_UNITS = 1 << 0,

    /// <summary>
    /// Generate the extension methods.
    /// </summary>
    GENERATE_EXTENSION_METHODS = 1 << 1,

    /// <summary>
    /// Generate the extension properties.
    /// </summary>
    GENERATE_EXTENSION_PROPERTIES = 1 << 2,
}