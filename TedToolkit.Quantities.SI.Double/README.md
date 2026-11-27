# TedToolkit.Quantities.SI.Double

Precompiled double-precision quantities and typed formulas using the existing
`TedToolkit.Quantities` package. [AssemblyInfo.cs](AssemblyInfo.cs) selects all 150 quantity kinds
in the repository's QUDT SI catalog and additional kinds used by the formulas, such as
`Pressure` and `ThermalConductance`. The package targets .NET Standard 2.0 and .NET 7.

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
the base generator. [QuantityUnits.json](QuantityUnits.json) adds the existing QUDT Celsius
unit to `ThermodynamicTemperature` so it has `FromDegreeCelsius` and `DegreeCelsius`.
The Kelvin conversion, including the offset for an absolute temperature, comes from [QUDT][qudt-celsius].

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
Likewise, `Radius.FromDiameter` calculates half the diameter of a circle and
`AngularVelocity.FromFrequency` computes the angular velocity for one revolution per cycle.

[QuantityConversions.cs](QuantityConversions.cs) defines the implicit and explicit casts in C#.
The package adds the following implicit conversions from specialized to general types.

| Specialized quantity | General quantity |
| --- | --- |
| `Width`, `Height`, `Depth`, `Radius`, `Diameter`, `Distance`, `Wavelength`, `PathLength` | `Length` |
| `GravitationalAttraction` | `Force` |
| `HeatFlowRate` | `Power` |
| `HeatFlowRatePerArea` | `PowerPerArea` |
| `Pressure`, scalar `Stress` | `ForcePerArea` |
| `Illuminance` | `LuminousFluxPerArea` |
| `ReynoldsNumber` | `DimensionlessRatio` |

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

The inventory lists **114 physical formulas**. Swapped products, solved forms, and typed variants
share a row. The conditions apply to each listed relation, and division requires a nonzero divisor.
Update the corresponding row when changing a formula's coverage.

### Geometry and algebra

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| A = πr² | `Area` | Area enclosed by a circle with positive radius. Supported methods include `Area.FromCircle`. | [OpenStax area][openstax-area] |
| C = 2πr | `Length` | Circumference of a circle with positive radius. Supported methods include `Radius.ToCircumference`. | [OpenStax geometry][openstax-geometry] |
| d = 2r | `Diameter`, `Radius` | The radius and diameter refer to the same circle. Supported methods include `Diameter.FromRadius`, `Radius.FromDiameter`. | [OpenStax geometry][openstax-geometry] |
| s = rθ | `Length`, `PlaneAngle` | Circular arc with a positive radius. A signed central angle gives a signed arc length. Supported methods include `PlaneAngle.FromArc`, `PlaneAngle.ToArcLength`. | [OpenStax rotational variables][openstax-rotational-variables] |
| κ = 1 / ℓ, r = κℓ | `DimensionlessRatio`, `InverseLength` | The reciprocal requires a nonzero length. The product is one when κ and ℓ are reciprocals. | [NIST SP 811][nist-sp-811] |
| Length ratio = ℓ₂ / ℓ₁ | `DimensionlessRatio` | Use a nonzero reference length. Both lengths use the same unit basis. | [QUDT quantity definitions][qudt-quantity-definitions] |
| V = Aℓ | `Area`, `Length`, `Volume` | Prism or extrusion with uniform cross section perpendicular to its length. | [OpenStax prism volume][openstax-prism-volume] |
| A = ab | `Area`, `Length` | Rectangular sides are perpendicular. The inverse solves for the other side. | [OpenStax area][openstax-area] |
| Energy product = E₁E₂ | `SquareEnergy` | Multiply two energy values. Equal operands give E². | [QUDT quantity definitions][qudt-quantity-definitions] |
| Time product = t₁t₂ | `SquareTime`, `Time` | Equal operands give t². Divide by a known nonzero factor to recover the other factor. | [QUDT quantity definitions][qudt-quantity-definitions] |

### Mechanics, rotation, and waves

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
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
| J = Δp = F̄Δt | `Force`, `Impulse`, `LinearMomentum`, `Time` | Use the signed component of the average force over the interval. Supported methods include `Impulse.FromAverageForce`. | [OpenStax impulse and momentum][openstax-impulse-and-momentum] |
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
| v = λf | `Frequency`, `LinearVelocity`, `Wavelength` | Wavelength and frequency belong to the same propagating wave. | [OpenStax waves][openstax-waves] |

