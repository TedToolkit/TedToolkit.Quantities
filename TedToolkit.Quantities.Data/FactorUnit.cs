// -----------------------------------------------------------------------
// <copyright file="FactorUnit.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Quantities.Data;

/// <summary>
/// Factor Unit.
/// </summary>
/// <param name="Exponent">exponent.</param>
/// <param name="Dimension">dimension.</param>
public readonly record struct FactorUnit(double Exponent, string Dimension);