# TedToolkit.Quantities.SI.Double

Precompiled double-precision quantity types from the repository's QUDT SI catalog, with additional
QUDT kinds such as `Thickness`, `Pressure`, and `ThermalConductance`. The package targets .NET
Standard 2.0 and .NET 7, matching the base library's generic math interface on .NET 7 and later.

```sh
dotnet add package TedToolkit.Quantities.SI.Double
```

```csharp
using TedToolkit.Quantities.SI.Double;

Length distance = Length.FromMillimetre(600);
Time elapsed = Time.FromSecond(30);
LinearVelocity speed = distance / elapsed;

Console.WriteLine(speed.MillimetrePerMinute); // 1200
```

Quantities are stored in coherent SI units. `Length.Value` is in metres, regardless of the unit used
to construct it. Use named factories and unit accessors such as `FromMillimetre`, `Millimetre`, or
`As(LengthUnit.Millimetre)` at API boundaries. Explicit casts to or from `double` use the coherent
value. The `None` member of each unit enum also selects the coherent unit, including quantities that
have no named unit in the catalog. Equality and ordering use the stored `double` values.
Unit conversions are generated from the multipliers and offsets in the QUDT data. A unit is exposed
when those parameters describe an invertible affine conversion.

The cross-quantity operators are declared in [QuantityRelations.cs](QuantityRelations.cs). The
generator checks the SI exponents of every declared relation. The operators use scalar values,
including aligned, normal, projected, and averaged components as specified in the coverage list.
Formulas that require a direction, geometry, or physical interpretation are available through
named methods in [ContextualFormulas.cs](ContextualFormulas.cs).

When adapting an API that stores millimetres, pass the value through `Length.FromMillimetre` and use
`Millimetre` on the way back to that API. The package types are under `TedToolkit.Quantities.SI.Double`,
so existing generated types in `TedToolkit.Quantities` can coexist in the same solution.

## Quantity conversions

The SI package generates a conversion for each ordered pair of distinct exposed types with
matching exponents for the seven SI base dimensions. A cast preserves the coherent SI value
and selects the quantity kind used to interpret it. For example, `(Torque)energy` preserves the
number. Calculating torque from a force and lever arm uses `Torque.FromPerpendicularLeverArm`.
Likewise, `Radius.FromDiameter` calculates half the diameter of a circle and
`AngularVelocity.FromFrequency` computes the angular velocity for one revolution per cycle.

`[QuantityImplicitlyConvertsTo<TTarget>]` in [QuantityWidenings.cs](QuantityWidenings.cs) makes a
validated same-dimension conversion implicit in the specific-to-general direction. The selected
directions are:

| Specialized quantity | General quantity |
| --- | --- |
| `Thickness`, `Width`, `Height`, `Depth`, `Radius`, `Diameter`, `Distance`, `Wavelength`, `PathLength` | `Length` |
| `GravitationalAttraction` | `Force` |
| `HeatFlowRate` | `Power` |
| `HeatFlowRatePerArea` | `PowerPerArea` |
| `Pressure`, scalar `Stress` | `ForcePerArea` |
| `Illuminance` | `LuminousFluxPerArea` |
| `ReynoldsNumber` | `DimensionlessRatio` |

```csharp
Thickness panel = Thickness.FromMillimetre(6);
Length length = panel;
Thickness measured = (Thickness)length;
Volume material = Area.FromSquareMetre(2) * panel;
```