### Fluids and mass flow

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| jₘ = ṁ / A = Δ(m/A) / Δt | `Area`, `MassPerArea`, `MassPerAreaTime`, `MassPerTime`, `Time` | Use the same oriented surface and interval for the areal mass flow rate jₘ. | [NASA mass flow][nasa-mass-flow] · [QUDT quantity definitions][qudt-quantity-definitions] |
| ρ = m / V, vₛ = V / m = 1 / ρ | `Density`, `DimensionlessRatio`, `Mass`, `SpecificVolume`, `Volume` | Density and specific volume describe the same material at the same state. | [OpenStax fluid mechanics][openstax-fluid-mechanics] · [QUDT quantity definitions][qudt-quantity-definitions] |
| q = ½ρv² | `Pressure` | Use local density and speed. Supported methods include `Pressure.FromDynamicPressure`. | [NASA dynamic pressure][nasa-dynamic-pressure] |
| P = ΔpQᵥ | `ForcePerArea`, `Power`, `Pressure`, `VolumePerTime` | Pressure difference and flow refer to the same path. Use pressures referenced to the same datum. Supported methods include `Pressure.FromHydraulicPower`. | [OpenStax fluid mechanics][openstax-fluid-mechanics] · [OpenStax pressure–volume work][openstax-pressure-volume-work] |
| Δp = ρgh | `Pressure` | Static fluid with uniform density and constant gravity over the vertical depth below the reference level. Supported methods include `Pressure.FromHydrostaticHead`. | [OpenStax fluids, density, and pressure][openstax-fluids-density-and-pressure] |
| ṁ = ρQᵥ | `Density`, `MassPerTime`, `VolumePerTime` | Use consistent density and volumetric flow at the same section. | [NASA mass flow][nasa-mass-flow] |
| Δm = ṁ̄Δt | `Mass`, `MassPerTime`, `Time` | Constant or time-averaged mass flow rate over the interval. | [NASA mass flow][nasa-mass-flow] |
| p = Fₙ / A | `Area`, `Force`, `ForcePerArea`, `Pressure` | Use the normal force over the surface. The general division returns `ForcePerArea`. Supported methods include `Pressure.FromNormalForce`. | [OpenStax fluid mechanics][openstax-fluid-mechanics] |
| W = pₑₓₜΔV | `Energy` | Constant external pressure. Expansion gives positive work by the system. Supported methods include `Energy.FromConstantPressureWork`. | [OpenStax pressure–volume work][openstax-pressure-volume-work] |
| Re = vℓ / ν = ρvℓ / μ | `ReynoldsNumber` | Choose the characteristic length for the flow and viscosity at that state. Supported methods include `ReynoldsNumber.FromFlow`. | [NASA Reynolds number][nasa-reynolds-number] |
| μ = ρν | `Density`, `DynamicViscosity`, `KinematicViscosity` | Density, dynamic viscosity, and kinematic viscosity at the same fluid state. | [NASA viscosity and flow][nasa-viscosity-and-flow] |
| Qᵥ = Avₙ | `Area`, `LinearVelocity`, `VolumePerTime` | Area-averaged normal speed component through the section. | [OpenStax fluid mechanics][openstax-fluid-mechanics] · [NASA mass flow][nasa-mass-flow] |
| ΔV = Q̄ᵥΔt | `Time`, `Volume`, `VolumePerTime` | Constant or time-averaged volumetric flow rate. | [OpenStax fluid mechanics][openstax-fluid-mechanics] |

