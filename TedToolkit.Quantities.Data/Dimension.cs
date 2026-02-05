// -----------------------------------------------------------------------
// <copyright file="Dimension.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Quantities.Data;

/// <summary>
/// The Dimension.
/// </summary>
/// <param name="AmountOfSubstance">substance.</param>
/// <param name="ElectricCurrent">electric current.</param>
/// <param name="Length">length.</param>
/// <param name="LuminousIntensity">luminous intensity.</param>
/// <param name="Mass">mass.</param>
/// <param name="ThermodynamicTemperature">thermodynamic temperature.</param>
/// <param name="Time">time.</param>
/// <param name="Dimensionless">dimensionless.</param>
public readonly record struct Dimension(
    int AmountOfSubstance,
    int ElectricCurrent,
    int Length,
    int LuminousIntensity,
    int Mass,
    int ThermodynamicTemperature,
    int Time,
    int Dimensionless)
{
    /// <summary>
    /// Get the exponent.
    /// </summary>
    /// <param name="key">key.</param>
    /// <returns>exponent.</returns>
    public int GetExponent(string key)
    {
        return key switch
            {
                nameof(AmountOfSubstance) => AmountOfSubstance,
                nameof(ElectricCurrent) => ElectricCurrent,
                nameof(Length) => Length,
                nameof(LuminousIntensity) => LuminousIntensity,
                nameof(Mass) => Mass,
                nameof(ThermodynamicTemperature) => ThermodynamicTemperature,
                nameof(Time) => Time,
                nameof(Dimensionless) => Dimensionless,
                _ => 0,
            };
    }

#pragma warning disable CA2225
    public static Dimension operator *(int left, scoped in Dimension right)
#pragma warning restore CA2225
    {
        return new(
            left * right.AmountOfSubstance,
            left * right.ElectricCurrent,
            left * right.Length,
            left * right.LuminousIntensity,
            left * right.Mass,
            left * right.ThermodynamicTemperature,
            left * right.Time,
            left * right.Dimensionless);
    }

#pragma warning disable CA2225
    public static Dimension operator *(scoped in Dimension left, int right)
#pragma warning restore CA2225
        => right * left;
}