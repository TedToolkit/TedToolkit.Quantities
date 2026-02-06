// -----------------------------------------------------------------------
// <copyright file="Link.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Newtonsoft.Json;

namespace TedToolkit.Quantities.Data;

/// <summary>
/// The Link.
/// </summary>
/// <param name="Name">name.</param>
/// <param name="Url">url.</param>
#pragma warning disable CA1054, CA1056
public readonly record struct Link(string Name, string Url)
#pragma warning restore CA1054, CA1056
{
    /// <summary>
    /// Gets the remarks.
    /// </summary>
    [JsonIgnore]
    public string Remarks
        => $"<see href=\"{Url}\">{Name}</see>";
}