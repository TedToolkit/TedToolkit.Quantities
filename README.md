# TedToolkit.Quantities

[![NuGet](https://img.shields.io/nuget/v/TedToolkit.Quantities)](https://www.nuget.org/packages/TedToolkit.Quantities)
[![License: LGPL-3.0](https://img.shields.io/badge/License-LGPL--3.0-blue.svg)](COPYING.LESSER)

A type-safe, source-generated quantities and units library for .NET, backed by the [QUDT](https://qudt.org/) ontology.

Declare a `partial struct` with a few attributes, and the bundled Roslyn analyzer generates:

- Strongly-typed unit enums
- Unit conversion (constructor, `.As()`, per-unit properties)
- Arithmetic and comparison operators (including cross-quantity operators like `Angle * Length → Length`)
- Fluent creation via extension properties (`10.0.Metre`, `45.0.Degree`)
- `ToString` formatting with unit symbols
- Scoped tolerance-aware equality and comparison

All code is emitted at compile time — no reflection, no runtime cost.

## Example

```csharp
using TedToolkit.Quantities;

// 1. Enable the generator for the quantities you want (typically in GlobalUsings.cs)
[assembly: Quantities<double>(
    QuantitySystems.ALL,
    "Angle",
    "Length",
    "DimensionlessRatio",
    Length = LengthUnit.Millimetre,
    Options = UnitOptions.GENERATE_EXTENSION_PROPERTIES)]

// 2. (Optional) Customize a quantity — e.g. set a display unit or add cross-quantity operators
[QuantityDisplayUnit<AngleUnit>(AngleUnit.Degree)]
[QuantityOperator<Angle, Angle, DimensionlessRatio>(Operator.DIVIDE)]
[QuantityOperator<Angle, Angle, Angle>(Operator.SUBTRACT)]
[QuantityOperator<Angle, Length, Length>(Operator.MULTIPLY)]
public partial struct Angle;

// 3. Use it
var right = 90.0.Degree;
var half  = right / 2.0;                           // 45°
double rad = right.As(AngleUnit.Radian);            // π/2
DimensionlessRatio r = right / 45.0.Degree;         // 2.0
Console.WriteLine(right.ToString(AngleUnit.Radian, "F4"));  // "1.5708 rad"
```

## Installation

```
dotnet add package TedToolkit.Quantities
```

**Supported targets:** .NET 6–10, .NET Framework 4.7.2 / 4.8, .NET Standard 2.0 / 2.1

## Solution Structure

| Project | Description |
|---|---|
| [`TedToolkit.Quantities`](TedToolkit.Quantities) | Public library and NuGet package |
| [`TedToolkit.Quantities.Analyzer`](TedToolkit.Quantities.Analyzer) | Roslyn incremental source generator (bundled in the NuGet package) |
| [`TedToolkit.Quantities.Data`](TedToolkit.Quantities.Data) | Data models for quantities, units, dimensions, and conversions |
| [`TedToolkit.Quantities.Generator`](TedToolkit.Quantities.Generator) | Console tool that extracts QUDT ontology data into JSON for the analyzer |
| [`TedToolkit.Quantities.Tests`](TedToolkit.Quantities.Tests) | Unit tests |
| [`Build`](Build) | CI/CD pipeline (ModularPipelines) |

## Quantity Systems

Unit data is sourced from the QUDT ontology and available for: **SI**, **CGS** (including EMU, ESU, Gauss variants), **Imperial**, **US Customary**, **ISQ**, and **Planck**.

## Building

```bash
dotnet build
dotnet test
```

## License

LGPL-3.0 — see [COPYING](COPYING) and [COPYING.LESSER](COPYING.LESSER).
