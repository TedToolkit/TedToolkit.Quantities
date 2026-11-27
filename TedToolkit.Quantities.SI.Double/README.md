# TedToolkit.Quantities.SI.Double

Precompiled double-precision quantities and typed formulas from the QUDT SI catalog.
The package uses the existing `TedToolkit.Quantities` generator with `QuantitySystems.SI` to expose
all 150 quantity kinds in the repository's SI catalog. It targets .NET Standard 2.0 and .NET 7.

```sh
dotnet add package TedToolkit.Quantities.SI.Double
```

```csharp
using TedToolkit.Quantities;

Length distance = Length.FromMillimetre(600);
Time elapsed = Time.FromSecond(30);
LinearVelocity speed = distance / elapsed;

Console.WriteLine(speed.MillimetrePerMinute); // 1200
```

Quantities are stored in coherent SI units. `Length.Value` is in metres, regardless of the unit used
to construct it. Use named factories and unit accessors such as `FromMillimetre`, `Millimetre`, or
`As(LengthUnit.Millimetre)` at API boundaries. Explicit casts to or from `double` use the coherent
value. Use `Value` for quantities without named units. Equality and ordering follow the base
library's `Tolerance` rules. Unit availability and conversions follow the QUDT data used by
the base generator. The SI base unit for `ThermodynamicTemperature` is the kelvin (K).
Use `DifferenceInKelvins` to get a signed temperature interval as a `double`. Use `AddKelvins`
to apply that interval to an absolute temperature.
[BIPM SI Brochure][bipm-si-brochure]

The QUDT SI catalog contains two dimension-one quantity kinds, `PlaneAngle` and `SolidAngle`,
with coherent units radian and steradian respectively. General ratios such as `Length / Length`
and the Reynolds number use `double`. A temperature interval or gradient also uses `double`,
with its unit stated in the method name, as in `DifferenceInKelvins` and `GradientInKelvinsPerMetre`.

The cross-quantity operators are declared with the existing `QuantityOperator` attribute in
[QuantityRelations.cs](QuantityRelations.cs). The operators use scalar values,
including aligned, normal, projected, and averaged components as specified in the coverage list.
Formulas that require a direction, geometry, or physical interpretation are available through
named methods in [ContextualFormulas.cs](ContextualFormulas.cs).

When adapting an API that stores millimetres, pass the value through `Length.FromMillimetre` and use
`Millimetre` on the way back to that API. The package provides types in `TedToolkit.Quantities`.
To migrate from source-generated quantities, replace the direct `TedToolkit.Quantities` package
reference with `TedToolkit.Quantities.SI.Double` and remove the assembly-level `Quantities<T>` declaration.

## Quantity conversions

The SI package defines a conversion for each ordered pair of distinct exposed types with
matching exponents for the seven SI base dimensions. A cast preserves the coherent SI value
and selects the quantity kind used to interpret it. For example, `(Torque)energy` preserves the
number. Calculating torque from a force and lever arm uses `Torque.FromPerpendicularLeverArm`.
Likewise, `Length.RadiusFromDiameter` calculates half the diameter of a circle and
`AngularVelocity.FromFrequency` computes the angular velocity for one revolution per cycle.

[QuantityConversions.cs](QuantityConversions.cs) defines the implicit and explicit casts in C#.
The package adds the following implicit conversions from specialized to general types.

| Specialized quantity | General quantity |
| --- | --- |
| `GravitationalAttraction` | `Force` |
| `HeatFlowRate` | `Power` |
| `HeatFlowRatePerArea` | `PowerPerArea` |
| Scalar `Stress` | `ForcePerArea` |
| `Illuminance` | `LuminousFluxPerArea` |

```csharp
GravitationalAttraction weight = (GravitationalAttraction)12d;
Force force = weight;
GravitationalAttraction restored = (GravitationalAttraction)force;
```

