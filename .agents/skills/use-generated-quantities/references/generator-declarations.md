# Quantity generator declarations

```csharp
using TedToolkit.Quantities;

[assembly: Quantities<double>(
    QuantitySystems.ALL,
    "Angle",
    "Length",
    "DimensionlessRatio",
    Length = LengthUnit.Millimetre,
    Options = UnitOptions.GENERATE_EXTENSION_PROPERTIES)]
```

```csharp
using TedToolkit.Quantities;

[QuantityDisplayUnit<AngleUnit>(AngleUnit.Degree)]
[QuantityOperator<Angle, Angle, DimensionlessRatio>(Operator.DIVIDE)]
[QuantityOperator<Angle, Angle, Angle>(Operator.SUBTRACT)]
[QuantityOperator<Angle, Length, Length>(Operator.MULTIPLY)]
public partial struct Angle;
```
