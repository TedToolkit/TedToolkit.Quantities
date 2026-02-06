// -----------------------------------------------------------------------
// <copyright file="DataCollection.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Quantities.Data;

/// <summary>
/// The Collection of the data.
/// </summary>
/// <param name="Quantities">quantities.</param>
/// <param name="Units">units.</param>
/// <param name="Dimensions">dimensions.</param>
#pragma warning disable CA1711
public readonly record struct DataCollection(
#pragma warning restore CA1711
    IReadOnlyDictionary<string, Quantity> Quantities,
    IReadOnlyDictionary<string, Unit> Units,
    IReadOnlyDictionary<string, Dimension> Dimensions);