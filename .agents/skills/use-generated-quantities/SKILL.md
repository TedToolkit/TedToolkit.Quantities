---
name: use-generated-quantities
description: >-
  Declare and consume units with TedToolkit.Quantities. Use for source-generated partial quantity structs, conversions, arithmetic, or tolerance-sensitive comparisons.
---

# Use Generated Quantities

- Use the existing quantity type when it represents the physical dimension. For a new quantity, follow a local attributed partial struct and let the generator emit units, conversion members, and operators.
- Make the input unit explicit with a generated constructor or factory. Read the result in the unit needed at the boundary, for example create an `Angle` with `AngleUnit.Degree` and convert explicitly when radians are required.
- Use the appropriate tolerance only for comparisons that need it. Test a known conversion and check the result dimension of cross-quantity arithmetic.