### Electricity and magnetism

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| Q = CV | `Capacitance`, `ElectricCharge`, `ElectricPotential` | Linear capacitor with the voltage across its terminals. | [OpenStax capacitance][openstax-capacitance] |
| U = ½CV² | `Energy` | Energy stored in a linear capacitor. Supported methods include `Energy.FromCapacitor`. | [OpenStax capacitance][openstax-capacitance] |
| ΔQ = ĪΔt | `ElectricCharge`, `ElectricCurrent`, `Time` | Use average electrical current through the same cross section and interval. | [OpenStax current and resistance][openstax-current-and-resistance] |
| Q = (Q / m)m | `ElectricCharge`, `ElectricChargePerMass`, `Mass` | Charge and mass refer to the same particles or sample. | [QUDT quantity definitions][qudt-quantity-definitions] |
| Q = (Q / n)n | `AmountOfSubstance`, `ElectricCharge`, `ElectricChargePerAmountOfSubstance` | Use charge per mole for the specified entities and process. | [QUDT quantity definitions][qudt-quantity-definitions] · [IUPAC molar quantities][iupac-molar-quantities] |
| ρₑ = 1 / σ | `DimensionlessRatio`, `ElectricConductivity`, `Resistivity` | Resistivity and conductivity refer to the same ohmic material and state. | [OpenStax resistivity][openstax-resistivity] |
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
| V = IR, I = GV, G = 1 / R | `Conductance`, `DimensionlessRatio`, `ElectricCurrent`, `ElectricPotential`, `Resistance` | Voltage and current refer to the same ohmic element or operating point. | [OpenStax current and resistance][openstax-current-and-resistance] · [NIST SP 811][nist-sp-811] |
| C = εA / d | `Capacitance` | Parallel plates with uniform dielectric, using overlap area and negligible fringing. Supported methods include `Capacitance.FromParallelPlates`. | [OpenStax capacitance][openstax-capacitance] |
| ΔU = QΔV | `ElectricCharge`, `ElectricPotential`, `Energy` | Change in electrical potential energy for charge moved through a fixed potential difference. | [OpenStax electric potential][openstax-electric-potential] |
| τ = RC | `Capacitance`, `Resistance`, `Time` | Time constant of the equivalent first-order RC circuit. | [OpenStax RC circuits][openstax-rc-circuits] |
| τ = L / R | `Inductance`, `Resistance`, `Time` | Time constant of the equivalent first-order RL circuit. | [OpenStax RL circuits][openstax-rl-circuits] |
| Q = σqA | `Area`, `ElectricCharge`, `ElectricChargePerArea` | Uniform or mean surface charge density σq over the area. | [OpenStax electric fields][openstax-electric-fields] · [QUDT quantity definitions][qudt-quantity-definitions] |
| R = ρₑℓ / A | `Resistance` | Uniform conductor with constant cross section and material resistivity. Supported methods include `Resistance.FromUniformConductor`. | [OpenStax resistivity][openstax-resistivity] |
| Q = ρqV | `ElectricCharge`, `ElectricChargeVolumeDensity`, `Volume` | Uniform or mean volume charge density ρq over the region. | [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |

### Heat and temperature

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| R″ = ARₜ | `Area`, `ThermalInsulance`, `ThermalResistance` | Relate total thermal resistance to area-normalized resistance for the same uniform surface. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient] |
| qₓ″ = −k∂T/∂x | `HeatFlowRatePerArea` | Signed temperature gradient along the axis with scalar conductivity. Supported methods include `HeatFlowRatePerArea.FromConduction`. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT heat flow rate][qudt-heat-flow-rate] |
| Pheat = q″A | `Area`, `HeatFlowRate`, `HeatFlowRatePerArea` | Use average normal heat flux over the surface. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient] |
| Gₜ = hA | `Area`, `CoefficientOfHeatTransfer`, `ThermalConductance` | Uniform or area-averaged heat-transfer coefficient for the same thermal path and driving temperature difference. | [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient] · [QUDT thermal conductance][qudt-thermal-conductance] |
| R″ = ℓ/k = ℓρₜ | `Length`, `ThermalConductivity`, `ThermalInsulance`, `ThermalResistivity` | Uniform plane layer. Use thickness along the heat-flow direction. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT thermal resistivity][qudt-thermal-resistivity] |
| Q = CΔT | `Energy`, `HeatCapacity`, `TemperatureDifference` | Sensible heat with constant or interval-averaged heat capacity. | [OpenStax heat and conduction][openstax-heat-and-conduction] |
| C = mc | `HeatCapacity`, `Mass`, `SpecificHeatCapacity` | Use mass-specific heat capacity for the specified thermodynamic process. | [OpenStax heat and conduction][openstax-heat-and-conduction] |
| q″ = hΔT | `CoefficientOfHeatTransfer`, `HeatFlowRatePerArea`, `TemperatureDifference` | Transfer coefficient h is per unit area. Use the driving temperature difference in the indicated direction. | [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient] |
| ΔT = T₂ − T₁, T₂ = T₁ + ΔT | `TemperatureDifference`, `ThermodynamicTemperature` | Use the temperature difference as the increment of an absolute temperature. | [BIPM SI Brochure][bipm-si-brochure] |
| ΔT = (dT/dt)̄Δt | `TemperatureDifference`, `TemperaturePerTime`, `Time` | Temperature-change rate is constant or averaged over the interval. | [QUDT quantity definitions][qudt-quantity-definitions] |
| ΔT = q″R″ | `HeatFlowRatePerArea`, `TemperatureDifference`, `ThermalInsulance` | Use areal thermal resistance and the corresponding heat-flux component. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient] |
| ∂T/∂x ≈ ΔT / Δx | `Length`, `TemperatureDifference`, `TemperatureGradient` | Signed temperature difference over a distance along the axis. The quotient gives the interval average. | [OpenStax heat and conduction][openstax-heat-and-conduction] |
| α = k / cᵥ | `ThermalConductivity`, `ThermalDiffusivity`, `VolumetricHeatCapacity` | Thermal diffusivity for scalar conductivity and volumetric heat capacity. | [NIST TN 622][nist-tn-622] |
| ΔL = L′ΔT, ΔA = A′ΔT, ΔV = V′ΔT | `Area`, `Length`, `TemperatureDifference`, `Volume` | Use the object's average dimensional expansion per kelvin over the interval, or the local slope for a small temperature change. The result is the change in size. | [OpenStax thermal expansion][openstax-thermal-expansion] · [QUDT quantity definitions][qudt-quantity-definitions] |
| Pheat = ΔT / Rₜ = GₜΔT, Gₜ = 1 / Rₜ | `DimensionlessRatio`, `HeatFlowRate`, `TemperatureDifference`, `ThermalConductance`, `ThermalResistance` | Use total resistance or conductance for the same thermal path. A signed temperature drop gives the signed heat-flow rate. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT thermal conductance][qudt-thermal-conductance] |
| ρₜ = 1 / k | `DimensionlessRatio`, `ThermalConductivity`, `ThermalResistivity` | Thermal resistivity and conductivity describe the same material and direction. | [QUDT thermal resistivity][qudt-thermal-resistivity] |
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
| Φₑ = IₑΩ | `Power`, `RadiantIntensity`, `SolidAngle` | Use mean radiant intensity over the solid angle. | [NIST photometric quantities][nist-photometric-quantities] |

