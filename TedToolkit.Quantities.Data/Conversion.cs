// -----------------------------------------------------------------------
// <copyright file="Conversion.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Newtonsoft.Json;

using PeterO.Numbers;

namespace TedToolkit.Quantities.Data;

/// <summary>
/// The Conversion data.
/// </summary>
/// <param name="Multiplier">The multiplier.</param>
/// <param name="Offset">The offset.</param>
public readonly record struct Conversion(ERational Multiplier, ERational Offset)
{
    /// <summary>
    /// Gets a value indicating whether the conversion is valid.
    /// </summary>
    [JsonIgnore]
    public bool IsValid
    {
        get
        {
            return !Multiplier.IsInfinity() && !Multiplier.IsZero && !Offset.IsInfinity();
        }
    }

    /// <summary>
    /// Gets the identity (unit) conversion.
    /// </summary>
    public static Conversion Unit { get; } = new(ERational.One, ERational.Zero);

    /// <summary>
    /// Transforms this conversion relative to another.
    /// </summary>
    /// <param name="unit">The target conversion.</param>
    /// <returns>The transformed conversion, or <see langword="null"/> if <paramref name="unit"/> is <see langword="null"/>.</returns>
    public Conversion? TransformTo(Conversion? unit)
    {
        if (unit is null)
        {
            return null;
        }

        var multiplier = Multiplier / unit.Value.Multiplier;
        var offset = (Offset - unit.Value.Offset) / unit.Value.Multiplier;
        return new Conversion(multiplier, offset);
    }

    /// <summary>
    /// Raises this conversion to the specified power.
    /// </summary>
    /// <param name="exponent">The exponent.</param>
    /// <returns>The resulting conversion, or <see langword="null"/> if the operation is not supported.</returns>
    public Conversion? Pow(int exponent)
    {
        switch (exponent)
        {
            case 0:
                return Unit;

            case 1:
                return this;

            case -1:
                var multiplier = ERational.One / Multiplier;
                var offset = -Offset / Multiplier;
                return new Conversion(multiplier, offset);
        }

        if (!Multiplier.IsZero && !Offset.IsZero)
        {
            return null;
        }

        return new Conversion(Pow(Multiplier, exponent), Pow(Offset, exponent));
    }

    private static ERational Pow(ERational rational, int exponent)
    {
        if (exponent == 0)
        {
            return ERational.One;
        }

        var one = rational;
        for (var i = 1; i < Math.Abs(exponent); i++)
        {
            rational *= one;
        }

        if (exponent < 0)
        {
            rational = ERational.One / rational;
        }

        return rational;
    }

    /// <summary>
    /// Merges two conversions.
    /// </summary>
    /// <param name="other">The other conversion.</param>
    /// <returns>The merged conversion, or <see langword="null"/> if the operation is not supported.</returns>
    public Conversion? Merge(Conversion? other)
    {
        if (other is null)
        {
            return null;
        }

        if (!Multiplier.IsZero && !Offset.IsZero)
        {
            return null;
        }

        return new Conversion(Multiplier * other.Value.Multiplier,
            Offset * other.Value.Offset);
    }
}