The base library also supplies implicit casts between the QUDT aliases `ElectricPotential`
and `EnergyPerElectricCharge`. The remaining same-dimension directions use explicit casts.
Quantities of different dimensions combine through the formulas below.
[C# conversion operators](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators)

## Formula coverage

The inventory lists **154 physical formulas**. Swapped products, solved forms, and typed variants
share a row. The conditions apply to each listed relation, and division requires a nonzero divisor.
Update the corresponding row when changing a formula's coverage.

### Geometry and algebra

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| A = 4πr² | `Area` | Total surface area of a sphere with positive radius. Supported methods include `Area.FromSphere`. | [OpenStax sphere geometry][openstax-sphere-geometry] |
| V = 4πr³ / 3 | `Volume` | Volume enclosed by a sphere with positive radius. Supported methods include `Volume.FromSphere`. | [OpenStax sphere geometry][openstax-sphere-geometry] |
| Ω = A / r² | `Area`, `SolidAngle` | The patch lies on a sphere centred at the point where the solid angle is measured. Use the sphere's positive radius. Supported methods include `SolidAngle.FromSphericalPatch` and `SolidAngle.ToSphericalPatchArea`. | [NIST SI units][nist-si-units] |
| Ā′ = ΔA / Δt | `Area`, `AreaPerTime`, `Time` | Signed change in area over the same interval. Use a constant or interval-averaged rate when recovering the change. | [QUDT area per time][qudt-area-per-time] |
| A = πr² | `Area` | Area enclosed by a circle with positive radius. Supported methods include `Area.FromCircle`. | [OpenStax area][openstax-area] |
| C = 2πr | `Length` | Circumference of a circle with positive radius. Supported methods include `Length.CircumferenceFromRadius`. | [OpenStax geometry][openstax-geometry] |
| d = 2r | `Length` | The radius and diameter refer to the same circle. Supported methods include `Length.DiameterFromRadius`, `Length.RadiusFromDiameter`. | [OpenStax geometry][openstax-geometry] |
| s = rθ | `Length`, `PlaneAngle` | Circular arc with a positive radius. A signed central angle gives a signed arc length. Supported methods include `PlaneAngle.FromArc`, `PlaneAngle.ToArcLength`. | [OpenStax rotational variables][openstax-rotational-variables] |
| κ = 1 / ℓ, r = κℓ | `InverseLength`, `double` | The reciprocal requires a nonzero length. The product is one when κ and ℓ are reciprocals. | [NIST SP 811][nist-sp-811] |
| Length ratio = ℓ₂ / ℓ₁ | `double` | Use a nonzero reference length. Both lengths use the same unit basis. | [QUDT quantity definitions][qudt-quantity-definitions] |
| V = Aℓ | `Area`, `Length`, `Volume` | Prism or extrusion with uniform cross section perpendicular to its length. | [OpenStax prism volume][openstax-prism-volume] |
| A = ab | `Area`, `Length` | Rectangular sides are perpendicular. The inverse solves for the other side. | [OpenStax area][openstax-area] |
| Energy product = E₁E₂ | `SquareEnergy` | Multiply two energy values. Equal operands give E². | [QUDT quantity definitions][qudt-quantity-definitions] |
| Time product = t₁t₂ | `SquareTime`, `Time` | Equal operands give t². Divide by a known nonzero factor to recover the other factor. | [QUDT quantity definitions][qudt-quantity-definitions] |

### Mechanics, rotation, and waves

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| I = mr² | `MomentOfInertia` | Point mass at perpendicular distance r from the axis. Supported methods include `MomentOfInertia.FromPointMass`. | [OpenStax moments of inertia][openstax-moments-of-inertia] |
| T = 2π√(m / k) | `Time` | Positive mass and stiffness in an undamped, linear, massless-spring system. Supported methods include `Time.FromMassSpring`. | [OpenStax periodic motion][openstax-periodic-motion] |
| T = 2π√(ℓ / g) | `Time` | Small-angle, undamped simple pendulum of positive length in uniform gravity. Supported methods include `Time.FromSimplePendulum`. | [OpenStax pendulums][openstax-pendulums] |
| U = −μm / r | `Energy` | Newtonian potential energy outside a spherical body, with zero energy at infinity. Use centre-to-centre distance r and the central body's gravitational parameter μ = GM. Supported methods include `Energy.FromGravitationalPotential`. | [OpenStax gravitational potential and escape][openstax-gravitational-potential] |
| vₑ = √(2μ / r) | `LinearVelocity` | Newtonian escape speed from radius r outside an isolated spherical body. Neglect the escaping body's mass. Supported methods include `LinearVelocity.EscapeSpeed`. | [OpenStax gravitational potential and escape][openstax-gravitational-potential] |
| Δx = v₀Δt + ½aΔt² | `Length` | Constant signed acceleration and initial velocity along the displacement axis. Use a nonnegative elapsed time. Supported methods include `Length.FromConstantAcceleration`. | [OpenStax kinematics][openstax-kinematics] |
| vₜ = rω | `AngularVelocity`, `Length`, `LinearVelocity` | Fixed-axis rotation. Radius is the perpendicular distance to the axis, and velocity is the signed tangential component. | [OpenStax circular motion][openstax-circular-motion] |
| aₜ = rα | `AngularAcceleration`, `Length`, `LinearAcceleration` | Fixed-axis rotation at a fixed radius. Acceleration is the signed tangential component. | [OpenStax circular motion][openstax-circular-motion] |
| a꜀ = vₜ² / r = rω² | `LinearAcceleration` | Circular motion at positive radius. Returns the inward acceleration magnitude. Supported methods include `LinearAcceleration.FromCentripetalMotion`. | [OpenStax circular motion][openstax-circular-motion] |
| Fₛ = −kx | `Force` | Linear spring with positive stiffness. Displacement is signed from equilibrium. Supported methods include `Force.FromSpringDisplacement`. | [OpenStax periodic motion][openstax-periodic-motion] |
| Uₛ = ½kx² | `Energy` | Elastic potential energy relative to equilibrium in the linear range of the spring. Supported methods include `Energy.FromSpringDisplacement`. | [OpenStax periodic motion][openstax-periodic-motion] |
| g = μ / r² | `LinearAcceleration` | Newtonian gravitational acceleration magnitude outside a spherical body. Use the body's gravitational parameter μ = GM and distance from its centre. Supported methods include `LinearAcceleration.FromGravitationalParameter`. | [OpenStax gravitation][openstax-gravitation] |
| v = √(μ / r) | `LinearVelocity` | Newtonian circular orbit with negligible orbiting mass. Use the central body's gravitational parameter and the orbital radius from its centre. Supported methods include `LinearVelocity.FromCircularOrbit`. | [OpenStax satellite orbits][openstax-satellite-orbits] |
| T = 2πr / v = 2π√(r³ / μ) | `Time` | Newtonian circular orbit with negligible orbiting mass. Use the central body's gravitational parameter and the orbital radius from its centre. Supported methods include `Time.FromCircularOrbit`. | [OpenStax satellite orbits][openstax-satellite-orbits] |
| Δθ = ω̄Δt | `AngularVelocity`, `PlaneAngle`, `Time` | Fixed axis and angular velocity averaged over the interval. | [OpenStax rotational variables][openstax-rotational-variables] |
| ω = 2πf | `AngularVelocity`, `Frequency` | One revolution per cycle. Supported methods include `AngularVelocity.FromFrequency`, `AngularVelocity.ToFrequency`. | [OpenStax rotational variables][openstax-rotational-variables] |
| L = Iω | `AngularMomentum`, `AngularVelocity`, `MomentOfInertia` | Rigid-body angular momentum component about the specified axis. | [OpenStax angular momentum][openstax-angular-momentum] |
| ΔL = τ̄Δt | `AngularMomentum`, `Time`, `Torque` | Average torque and angular momentum change about the same axis. | [OpenStax angular momentum][openstax-angular-momentum] |
| Δω = ᾱΔt | `AngularAcceleration`, `AngularVelocity`, `Time` | Fixed axis and angular acceleration averaged over the interval. | [OpenStax fixed-axis rotation][openstax-fixed-axis-rotation] |
| m = ρₐA | `Area`, `Mass`, `MassPerArea` | Uniform or mean areal mass density ρₐ over the area. | [QUDT quantity definitions][qudt-quantity-definitions] |
| F = (F / ℓ) × ℓ | `Force`, `ForcePerLength`, `Length` | Uniform or mean force per length over the loaded span. | [QUDT quantity definitions][qudt-quantity-definitions] · [NIST SP 811][nist-sp-811] |
| u = E / V | `Energy`, `EnergyDensity`, `Volume` | Average energy per volume of the same region. | [NIST SP 811][nist-sp-811] |
| E = P̄Δt | `Energy`, `Power`, `Time` | Constant or average power or heat-flow rate over the interval. | [OpenStax work and energy][openstax-work-and-energy] · [OpenStax heat and conduction][openstax-heat-and-conduction] |
| Eₐ = E / A | `Area`, `Energy`, `EnergyPerArea` | Average energy per area over the relevant surface. | [QUDT quantity definitions][qudt-quantity-definitions] |
| F = ma | `Force`, `LinearAcceleration`, `Mass` | Net force along an axis for a constant-mass body. | [OpenStax Newton's laws][openstax-newton-s-laws] |
| J = Δp = F̄Δt | `Force`, `LinearMomentum`, `Time` | Use the signed component of the average force over the interval. | [OpenStax impulse and momentum][openstax-impulse-and-momentum] |
| τ = F⊥r⊥ | `Torque` | Use the force component in the rotation plane and its perpendicular lever arm. Supported methods include `Torque.FromPerpendicularLeverArm`. | [OpenStax fixed-axis rotation][openstax-fixed-axis-rotation] |
| Δx = v̄Δt | `Length`, `LinearVelocity`, `Time` | Constant or average velocity component along the displacement axis. | [OpenStax kinematics][openstax-kinematics] |
| m = λₘℓ | `Length`, `Mass`, `MassPerLength` | Uniform or mean linear mass density λₘ over the length. | [QUDT quantity definitions][qudt-quantity-definitions] |
| p = mv | `LinearMomentum`, `LinearVelocity`, `Mass` | Linear momentum component for the same mass and velocity. | [OpenStax impulse and momentum][openstax-impulse-and-momentum] |
| P = F∥v | `Force`, `LinearVelocity`, `Power` | Use the signed force component along the velocity axis. | [OpenStax work and energy][openstax-work-and-energy] |
| W = F∥Δx | `Energy` | Constant force component along the displacement. Supported methods include `Energy.FromMechanicalWork`. | [OpenStax work and energy][openstax-work-and-energy] |
| f = 1 / T | `Frequency`, `Time` | Period of one cycle of a periodic phenomenon. Supported methods include `Frequency.FromPeriod`, `Frequency.ToPeriod`. | [OpenStax periodic motion][openstax-periodic-motion] |
| Pₐ = P / A | `Area`, `Power`, `PowerPerArea` | Average power per area over the same surface. | [NIST SP 811][nist-sp-811] |
| K = ½Iω² | `Energy` | Rigid body rotating about a fixed axis. Supported methods include `Energy.FromRotationalKineticEnergy`. | [OpenStax fixed-axis rotation][openstax-fixed-axis-rotation] |
| W = τ̄Δθ | `Energy`, `PlaneAngle`, `Torque` | Constant or angle-averaged torque along the rotation. | [OpenStax fixed-axis rotation][openstax-fixed-axis-rotation] |
| P = τω | `AngularVelocity`, `Power`, `Torque` | Corresponding torque and angular velocity components about the same axis. | [OpenStax fixed-axis rotation][openstax-fixed-axis-rotation] |
| e = E / m | `Energy`, `Mass`, `SpecificEnergy` | Energy and mass refer to the same body or sample. | [NIST SP 811][nist-sp-811] |
| τ = Iα | `AngularAcceleration`, `MomentOfInertia`, `Torque` | Net torque and moment of inertia about the same fixed axis. | [OpenStax fixed-axis rotation][openstax-fixed-axis-rotation] |
| K = ½mv² | `Energy` | Nonrelativistic translation. Supported methods include `Energy.FromTranslationalKineticEnergy`. | [OpenStax work and energy][openstax-work-and-energy] |
| Δv = āΔt | `LinearAcceleration`, `LinearVelocity`, `Time` | Constant or average acceleration component over the interval. | [OpenStax kinematics][openstax-kinematics] |
| v = λf | `Frequency`, `Length`, `LinearVelocity` | Wavelength and frequency belong to the same propagating wave. Supported methods include `Frequency.FromWaveSpeed`, `Length.WavelengthFromSpeed`, `LinearVelocity.FromWavelength`. | [OpenStax waves][openstax-waves] |

### Fluids and mass flow

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| λₘ = ρA | `Area`, `Density`, `MassPerLength` | Use cross-sectional area and the mean mass density over that section. Follows from m = ρV and V = Aℓ for a uniform section. | [OpenStax fluids, density, and pressure][openstax-fluids-density-and-pressure] · [QUDT quantity definitions][qudt-quantity-definitions] |
| ρₐ = ρℓ | `Density`, `Length`, `MassPerArea` | Use uniform thickness perpendicular to the surface and mean mass density through the thickness. Follows from m = ρV and V = Aℓ. | [OpenStax fluids, density, and pressure][openstax-fluids-density-and-pressure] · [QUDT quantity definitions][qudt-quantity-definitions] |
| τ = μΔv / Δy | `Stress` | Newtonian fluid with a linear profile across the gap. Use signed tangential velocity difference and positive normal separation. Supported methods include `Stress.FromNewtonianShear`. | [OpenStax viscosity and laminar flow][openstax-viscosity] |
| Qᵥ = πr⁴Δp / (8μℓ) | `VolumePerTime` | Steady, fully developed, incompressible Newtonian laminar flow in a straight, rigid, horizontal circular pipe with no wall slip. Use internal radius r and inlet minus outlet pressure across length ℓ. Supported methods include `VolumePerTime.FromLaminarPipeFlow`. | [OpenStax viscosity and laminar flow][openstax-viscosity] |
| jₘ = ṁ / A = Δ(m/A) / Δt | `Area`, `MassPerArea`, `MassPerAreaTime`, `MassPerTime`, `Time` | Use the same oriented surface and interval for the areal mass flow rate jₘ. | [NASA mass flow][nasa-mass-flow] · [QUDT quantity definitions][qudt-quantity-definitions] |
| ρ = m / V, vₛ = V / m = 1 / ρ | `Density`, `Mass`, `SpecificVolume`, `Volume`, `double` | Density and specific volume describe the same material at the same state. | [OpenStax fluid mechanics][openstax-fluid-mechanics] · [QUDT quantity definitions][qudt-quantity-definitions] |
| q = ½ρv² | `ForcePerArea` | Use local density and speed. Supported methods include `ForcePerArea.FromDynamicPressure`. | [NASA dynamic pressure][nasa-dynamic-pressure] |
| p₂ = p₁ + ½ρ(v₁² − v₂²) − ρg(z₂ − z₁) | `ForcePerArea` | Steady, incompressible, inviscid flow along a streamline. Use one pressure datum and signed elevation change from point 1 to point 2. Supported methods include `ForcePerArea.FromBernoulli`. | [OpenStax Bernoulli equation][openstax-bernoulli] |
| Fᵦ = ρgVdisplaced | `Force` | Upward buoyant force magnitude with uniform fluid density and gravity over the displaced volume. Supported methods include `Force.FromBuoyancy`. | [OpenStax buoyancy][openstax-buoyancy] |
| P = ΔpQᵥ | `ForcePerArea`, `Power`, `VolumePerTime` | Pressure difference and flow refer to the same path. Use pressures referenced to the same datum. Supported methods include `ForcePerArea.FromHydraulicPower`. | [OpenStax fluid mechanics][openstax-fluid-mechanics] · [OpenStax pressure–volume work][openstax-pressure-volume-work] |
| Δp = ρgh | `ForcePerArea` | Static fluid with uniform density and constant gravity over the vertical depth below the reference level. Supported methods include `ForcePerArea.FromHydrostaticHead`. | [OpenStax fluids, density, and pressure][openstax-fluids-density-and-pressure] |
| ṁ = ρQᵥ | `Density`, `MassPerTime`, `VolumePerTime` | Use consistent density and volumetric flow at the same section. | [NASA mass flow][nasa-mass-flow] |
| Δm = ṁ̄Δt | `Mass`, `MassPerTime`, `Time` | Constant or time-averaged mass flow rate over the interval. | [NASA mass flow][nasa-mass-flow] |
| p = Fₙ / A | `Area`, `Force`, `ForcePerArea` | Use the normal force over the surface. The result is the average pressure represented by ForcePerArea. Supported methods include `ForcePerArea.FromNormalForce`. | [OpenStax fluid mechanics][openstax-fluid-mechanics] |
| W = pₑₓₜΔV | `Energy` | Constant external pressure. Expansion gives positive work by the system. Supported methods include `Energy.FromConstantPressureWork`. | [OpenStax pressure–volume work][openstax-pressure-volume-work] |
| Re = vℓ / ν = ρvℓ / μ | `double` | Choose the characteristic length for the flow and viscosity at that state. Supported methods include `LinearVelocity.ReynoldsNumber`. | [NASA Reynolds number][nasa-reynolds-number] |
| μ = ρν | `Density`, `DynamicViscosity`, `KinematicViscosity` | Density, dynamic viscosity, and kinematic viscosity at the same fluid state. | [NASA viscosity and flow][nasa-viscosity-and-flow] |
| Qᵥ = Avₙ | `Area`, `LinearVelocity`, `VolumePerTime` | Area-averaged normal speed component through the section. | [OpenStax fluid mechanics][openstax-fluid-mechanics] · [NASA mass flow][nasa-mass-flow] |
| ΔV = Q̄ᵥΔt | `Time`, `Volume`, `VolumePerTime` | Constant or time-averaged volumetric flow rate. | [OpenStax fluid mechanics][openstax-fluid-mechanics] |

### Electricity and magnetism

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| Fᵣ = q₁q₂ / (4πεr²) | `Force` | Two point charges in an unbounded homogeneous, isotropic linear dielectric. The radial force is positive for repulsion. Supported methods include `Force.FromPointCharges`. | [OpenStax electric fields][openstax-electric-fields] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| Eᵣ = q / (4πεr²) | `ElectricFieldStrength` | Point charge in an unbounded homogeneous, isotropic linear dielectric. Positive field points radially outward. Supported methods include `ElectricFieldStrength.FromPointCharge`. | [OpenStax electric fields][openstax-electric-fields] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| V = q / (4πεr) | `ElectricPotential` | Point charge in an unbounded homogeneous, isotropic linear dielectric, with zero potential at infinity. Supported methods include `ElectricPotential.FromPointCharge`. | [OpenStax point-charge potential][openstax-point-charge-potential] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| P = I²R = V² / R | `Power` | Joule dissipation in a positive ohmic resistance. Use current through or voltage across the same resistor. Supported methods include `Power.FromResistiveHeating`. | [OpenStax electrical power][openstax-electrical-power] |
| B = μNI / ℓ | `MagneticFluxDensity` | Axial field near the centre of a long, uniformly wound solenoid in a uniform linear medium. Use signed current relative to the winding orientation. Supported methods include `MagneticFluxDensity.FromLongSolenoid`. | [OpenStax solenoids][openstax-solenoids] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| τ = pE sin θ | `Torque` | Torque magnitude on an electric dipole in a uniform field. θ is the angle between dipole moment and field. Supported methods include `Torque.FromElectricDipole`. | [OpenStax electric dipoles][openstax-electric-dipoles] |
| τ = mB sin θ | `Torque` | Torque magnitude on a magnetic dipole in a uniform field. θ is the angle between dipole moment and flux density. Supported methods include `Torque.FromMagneticDipole`. | [OpenStax magnetic dipole torque][openstax-magnetic-dipole-torque] |
| Q = CV | `Capacitance`, `ElectricCharge`, `ElectricPotential` | Linear capacitor with the voltage across its terminals. | [OpenStax capacitance][openstax-capacitance] |
| U = ½CV² | `Energy` | Energy stored in a linear capacitor. Supported methods include `Energy.FromCapacitor`. | [OpenStax capacitance][openstax-capacitance] |
| ΔQ = ĪΔt | `ElectricCharge`, `ElectricCurrent`, `Time` | Use average electrical current through the same cross section and interval. | [OpenStax current and resistance][openstax-current-and-resistance] |
| Q = (Q / m)m | `ElectricCharge`, `ElectricChargePerMass`, `Mass` | Charge and mass refer to the same particles or sample. | [QUDT quantity definitions][qudt-quantity-definitions] |
| Q = (Q / n)n | `AmountOfSubstance`, `ElectricCharge`, `ElectricChargePerAmountOfSubstance` | Use charge per mole for the specified entities and process. | [QUDT quantity definitions][qudt-quantity-definitions] · [IUPAC molar quantities][iupac-molar-quantities] |
| I = JₙA | `Area`, `ElectricCurrent`, `ElectricCurrentDensity` | Uniform or area-averaged normal current density through the section. | [OpenStax current and resistance][openstax-current-and-resistance] |
| p = Qd | `ElectricCharge`, `ElectricDipoleMoment`, `Length` | Equal and opposite charges. Separation is directed from negative charge to positive charge. | [OpenStax electric fields][openstax-electric-fields] |
| D = εE | `ElectricDisplacement`, `ElectricFieldStrength`, `Permittivity` | Linear isotropic dielectric, using corresponding field components. | [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| Vdrop = E∥ℓ, ΔV = −E∥ℓ | `ElectricFieldStrength`, `ElectricPotential`, `Length` | Use the path-averaged field component and final-minus-initial voltage for ΔV. Supported methods include `ElectricFieldStrength.FromPotentialChange`. | [OpenStax electric potential][openstax-electric-potential] |
| Φₑ = EₙA | `Area`, `ElectricFieldStrength`, `ElectricFlux` | Uniform or area-averaged normal electric field on the surface. | [OpenStax electric fields][openstax-electric-fields] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| F = QE | `ElectricCharge`, `ElectricFieldStrength`, `Force` | Use the electric field component at the charge. | [OpenStax electric fields][openstax-electric-fields] |
| P = VI | `ElectricCurrent`, `ElectricPotential`, `Power` | Use corresponding instantaneous voltage and current. Use the passive sign convention for absorbed power. | [OpenStax current and resistance][openstax-current-and-resistance] · [OpenStax AC power][openstax-ac-power] |
| ℰ = −ΔΛ / Δt | `ElectromotiveForce` | Signed final-minus-initial flux linkage summed over all turns. Supported methods include `ElectromotiveForce.FromFluxLinkageChange`. | [OpenStax Faraday's law][openstax-faraday-s-law] · [OpenStax RL circuits][openstax-rl-circuits] |
| Q = λqℓ | `ElectricCharge`, `ElectricChargeLineDensity`, `Length` | Uniform or mean line charge density λq over the length. | [OpenStax electric fields][openstax-electric-fields] · [QUDT quantity definitions][qudt-quantity-definitions] |
| B = μH | `AuxillaryMagneticField`, `MagneticFluxDensity`, `Permeability` | Linear isotropic magnetic medium. The QUDT `AuxillaryMagneticField` type represents H. | [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| Φ = BₙA | `Area`, `MagneticFlux`, `MagneticFluxDensity` | Use the uniform or area-averaged normal flux density. | [OpenStax Faraday's law][openstax-faraday-s-law] |
| Φ = (Φ / ℓ)ℓ | `Length`, `MagneticFlux`, `MagneticFluxPerLength` | Uniform or mean magnetic flux per length over the interval. | [QUDT quantity definitions][qudt-quantity-definitions] |
| m = (m / Q)Q | `ElectricCharge`, `Mass`, `MassPerElectricCharge` | Mass and signed charge refer to the same charged entities. | [QUDT quantity definitions][qudt-quantity-definitions] |
| J = σE | `ElectricConductivity`, `ElectricCurrentDensity`, `ElectricFieldStrength` | Scalar conductivity in the local ohmic regime. Use corresponding components. | [OpenStax resistivity][openstax-resistivity] |
| V = IR | `ElectricCurrent`, `ElectricPotential`, `Resistance` | Voltage and current refer to the same ohmic element or operating point. | [OpenStax current and resistance][openstax-current-and-resistance] · [NIST SP 811][nist-sp-811] |
| C = εA / d | `Capacitance` | Parallel plates with uniform dielectric, using overlap area and negligible fringing. Supported methods include `Capacitance.FromParallelPlates`. | [OpenStax capacitance][openstax-capacitance] |
| ΔU = QΔV | `ElectricCharge`, `ElectricPotential`, `Energy` | Change in electrical potential energy for charge moved through a fixed potential difference. | [OpenStax electric potential][openstax-electric-potential] |
| τ = RC | `Capacitance`, `Resistance`, `Time` | Time constant of the equivalent first-order RC circuit. | [OpenStax RC circuits][openstax-rc-circuits] |
| τ = L / R | `Inductance`, `Resistance`, `Time` | Time constant of the equivalent first-order RL circuit. | [OpenStax RL circuits][openstax-rl-circuits] |
| Λ = LI | `MagneticFlux` | Signed self-flux linkage summed over all turns of a linear inductor. Supported methods include `MagneticFlux.LinkageFromInductor`. | [OpenStax self-inductance][openstax-self-inductance] |
| U = ½LI² | `Energy` | Magnetic energy stored in a linear inductor of fixed inductance. Supported methods include `Energy.FromInductor`. | [OpenStax magnetic energy][openstax-magnetic-energy] |
| ω₀ = 1 / √(LC), f₀ = ω₀ / (2π) | `AngularVelocity`, `Frequency` | Ideal lossless LC circuit with positive inductance and capacitance. Supported methods include `AngularVelocity.FromLcResonance` and `Frequency.FromLcResonance`. | [OpenStax LC oscillations][openstax-lc-oscillations] |
| F = abs(q)vB⊥ | `Force` | Magnetic force magnitude. B⊥ is the magnitude of the field component perpendicular to particle velocity. Supported methods include `Force.FromMovingChargeInMagneticField`. | [OpenStax magnetic forces][openstax-magnetic-forces] |
| F = abs(I)ℓB⊥ | `Force` | Magnetic force magnitude on a straight conductor segment in a uniform field. Use the active length and field component perpendicular to the segment. Supported methods include `Force.FromCurrentInMagneticField`. | [OpenStax magnetic forces][openstax-magnetic-forces] |
| Q = σqA | `Area`, `ElectricCharge`, `ElectricChargePerArea` | Uniform or mean surface charge density σq over the area. | [OpenStax electric fields][openstax-electric-fields] · [QUDT quantity definitions][qudt-quantity-definitions] |
| R = ℓ / (σA) | `Resistance` | Uniform conductor with constant cross section and positive electrical conductivity σ. Supported methods include `Resistance.FromUniformConductor`. | [OpenStax resistivity][openstax-resistivity] |
| Q = ρqV | `ElectricCharge`, `ElectricChargeVolumeDensity`, `Volume` | Uniform or mean volume charge density ρq over the region. | [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |

### Heat and temperature

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| q″ = εσ(Tₛ⁴ − Tₑ⁴) | `HeatFlowRatePerArea` | Net outward flux from a grey surface surrounded by a large black isothermal enclosure. Use absolute temperatures, emissivity from zero to one, and a supplied Stefan–Boltzmann constant. Supported methods include `HeatFlowRatePerArea.FromThermalRadiation`. | [OpenStax radiative heat transfer][openstax-radiative-heat-transfer] |
| p₂ = p₁(V₁ / V₂)(T₂ / T₁) | `ForcePerArea` | Two equilibrium states of a fixed amount of ideal gas. Use absolute pressures and absolute temperatures. Supported methods include `ForcePerArea.AfterIdealGasStateChange`. | [OpenStax ideal gas law][openstax-ideal-gas-law] |
| R″ = ARₜ | `Area`, `ThermalInsulance`, `ThermalResistance` | Relate total thermal resistance to area-normalized resistance for the same uniform surface. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient] |
| qₓ″ = −k ΔT / Δx | `HeatFlowRatePerArea` | Use scalar conductivity and the signed average gradient between the two temperatures along the axis. Supported methods include `HeatFlowRatePerArea.FromConduction`. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT heat flow rate][qudt-heat-flow-rate] |
| Pheat = q″A | `Area`, `HeatFlowRate`, `HeatFlowRatePerArea` | Use average normal heat flux over the surface. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient] |
| Rₜ = 1 / (hA) | `ThermalResistance` | Use a positive uniform or area-averaged heat-transfer coefficient h and the area A of the same surface. Supported methods include `ThermalResistance.FromHeatTransferCoefficient`. | [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient] · [QUDT thermal conductance][qudt-thermal-conductance] |
| R″ = ℓ/k = ℓρₜ | `Length`, `ThermalConductivity`, `ThermalInsulance`, `ThermalResistivity` | Uniform plane layer. Use thickness along the heat-flow direction. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT thermal resistivity][qudt-thermal-resistivity] |
| Q = CΔT | `Energy`, `HeatCapacity` | Sensible heat with constant or interval-averaged heat capacity. Supported methods include `HeatCapacity.EnergyForTemperatureChange`, `HeatCapacity.FromEnergyAndTemperatureChange`. | [OpenStax heat and conduction][openstax-heat-and-conduction] |
| C = mc | `HeatCapacity`, `Mass`, `SpecificHeatCapacity` | Use mass-specific heat capacity for the specified thermodynamic process. | [OpenStax heat and conduction][openstax-heat-and-conduction] |
| q″ = hΔT | `HeatFlowRatePerArea` | Transfer coefficient h is per unit area. Use the driving temperature difference in the indicated direction. Supported methods include `CoefficientOfHeatTransfer.HeatFluxForTemperatureChange`. | [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient] |
| ΔT = T₂ − T₁, T₂ = T₁ + ΔT | `ThermodynamicTemperature`, `double` | Subtract two absolute temperatures to get a signed difference in kelvins. Add that difference to an absolute temperature to get another absolute temperature. Supported methods include `ThermodynamicTemperature.AddKelvins`, `ThermodynamicTemperature.DifferenceInKelvins`. | [BIPM SI Brochure][bipm-si-brochure] |
| ΔT = (dT/dt)̄Δt | `double` | Temperature-change rate is constant or averaged over the interval. Supported methods include `TemperaturePerTime.ChangeInKelvins`. | [QUDT quantity definitions][qudt-quantity-definitions] |
| ΔT = q″R″ | `double` | Use areal thermal resistance and the corresponding heat-flux component. Supported methods include `ThermalInsulance.TemperatureDropInKelvins`. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient] |
| ∂T/∂x ≈ ΔT / Δx | `double` | Signed temperature difference over a distance along the axis. The quotient gives the interval average. Supported methods include `ThermodynamicTemperature.GradientInKelvinsPerMetre`. | [OpenStax heat and conduction][openstax-heat-and-conduction] |
| α = k / cᵥ | `ThermalConductivity`, `ThermalDiffusivity`, `VolumetricHeatCapacity` | Thermal diffusivity for scalar conductivity and volumetric heat capacity. | [NIST TN 622][nist-tn-622] |
| ΔL = L′ΔT, ΔA = A′ΔT, ΔV = V′ΔT | `Area`, `Length`, `Volume` | Use the object's average dimensional expansion per kelvin over the interval, or the local slope for a small temperature change. The result is the change in size. Supported methods include `AreaThermalExpansion.AreaChange`, `LinearThermalExpansion.LengthChange`, `VolumeThermalExpansion.VolumeChange`. | [OpenStax thermal expansion][openstax-thermal-expansion] · [QUDT quantity definitions][qudt-quantity-definitions] |
| Pheat = ΔT / Rₜ | `HeatFlowRate`, `ThermalResistance`, `double` | Use total thermal resistance and a signed temperature drop in kelvins for the same thermal path. Supported methods include `ThermalResistance.FromTemperatureDrop`, `ThermalResistance.HeatFlowForTemperatureChange`, `ThermalResistance.TemperatureDropInKelvins`. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT thermal conductance][qudt-thermal-conductance] |
| ρₜ = 1 / k | `ThermalConductivity`, `ThermalResistivity`, `double` | Thermal resistivity and conductivity describe the same material and direction. | [QUDT thermal resistivity][qudt-thermal-resistivity] |
| cᵥ = ρc | `Density`, `SpecificHeatCapacity`, `VolumetricHeatCapacity` | Density and mass-specific heat capacity describe the same state. | [NIST TN 622][nist-tn-622] |
| C = Vcᵥ | `HeatCapacity`, `Volume`, `VolumetricHeatCapacity` | Uniform or volume-averaged volumetric heat capacity cᵥ. | [NIST SP 811][nist-sp-811] |

### Photometry and radiometry

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| Eᵥ = Φᵥ,incident / A | `Area`, `Illuminance`, `LuminousFlux` | Incident luminous flux averaged over the receiver area. | [NIST photometric quantities][nist-photometric-quantities] · [NIST photometric equations 2–6][nist-photometric-equations-2-6] |
| Iᵥ = LᵥAproj | `Area`, `Luminance`, `LuminousIntensity` | Use projected emitting area as viewed from the specified direction. | [NIST photometric quantities][nist-photometric-quantities] |
| Φᵥ = ηᵥP | `LuminousEfficacy`, `LuminousFlux`, `Power` | Use the power basis for which the luminous efficacy was specified. | [NIST photometric quantities][nist-photometric-quantities] · [QUDT quantity definitions][qudt-quantity-definitions] |
| Qᵥ = Φ̄ᵥΔt | `LuminousEnergy`, `LuminousFlux`, `Time` | Constant or time-averaged luminous flux over the interval. | [NIST photometric quantities][nist-photometric-quantities] |
| Φᵥ = IᵥΩ | `LuminousFlux`, `LuminousIntensity`, `SolidAngle` | Use mean luminous intensity over the solid angle. | [NIST photometric quantities][nist-photometric-quantities] · [NIST photometric equations 2–6][nist-photometric-equations-2-6] |
| Eᵥ = Iᵥ / r² | `Illuminance` | Point source and receiving surface perpendicular to the incident ray. Supported methods include `Illuminance.FromNormalPointSource`. | [NIST photometric equations 2–6][nist-photometric-equations-2-6] |
| Eₑ = Iₑ / r² | `PowerPerArea` | Point source, positive distance and normal incidence. Use the radiant intensity in the direction of the receiver. From the CIE definitions and dΩ = dA / r² at normal incidence. Supported methods include `PowerPerArea.FromNormalPointSource`. | [CIE irradiance][cie-irradiance] · [CIE radiant intensity][cie-radiant-intensity] · [NIST photometric equations 2–6][nist-photometric-equations-2-6] |
| Φₑ = IₑΩ | `Power`, `RadiantIntensity`, `SolidAngle` | Use mean radiant intensity over the solid angle. | [NIST photometric quantities][nist-photometric-quantities] |

### Chemistry and radiation dose

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| A(t) = A₀ × 0.5^(t / t½) | `Activity` | Single radionuclide with positive half-life, starting activity A₀ and nonnegative elapsed time. The nuclide has no other source or removal process. Supported methods include `Activity.AfterDecay`. | [OpenStax radioactive decay][openstax-radioactive-decay] |
| D = Edeposited / m | `AbsorbedDose`, `Energy`, `Mass` | Mean energy deposited by ionizing radiation in the specified matter. Supported methods include `AbsorbedDose.FromDepositedEnergy`. | [IAEA dosimetry][iaea-dosimetry] |
| ΔD = Ḋ̄Δt | `AbsorbedDose`, `AbsorbedDoseRate`, `Time` | Constant or interval-averaged absorbed dose rate for the same matter. | [IAEA dosimetry][iaea-dosimetry] |
| c = n / V | `AmountOfSubstance`, `AmountOfSubstancePerVolume`, `Volume` | Amount of the constituent divided by the volume of the mixture. | [IUPAC amount concentration][iupac-amount-concentration] |
| b = n / m | `AmountOfSubstance`, `AmountOfSubstancePerMass`, `Mass` | Use the reference mass for the specified amount-per-mass quantity, such as solvent mass when calculating molality. | [QUDT quantity definitions][qudt-quantity-definitions] · [IUPAC molar quantities][iupac-molar-quantities] |
| Δn = ācΔt | `AmountOfSubstance`, `Time` | Use constant or time-averaged catalytic activity for the specified indicator reaction and assay conditions. Δn is the amount converted due to the catalyst. | [BIPM katal resolution][bipm-katal-resolution] · [QUDT quantity definitions][qudt-quantity-definitions] |
| Eₘ = E / n | `AmountOfSubstance`, `Energy`, `MolarEnergy` | Use energy for the same specified amount of substance. | [IUPAC molar quantities][iupac-molar-quantities] |
| Cₘ = C / n | `AmountOfSubstance`, `HeatCapacity`, `MolarHeatCapacity` | Use heat capacity for the same amount and thermodynamic process. | [IUPAC molar quantities][iupac-molar-quantities] · [OpenStax heat and conduction][openstax-heat-and-conduction] |
| M = m / n | `AmountOfSubstance`, `Mass`, `MolarMass` | Mass and amount describe the same substance or specified entities. | [IUPAC molar mass][iupac-molar-mass] |
| M = ρVₘ | `Density`, `MolarMass`, `MolarVolume` | Density and molar volume describe the same material at the same state. | [IUPAC molar quantities][iupac-molar-quantities] · [QUDT molar volume][qudt-molar-volume] |
| c = ρ / M | `AmountOfSubstancePerVolume`, `Density`, `MolarMass` | For a pure substance, use the density and molar mass of the same substance at the same state. Follows from ρ = m / V and M = m / n. | [IUPAC amount concentration][iupac-amount-concentration] · [IUPAC molar mass][iupac-molar-mass] · [OpenStax fluids, density, and pressure][openstax-fluids-density-and-pressure] |
| Vₘ = V / n | `AmountOfSubstance`, `MolarVolume`, `Volume` | Volume and amount refer to the same sample at the same state. | [IUPAC molar quantities][iupac-molar-quantities] |

## Formula references

- [BIPM SI Brochure][bipm-si-brochure]
- [BIPM katal resolution][bipm-katal-resolution]
- [CIE irradiance][cie-irradiance]
- [CIE radiant intensity][cie-radiant-intensity]
- [IAEA dosimetry][iaea-dosimetry]
- [IUPAC amount concentration][iupac-amount-concentration]
- [IUPAC molar mass][iupac-molar-mass]
- [IUPAC molar quantities][iupac-molar-quantities]
- [MIT linear electromagnetic media][mit-linear-electromagnetic-media]
- [NASA Reynolds number][nasa-reynolds-number]
- [NASA dynamic pressure][nasa-dynamic-pressure]
- [NASA mass flow][nasa-mass-flow]
- [NASA viscosity and flow][nasa-viscosity-and-flow]
- [NIST SP 811][nist-sp-811]
- [NIST TN 622][nist-tn-622]
- [NIST photometric equations 2–6][nist-photometric-equations-2-6]
- [NIST photometric quantities][nist-photometric-quantities]
- [OpenStax AC power][openstax-ac-power]
- [OpenStax Faraday's law][openstax-faraday-s-law]
- [OpenStax Newton's laws][openstax-newton-s-laws]
- [OpenStax RC circuits][openstax-rc-circuits]
- [OpenStax RL circuits][openstax-rl-circuits]
- [OpenStax angular momentum][openstax-angular-momentum]
- [OpenStax area][openstax-area]
- [OpenStax Bernoulli equation][openstax-bernoulli]
- [OpenStax buoyancy][openstax-buoyancy]
- [OpenStax capacitance][openstax-capacitance]
- [OpenStax circular motion][openstax-circular-motion]
- [OpenStax current and resistance][openstax-current-and-resistance]
- [OpenStax electric fields][openstax-electric-fields]
- [OpenStax electric potential][openstax-electric-potential]
- [OpenStax fixed-axis rotation][openstax-fixed-axis-rotation]
- [OpenStax fluid mechanics][openstax-fluid-mechanics]
- [OpenStax fluids, density, and pressure][openstax-fluids-density-and-pressure]
- [OpenStax geometry][openstax-geometry]
- [OpenStax gravitation][openstax-gravitation]
- [OpenStax heat and conduction][openstax-heat-and-conduction]
- [OpenStax impulse and momentum][openstax-impulse-and-momentum]
- [OpenStax kinematics][openstax-kinematics]
- [OpenStax LC oscillations][openstax-lc-oscillations]
- [OpenStax magnetic energy][openstax-magnetic-energy]
- [OpenStax magnetic forces][openstax-magnetic-forces]
- [OpenStax periodic motion][openstax-periodic-motion]
- [OpenStax pressure–volume work][openstax-pressure-volume-work]
- [OpenStax prism volume][openstax-prism-volume]
- [OpenStax resistivity][openstax-resistivity]
- [OpenStax rotational variables][openstax-rotational-variables]
- [OpenStax satellite orbits][openstax-satellite-orbits]
- [OpenStax self-inductance][openstax-self-inductance]
- [OpenStax thermal expansion][openstax-thermal-expansion]
- [OpenStax waves][openstax-waves]
- [OpenStax work and energy][openstax-work-and-energy]
- [QUDT area per time][qudt-area-per-time]
- [QUDT heat flow rate][qudt-heat-flow-rate]
- [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient]
- [QUDT molar volume][qudt-molar-volume]
- [QUDT quantity definitions][qudt-quantity-definitions]
- [QUDT thermal conductance][qudt-thermal-conductance]
- [QUDT thermal resistivity][qudt-thermal-resistivity]
- [NIST SI units][nist-si-units]
- [OpenStax sphere geometry][openstax-sphere-geometry]
- [OpenStax gravitational potential and escape][openstax-gravitational-potential]
- [OpenStax moments of inertia][openstax-moments-of-inertia]
- [OpenStax pendulums][openstax-pendulums]
- [OpenStax viscosity and laminar flow][openstax-viscosity]
- [OpenStax point-charge potential][openstax-point-charge-potential]
- [OpenStax electrical power][openstax-electrical-power]
- [OpenStax solenoids][openstax-solenoids]
- [OpenStax electric dipoles][openstax-electric-dipoles]
- [OpenStax magnetic dipole torque][openstax-magnetic-dipole-torque]
- [OpenStax radiative heat transfer][openstax-radiative-heat-transfer]
- [OpenStax ideal gas law][openstax-ideal-gas-law]
- [OpenStax radioactive decay][openstax-radioactive-decay]

[bipm-si-brochure]: https://www.bipm.org/en/publications/si-brochure
[bipm-katal-resolution]: https://www.bipm.org/en/-/resolution-cgpm-21-12
[cie-irradiance]: https://cie.co.at/eilvterm/17-21-053
[cie-radiant-intensity]: https://cie.co.at/eilvterm/17-21-044
[iaea-dosimetry]: https://www-pub.iaea.org/MTCD/Publications/PDF/te_1540_web.pdf
[iupac-amount-concentration]: https://goldbook.iupac.org/terms/view/A00295
[iupac-molar-mass]: https://goldbook.iupac.org/terms/view/12214
[iupac-molar-quantities]: https://goldbook.iupac.org/terms/view/M03971
[mit-linear-electromagnetic-media]: https://ocw.mit.edu/courses/18-325-topics-in-applied-mathematics-waves-and-imaging-fall-2015/c5db24b1a6d0d3301b1a3f21f6ceba3a_MIT18_325F15_CompleteLect.pdf
[nasa-reynolds-number]: https://www.grc.nasa.gov/www/k-12/airplane/reynolds.html
[nasa-dynamic-pressure]: https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/dynamic-pressure-2/
[nasa-mass-flow]: https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/mass-flow-rate/
[nasa-viscosity-and-flow]: https://ntrs.nasa.gov/api/citations/20250001477/downloads/Paul%20Gradl_Dissertation_NASA_Title_2-11-2025.pdf
[nist-sp-811]: https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes
[nist-tn-622]: https://nvlpubs.nist.gov/nistpubs/Legacy/TN/nbstechnicalnote622.pdf
[nist-photometric-equations-2-6]: https://www.nist.gov/system/files/documents/calibrations/sp250-15.pdf
[nist-photometric-quantities]: https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf
[openstax-ac-power]: https://openstax.org/books/university-physics-volume-2/pages/15-key-equations
[openstax-faraday-s-law]: https://openstax.org/books/university-physics-volume-2/pages/13-key-equations
[openstax-newton-s-laws]: https://openstax.org/books/university-physics-volume-1/pages/5-key-equations
[openstax-rc-circuits]: https://openstax.org/books/university-physics-volume-2/pages/10-key-equations
[openstax-rl-circuits]: https://openstax.org/books/university-physics-volume-2/pages/14-key-equations
[openstax-angular-momentum]: https://openstax.org/books/university-physics-volume-1/pages/11-summary
[openstax-area]: https://openstax.org/books/contemporary-mathematics/pages/10-6-area
[openstax-bernoulli]: https://openstax.org/books/university-physics-volume-1/pages/14-6-bernoullis-equation
[openstax-buoyancy]: https://openstax.org/books/university-physics-volume-1/pages/14-4-archimedes-principle-and-buoyancy
[openstax-capacitance]: https://openstax.org/books/university-physics-volume-2/pages/8-key-equations
[openstax-circular-motion]: https://openstax.org/books/physics/pages/6-key-equations
[openstax-current-and-resistance]: https://openstax.org/books/university-physics-volume-2/pages/9-key-equations
[openstax-electric-fields]: https://openstax.org/books/university-physics-volume-2/pages/5-key-equations
[openstax-electric-potential]: https://openstax.org/books/university-physics-volume-2/pages/7-key-equations
[openstax-fixed-axis-rotation]: https://openstax.org/books/university-physics-volume-1/pages/10-key-equations
[openstax-fluid-mechanics]: https://openstax.org/books/university-physics-volume-1/pages/14-key-equations
[openstax-fluids-density-and-pressure]: https://openstax.org/books/university-physics-volume-1/pages/14-1-fluids-density-and-pressure
[openstax-geometry]: https://openstax.org/books/contemporary-mathematics/pages/10-4-polygons-perimeter-and-circumference
[openstax-gravitation]: https://openstax.org/books/university-physics-volume-1/pages/13-2-gravitation-near-earths-surface
[openstax-heat-and-conduction]: https://openstax.org/books/university-physics-volume-2/pages/1-key-equations
[openstax-impulse-and-momentum]: https://openstax.org/books/university-physics-volume-1/pages/9-key-equations
[openstax-kinematics]: https://openstax.org/books/university-physics-volume-1/pages/3-key-equations
[openstax-lc-oscillations]: https://openstax.org/books/university-physics-volume-2/pages/14-5-oscillations-in-an-lc-circuit
[openstax-magnetic-energy]: https://openstax.org/books/university-physics-volume-2/pages/14-3-energy-in-a-magnetic-field
[openstax-magnetic-forces]: https://openstax.org/books/university-physics-volume-2/pages/11-key-equations
[openstax-periodic-motion]: https://openstax.org/books/university-physics-volume-1/pages/15-key-equations
[openstax-pressure-volume-work]: https://openstax.org/books/university-physics-volume-2/pages/3-key-equations
[openstax-prism-volume]: https://openstax.org/books/contemporary-mathematics/pages/10-7-volume-and-surface-area
[openstax-resistivity]: https://openstax.org/books/university-physics-volume-2/pages/9-3-resistivity-and-resistance
[openstax-rotational-variables]: https://openstax.org/books/university-physics-volume-1/pages/10-1-rotational-variables
[openstax-satellite-orbits]: https://openstax.org/books/university-physics-volume-1/pages/13-4-satellite-orbits-and-energy
[openstax-self-inductance]: https://openstax.org/books/university-physics-volume-2/pages/14-2-self-inductance-and-inductors
[openstax-thermal-expansion]: https://openstax.org/books/university-physics-volume-2/pages/1-3-thermal-expansion
[openstax-waves]: https://openstax.org/books/university-physics-volume-1/pages/16-1-traveling-waves
[openstax-work-and-energy]: https://openstax.org/books/university-physics-volume-1/pages/7-key-equations
[qudt-area-per-time]: https://qudt.org/vocab/quantitykind/AreaPerTime
[qudt-heat-flow-rate]: https://qudt.org/vocab/quantitykind/HeatFlowRate
[qudt-heat-transfer-coefficient]: https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer
[qudt-molar-volume]: https://qudt.org/vocab/quantitykind/MolarVolume
[qudt-quantity-definitions]: https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4
[qudt-thermal-conductance]: https://qudt.org/vocab/quantitykind/ThermalConductance
[qudt-thermal-resistivity]: https://qudt.org/vocab/quantitykind/ThermalResistivity
[nist-si-units]: https://www.nist.gov/pml/special-publication-330/sp-330-section-5
[openstax-sphere-geometry]: https://openstax.org/books/prealgebra-2e/pages/9-6-solve-geometry-applications-volume-and-surface-area
[openstax-gravitational-potential]: https://openstax.org/books/university-physics-volume-1/pages/13-3-gravitational-potential-energy-and-total-energy
[openstax-moments-of-inertia]: https://openstax.org/books/university-physics-volume-1/pages/10-5-calculating-moments-of-inertia
[openstax-pendulums]: https://openstax.org/books/university-physics-volume-1/pages/15-4-pendulums
[openstax-viscosity]: https://openstax.org/books/university-physics-volume-1/pages/14-7-viscosity-and-turbulence
[openstax-point-charge-potential]: https://openstax.org/books/university-physics-volume-2/pages/7-3-calculations-of-electric-potential
[openstax-electrical-power]: https://openstax.org/books/university-physics-volume-2/pages/9-5-electrical-energy-and-power
[openstax-solenoids]: https://openstax.org/books/university-physics-volume-2/pages/12-6-solenoids-and-toroids
[openstax-electric-dipoles]: https://openstax.org/books/university-physics-volume-2/pages/5-7-electric-dipoles
[openstax-magnetic-dipole-torque]: https://openstax.org/books/university-physics-volume-2/pages/11-5-force-and-torque-on-a-current-loop
[openstax-radiative-heat-transfer]: https://openstax.org/books/university-physics-volume-2/pages/1-6-mechanisms-of-heat-transfer
[openstax-ideal-gas-law]: https://openstax.org/books/chemistry-2e/pages/9-2-relating-pressure-volume-amount-and-temperature-the-ideal-gas-law
[openstax-radioactive-decay]: https://openstax.org/books/university-physics-volume-3/pages/10-3-radioactive-decay
