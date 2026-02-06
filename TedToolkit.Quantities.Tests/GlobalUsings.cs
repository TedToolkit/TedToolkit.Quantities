using TedToolkit.Quantities;

[assembly: Quantities<double>(
    QuantitySystems.ALL,
    "Area",
    "Volume",
    "Angle",
    "LinearVelocity",
    "DimensionlessRatio",
    "Dimensionless",
    "InverseLength",
    Length = LengthUnit.Millimetre,
    Options = UnitOptions.GENERATE_EXTENSION_METHODS)]