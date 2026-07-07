# TedToolkit.Quantities

A type-safe, source-generated quantities and units library for .NET. Define a `partial struct`, and the Roslyn analyzer generates unit conversions, arithmetic operators, formatting, and more — all backed by the [QUDT](https://qudt.org/) ontology.

## Features

- **Source-generated** — zero reflection, zero runtime overhead; everything is produced at compile time
- **Type-safe operators** — `Angle / Angle` yields `DimensionlessRatio`, `Angle * Length` yields `Length`, etc.
- **Broad framework support** — .NET 6–10, .NET Framework 4.7.2+, .NET Standard 2.0/2.1
- **QUDT-backed** — units and conversion factors come from the W3C QUDT ontology
- **Multiple quantity systems** — SI, CGS, Imperial, US Customary, ISQ, Planck, and more
- **Tolerance-aware comparisons** — configurable precision for equality and ordering via scoped `Tolerance`

## Quick Start

Install the package:

```
dotnet add package TedToolkit.Quantities
```

### Enable the Generator

Add an assembly-level `Quantities` attribute (typically in `GlobalUsings.cs`) to tell the generator which quantities to emit:

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

### Define a Quantity (Optional)

To customize a quantity — e.g. set a display unit or add cross-quantity operators — declare a `partial struct` in the `TedToolkit.Quantities` namespace:

```csharp
using TedToolkit.Quantities;

[QuantityDisplayUnit<AngleUnit>(AngleUnit.Degree)]
[QuantityOperator<Angle, Angle, DimensionlessRatio>(Operator.DIVIDE)]
[QuantityOperator<Angle, Angle, Angle>(Operator.SUBTRACT)]
[QuantityOperator<Angle, Length, Length>(Operator.MULTIPLY)]
public partial struct Angle;
```

### Create and Convert

```csharp
// From a constructor
var angle = new Angle(180.0, AngleUnit.Degree);

// From a static factory
var angle2 = Angle.FromRadian(Math.PI);

// From an extension property on double
var angle3 = 90.0.Degree;

// Convert between units
double rad = angle.As(AngleUnit.Radian);   // π
double deg = angle.Degree;                  // 180.0
```

### Arithmetic

```csharp
var a = 90.0.Degree;
var b = 45.0.Degree;

Angle sum  = a + b;            // 135°
Angle diff = a - b;            // 45°
Angle half = a / 2.0;          // 45°

// Cross-quantity operators (defined via attributes)
DimensionlessRatio ratio = a / b;   // 2.0
```

### Formatting

```csharp
var angle = 45.0.Degree;

angle.ToString();                                    // "45 °"
angle.ToString(AngleUnit.Radian, "F4");              // "0.7854 rad"
```

### Tolerance

Quantity comparisons use a scoped `Tolerance` so floating-point results can be compared meaningfully:

```csharp
using TedToolkit.Scopes;

using (new Tolerance { Angle = 1e-9 }.FastPush())
{
    var a = Angle.FromDegree(360.0);
    var b = Angle.FromRadian(2 * Math.PI);
    Console.WriteLine(a == b);  // True
}
```

## Attributes Reference

| Attribute | Purpose |
|---|---|
| `QuantityDisplayUnit<TUnit>(unit)` | Sets the default display unit for `ToString()` |
| `QuantityOperator<TLeft, TRight, TResult>(op)` | Generates a binary operator (`+`, `-`, `*`, `/`) |
| `QuantityImplicitToValueType` | Adds implicit conversion from quantity to its value type |
| `QuantityImplicitFromValueType` | Adds implicit conversion from value type to quantity |

## Quantity Systems

The library ships with unit data for multiple systems via `QuantitySystems`:

| Constant | System |
|---|---|
| `QuantitySystems.SI` | International System of Units |
| `QuantitySystems.CGS` | Centimetre-Gram-Second |
| `QuantitySystems.IMPERIAL` | Imperial |
| `QuantitySystems.USCS` | US Customary |
| `QuantitySystems.ISQ` | ISO System of Quantities |
| `QuantitySystems.Planck` | Planck units |
| `QuantitySystems.ALL` | All available units |

## License

LGPL-3.0 — see [COPYING](https://github.com/TedToolkit/TedToolkit.Quantities/blob/development/COPYING) and [COPYING.LESSER](https://github.com/TedToolkit/TedToolkit.Quantities/blob/development/COPYING.LESSER).