### Chemistry and radiation dose

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| D = Edeposited / m | `AbsorbedDose`, `Energy`, `Mass` | Mean energy deposited by ionizing radiation in the specified matter. Supported methods include `AbsorbedDose.FromDepositedEnergy`. | [IAEA dosimetry][iaea-dosimetry] |
| ΔD = Ḋ̄Δt | `AbsorbedDose`, `AbsorbedDoseRate`, `Time` | Constant or interval-averaged absorbed dose rate for the same matter. | [IAEA dosimetry][iaea-dosimetry] |
| c = n / V | `AmountOfSubstance`, `AmountOfSubstancePerVolume`, `Volume` | Amount of the constituent divided by the volume of the mixture. | [IUPAC amount concentration][iupac-amount-concentration] |
| b = n / m | `AmountOfSubstance`, `AmountOfSubstancePerMass`, `Mass` | Use the reference mass for the specified amount-per-mass quantity, such as solvent mass when calculating molality. | [QUDT quantity definitions][qudt-quantity-definitions] · [IUPAC molar quantities][iupac-molar-quantities] |
| Δn = ācΔt | `AmountOfSubstance`, `Time` | Use constant or time-averaged catalytic activity for the specified indicator reaction and assay conditions. Δn is the amount converted due to the catalyst. | [BIPM katal resolution][bipm-katal-resolution] · [QUDT quantity definitions][qudt-quantity-definitions] |
| Eₘ = E / n | `AmountOfSubstance`, `Energy`, `MolarEnergy` | Use energy for the same specified amount of substance. | [IUPAC molar quantities][iupac-molar-quantities] |
| Cₘ = C / n | `AmountOfSubstance`, `HeatCapacity`, `MolarHeatCapacity` | Use heat capacity for the same amount and thermodynamic process. | [IUPAC molar quantities][iupac-molar-quantities] · [OpenStax heat and conduction][openstax-heat-and-conduction] |
| M = m / n | `AmountOfSubstance`, `Mass`, `MolarMass` | Mass and amount describe the same substance or specified entities. | [IUPAC molar mass][iupac-molar-mass] |
| M = ρVₘ | `Density`, `MolarMass`, `MolarVolume` | Density and molar volume describe the same material at the same state. | [IUPAC molar quantities][iupac-molar-quantities] · [QUDT molar volume][qudt-molar-volume] |
| Vₘ = V / n | `AmountOfSubstance`, `MolarVolume`, `Volume` | Volume and amount refer to the same sample at the same state. | [IUPAC molar quantities][iupac-molar-quantities] |

## Formula references

