---
name: use-generated-quantities
description: >-
  Declare and consume units with TedToolkit.Quantities. Use for source-generated partial quantity structs, conversions, arithmetic, or tolerance-sensitive comparisons.
---

# Use Generated Quantities

Identify the physical dimension and input and output units first. Reuse the quantity type when the model already represents the dimension.

## Workflow

- Use the existing quantity type when it represents the physical dimension. For a new quantity, follow a local attributed partial struct and let the generator emit units, conversion members, and operators.
- Make the input unit explicit with a generated constructor or factory. Read the result in the unit needed at the boundary, for example create an `Angle` with `AngleUnit.Degree` and convert explicitly when radians are required.
- Use the appropriate tolerance only for comparisons that need it. Test a known conversion and check the result dimension of cross-quantity arithmetic.

## Example

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

## Check the result

When a member is missing, check the enabled systems and quantity list in the assembly declaration before adding helpers. Verify a known conversion, dimension-changing operator, and expected tolerance in the project’s numeric type. See [generator declarations](references/generator-declarations.md) when adding quantity configuration.
