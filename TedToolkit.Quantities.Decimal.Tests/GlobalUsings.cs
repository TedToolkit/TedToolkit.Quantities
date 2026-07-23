using TedToolkit.Quantities;

[assembly: global::TedToolkit.Quantities.Quantities<decimal>(
    global::TedToolkit.Quantities.QuantitySystems.SI,
    Length = global::TedToolkit.Quantities.LengthUnit.Metre,
    Options = global::TedToolkit.Quantities.UnitOptions.GENERATE_EXTENSION_PROPERTIES)]