- [BIPM SI Brochure][bipm-si-brochure]
- [BIPM katal resolution][bipm-katal-resolution]
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
- [OpenStax capacitance][openstax-capacitance]
- [OpenStax current and resistance][openstax-current-and-resistance]
- [OpenStax electric fields][openstax-electric-fields]
- [OpenStax electric potential][openstax-electric-potential]
- [OpenStax fixed-axis rotation][openstax-fixed-axis-rotation]
- [OpenStax fluid mechanics][openstax-fluid-mechanics]
- [OpenStax fluids, density, and pressure][openstax-fluids-density-and-pressure]
- [OpenStax geometry][openstax-geometry]
- [OpenStax heat and conduction][openstax-heat-and-conduction]
- [OpenStax impulse and momentum][openstax-impulse-and-momentum]
- [OpenStax kinematics][openstax-kinematics]
- [OpenStax periodic motion][openstax-periodic-motion]
- [OpenStax pressure–volume work][openstax-pressure-volume-work]
- [OpenStax prism volume][openstax-prism-volume]
- [OpenStax resistivity][openstax-resistivity]
- [OpenStax rotational variables][openstax-rotational-variables]
- [OpenStax thermal expansion][openstax-thermal-expansion]
- [OpenStax waves][openstax-waves]
- [OpenStax work and energy][openstax-work-and-energy]
- [QUDT heat flow rate][qudt-heat-flow-rate]
- [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient]
- [QUDT molar volume][qudt-molar-volume]
- [QUDT quantity definitions][qudt-quantity-definitions]
- [QUDT thermal conductance][qudt-thermal-conductance]
- [QUDT thermal resistivity][qudt-thermal-resistivity]

[bipm-si-brochure]: https://www.bipm.org/en/publications/si-brochure
[bipm-katal-resolution]: https://www.bipm.org/en/-/resolution-cgpm-21-12
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
[openstax-capacitance]: https://openstax.org/books/university-physics-volume-2/pages/8-key-equations
[openstax-current-and-resistance]: https://openstax.org/books/university-physics-volume-2/pages/9-key-equations
[openstax-electric-fields]: https://openstax.org/books/university-physics-volume-2/pages/5-key-equations
[openstax-electric-potential]: https://openstax.org/books/university-physics-volume-2/pages/7-key-equations
[openstax-fixed-axis-rotation]: https://openstax.org/books/university-physics-volume-1/pages/10-key-equations
[openstax-fluid-mechanics]: https://openstax.org/books/university-physics-volume-1/pages/14-key-equations
[openstax-fluids-density-and-pressure]: https://openstax.org/books/university-physics-volume-1/pages/14-1-fluids-density-and-pressure
[openstax-geometry]: https://openstax.org/books/contemporary-mathematics/pages/10-4-polygons-perimeter-and-circumference
[openstax-heat-and-conduction]: https://openstax.org/books/university-physics-volume-2/pages/1-key-equations
[openstax-impulse-and-momentum]: https://openstax.org/books/university-physics-volume-1/pages/9-key-equations
[openstax-kinematics]: https://openstax.org/books/university-physics-volume-1/pages/3-key-equations
[openstax-periodic-motion]: https://openstax.org/books/university-physics-volume-1/pages/15-key-equations
[openstax-pressure-volume-work]: https://openstax.org/books/university-physics-volume-2/pages/3-key-equations
[openstax-prism-volume]: https://openstax.org/books/contemporary-mathematics/pages/10-7-volume-and-surface-area
[openstax-resistivity]: https://openstax.org/books/university-physics-volume-2/pages/9-3-resistivity-and-resistance
[openstax-rotational-variables]: https://openstax.org/books/university-physics-volume-1/pages/10-1-rotational-variables
[openstax-thermal-expansion]: https://openstax.org/books/university-physics-volume-2/pages/1-3-thermal-expansion
[openstax-waves]: https://openstax.org/books/university-physics-volume-1/pages/16-1-traveling-waves
[openstax-work-and-energy]: https://openstax.org/books/university-physics-volume-1/pages/7-key-equations
[qudt-heat-flow-rate]: https://qudt.org/vocab/quantitykind/HeatFlowRate
[qudt-heat-transfer-coefficient]: https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer
[qudt-molar-volume]: https://qudt.org/vocab/quantitykind/MolarVolume
[qudt-quantity-definitions]: https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4
[qudt-thermal-conductance]: https://qudt.org/vocab/quantitykind/ThermalConductance
[qudt-thermal-resistivity]: https://qudt.org/vocab/quantitykind/ThermalResistivity
[qudt-celsius]: https://qudt.org/vocab/unit/DEG_C