The other same-dimension directions use explicit casts. Quantities of different dimensions
combine through the formulas below. [C# conversion operators](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators)

## Maintaining the formula inventory

Add operators with `QuantityOperator` and named calculations with `QuantityFormula`. One declaration
per physical relation supplies its formula ID, equation, applicability, and source links. Swapped
operands and solved forms inherit the documentation from their operator family. Use the same ID
for a named method or a different typed variant of that relation. Source links are reusable
constants in [FormulaSources.cs](FormulaSources.cs).

The `net7.0` build updates the marked formula coverage and reference sections below. GitHub Actions
checks that the committed README matches the attributes. The same check can run locally:

```sh
dotnet build TedToolkit.Quantities.SI.Double -c Release -f net7.0 -p:CheckFormulaReadme=true
```

<!-- BEGIN GENERATED FORMULA COVERAGE -->
## Formula coverage

The inventory has **114 rows**. Swapped products, solved forms, and typed variants share a row. Result types include the supported operators and named calculations.

### Geometry and algebra

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| A = πr² | `Area` | Area enclosed by a circle. Supported methods include `Area.FromCircle`. | [OpenStax area](https://openstax.org/books/contemporary-mathematics/pages/10-6-area) |
| C = 2πr | `Length` | Circumference of a circle. Supported methods include `Radius.ToCircumference`. | [OpenStax geometry](https://openstax.org/books/contemporary-mathematics/pages/10-4-polygons-perimeter-and-circumference) |
| d = 2r | `Diameter`, `Radius` | The radius and diameter refer to the same circle. Supported methods include `Diameter.FromRadius`, `Radius.FromDiameter`. | [OpenStax geometry](https://openstax.org/books/contemporary-mathematics/pages/10-4-polygons-perimeter-and-circumference) |
| s = rθ | `Length`, `PlaneAngle` | Circular arc with a positive radius. A signed central angle gives a signed arc length. Supported methods include `PlaneAngle.FromArc`, `PlaneAngle.ToArcLength`. | [OpenStax rotational variables](https://openstax.org/books/university-physics-volume-1/pages/10-1-rotational-variables) |
| Inverse length = 1 / ℓ | `DimensionlessRatio`, `InverseLength` | Use a nonzero length. The product with the corresponding inverse length is one. | [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes) |
| Length ratio = ℓ₂ / ℓ₁ | `DimensionlessRatio` | The reference length is nonzero. | [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |
| V = Aℓ | `Area`, `Length`, `Volume` | Prism or extrusion with uniform cross section perpendicular to its length. | [OpenStax prism volume](https://openstax.org/books/contemporary-mathematics/pages/10-7-volume-and-surface-area) |
| A = ab | `Area`, `Length` | Rectangular sides are perpendicular. The inverse solves for the other side. | [OpenStax area](https://openstax.org/books/contemporary-mathematics/pages/10-6-area) |
| Squared energy = E² | `SquareEnergy` | Algebraic square of the energy value. | [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |
| Squared time = t² | `SquareTime`, `Time` | Algebraic square of the elapsed time. | [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |

### Mechanics, rotation, and waves

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| Δθ = ω̄Δt | `AngularVelocity`, `PlaneAngle`, `Time` | Fixed axis and angular velocity averaged over the interval. | [OpenStax rotational variables](https://openstax.org/books/university-physics-volume-1/pages/10-1-rotational-variables) |
| ω = 2πf | `AngularVelocity`, `Frequency` | One revolution per cycle. Supported methods include `AngularVelocity.FromFrequency`, `AngularVelocity.ToFrequency`. | [OpenStax rotational variables](https://openstax.org/books/university-physics-volume-1/pages/10-1-rotational-variables) |
| L = Iω | `AngularMomentum`, `AngularVelocity`, `MomentOfInertia` | Rigid-body angular momentum component about the specified axis. | [OpenStax angular momentum](https://openstax.org/books/university-physics-volume-1/pages/11-summary) |
| ΔL = τ̄Δt | `AngularMomentum`, `Time`, `Torque` | Average torque and angular momentum change about the same axis. | [OpenStax angular momentum](https://openstax.org/books/university-physics-volume-1/pages/11-summary) |
| Δω = ᾱΔt | `AngularAcceleration`, `AngularVelocity`, `Time` | Fixed axis and angular acceleration averaged over the interval. | [OpenStax fixed-axis rotation](https://openstax.org/books/university-physics-volume-1/pages/10-key-equations) |
| m = ρₐA | `Area`, `Mass`, `MassPerArea` | Uniform or mean areal mass density ρₐ over the area. | [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |
| F = (F / ℓ) × ℓ | `Force`, `ForcePerLength`, `Length` | Uniform or mean force per length over the loaded span. | [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) · [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes) |
| u = E / V | `Energy`, `EnergyDensity`, `Volume` | Average energy per volume of the same region. | [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes) |
| E = P̄Δt | `Energy`, `Power`, `Time` | Constant or average power or heat-flow rate over the interval. | [OpenStax work and energy](https://openstax.org/books/university-physics-volume-1/pages/7-key-equations) · [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations) |
| Eₐ = E / A | `Area`, `Energy`, `EnergyPerArea` | Average energy per area over the relevant surface. | [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |
| F = ma | `Force`, `LinearAcceleration`, `Mass` | Net force along an axis for a constant-mass body. | [OpenStax Newton's laws](https://openstax.org/books/university-physics-volume-1/pages/5-key-equations) |
| J = Δp = F̄Δt | `Force`, `Impulse`, `LinearMomentum`, `Time` | Use the signed component of the average force over the interval. Supported methods include `Impulse.FromAverageForce`. | [OpenStax impulse and momentum](https://openstax.org/books/university-physics-volume-1/pages/9-key-equations) |
| τ = F⊥r⊥ | `Torque` | Use the force component in the rotation plane and its perpendicular lever arm. Supported methods include `Torque.FromPerpendicularLeverArm`. | [OpenStax fixed-axis rotation](https://openstax.org/books/university-physics-volume-1/pages/10-key-equations) |
| Δx = v̄Δt | `Length`, `LinearVelocity`, `Time` | Constant or average velocity component along the displacement axis. | [OpenStax kinematics](https://openstax.org/books/university-physics-volume-1/pages/3-key-equations) |
| m = λₘℓ | `Length`, `Mass`, `MassPerLength` | Uniform or mean linear mass density λₘ over the length. | [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |
| p = mv | `LinearMomentum`, `LinearVelocity`, `Mass` | Linear momentum component for the same mass and velocity. | [OpenStax impulse and momentum](https://openstax.org/books/university-physics-volume-1/pages/9-key-equations) |
| P = F∥v | `Force`, `LinearVelocity`, `Power` | Use the signed force component along the velocity axis. | [OpenStax work and energy](https://openstax.org/books/university-physics-volume-1/pages/7-key-equations) |
| W = F∥Δx | `Energy` | Constant force component along the displacement. Supported methods include `Energy.FromMechanicalWork`. | [OpenStax work and energy](https://openstax.org/books/university-physics-volume-1/pages/7-key-equations) |
| f = 1 / T | `Frequency`, `Time` | Period of one cycle of a periodic phenomenon. Supported methods include `Frequency.FromPeriod`, `Frequency.ToPeriod`. | [OpenStax periodic motion](https://openstax.org/books/university-physics-volume-1/pages/15-key-equations) |
| Pₐ = P / A | `Area`, `Power`, `PowerPerArea` | Average power per area over the same surface. | [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes) |
| K = ½Iω² | `Energy` | Rigid body rotating about a fixed axis. Supported methods include `Energy.FromRotationalKineticEnergy`. | [OpenStax fixed-axis rotation](https://openstax.org/books/university-physics-volume-1/pages/10-key-equations) |
| W = τ̄Δθ | `Energy`, `PlaneAngle`, `Torque` | Constant or angle-averaged torque along the rotation. | [OpenStax fixed-axis rotation](https://openstax.org/books/university-physics-volume-1/pages/10-key-equations) |
| P = τω | `AngularVelocity`, `Power`, `Torque` | Corresponding torque and angular velocity components about the same axis. | [OpenStax fixed-axis rotation](https://openstax.org/books/university-physics-volume-1/pages/10-key-equations) |
| e = E / m | `Energy`, `Mass`, `SpecificEnergy` | Energy and mass refer to the same body or sample. | [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes) |
| τ = Iα | `AngularAcceleration`, `MomentOfInertia`, `Torque` | Net torque and moment of inertia about the same fixed axis. | [OpenStax fixed-axis rotation](https://openstax.org/books/university-physics-volume-1/pages/10-key-equations) |
| K = ½mv² | `Energy` | Nonrelativistic translation. Supported methods include `Energy.FromTranslationalKineticEnergy`. | [OpenStax work and energy](https://openstax.org/books/university-physics-volume-1/pages/7-key-equations) |
| Δv = āΔt | `LinearAcceleration`, `LinearVelocity`, `Time` | Constant or average acceleration component over the interval. | [OpenStax kinematics](https://openstax.org/books/university-physics-volume-1/pages/3-key-equations) |
| v = λf | `Frequency`, `LinearVelocity`, `Wavelength` | Wavelength and frequency belong to the same propagating wave. | [OpenStax waves](https://openstax.org/books/university-physics-volume-1/pages/16-1-traveling-waves) |

### Fluids and mass flow

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| jₘ = ṁ / A = Δ(m/A) / Δt | `Area`, `MassPerArea`, `MassPerAreaTime`, `MassPerTime`, `Time` | Use the same oriented surface and interval for the areal mass flow rate jₘ. | [NASA mass flow](https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/mass-flow-rate/) · [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |
| ρ = m / V, vₛ = V / m = 1 / ρ | `Density`, `DimensionlessRatio`, `Mass`, `SpecificVolume`, `Volume` | Density and specific volume describe the same material at the same state. | [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations) · [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |
| q = ½ρv² | `Pressure` | Use local density and speed. Supported methods include `Pressure.FromDynamicPressure`. | [NASA dynamic pressure](https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/dynamic-pressure-2/) |
| P = ΔpQᵥ | `ForcePerArea`, `Power`, `Pressure`, `VolumePerTime` | Pressure difference and flow refer to the same path. Use pressures referenced to the same datum. Supported methods include `Pressure.FromHydraulicPower`. | [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations) · [OpenStax pressure–volume work](https://openstax.org/books/university-physics-volume-2/pages/3-key-equations) |
| Δp = ρgh | `Pressure` | Uniform-density fluid at rest below a reference level. Supported methods include `Pressure.FromHydrostaticHead`. | [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations) |
| ṁ = ρQᵥ | `Density`, `MassPerTime`, `VolumePerTime` | Use consistent density and volumetric flow at the same section. | [NASA mass flow](https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/mass-flow-rate/) |
| Δm = ṁ̄Δt | `Mass`, `MassPerTime`, `Time` | Constant or time-averaged mass flow rate over the interval. | [NASA mass flow](https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/mass-flow-rate/) |
| p = Fₙ / A | `Area`, `Force`, `ForcePerArea`, `Pressure` | Use the normal force over the surface. The general division returns `ForcePerArea`. Supported methods include `Pressure.FromNormalForce`. | [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations) |
| W = pₑₓₜΔV | `Energy` | Constant external pressure. Expansion gives positive work by the system. Supported methods include `Energy.FromConstantPressureWork`. | [OpenStax pressure–volume work](https://openstax.org/books/university-physics-volume-2/pages/3-key-equations) |
| Re = vℓ / ν = ρvℓ / μ | `ReynoldsNumber` | Choose the characteristic length for the flow and viscosity at that state. Supported methods include `ReynoldsNumber.FromFlow`. | [NASA Reynolds number](https://www.grc.nasa.gov/www/k-12/airplane/reynolds.html) |
| μ = ρν | `Density`, `DynamicViscosity`, `KinematicViscosity` | Density, dynamic viscosity, and kinematic viscosity at the same fluid state. | [NASA viscosity and flow](https://ntrs.nasa.gov/api/citations/20250001477/downloads/Paul%20Gradl_Dissertation_NASA_Title_2-11-2025.pdf) |
| Qᵥ = Avₙ | `Area`, `LinearVelocity`, `VolumePerTime` | Area-averaged normal speed component through the section. | [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations) · [NASA mass flow](https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/mass-flow-rate/) |
| ΔV = Q̄ᵥΔt | `Time`, `Volume`, `VolumePerTime` | Constant or time-averaged volumetric flow rate. | [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations) |

### Electricity and magnetism

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| Q = CV | `Capacitance`, `ElectricCharge`, `ElectricPotential` | Linear capacitor with the voltage across its terminals. | [OpenStax capacitance](https://openstax.org/books/university-physics-volume-2/pages/8-key-equations) |
| U = ½CV² | `Energy` | Energy stored in a linear capacitor. Supported methods include `Energy.FromCapacitor`. | [OpenStax capacitance](https://openstax.org/books/university-physics-volume-2/pages/8-key-equations) |
| ΔQ = ĪΔt | `ElectricCharge`, `ElectricCurrent`, `Time` | Use average electrical current through the same cross section and interval. | [OpenStax current and resistance](https://openstax.org/books/university-physics-volume-2/pages/9-key-equations) |
| Q = (Q / m)m | `ElectricCharge`, `ElectricChargePerMass`, `Mass` | Charge and mass refer to the same particles or sample. | [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |
| Q = (Q / n)n | `AmountOfSubstance`, `ElectricCharge`, `ElectricChargePerAmountOfSubstance` | Use charge per mole for the specified entities and process. | [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) · [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971) |
| ρₑ = 1 / σ | `DimensionlessRatio`, `ElectricConductivity`, `Resistivity` | Resistivity and conductivity refer to the same ohmic material and state. | [OpenStax resistivity](https://openstax.org/books/university-physics-volume-2/pages/9-3-resistivity-and-resistance) |
| I = JₙA | `Area`, `ElectricCurrent`, `ElectricCurrentDensity` | Uniform or area-averaged normal current density through the section. | [OpenStax current and resistance](https://openstax.org/books/university-physics-volume-2/pages/9-key-equations) |
| p = Qd | `ElectricCharge`, `ElectricDipoleMoment`, `Length` | Equal and opposite charges. Separation is directed from negative charge to positive charge. | [OpenStax electric fields](https://openstax.org/books/university-physics-volume-2/pages/5-key-equations) |
| D = εE | `ElectricDisplacement`, `ElectricFieldStrength`, `Permittivity` | Linear isotropic dielectric, using corresponding field components. | [MIT linear electromagnetic media](https://ocw.mit.edu/courses/18-325-topics-in-applied-mathematics-waves-and-imaging-fall-2015/c5db24b1a6d0d3301b1a3f21f6ceba3a_MIT18_325F15_CompleteLect.pdf) |
| Vdrop = E∥ℓ, ΔV = −E∥ℓ | `ElectricFieldStrength`, `ElectricPotential`, `Length` | Use the path-averaged field component and final-minus-initial voltage for ΔV. Supported methods include `ElectricFieldStrength.FromPotentialChange`. | [OpenStax electric potential](https://openstax.org/books/university-physics-volume-2/pages/7-key-equations) |
| Φₑ = EₙA | `Area`, `ElectricFieldStrength`, `ElectricFlux` | Uniform or area-averaged normal electric field on the surface. | [OpenStax electric fields](https://openstax.org/books/university-physics-volume-2/pages/5-key-equations) · [MIT linear electromagnetic media](https://ocw.mit.edu/courses/18-325-topics-in-applied-mathematics-waves-and-imaging-fall-2015/c5db24b1a6d0d3301b1a3f21f6ceba3a_MIT18_325F15_CompleteLect.pdf) |
| F = QE | `ElectricCharge`, `ElectricFieldStrength`, `Force` | Use the electric field component at the charge. | [OpenStax electric fields](https://openstax.org/books/university-physics-volume-2/pages/5-key-equations) |
| P = VI | `ElectricCurrent`, `ElectricPotential`, `Power` | Use corresponding instantaneous voltage and current. Use the passive sign convention for absorbed power. | [OpenStax current and resistance](https://openstax.org/books/university-physics-volume-2/pages/9-key-equations) · [OpenStax AC power](https://openstax.org/books/university-physics-volume-2/pages/15-key-equations) |
| ℰ = −ΔΛ / Δt | `ElectromotiveForce` | Signed final-minus-initial flux linkage summed over all turns. Supported methods include `ElectromotiveForce.FromFluxLinkageChange`. | [OpenStax Faraday's law](https://openstax.org/books/university-physics-volume-2/pages/13-key-equations) · [OpenStax RL circuits](https://openstax.org/books/university-physics-volume-2/pages/14-key-equations) |
| Q = λqℓ | `ElectricCharge`, `ElectricChargeLineDensity`, `Length` | Uniform or mean line charge density λq over the length. | [OpenStax electric fields](https://openstax.org/books/university-physics-volume-2/pages/5-key-equations) · [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |
| B = μH | `AuxillaryMagneticField`, `MagneticFluxDensity`, `Permeability` | Linear isotropic magnetic medium. The QUDT `AuxillaryMagneticField` type represents H. | [MIT linear electromagnetic media](https://ocw.mit.edu/courses/18-325-topics-in-applied-mathematics-waves-and-imaging-fall-2015/c5db24b1a6d0d3301b1a3f21f6ceba3a_MIT18_325F15_CompleteLect.pdf) |
| Φ = BₙA | `Area`, `MagneticFlux`, `MagneticFluxDensity` | Use the uniform or area-averaged normal flux density. | [OpenStax Faraday's law](https://openstax.org/books/university-physics-volume-2/pages/13-key-equations) |
| Φ = (Φ / ℓ)ℓ | `Length`, `MagneticFlux`, `MagneticFluxPerLength` | Uniform or mean magnetic flux per length over the interval. | [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |
| m = (m / Q)Q | `ElectricCharge`, `Mass`, `MassPerElectricCharge` | Mass and signed charge refer to the same charged entities. | [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |
| J = σE | `ElectricConductivity`, `ElectricCurrentDensity`, `ElectricFieldStrength` | Scalar conductivity in the local ohmic regime. Use corresponding components. | [OpenStax resistivity](https://openstax.org/books/university-physics-volume-2/pages/9-3-resistivity-and-resistance) |
| V = IR, I = GV, G = 1 / R | `Conductance`, `DimensionlessRatio`, `ElectricCurrent`, `ElectricPotential`, `Resistance` | Voltage and current refer to the same ohmic element or operating point. | [OpenStax current and resistance](https://openstax.org/books/university-physics-volume-2/pages/9-key-equations) · [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes) |
| C = εA / d | `Capacitance` | Parallel plates with uniform dielectric, using overlap area and negligible fringing. Supported methods include `Capacitance.FromParallelPlates`. | [OpenStax capacitance](https://openstax.org/books/university-physics-volume-2/pages/8-key-equations) |
| ΔU = QΔV | `ElectricCharge`, `ElectricPotential`, `Energy` | Change in electrical potential energy for charge moved through a fixed potential difference. | [OpenStax electric potential](https://openstax.org/books/university-physics-volume-2/pages/7-key-equations) |
| τ = RC | `Capacitance`, `Resistance`, `Time` | Time constant of the equivalent first-order RC circuit. | [OpenStax RC circuits](https://openstax.org/books/university-physics-volume-2/pages/10-key-equations) |
| τ = L / R | `Inductance`, `Resistance`, `Time` | Time constant of the equivalent first-order RL circuit. | [OpenStax RL circuits](https://openstax.org/books/university-physics-volume-2/pages/14-key-equations) |
| Q = σqA | `Area`, `ElectricCharge`, `ElectricChargePerArea` | Uniform or mean surface charge density σq over the area. | [OpenStax electric fields](https://openstax.org/books/university-physics-volume-2/pages/5-key-equations) · [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |
| R = ρₑℓ / A | `Resistance` | Uniform conductor with constant cross section and material resistivity. Supported methods include `Resistance.FromUniformConductor`. | [OpenStax resistivity](https://openstax.org/books/university-physics-volume-2/pages/9-3-resistivity-and-resistance) |
| Q = ρqV | `ElectricCharge`, `ElectricChargeVolumeDensity`, `Volume` | Uniform or mean volume charge density ρq over the region. | [MIT linear electromagnetic media](https://ocw.mit.edu/courses/18-325-topics-in-applied-mathematics-waves-and-imaging-fall-2015/c5db24b1a6d0d3301b1a3f21f6ceba3a_MIT18_325F15_CompleteLect.pdf) |

### Heat and temperature

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| R″ = ARₜ | `Area`, `ThermalInsulance`, `ThermalResistance` | Relate total thermal resistance to area-normalized resistance for the same uniform surface. | [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations) · [QUDT heat-transfer coefficient](https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer) |
| qₓ″ = −k∂T/∂x | `HeatFlowRatePerArea` | Signed temperature gradient along the axis with scalar conductivity. Supported methods include `HeatFlowRatePerArea.FromConduction`. | [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations) · [QUDT heat flow rate](https://qudt.org/vocab/quantitykind/HeatFlowRate) |
| Pheat = q″A | `Area`, `HeatFlowRate`, `HeatFlowRatePerArea` | Use average normal heat flux over the surface. | [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations) · [QUDT heat-transfer coefficient](https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer) |
| Gₜ = hA | `Area`, `CoefficientOfHeatTransfer`, `ThermalConductance` | Uniform or area-averaged heat-transfer coefficient for the same thermal path and driving temperature difference. | [QUDT heat-transfer coefficient](https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer) · [QUDT thermal conductance](https://qudt.org/vocab/quantitykind/ThermalConductance) |
| R″ = ℓ/k = ℓρₜ | `Length`, `ThermalConductivity`, `ThermalInsulance`, `ThermalResistivity` | Uniform plane layer. Use thickness along the heat-flow direction. | [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations) · [QUDT thermal resistivity](https://qudt.org/vocab/quantitykind/ThermalResistivity) |
| Q = CΔT | `Energy`, `HeatCapacity`, `TemperatureDifference` | Sensible heat with constant or interval-averaged heat capacity. | [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations) |
| C = mc | `HeatCapacity`, `Mass`, `SpecificHeatCapacity` | Use mass-specific heat capacity for the specified thermodynamic process. | [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations) |
| q″ = hΔT | `CoefficientOfHeatTransfer`, `HeatFlowRatePerArea`, `TemperatureDifference` | Transfer coefficient h is per unit area. Use the driving temperature difference in the indicated direction. | [QUDT heat-transfer coefficient](https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer) |
| ΔT = T₂ − T₁, T₂ = T₁ + ΔT | `TemperatureDifference`, `ThermodynamicTemperature` | Use the temperature difference as the increment of an absolute temperature. | [BIPM SI Brochure](https://www.bipm.org/en/publications/si-brochure) |
| ΔT = (dT/dt)̄Δt | `TemperatureDifference`, `TemperaturePerTime`, `Time` | Temperature-change rate is constant or averaged over the interval. | [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |
| ΔT = q″R″ | `HeatFlowRatePerArea`, `TemperatureDifference`, `ThermalInsulance` | Use areal thermal resistance and the corresponding heat-flux component. | [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations) · [QUDT heat-transfer coefficient](https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer) |
| ∂T/∂x ≈ ΔT / Δx | `Length`, `TemperatureDifference`, `TemperatureGradient` | Signed temperature difference over a distance along the axis. The quotient gives the interval average. | [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations) |
| α = k / cᵥ | `ThermalConductivity`, `ThermalDiffusivity`, `VolumetricHeatCapacity` | Thermal diffusivity for scalar conductivity and volumetric heat capacity. | [NIST TN 622](https://nvlpubs.nist.gov/nistpubs/Legacy/TN/nbstechnicalnote622.pdf) |
| ΔL = L′ΔT, ΔA = A′ΔT, ΔV = V′ΔT | `Area`, `Length`, `TemperatureDifference`, `Volume` | Use each whole object's dimensional expansion per kelvin, incorporating its initial size. The return is a change in size. | [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations) · [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |
| Pheat = ΔT / Rₜ = GₜΔT, Gₜ = 1 / Rₜ | `DimensionlessRatio`, `HeatFlowRate`, `TemperatureDifference`, `ThermalConductance`, `ThermalResistance` | Use total resistance or conductance for the same thermal path. A signed temperature drop gives the signed heat-flow rate. | [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations) · [QUDT thermal conductance](https://qudt.org/vocab/quantitykind/ThermalConductance) |
| ρₜ = 1 / k | `DimensionlessRatio`, `ThermalConductivity`, `ThermalResistivity` | Thermal resistivity and conductivity describe the same material and direction. | [QUDT thermal resistivity](https://qudt.org/vocab/quantitykind/ThermalResistivity) |
| cᵥ = ρc | `Density`, `SpecificHeatCapacity`, `VolumetricHeatCapacity` | Density and mass-specific heat capacity describe the same state. | [NIST TN 622](https://nvlpubs.nist.gov/nistpubs/Legacy/TN/nbstechnicalnote622.pdf) |
| C = Vcᵥ | `HeatCapacity`, `Volume`, `VolumetricHeatCapacity` | Uniform or volume-averaged volumetric heat capacity cᵥ. | [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes) |

### Photometry and radiometry

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| Eᵥ = Φᵥ,incident / A | `Area`, `Illuminance`, `LuminousFlux` | Incident luminous flux averaged over the receiver area. | [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf) · [NIST photometric equations 2–6](https://www.nist.gov/system/files/documents/calibrations/sp250-15.pdf) |
| Iᵥ = LᵥAproj | `Area`, `Luminance`, `LuminousIntensity` | Use projected emitting area as viewed from the specified direction. | [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf) |
| Φᵥ = ηᵥP | `LuminousEfficacy`, `LuminousFlux`, `Power` | Use the power basis for which the luminous efficacy was specified. | [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf) · [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |
| Qᵥ = Φ̄ᵥΔt | `LuminousEnergy`, `LuminousFlux`, `Time` | Constant or time-averaged luminous flux over the interval. | [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf) |
| Φᵥ = IᵥΩ | `LuminousFlux`, `LuminousIntensity`, `SolidAngle` | Use mean luminous intensity over the solid angle. | [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf) · [NIST photometric equations 2–6](https://www.nist.gov/system/files/documents/calibrations/sp250-15.pdf) |
| Eᵥ = Iᵥ / r² | `Illuminance` | Point source and receiving surface perpendicular to the incident ray. Supported methods include `Illuminance.FromNormalPointSource`. | [NIST photometric equations 2–6](https://www.nist.gov/system/files/documents/calibrations/sp250-15.pdf) |
| Φₑ = IₑΩ | `Power`, `RadiantIntensity`, `SolidAngle` | Use mean radiant intensity over the solid angle. | [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf) |

### Chemistry and radiation dose

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| D = Edeposited / m | `AbsorbedDose`, `Energy`, `Mass` | Mean energy deposited by ionizing radiation in the specified matter. Supported methods include `AbsorbedDose.FromDepositedEnergy`. | [IAEA dosimetry](https://www-pub.iaea.org/MTCD/Publications/PDF/te_1540_web.pdf) |
| ΔD = Ḋ̄Δt | `AbsorbedDose`, `AbsorbedDoseRate`, `Time` | Constant or interval-averaged absorbed dose rate for the same matter. | [IAEA dosimetry](https://www-pub.iaea.org/MTCD/Publications/PDF/te_1540_web.pdf) |
| c = n / V | `AmountOfSubstance`, `AmountOfSubstancePerVolume`, `Volume` | Amount of the constituent divided by the volume of the mixture. | [IUPAC amount concentration](https://goldbook.iupac.org/terms/view/A00295) |
| b = n / m | `AmountOfSubstance`, `AmountOfSubstancePerMass`, `Mass` | Use the reference mass for the specified amount-per-mass quantity, such as solvent mass when calculating molality. | [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) · [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971) |
| Δn = ācΔt | `AmountOfSubstance`, `Time` | Use the constant or time-averaged catalytic activity under the specified assay conditions. | [BIPM SI Brochure](https://www.bipm.org/en/publications/si-brochure) · [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4) |
| Eₘ = E / n | `AmountOfSubstance`, `Energy`, `MolarEnergy` | Use energy for the same specified amount of substance. | [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971) |
| Cₘ = C / n | `AmountOfSubstance`, `HeatCapacity`, `MolarHeatCapacity` | Use heat capacity for the same amount and thermodynamic process. | [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971) · [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations) |
| M = m / n | `AmountOfSubstance`, `Mass`, `MolarMass` | Mass and amount describe the same substance or specified entities. | [IUPAC molar mass](https://goldbook.iupac.org/terms/view/12214) |
| M = ρVₘ | `Density`, `MolarMass`, `MolarVolume` | Density and molar volume describe the same material at the same state. | [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971) · [QUDT molar volume](https://qudt.org/vocab/quantitykind/MolarVolume) |
| Vₘ = V / n | `AmountOfSubstance`, `MolarVolume`, `Volume` | Volume and amount refer to the same sample at the same state. | [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971) |

## Formula references

- [BIPM SI Brochure](https://www.bipm.org/en/publications/si-brochure)
- [IAEA dosimetry](https://www-pub.iaea.org/MTCD/Publications/PDF/te_1540_web.pdf)
- [IUPAC amount concentration](https://goldbook.iupac.org/terms/view/A00295)
- [IUPAC molar mass](https://goldbook.iupac.org/terms/view/12214)
- [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971)
- [MIT linear electromagnetic media](https://ocw.mit.edu/courses/18-325-topics-in-applied-mathematics-waves-and-imaging-fall-2015/c5db24b1a6d0d3301b1a3f21f6ceba3a_MIT18_325F15_CompleteLect.pdf)
- [NASA Reynolds number](https://www.grc.nasa.gov/www/k-12/airplane/reynolds.html)
- [NASA dynamic pressure](https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/dynamic-pressure-2/)
- [NASA mass flow](https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/mass-flow-rate/)
- [NASA viscosity and flow](https://ntrs.nasa.gov/api/citations/20250001477/downloads/Paul%20Gradl_Dissertation_NASA_Title_2-11-2025.pdf)
- [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes)
- [NIST TN 622](https://nvlpubs.nist.gov/nistpubs/Legacy/TN/nbstechnicalnote622.pdf)
- [NIST photometric equations 2–6](https://www.nist.gov/system/files/documents/calibrations/sp250-15.pdf)
- [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf)
- [OpenStax AC power](https://openstax.org/books/university-physics-volume-2/pages/15-key-equations)
- [OpenStax Faraday's law](https://openstax.org/books/university-physics-volume-2/pages/13-key-equations)
- [OpenStax Newton's laws](https://openstax.org/books/university-physics-volume-1/pages/5-key-equations)
- [OpenStax RC circuits](https://openstax.org/books/university-physics-volume-2/pages/10-key-equations)
- [OpenStax RL circuits](https://openstax.org/books/university-physics-volume-2/pages/14-key-equations)
- [OpenStax angular momentum](https://openstax.org/books/university-physics-volume-1/pages/11-summary)
- [OpenStax area](https://openstax.org/books/contemporary-mathematics/pages/10-6-area)
- [OpenStax capacitance](https://openstax.org/books/university-physics-volume-2/pages/8-key-equations)
- [OpenStax current and resistance](https://openstax.org/books/university-physics-volume-2/pages/9-key-equations)
- [OpenStax electric fields](https://openstax.org/books/university-physics-volume-2/pages/5-key-equations)
- [OpenStax electric potential](https://openstax.org/books/university-physics-volume-2/pages/7-key-equations)
- [OpenStax fixed-axis rotation](https://openstax.org/books/university-physics-volume-1/pages/10-key-equations)
- [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations)
- [OpenStax geometry](https://openstax.org/books/contemporary-mathematics/pages/10-4-polygons-perimeter-and-circumference)
- [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
- [OpenStax impulse and momentum](https://openstax.org/books/university-physics-volume-1/pages/9-key-equations)
- [OpenStax kinematics](https://openstax.org/books/university-physics-volume-1/pages/3-key-equations)
- [OpenStax periodic motion](https://openstax.org/books/university-physics-volume-1/pages/15-key-equations)
- [OpenStax pressure–volume work](https://openstax.org/books/university-physics-volume-2/pages/3-key-equations)
- [OpenStax prism volume](https://openstax.org/books/contemporary-mathematics/pages/10-7-volume-and-surface-area)
- [OpenStax resistivity](https://openstax.org/books/university-physics-volume-2/pages/9-3-resistivity-and-resistance)
- [OpenStax rotational variables](https://openstax.org/books/university-physics-volume-1/pages/10-1-rotational-variables)
- [OpenStax waves](https://openstax.org/books/university-physics-volume-1/pages/16-1-traveling-waves)
- [OpenStax work and energy](https://openstax.org/books/university-physics-volume-1/pages/7-key-equations)
- [QUDT heat flow rate](https://qudt.org/vocab/quantitykind/HeatFlowRate)
- [QUDT heat-transfer coefficient](https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer)
- [QUDT molar volume](https://qudt.org/vocab/quantitykind/MolarVolume)
- [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
- [QUDT thermal conductance](https://qudt.org/vocab/quantitykind/ThermalConductance)
- [QUDT thermal resistivity](https://qudt.org/vocab/quantitykind/ThermalResistivity)
<!-- END GENERATED FORMULA COVERAGE -->
