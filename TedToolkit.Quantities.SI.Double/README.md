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

The inventory lists **305 physical formulas**. Swapped products, solved forms, and typed variants
share a row. The conditions apply to each listed relation, and division requires a nonzero divisor.
Update the corresponding row when changing a formula's coverage.

### Geometry and algebra

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| Ω = 2π(1 − cos θ) | `SolidAngle` | Circular cone about the observation point. θ is its half-angle from zero to π radians. Supported methods include `SolidAngle.FromCone`. | [NIST radiometric calibration handbook][nist-cone-solid-angle] |
| A = 4πr² | `Area` | Total surface area of a sphere with positive radius. Supported methods include `Area.FromSphere`. | [OpenStax sphere geometry][openstax-sphere-geometry] |
| V = 4πr³ / 3 | `Volume` | Volume enclosed by a sphere with positive radius. Supported methods include `Volume.FromSphere`. | [OpenStax sphere geometry][openstax-sphere-geometry] |
| Ω = A / r² | `Area`, `SolidAngle` | The patch lies on a sphere centred at the point where the solid angle is measured. Use the sphere's positive radius. Supported methods include `SolidAngle.FromSphericalPatch` and `SolidAngle.ToSphericalPatchArea`. | [NIST SI units][nist-si-units] |
| Ā′ = ΔA / Δt | `Area`, `AreaPerTime`, `Time` | Signed change in area over the same interval. Use a constant or interval-averaged rate when recovering the change. | [QUDT area per time][qudt-area-per-time] |
| A = πr² | `Area` | Area enclosed by a circle with positive radius. Supported methods include `Area.FromCircle`. | [OpenStax area][openstax-area] |
| C = 2πr | `Length` | Circumference of a circle with positive radius. Supported methods include `Length.CircumferenceFromRadius`. | [OpenStax geometry][openstax-geometry] |
| d = 2r | `Length` | The radius and diameter refer to the same circle. Supported methods include `Length.DiameterFromRadius`, `Length.RadiusFromDiameter`. | [OpenStax geometry][openstax-geometry] |
| s = rθ | `Length`, `PlaneAngle` | Circular arc with a positive radius. A signed central angle gives a signed arc length. Supported methods include `PlaneAngle.FromArc`, `PlaneAngle.ToArcLength`. | [OpenStax rotational variables][openstax-rotational-variables] |
| κ = 1 / ℓ, r = κℓ | `InverseLength`, `Length`, `double` | The reciprocal requires a nonzero divisor. The product is one when κ and ℓ are reciprocals. | [NIST SP 811][nist-sp-811] |
| Length ratio = ℓ₂ / ℓ₁ | `double` | Use a nonzero reference length. Both lengths use the same unit basis. | [QUDT quantity definitions][qudt-quantity-definitions] |
| V = Aℓ | `Area`, `Length`, `Volume` | Prism or extrusion with uniform cross section perpendicular to its length. | [OpenStax prism volume][openstax-prism-volume] |
| A = ab | `Area`, `Length` | Rectangular sides are perpendicular. The inverse solves for the other side. | [OpenStax area][openstax-area] |
| Energy product = E₁E₂ | `SquareEnergy` | Multiply two energy values. Equal operands give E². | [QUDT quantity definitions][qudt-quantity-definitions] |
| Time product = t₁t₂ | `SquareTime`, `Time` | Equal operands give t². Divide by a known nonzero factor to recover the other factor. | [QUDT quantity definitions][qudt-quantity-definitions] |

### Mechanics, rotation, and waves

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| F = −bv | `Force` | Linear viscous damping. Use nonnegative damping coefficient b in mass per time and signed relative velocity. Supported methods include `Force.FromViscousDamping`. | [OpenStax damped oscillations][openstax-damped-oscillations] |
| ωd = √(k/m − (b/(2m))²) | `AngularVelocity` | Linear mass–spring oscillator with positive mass and stiffness and subcritical viscous damping. Supported methods include `AngularVelocity.FromUnderdampedSpring`. | [OpenStax damped oscillations][openstax-damped-oscillations] |
| A(t) = A₀ exp(−bt/(2m)) | `Length` | Amplitude envelope of an underdamped linear oscillator, with viscous damping, positive mass, and nonnegative elapsed time. Supported methods include `Length.AfterViscousDamping`. | [OpenStax damped oscillations][openstax-damped-oscillations] |
| Fₛ,max = μₛN | `Force` | Maximum static friction for a dry contact under the Coulomb model. Use the nonnegative coefficient and compressive normal force. Supported methods include `Force.MaximumStaticFriction`. | [OpenStax friction][openstax-friction] |
| Fₖ = μₖN | `Force` | Kinetic friction magnitude for dry sliding contact under the Coulomb model. Use the nonnegative coefficient and compressive normal force. Supported methods include `Force.FromKineticFriction`. | [OpenStax friction][openstax-friction] |
| ΔU = mgΔh | `Energy` | Uniform gravity over the height interval. Height change is signed and positive upward. Supported methods include `Energy.FromUniformGravity`. | [OpenStax potential energy][openstax-potential-energy] |
| E = −μm / (2r) | `Energy` | Total mechanical energy of a Newtonian circular orbit with negligible orbiting mass and potential energy zero at infinity. Supported methods include `Energy.FromCircularOrbit`. | [OpenStax satellite orbits][openstax-satellite-orbits] |
| Δθ = ω₀Δt + ½αΔt² | `PlaneAngle` | Constant signed angular acceleration about a fixed axis, with initial angular velocity about the same axis. Supported methods include `PlaneAngle.FromConstantAngularAcceleration`. | [OpenStax fixed-axis rotation][openstax-fixed-axis-rotation] |
| v² = v₀² + 2aΔx | `LinearVelocity` | Constant acceleration along one axis. Use signed acceleration and displacement with a nonnegative squared final speed. The method returns speed magnitude. Supported methods include `LinearVelocity.SpeedAfterConstantAcceleration`. | [OpenStax kinematics][openstax-kinematics] |
| I = mr² | `MomentOfInertia` | Point mass at perpendicular distance r from the axis or a thin ring about its central symmetry axis. Supported methods include `MomentOfInertia.FromPointMass` and `MomentOfInertia.FromThinRing`. | [OpenStax moments of inertia][openstax-moments-of-inertia] · [OpenStax moments of inertia reference table][openstax-rotation-tables] |
| I = mR² / 2 | `MomentOfInertia` | Uniform solid disk or cylinder about its central symmetry axis. Supported methods include `MomentOfInertia.FromSolidCylinder`. | [OpenStax moments of inertia reference table][openstax-rotation-tables] |
| I = mℓ² / 12 | `MomentOfInertia` | Uniform thin rod about an axis through the midpoint perpendicular to its length. Supported methods include `MomentOfInertia.FromThinRod`. | [OpenStax moments of inertia reference table][openstax-rotation-tables] |
| I = 2mR² / 5 | `MomentOfInertia` | Uniform solid sphere about an axis through its centre. Supported methods include `MomentOfInertia.FromSolidSphere`. | [OpenStax moments of inertia reference table][openstax-rotation-tables] |
| I = Icm + md² | `MomentOfInertia` | Parallel-axis theorem for the same body and its mass m. Use perpendicular separation d from the centre-of-mass axis. Supported methods include `MomentOfInertia.AboutParallelAxis`. | [OpenStax moments of inertia][openstax-moments-of-inertia] |
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

### Elasticity and structural mechanics

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| ε = Δℓ/ℓ₀, εᵥ = ΔV/V₀ | `double` | Signed engineering or volumetric strain relative to the positive initial gauge length or volume. Supported methods include `Length.EngineeringStrainFrom` and `Volume.VolumetricStrainFrom`. | [OpenStax stress, strain, and elastic modulus][openstax-elasticity] |
| σ = Eε, τ = Gγ | `Stress` | Linear elasticity. Use Young's modulus E under uniaxial stress, or shear modulus G and engineering shear strain γ for simple shear. Supported methods include `Stress.FromUniaxialStrain` and `Stress.FromElasticShearStrain`. | [OpenStax stress, strain, and elastic modulus][openstax-elasticity] |
| Δp = −KΔV/V₀ | `ForcePerArea` | Small volumetric strain at constant bulk modulus K. Compression raises pressure. Supported methods include `ForcePerArea.PressureChangeFromBulkModulus`. | [OpenStax stress, strain, and elastic modulus][openstax-elasticity] |
| u = ½σε | `EnergyDensity` | Recoverable energy density for a single linearly elastic stress and conjugate strain, such as uniaxial loading. Supported methods include `EnergyDensity.FromElasticStrain`. | [Purdue elastic strain energy][purdue-elastic-energy] |
| k = EA/ℓ | `ForcePerLength` | Axial stiffness of a uniform straight bar in the linear elastic regime. Use Young's modulus, constant cross section and positive length. Supported methods include `ForcePerLength.AxialStiffness`. | [Illinois axial stiffness][illinois-axial-stiffness] |
| τsurface = 2T/(πr³) | `Stress` | Signed surface shear stress from elastic torsion of a solid circular shaft of positive radius. Supported methods include `Stress.FromSolidShaftTorque`. | [MIT circular shaft torsion][mit-shaft-torsion] |
| θ = Tℓ/(GJ), J = πr⁴/2 | `PlaneAngle` | Uniform solid circular shaft with constant torque T and shear modulus G. θ is the signed twist over the length ℓ. Supported methods include `PlaneAngle.TwistOfSolidCircularShaft`. | [MIT circular shaft torsion][mit-shaft-torsion] |
| G = E/[2(1 + ν)] | `ForcePerArea` | Isotropic linear elasticity. Use positive Young's modulus and Poisson's ratio ν between −1 and ½, excluding the endpoints. Supported methods include `ForcePerArea.ShearModulusFromYoungsModulus`. | [University of Hawaiʻi elastic moduli][hawaii-elastic-moduli] |
| K = E/[3(1 − 2ν)] | `ForcePerArea` | Isotropic linear elasticity with the same conditions on Young's modulus and Poisson's ratio. Supported methods include `ForcePerArea.BulkModulusFromYoungsModulus`. | [University of Hawaiʻi elastic moduli][hawaii-elastic-moduli] |

### Special relativity

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| fobs = femit√((1 − β)/(1 + β)), β = v/c | `Frequency` | Collinear relative motion in vacuum. The signed subluminal relative velocity v is positive for recession. Supported methods include `Frequency.ObservedWithRelativisticDopplerShift`. | [OpenStax Doppler effect for light][openstax-light-doppler] |
| z = (λobs − λemit)/λemit | `double` | Use observed and emitted wavelengths of the same spectral feature with positive emitted wavelength. Supported methods include `Length.RedshiftFrom`. | [NASA JPL spectral redshift][nasa-spectral-redshift] |
| γ = 1/√(1 − v²/c²) | `double` | Relative speed magnitude below the positive vacuum speed of light c. Supported methods include `LinearVelocity.LorentzFactor`. | [OpenStax special relativity equations][openstax-relativity] |
| E₀ = mc² | `Energy` | Rest energy of a body of rest mass m. Supply the positive vacuum speed of light. Supported methods include `Energy.FromRestMass`. | [OpenStax special relativity equations][openstax-relativity] |
| E = γmc² | `Energy` | Total energy of a body with positive rest mass and subluminal speed measured in the same inertial frame. Supported methods include `Energy.FromRelativisticMotion`. | [OpenStax special relativity equations][openstax-relativity] |
| K = (γ − 1)mc² | `Energy` | Relativistic kinetic energy at subluminal speed. Evaluated using γ − 1 = (γv/c)²/(γ + 1) for small speeds. Supported methods include `Energy.FromRelativisticKineticEnergy`. | [OpenStax special relativity equations][openstax-relativity] |
| E² = (pc)² + (mc²)² | `Energy` | Use rest mass and momentum in the frame of interest. The result is the positive total energy. Supported methods include `Energy.FromRelativisticMomentum`. | [OpenStax special relativity equations][openstax-relativity] |
| p = γmv | `LinearMomentum` | Signed one-dimensional relativistic momentum of a body with positive rest mass at subluminal velocity. Supported methods include `LinearMomentum.FromRelativisticMotion`. | [OpenStax special relativity equations][openstax-relativity] |
| Δt = γΔτ | `Time` | Proper time interval Δτ measured by a clock moving at constant relative velocity in the reference frame. Supported methods include `Time.DilatedForRelativeVelocity`. | [OpenStax special relativity equations][openstax-relativity] |
| ℓ = ℓ₀/γ | `Length` | Length measured along relative motion. ℓ₀ is the proper length measured in the object's rest frame. Supported methods include `Length.ContractedAlongMotion`. | [OpenStax special relativity equations][openstax-relativity] |
| u = (u′ + v)/(1 + u′v/c²) | `LinearVelocity` | Collinear signed subluminal velocities. u′ is measured in the moving inertial frame, whose velocity in the reference frame is v. Supported methods include `LinearVelocity.ComposeRelativistically`. | [OpenStax special relativity equations][openstax-relativity] |

### Acoustics and surface waves

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| c = √(Fₜ / λₘ) | `LinearVelocity` | Transverse waves of small amplitude on a uniform taut string. Use positive tension and linear mass density. Supported methods include `LinearVelocity.FromStretchedString`. | [OpenStax wave speed on a string][openstax-stretched-string] |
| c = √(B / ρ) | `LinearVelocity` | Small acoustic perturbations in a fluid. Use its adiabatic bulk modulus and density at the same state. Supported methods include `LinearVelocity.FromFluidBulkModulus`. | [OpenStax speed of sound][openstax-speed-of-sound] |
| fₒ = fₛ(c + vₒ) / (c − vₛ) | `Frequency` | Classical collinear sound in a uniform medium. Measure both velocities relative to the medium, positive toward the other party, with both speeds below c. Supported methods include `Frequency.ObservedWithDopplerShift`. | [OpenStax acoustic Doppler effect][openstax-doppler] |
| I = p̂² / (2ρc) | `PowerPerArea` | Time-averaged intensity of a progressive sinusoidal plane sound wave. Use peak pressure amplitude, ambient density and local sound speed. Supported methods include `PowerPerArea.FromPeakSoundPressure`. | [OpenStax sound intensity][openstax-sound-intensity] |
| I = P / (4πr²) | `PowerPerArea` | Isotropic source in a lossless medium. Use total emitted power and positive distance. Supported methods include `PowerPerArea.FromIsotropicSource`. | [OpenStax sound intensity][openstax-sound-intensity] |
| fbeat = abs(f₁ − f₂) | `Frequency` | Beat frequency for two nearby tones superposed at the observer. Supported methods include `Frequency.BeatFrequency`. | [OpenStax beats][openstax-beats] |
| fₙ = nc / (2ℓ) | `Frequency` | Positive mode number n. String fixed at both ends or ideal pipe with both ends open or both closed. Supported methods include `Frequency.FromStandingWave`. | [OpenStax standing waves][openstax-standing-waves] · [OpenStax standing sound waves][openstax-sound-modes] |
| fₙ = (2n − 1)c / (4ℓ) | `Frequency` | Ideal pipe open at one end and closed at the other. Positive mode number n corresponds to harmonic 2n − 1. Supported methods include `Frequency.FromQuarterWaveResonance`. | [OpenStax standing sound waves][openstax-sound-modes] |
| c = √((g/k)tanh(kh)) | `LinearVelocity` | Phase speed of a linear surface gravity wave in still water of uniform depth h. Use k = 2π/λ and negligible surface tension. When kh is small, c ≈ √(gh). When kh is large, c ≈ √(g/k). Supported methods include `LinearVelocity.FromSurfaceGravityWave`, `LinearVelocity.FromShallowWaterGravityWave`, `LinearVelocity.FromDeepWaterGravityWave`. | [MIT surface gravity waves][mit-surface-gravity-waves] |

### Fluids and mass flow

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| v = √(2gh) | `LinearVelocity` | Ideal outflow from a small opening at depth h below the free surface of a large vessel. The two pressures are the same and losses are negligible. Supported methods include `LinearVelocity.FromTorricelliHead`. | [OpenStax Torricelli's theorem][openstax-torricelli] |
| Qᵥ = CᴅA√(2Δp/ρ) | `VolumePerTime` | Steady incompressible orifice flow with negligible approach speed. Use the opening area, elevation-corrected driving pressure and the discharge coefficient for that geometry. Supported methods include `VolumePerTime.FromOrificeFlow`. | [USBR orifice flow][usbr-orifice-flow] |
| Qᵥ = CᴅA₂√(2Δp/[ρ(1 − (A₂/A₁)²)]) | `VolumePerTime` | Steady incompressible Venturi flow. Use upstream area A₁, smaller throat area A₂, measured differential pressure corrected for elevation, and the discharge coefficient for the meter. Supported methods include `VolumePerTime.FromVenturi`. | [USBR differential head flowmeters][usbr-differential-flowmeters] |
| Δp_loss = Kρv²/2 | `ForcePerArea` | Mechanical head loss expressed as pressure for a fitting in steady incompressible flow. Use the fitting's loss coefficient and its reference mean speed. Supported methods include `ForcePerArea.FromLocalFlowLoss`. | [DOE fundamentals handbook, fluid flow][doe-local-flow-losses] |
| Ca = μv/σ | `double` | Use dynamic viscosity, speed magnitude and positive surface tension from the same interface flow. Supported methods include `LinearVelocity.CapillaryNumber`. | [MIT capillary and Bond numbers][mit-bond-capillary] |
| Bo = Δρgℓ²/σ | `double` | Use the heavier minus lighter fluid density, gravity and the characteristic interface length ℓ. Supported methods include `Length.BondNumber`. | [MIT capillary and Bond numbers][mit-bond-capillary] |
| ℓ꜀ = √(σ/(Δρg)) | `Length` | Capillary length of a fluid interface. Use positive surface tension, positive gravity and the heavier minus lighter fluid density. Supported methods include `Length.CapillaryLength`. | [MIT capillarity and gravity][mit-capillary-length] · [MIT capillary and Bond numbers][mit-bond-capillary] |
| St = fℓ/v | `double` | Use the oscillation frequency and characteristic length and positive reference speed of the same unsteady flow. Supported methods include `Frequency.StrouhalNumber`. | [Embry–Riddle unsteady flows][embry-riddle-strouhal] |
| Pe = vℓ/α | `double` | Thermal Péclet number. Use flow speed, characteristic length and thermal diffusivity at the flow conditions. Supported methods include `LinearVelocity.ThermalPecletNumber`. | [MIT heat and mass transfer dimensionless numbers][mit-transport-numbers] |
| c = √(γp/ρ) | `LinearVelocity` | Local sound speed of a calorically perfect ideal gas. Use static absolute pressure, density and heat-capacity ratio from the same state. Supported methods include `LinearVelocity.FromIdealGasSoundSpeed`. | [NASA speed of sound derivation][nasa-ideal-gas-sound-speed] |
| H = p/(ρg) | `Length` | Constant scale height of a hydrostatic isothermal ideal gas of uniform composition under constant gravity. Use absolute pressure and density at the same height. Supported methods include `Length.IsothermalScaleHeight`. | [NASA isothermal atmospheres][nasa-atmosphere] |
| p = p₀ exp(−Δh/H) | `ForcePerArea` | Absolute pressure after a change in height Δh in an isothermal, hydrostatic, uniform-composition ideal gas with constant scale height H. Positive Δh is upward. Supported methods include `ForcePerArea.AfterIsothermalElevationChange`. | [NASA isothermal atmospheres][nasa-atmosphere] |
| Δp = fᴅ(ℓ / Dₕ)ρv² / 2 | `ForcePerArea` | Friction loss in fully developed, constant-density flow through a straight duct of uniform section. Use the Darcy factor, the mean speed and the hydraulic diameter. Supported methods include `ForcePerArea.FromDarcyWeisbach`. | [USBR friction head loss][usbr-friction] · [NMSU pipe flow manual][nmsu-pipe-networks] |
| fᴅ = 64 / Reᴅ | `double` | Fully developed laminar flow of a Newtonian fluid in a circular pipe. Use Reynolds number based on pipe diameter and positive mean speed. Also follows by substituting the Poiseuille flow into Darcy–Weisbach. Supported methods include `LinearVelocity.DarcyFrictionFactorForLaminarPipe`. | [NMSU pipe flow manual][nmsu-pipe-networks] · [OpenStax viscosity and laminar flow][openstax-viscosity] |
| Dₕ = 4A / Pwet | `Length` | Use the flow cross section and its positive wetted perimeter. The result is four times the hydraulic radius. Supported methods include `Length.HydraulicDiameter`. | [USBR friction head loss][usbr-friction] · [NMSU pipe flow manual][nmsu-pipe-networks] |
| Fr = v / √(gD) | `double` | Open-channel flow. Use mean speed magnitude and hydraulic depth D, equal to flow area divided by the free-surface width. Supported methods include `LinearVelocity.FroudeNumber`. | [USBR Froude number][usbr-froude] · [MIT surface gravity waves][mit-surface-gravity-waves] |
| D = 6πμrv | `Force` | Magnitude of Stokes drag on an isolated rigid sphere without slip in unbounded Newtonian fluid, at Reynolds number much smaller than one. Supported methods include `Force.FromStokesDrag`. | [OpenStax drag and terminal speed][openstax-stokes-drag] |
| vₜ = 2(ρₛ − ρf)gr² / (9μ) | `LinearVelocity` | Terminal settling or rise of an isolated rigid sphere in an unbounded Newtonian fluid in the Stokes regime. The result is positive downward. Supported methods include `LinearVelocity.StokesTerminalVelocity`. | [UC San Diego Stokes settling][ucsd-stokes-settling] |
| pinside − poutside = 2σ / r | `ForcePerArea` | Pressure jump across one spherical interface of positive radius r and uniform surface tension σ. Supported methods include `ForcePerArea.FromSphericalSurfaceTension`. | [MIT interfacial pressure][mit-interfacial-pressure] |
| h = 2σ cos θ / (ρgr) | `Length` | Equilibrium in a vertical circular capillary of internal radius r with negligible gas density. Measure contact angle through the liquid. Negative height represents depression. Supported methods include `Length.CapillaryRise`. | [OpenStax capillary action][openstax-capillary-action] |
| We = ρv²ℓ / σ | `double` | Use fluid density, relative speed, representative length and positive surface tension for the same flow. Supported methods include `LinearVelocity.WeberNumber`. | [MIT Weber number][mit-weber] |
| Ma = v / c | `double` | Use the flow speed magnitude relative to the body and the speed of sound at the relevant local fluid state. Supported methods include `LinearVelocity.MachNumber`. | [NACA compressible flow report][naca-compressible-flow] |
| D = ½ρv²CᴅA | `Force` | Drag magnitude. Use a nonnegative drag coefficient and the reference area used to define it at the specified flow condition. Supported methods include `Force.FromDrag`. | [NASA drag equation][nasa-drag] |
| L = ½ρv²CʟA | `Force` | Signed lift along the convention used for the lift coefficient. Use its reference area and the specified flow condition. Supported methods include `Force.FromLift`. | [NASA lift equation][nasa-lift] |
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
| fc = 1/(2πτ) | `Frequency` | Half-power cutoff of a first-order RC or RL filter. Use positive time constant RC or L/R as appropriate. Supported methods include `Frequency.FromFirstOrderCutoff`. | [University of Oklahoma AC filters][ou-first-order-filters] |
| Q = ω₀L/R | `double` | Series RLC circuit with positive components. ω₀ is the angular resonant frequency. Supported methods include `Inductance.SeriesQualityFactor`. | [MIT series RLC response][mit-series-rlc] |
| P = VᵣₘₛIᵣₘₛ cos φ | `Power` | Average active power in sinusoidal steady state. Use the phase difference between terminal voltage and current entering the positive terminal. Supported methods include `Power.FromSinusoidalAc`. | [OpenStax AC power][openstax-ac-power] |
| V₂ = V₁N₂/N₁, I₂ = I₁N₁/N₂ | `ElectricPotential`, `ElectricCurrent` | Ideal loaded transformer in steady AC operation. Use positive turn counts and RMS magnitudes. Supported methods include `ElectricPotential.AfterIdealTransformer` and `ElectricCurrent.AfterIdealTransformer`. | [OpenStax transformers][openstax-transformers] |
| Rseries = R₁ + R₂ | `Resistance` | Ohmic resistors in series carry the same current. Supported methods include `Resistance.InSeriesWith`. | [OpenStax RC circuits][openstax-rc-circuits] |
| 1/Rparallel = 1/R₁ + 1/R₂ | `Resistance` | Two positive ohmic resistances connected between the same nodes. Supported methods include `Resistance.InParallelWith`. | [OpenStax RC circuits][openstax-rc-circuits] |
| 1/Cseries = 1/C₁ + 1/C₂ | `Capacitance` | Two positive ideal capacitors in series. Supported methods include `Capacitance.InSeriesWith`. | [OpenStax capacitance][openstax-capacitance] |
| Cparallel = C₁ + C₂ | `Capacitance` | Two ideal capacitors connected across the same nodes. Supported methods include `Capacitance.InParallelWith`. | [OpenStax capacitance][openstax-capacitance] |
| Lseries = L₁ + L₂ | `Inductance` | Linear series inductors with negligible mutual coupling. Supported methods include `Inductance.InSeriesWith`. | [MIT circuit energy storage][mit-inductors] |
| 1/Lparallel = 1/L₁ + 1/L₂ | `Inductance` | Two positive linear inductors in parallel with negligible mutual coupling. Supported methods include `Inductance.InParallelWith`. | [MIT circuit energy storage][mit-inductors] |
| V(t) = V∞ + (V₀ − V∞) exp(−t/(RC)) | `ElectricPotential` | Capacitor voltage after a first-order RC step, with constant final voltage, positive R and C, and nonnegative elapsed time. Supported methods include `ElectricPotential.AfterRcStep`. | [OpenStax RC circuits][openstax-rc-circuits] · [University of Oklahoma RC and RL transients][ou-rc-rl-step] |
| I(t) = I∞ + (I₀ − I∞) exp(−tR/L) | `ElectricCurrent` | Inductor current after a first-order RL step, with constant final current, positive R and L, and nonnegative elapsed time. Supported methods include `ElectricCurrent.AfterRlStep`. | [OpenStax RL circuits][openstax-rl-circuits] · [University of Oklahoma RC and RL transients][ou-rc-rl-step] |
| Vᵣₘₛ = Iᵣₘₛ / (ωC) | `ElectricPotential` | Ideal capacitor in sinusoidal steady state, with positive angular frequency ω = 2πf and positive capacitance. The output is an RMS magnitude. Supported methods include `ElectricPotential.FromSinusoidalCapacitor`. | [OpenStax AC power][openstax-ac-power] |
| Vᵣₘₛ = IᵣₘₛωL | `ElectricPotential` | Ideal inductor in sinusoidal steady state. Input and output are RMS magnitudes. Supported methods include `ElectricPotential.FromSinusoidalInductor`. | [OpenStax AC power][openstax-ac-power] |
| Iᵣₘₛ = Vᵣₘₛ / √(R² + (ωL − 1/(ωC))²) | `ElectricCurrent` | Linear series RLC circuit in sinusoidal steady state with positive R, L, C, and frequency. Supported methods include `ElectricCurrent.FromSeriesRlc`. | [OpenStax AC power][openstax-ac-power] |
| Vout = Vin R₂ / (R₁ + R₂) | `ElectricPotential` | Unloaded ohmic divider. R₁ is the upper resistance and R₂ is the resistance across the output. Follows from Ohm's law and the series circuit current. Supported methods include `ElectricPotential.FromVoltageDivider`. | [OpenStax RC circuits][openstax-rc-circuits] · [OpenStax voltage divider][openstax-voltage-divider] |
| L = μN²A / ℓ | `Inductance` | Long uniformly wound solenoid with uniform linear permeability, constant cross section, and negligible flux leakage and end effects. Supported methods include `Inductance.FromLongSolenoid`. | [OpenStax self-inductance][openstax-self-inductance] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| j = μ₀NIA | `MagneticDipoleMoment` | Identical planar turns with the same orientation. QUDT uses the moment j in Wb·m, related to the area moment m = NIA by j = μ₀m. Use signed current with the winding convention. Supported methods include `MagneticDipoleMoment.FromCurrentLoop`. | [OpenStax magnetic dipole torque][openstax-magnetic-dipole-torque] · [QUDT magnetic dipole moment][qudt-magnetic-dipole] |
| Eᵣ = λq / (2πεr) | `ElectricFieldStrength` | Infinite uniform line charge in an unbounded homogeneous linear dielectric. Use positive radial distance and the signed line charge density. Supported methods include `ElectricFieldStrength.FromLineCharge`. | [OpenStax electric fields][openstax-electric-fields] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| Bφ = μI / (2πr) | `MagneticFluxDensity` | Long straight wire with steady signed current and a uniform linear medium. Use perpendicular distance outside the wire. Supported methods include `MagneticFluxDensity.FromLongStraightWire`. | [OpenStax thin straight wire][openstax-straight-wire] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| F/ℓ = μI₁I₂ / (2πd) | `ForcePerLength` | Long parallel conductors in a uniform linear medium, separated by distance d. Positive force denotes attraction of currents flowing in the same direction. Supported methods include `ForcePerLength.FromParallelCurrents`. | [OpenStax parallel currents][openstax-parallel-currents] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| ℱ = NI | `MagnetomotiveForce` | Winding with N series turns sharing the same signed current and orientation. Supported methods include `MagnetomotiveForce.FromWinding`. | [OpenStax Ampère's law][openstax-ampere] · [QUDT magnetomotive force][qudt-magnetomotive-force] |
| ℱ = H∥ℓ | `AuxillaryMagneticField`, `Length`, `MagnetomotiveForce` | Constant tangential component of magnetic field strength along the path, or the average tangential component over the path. | [OpenStax Ampère's law][openstax-ampere] · [QUDT magnetomotive force][qudt-magnetomotive-force] |
| U = −pE cos θ | `Energy` | Fixed electric dipole and uniform external field. Use dipole and field magnitudes and the angle between them. Energy is zero at perpendicular orientation. Supported methods include `Energy.FromElectricDipole`. | [OpenStax electric dipoles][openstax-electric-dipoles] |
| U = −(j/μ₀)B cos θ | `Energy` | Fixed magnetic dipole and uniform external field. Use the QUDT moment j and vacuum permeability μ₀ to recover the area moment. Energy is zero at perpendicular orientation. Supported methods include `Energy.FromMagneticDipole`. | [OpenStax magnetic dipole torque][openstax-magnetic-dipole-torque] · [QUDT magnetic dipole moment][qudt-magnetic-dipole] |
| V̄ = LΔI / Δt | `ElectricPotential` | Passive voltage across an ideal linear inductor, averaged over the interval. The current enters the positive terminal and its change is signed. Supported methods include `ElectricPotential.FromInductorCurrentChange`. | [MIT capacitors and inductors][mit-capacitors-inductors] |
| Ī = CΔV / Δt | `ElectricCurrent` | Passive current into the positive terminal of an ideal linear capacitor, averaged over the interval. The terminal voltage change is signed. Supported methods include `ElectricCurrent.FromCapacitorVoltageChange`. | [MIT capacitors and inductors][mit-capacitors-inductors] |
| Fᵣ = q₁q₂ / (4πεr²) | `Force` | Two point charges in an unbounded homogeneous, isotropic linear dielectric. The radial force is positive for repulsion. Supported methods include `Force.FromPointCharges`. | [OpenStax electric fields][openstax-electric-fields] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| Eᵣ = q / (4πεr²) | `ElectricFieldStrength` | Point charge in an unbounded homogeneous, isotropic linear dielectric. Positive field points radially outward. Supported methods include `ElectricFieldStrength.FromPointCharge`. | [OpenStax electric fields][openstax-electric-fields] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| V = q / (4πεr) | `ElectricPotential` | Point charge in an unbounded homogeneous, isotropic linear dielectric, with zero potential at infinity. Supported methods include `ElectricPotential.FromPointCharge`. | [OpenStax point-charge potential][openstax-point-charge-potential] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| P = I²R = V² / R | `Power` | Joule dissipation in a positive ohmic resistance. Use current through or voltage across the same resistor. Supported methods include `Power.FromResistiveHeating`. | [OpenStax electrical power][openstax-electrical-power] |
| B = μNI / ℓ | `MagneticFluxDensity` | Axial field near the centre of a long, uniformly wound solenoid in a uniform linear medium. Use signed current relative to the winding orientation. Supported methods include `MagneticFluxDensity.FromLongSolenoid`. | [OpenStax solenoids][openstax-solenoids] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| τ = pE sin θ | `Torque` | Torque magnitude on an electric dipole in a uniform field. θ is the angle between dipole moment and field. Supported methods include `Torque.FromElectricDipole`. | [OpenStax electric dipoles][openstax-electric-dipoles] |
| τ = (j/μ₀)B sin θ | `Torque` | Torque magnitude on a magnetic dipole in a uniform field. θ is the angle between the dipole and flux density. The QUDT moment j in Wb·m is divided by vacuum permeability μ₀ to recover the magnetic area moment. Supported methods include `Torque.FromMagneticDipole`. | [OpenStax magnetic dipole torque][openstax-magnetic-dipole-torque] · [QUDT magnetic dipole moment][qudt-magnetic-dipole] |
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
| T₀ = T[1 + (γ − 1)Ma²/2] | `ThermodynamicTemperature` | Stagnation temperature for isentropic deceleration to rest of a calorically perfect ideal gas. Use local static absolute temperature and Mach number. Supported methods include `ThermodynamicTemperature.ToIsentropicStagnation`. | [NASA stagnation quantities][nasa-stagnation] |
| p₀ = p(T₀/T)^(γ/(γ − 1)) | `ForcePerArea` | Stagnation pressure for the same isentropic model, using static absolute pressure and γ greater than one. Supported methods include `ForcePerArea.ToIsentropicStagnation`. | [NASA stagnation quantities][nasa-stagnation] |
| ρ₀ = ρ(T₀/T)^(1/(γ − 1)) | `Density` | Stagnation density for the same isentropic model, using the static density and γ greater than one. Supported methods include `Density.ToIsentropicStagnation`. | [NASA stagnation quantities][nasa-stagnation] |
| W = p₁V₁ ln(V₂/V₁) | `Energy` | Reversible isothermal change of a fixed amount of ideal gas. Use initial absolute pressure and positive volumes. Work done by the gas is positive on expansion. Supported methods include `Energy.FromIsothermalWork`. | [OpenStax isothermal work][openstax-isothermal-work] |
| W = (p₁V₁ − p₂V₂)/(γ − 1) | `Energy` | Reversible adiabatic change of a calorically perfect ideal gas. Use absolute pressure, positive volumes and heat capacity ratio γ greater than one. The final pressure follows the isentropic relation. Supported methods include `Energy.FromIsentropicWork`. | [OpenStax adiabatic work][openstax-adiabatic-work] |
| λmax T = b | `Length`, `LengthTemperature`, `ThermodynamicTemperature` | Blackbody spectral radiance per unit wavelength at positive absolute temperature. Use the wavelength form of the Wien displacement constant b. | [OpenStax blackbody radiation][openstax-wien] |
| γ = cₚ / cᵥ | `double` | Use two `SpecificHeatCapacity` inputs with units of J/(kg·K), measured at constant pressure and constant volume for the same gas state. Supported methods include `SpecificHeatCapacity.HeatCapacityRatio`. | [NASA specific heat ratio][nasa-specific-heat-ratio] |
| p₂ = p₁(V₁ / V₂)ᵞ | `ForcePerArea` | Fixed amount of calorically perfect ideal gas undergoing reversible adiabatic compression or expansion. Use absolute pressure, positive volumes and constant heat capacity ratio γ. Supported methods include `ForcePerArea.AfterIsentropicVolumeChange`. | [NASA isentropic compression and expansion][nasa-isentropic-compression] |
| T₂ = T₁(V₁ / V₂)^(γ − 1) | `ThermodynamicTemperature` | Same reversible adiabatic model with absolute temperatures and positive volumes. Supported methods include `ThermodynamicTemperature.AfterIsentropicVolumeChange`. | [NASA isentropic compression and expansion][nasa-isentropic-compression] |
| η = Wnet / Qhot | `double` | Net work delivered and positive heat absorbed from the hot reservoir over the same engine cycle. Supported methods include `Energy.HeatEngineEfficiency`. | [OpenStax heat engines][openstax-heat-engines] |
| ηCarnot = 1 − Tcold / Thot | `double` | Reversible engine between two reservoirs at positive absolute temperatures, with the hot reservoir warmer. Supported methods include `ThermodynamicTemperature.CarnotEfficiency`. | [OpenStax Carnot cycle][openstax-carnot] |
| COPcool = Tcold / (Thot − Tcold) | `double` | Reversible refrigerator between the given positive absolute temperatures. The numerator uses the cold reservoir. Supported methods include `ThermodynamicTemperature.CarnotRefrigeratorCoefficientOfPerformance`. | [OpenStax Carnot cycle][openstax-carnot] |
| COPheat = Thot / (Thot − Tcold) | `double` | Reversible heat pump between the given positive absolute temperatures. The numerator uses the hot reservoir. Supported methods include `ThermodynamicTemperature.CarnotHeatPumpCoefficientOfPerformance`. | [OpenStax Carnot cycle][openstax-carnot] |
| τ = RₜC | `HeatCapacity`, `ThermalResistance`, `Time` | Single thermal lump with positive constant thermal resistance and heat capacity. For convection, Rₜ = 1/(hA). | [Michigan Tech lumped heat transfer][michigan-tech-lumped-heat] |
| T(t) = T∞ + (T₀ − T∞) exp(−t/τ) | `ThermodynamicTemperature` | Lumped body with negligible internal temperature gradients. Use constant surroundings, positive constant Rₜ and C, and nonnegative time t. Supported methods include `ThermodynamicTemperature.AfterLumpedHeatTransfer`. | [Michigan Tech lumped heat transfer][michigan-tech-lumped-heat] |
| Rₜ = ln(rₒ/rᵢ) / (2πkℓ) | `ThermalResistance` | Steady radial conduction through a homogeneous cylindrical shell of axial length ℓ, constant positive conductivity k and outer radius larger than the positive inner radius. Neglect end effects. Supported methods include `ThermalResistance.FromCylindricalConduction`. | [Princeton thermal resistance notes, §§8.7–8.8][princeton-thermal-resistance] |
| Rₜ = (1/rᵢ − 1/rₒ) / (4πk) | `ThermalResistance` | Steady radial conduction through a concentric spherical shell with constant positive conductivity and outer radius larger than the positive inner radius. Supported methods include `ThermalResistance.FromSphericalConduction`. | [Princeton thermal resistance notes, §§8.7–8.8][princeton-thermal-resistance] |
| Rseries = R₁ + R₂ | `ThermalResistance` | Series thermal paths with the same rate of heat transfer through each resistance. Supported methods include `ThermalResistance.InSeriesWith`. | [Princeton thermal resistance notes, §§8.7–8.8][princeton-thermal-resistance] |
| 1 / Rparallel = 1 / R₁ + 1 / R₂ | `ThermalResistance` | Two positive thermal resistances connecting the same pair of temperature nodes. Supported methods include `ThermalResistance.InParallelWith`. | [Princeton thermal resistance notes, §§8.7–8.8][princeton-thermal-resistance] |
| Bi = hℓ / ksolid, Nu = hℓ / kfluid | `double` | Use the solid characteristic length and conductivity for Biot. Use the fluid conductivity and length associated with the convection problem for Nusselt. Supported methods include `CoefficientOfHeatTransfer.BiotNumber` and `CoefficientOfHeatTransfer.NusseltNumber`. | [MIT heat and mass transfer dimensionless numbers][mit-transport-numbers] |
| Pr = ν / α | `double` | Use kinematic viscosity and thermal diffusivity for the same fluid state. | [MIT heat and mass transfer dimensionless numbers][mit-transport-numbers] |
| Fo = αt / ℓ² | `double` | Use thermal diffusivity, elapsed diffusion time and the positive characteristic length for the same body. Supported methods include `ThermalDiffusivity.FourierNumber`. | [MIT heat and mass transfer dimensionless numbers][mit-transport-numbers] |
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

### Optics and diffraction

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| 1/f = 1/dₒ + 1/dᵢ | `Length` | Paraxial thin lens with signed distances. Under the real-is-positive convention, real object and real image distances are positive. For a virtual image, dᵢ is negative. Supported methods include `Length.FocalLengthFromConjugates` and `Length.ImageDistanceFromThinLens`. | [OpenStax thin lenses][openstax-thin-lenses] |
| m = −dᵢ / dₒ | `double` | Paraxial thin-lens lateral magnification with the same distance convention. Negative magnification gives an inverted image. Supported methods include `Length.LateralMagnification`. | [OpenStax thin lenses][openstax-thin-lenses] |
| 1/f = (nℓ/nₘ − 1)(1/R₁ − 1/R₂) | `Length` | Thin lens with the same surrounding medium on both sides. A surface radius is positive if its centre of curvature lies downstream. Use infinite radius for a planar surface. Supported methods include `Length.FocalLengthFromLensMaker`. | [OpenStax thin lenses][openstax-thin-lenses] |
| n = c / vphase | `LinearVelocity`, `double` | Phase refractive index at a specified wavelength. Use the vacuum light speed c and the positive phase speed in the medium. Supported methods include `LinearVelocity.FromRefractiveIndex` and `LinearVelocity.RefractiveIndex`. | [OpenStax refraction][openstax-refraction] |
| n₁ sin θ₁ = n₂ sin θ₂ | `PlaneAngle`, `bool` | Positive refractive indices at the same wavelength. Measure signed angles from the interface normal. The method returns false when no propagating transmitted ray exists. Supported methods include `PlaneAngle.TryFromRefraction`. | [OpenStax refraction][openstax-refraction] |
| θ꜀ = asin(n₂/n₁) | `PlaneAngle` | Critical angle from the normal when the incident medium has the higher positive refractive index. Supported methods include `PlaneAngle.CriticalAngle`. | [OpenStax total internal reflection][openstax-total-internal-reflection] |
| θᵦ = atan(n₂/n₁) | `PlaneAngle` | Brewster incidence angle from the normal for an interface between lossless, nonmagnetic media. Supported methods include `PlaneAngle.BrewsterAngle`. | [OpenStax polarization][openstax-polarization] |
| I = I₀ cos² θ | `PowerPerArea` | Malus's law for linearly polarized incident light and an ideal polarizer. θ is the angle from the polarization to the transmission axis. Supported methods include `PowerPerArea.AfterIdealPolarizer`. | [OpenStax polarization][openstax-polarization] |
| mλ = d sin θ | `Length` | Normal incidence on a diffraction grating or two slits. Use a nonzero signed order m and its corresponding signed diffraction angle. Supported methods include `Length.WavelengthFromDiffractionGrating`. | [OpenStax diffraction equations][openstax-diffraction] |
| a sin θ = mλ | `PlaneAngle` | Far-field single-slit minima at normal incidence, with positive order m and mλ ≤ a. Supported methods include `PlaneAngle.FromSingleSlitMinimum`. | [OpenStax diffraction equations][openstax-diffraction] |
| mλ = 2d sin θ | `Length` | Bragg reflection from parallel lattice planes of spacing d. The angle is measured from the planes, and the order m is positive. Supported methods include `Length.WavelengthFromBraggDiffraction`. | [OpenStax diffraction equations][openstax-diffraction] |
| θmin ≈ 1.22 λ / D | `PlaneAngle` | Small-angle Rayleigh criterion for a circular aperture of positive diameter D and wavelength λ. Supported methods include `PlaneAngle.FromRayleighCriterion`. | [OpenStax diffraction equations][openstax-diffraction] |
| Δφ = 2πΔLopt / λ₀ | `PlaneAngle` | Coherent monochromatic waves. Use signed optical path difference and positive vacuum wavelength, with no additional phase shifts. Supported methods include `PlaneAngle.FromOpticalPathDifference`. | [OpenStax mathematics of interference][openstax-interference] |
| Δy ≈ Lλ / d | `Length` | Paraxial double-slit fringe spacing at normal incidence. Use positive screen distance L and slit separation d. Supported methods include `Length.FringeSpacingForDoubleSlit`. | [OpenStax mathematics of interference][openstax-interference] |

### Photometry and radiometry

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| Eᵥ = Φᵥ,incident / A | `Area`, `Illuminance`, `LuminousFlux` | Incident luminous flux averaged over the receiver area. | [NIST photometric quantities][nist-photometric-quantities] · [NIST photometric equations 2–6][nist-photometric-equations-2-6] |
| Iᵥ = LᵥAproj | `Area`, `Luminance`, `LuminousIntensity` | Use projected emitting area as viewed from the specified direction. | [NIST photometric quantities][nist-photometric-quantities] |
| Φᵥ = ηᵥP | `LuminousEfficacy`, `LuminousFlux`, `Power` | Use the power basis for which the luminous efficacy was specified. | [NIST photometric quantities][nist-photometric-quantities] · [QUDT quantity definitions][qudt-quantity-definitions] |
| Qᵥ = Φ̄ᵥΔt | `LuminousEnergy`, `LuminousFlux`, `Time` | Constant or time-averaged luminous flux over the interval. | [NIST photometric quantities][nist-photometric-quantities] |
| Φᵥ = IᵥΩ | `LuminousFlux`, `LuminousIntensity`, `SolidAngle` | Use mean luminous intensity over the solid angle. | [NIST photometric quantities][nist-photometric-quantities] · [NIST photometric equations 2–6][nist-photometric-equations-2-6] |
| Eᵥ = Iᵥ cos θ / r² | `Illuminance` | Use luminous intensity in the direction of the receiver, positive distance r and incidence angle θ from the receiving normal. The receiving face points toward the source. Supported methods include `Illuminance.FromPointSource` and `Illuminance.FromNormalPointSource`. | [CIE photometric distance law][cie-photometric-distance] · [NIST photometric equations 2–6][nist-photometric-equations-2-6] |
| Eₑ = Iₑ cos θ / r² | `PowerPerArea` | Use radiant intensity in the direction of the receiver and positive distance. For a face pointing toward the source, the projected receiver area satisfies dΩ = cos θ dA / r². Supported methods include `PowerPerArea.FromPointSource` and `PowerPerArea.FromNormalPointSource`. | [CIE irradiance][cie-irradiance] · [CIE radiant intensity][cie-radiant-intensity] · [CIE basic law of radiometry and photometry][cie-basic-radiometry] |
| Φₑ = IₑΩ | `Power`, `RadiantIntensity`, `SolidAngle` | Use mean radiant intensity over the solid angle. | [NIST photometric quantities][nist-photometric-quantities] |

### Chemistry and radiation dose

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| pᵢ = xᵢptotal | `ForcePerArea` | Dalton's law for a constituent of an ideal gas mixture. Use its gas-phase mole fraction and the total absolute pressure. Supported methods include `ForcePerArea.PartialPressureInIdealGas`. | [OpenStax partial pressures][openstax-dalton] |
| pᵢ = xᵢpᵢ* | `ForcePerArea` | Raoult's law for a constituent of an ideal liquid solution in vapour equilibrium. Use its liquid mole fraction and the pure-component saturation pressure at the same temperature. Supported methods include `ForcePerArea.VapourPartialPressureInIdealSolution`. | [OpenStax Raoult's law][openstax-raoult] |
| N = nNₐ | `AmountOfSubstance`, `InverseAmountOfSubstance`, `double` | Use amount of substance for the specified entities and the Avogadro constant in reciprocal moles. The count is a dimension-one scalar. | [BIPM definition of the mole][bipm-mole] |
| A = nNₐ ln 2 / t½ | `Activity` | Single radionuclide with a positive half-life. Use its own amount and the Avogadro constant. Supported methods include `Activity.FromAmountAndHalfLife`. | [BIPM definition of the mole][bipm-mole] · [OpenStax radioactive decay][openstax-radioactive-decay] |
| H = DQ | `DoseEquivalent` | Operational dose equivalent at a specified point. Use the absorbed dose and the mean radiation quality factor for that same point. Supported methods include `DoseEquivalent.FromAbsorbedDose`. | [IAEA operational dose quantities][iaea-dose-equivalent] |
| X = abs(Qion) / mₐᵢᵣ | `Exposure` | Use the total charge of ions of one sign per mass of air when electrons liberated by the photons are fully stopped in air. Supported methods include `Exposure.FromIonizationInAir`. | [IAEA radiation dosimetry handbook][iaea-exposure] |
| I = I₀ exp(−μx) | `PowerPerArea` | Narrow monoenergetic photon beam in a homogeneous material, with negligible contribution from scattered photons at the detector. Use the linear attenuation coefficient at that photon energy. Supported methods include `PowerPerArea.AfterExponentialAttenuation`. | [NIST photon attenuation][nist-photon-attenuation] |
| x½ = ln 2 / μ | `Length` | Half-value thickness for a narrow monoenergetic beam in a homogeneous material. Use the positive linear attenuation coefficient at the photon energy. Supported methods include `Length.HalfValueThickness`. | [NIST half-value layer][nist-half-value-layer] |
| c₂ = c₁V₁ / V₂ | `AmountOfSubstancePerVolume` | Dilution of one solute without loss or reaction. Use the initial and actual final solution volumes. Supported methods include `AmountOfSubstancePerVolume.AfterDilution`. | [OpenStax molarity and dilution][openstax-molarity] |
| c = (c₁V₁ + c₂V₂) / Vfinal | `AmountOfSubstancePerVolume` | Mix two solutions of the same solute without loss or reaction, and use the actual final volume. Derived by adding the solute amounts from cV. Supported methods include `AmountOfSubstancePerVolume.FromMixing`. | [IUPAC amount concentration][iupac-amount-concentration] · [OpenStax molarity and dilution][openstax-molarity] |
| wᵢ = mᵢ/m, xᵢ = nᵢ/n, φᵢ = Vᵢ/ΣVⱼ | `double` | Use the mass, amount or volume of the component and the corresponding positive total over all components. Volume fractions use each volume before mixing. Supported methods include `Mass.MassFraction`, `AmountOfSubstance.AmountFraction`, `Volume.VolumeFraction`. | [IUPAC fractions][iupac-fraction] · [IUPAC volume fraction][iupac-volume-fraction] |
| c = wρ / M | `AmountOfSubstancePerVolume` | Use the solute mass fraction w, complete solution density ρ, and solute molar mass M. Follows from solute mass wρV and c = n/V. Supported methods include `AmountOfSubstancePerVolume.FromMassFraction`. | [IUPAC fractions][iupac-fraction] · [IUPAC amount concentration][iupac-amount-concentration] · [IUPAC molar mass][iupac-molar-mass] |
| c = bρ / (1 + bM), b = c / (ρ − cM) | `AmountOfSubstancePerMass`, `AmountOfSubstancePerVolume` | Single solute of molar mass M in a solvent. Use total solution density ρ and positive solvent mass. Derived by subtracting solute mass from total solution mass to obtain the solvent mass. Supported methods include `AmountOfSubstancePerVolume.FromMolality`, `AmountOfSubstancePerMass.FromAmountConcentration`. | [IUPAC molality][iupac-molality] · [IUPAC amount concentration][iupac-amount-concentration] · [IUPAC molar mass][iupac-molar-mass] |
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

### Electromagnetic waves

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| v = 1/√(εμ) | `LinearVelocity` | Phase speed in a lossless, isotropic, nondispersive linear medium with positive absolute permittivity and permeability. Supported methods include `LinearVelocity.FromElectromagneticMedium`. | [OpenStax electromagnetic wave equations][openstax-em-wave-equations] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| E = vB | `ElectricFieldStrength`, `MagneticFluxDensity` | Progressive plane wave in a lossless medium. Use matching electric and magnetic magnitudes and the phase speed. Supported methods include `ElectricFieldStrength.FromPlaneWave` and `MagneticFluxDensity.FromPlaneWave`. | [OpenStax electromagnetic wave energy][openstax-em-wave-energy] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| I = EᵣₘₛBᵣₘₛ/μ | `PowerPerArea` | Time-averaged intensity of a progressive sinusoidal plane wave in a lossless linear medium. The perpendicular field components are in phase. Supported methods include `PowerPerArea.FromPlaneWave`. | [OpenStax electromagnetic wave energy][openstax-em-wave-energy] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| u = ½(εE² + B²/μ) | `EnergyDensity` | Instantaneous field energy density in a lossless, isotropic, nondispersive linear medium. Use instantaneous field magnitudes. Supported methods include `EnergyDensity.FromElectromagneticFields`. | [OpenStax electromagnetic wave energy][openstax-em-wave-energy] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| p = (1 + R)I/c | `ForcePerArea` | Normal incidence in vacuum on an opaque surface. The specularly reflected fraction is R and the remaining fraction is absorbed. Follows by adding the absorbed and reflected momentum changes. Supported methods include `ForcePerArea.FromRadiationPressure`. | [OpenStax radiation pressure][openstax-radiation-pressure] |

### Photon interactions

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| Eλ = hc | `Energy`, `Length`, `LengthEnergy` | Photon energy and vacuum wavelength. The product hc is a `LengthEnergy` input when solving for either quantity. | [OpenStax photon equations][openstax-photon-equations] |
| Δλ = λ꜀(1 − cos θ) | `Length` | Compton scattering from an initially stationary free electron. Use the electron Compton wavelength and photon scattering angle. Supported methods include `Length.ComptonWavelengthShift`. | [OpenStax Compton scattering][openstax-compton] |
| Kmax = Eγ − W | `Energy`, `bool` | Single-photon photoemission with material work function W. The method returns false below the emission threshold. Supported methods include `Energy.TryPhotoelectronKineticEnergy`. | [OpenStax photoelectric effect][openstax-photoelectric] |

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
- [MIT surface gravity waves][mit-surface-gravity-waves]
- [NASA drag equation][nasa-drag]
- [NASA lift equation][nasa-lift]
- [NACA compressible flow report][naca-compressible-flow]
- [OpenStax beats][openstax-beats]
- [OpenStax acoustic Doppler effect][openstax-doppler]
- [OpenStax friction][openstax-friction]
- [OpenStax potential energy][openstax-potential-energy]
- [OpenStax sound intensity][openstax-sound-intensity]
- [OpenStax standing sound waves][openstax-sound-modes]
- [OpenStax speed of sound][openstax-speed-of-sound]
- [OpenStax standing waves][openstax-standing-waves]
- [OpenStax wave speed on a string][openstax-stretched-string]
- [USBR friction head loss][usbr-friction]
- [USBR Froude number][usbr-froude]
- [NMSU pipe flow manual][nmsu-pipe-networks]
- [OpenStax drag and terminal speed][openstax-stokes-drag]
- [UC San Diego Stokes settling][ucsd-stokes-settling]
- [MIT interfacial pressure][mit-interfacial-pressure]
- [MIT Weber number][mit-weber]
- [OpenStax capillary action][openstax-capillary-action]
- [NASA isentropic compression and expansion][nasa-isentropic-compression]
- [NASA specific heat ratio][nasa-specific-heat-ratio]
- [OpenStax Carnot cycle][openstax-carnot]
- [OpenStax heat engines][openstax-heat-engines]
- [Princeton thermal resistance notes, §§8.7–8.8][princeton-thermal-resistance]
- [Michigan Tech lumped heat transfer][michigan-tech-lumped-heat]
- [MIT heat and mass transfer dimensionless numbers][mit-transport-numbers]
- [QUDT magnetic dipole moment][qudt-magnetic-dipole]
- [QUDT magnetomotive force][qudt-magnetomotive-force]
- [MIT circuit energy storage][mit-inductors]
- [MIT capacitors and inductors][mit-capacitors-inductors]
- [University of Oklahoma RC and RL transients][ou-rc-rl-step]
- [OpenStax voltage divider][openstax-voltage-divider]
- [OpenStax thin straight wire][openstax-straight-wire]
- [OpenStax parallel currents][openstax-parallel-currents]
- [OpenStax Ampère's law][openstax-ampere]
- [OpenStax thin lenses][openstax-thin-lenses]
- [OpenStax refraction][openstax-refraction]
- [OpenStax total internal reflection][openstax-total-internal-reflection]
- [OpenStax polarization][openstax-polarization]
- [OpenStax diffraction equations][openstax-diffraction]
- [OpenStax mathematics of interference][openstax-interference]
- [NIST radiometric calibration handbook][nist-cone-solid-angle]
- [CIE photometric distance law][cie-photometric-distance]
- [CIE basic law of radiometry and photometry][cie-basic-radiometry]
- [BIPM definition of the mole][bipm-mole]
- [IAEA operational dose quantities][iaea-dose-equivalent]
- [IAEA radiation dosimetry handbook][iaea-exposure]
- [NIST photon attenuation][nist-photon-attenuation]
- [NIST half-value layer][nist-half-value-layer]
- [IUPAC molality][iupac-molality]
- [IUPAC fractions][iupac-fraction]
- [IUPAC volume fraction][iupac-volume-fraction]
- [OpenStax molarity and dilution][openstax-molarity]
- [OpenStax stress, strain, and elastic modulus][openstax-elasticity]
- [Purdue elastic strain energy][purdue-elastic-energy]
- [Illinois axial stiffness][illinois-axial-stiffness]
- [University of Hawaiʻi elastic moduli][hawaii-elastic-moduli]
- [MIT circular shaft torsion][mit-shaft-torsion]
- [OpenStax moments of inertia reference table][openstax-rotation-tables]
- [OpenStax special relativity equations][openstax-relativity]
- [OpenStax damped oscillations][openstax-damped-oscillations]
- [OpenStax electromagnetic wave equations][openstax-em-wave-equations]
- [OpenStax electromagnetic wave energy][openstax-em-wave-energy]
- [OpenStax radiation pressure][openstax-radiation-pressure]
- [University of Oklahoma AC filters][ou-first-order-filters]
- [MIT series RLC response][mit-series-rlc]
- [OpenStax transformers][openstax-transformers]
- [OpenStax isothermal work][openstax-isothermal-work]
- [OpenStax adiabatic work][openstax-adiabatic-work]
- [OpenStax blackbody radiation][openstax-wien]
- [OpenStax photon equations][openstax-photon-equations]
- [OpenStax Compton scattering][openstax-compton]
- [OpenStax photoelectric effect][openstax-photoelectric]
- [OpenStax Doppler effect for light][openstax-light-doppler]
- [NASA JPL spectral redshift][nasa-spectral-redshift]
- [OpenStax Torricelli's theorem][openstax-torricelli]
- [USBR orifice flow][usbr-orifice-flow]
- [USBR differential head flowmeters][usbr-differential-flowmeters]
- [DOE fundamentals handbook, fluid flow][doe-local-flow-losses]
- [MIT capillary and Bond numbers][mit-bond-capillary]
- [MIT capillarity and gravity][mit-capillary-length]
- [Embry–Riddle unsteady flows][embry-riddle-strouhal]
- [NASA stagnation quantities][nasa-stagnation]
- [NASA isothermal atmospheres][nasa-atmosphere]
- [NASA speed of sound derivation][nasa-ideal-gas-sound-speed]
- [OpenStax partial pressures][openstax-dalton]
- [OpenStax Raoult's law][openstax-raoult]

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
[mit-surface-gravity-waves]: https://ocw.mit.edu/courses/12-802-wave-motion-in-the-ocean-and-the-atmosphere-spring-2008/fca533a5edb5f5d3e975ec4c2e63019f_MIT12_802S08_lec03.pdf
[nasa-drag]: https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/drag-equation/
[nasa-lift]: https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/lift-equation/
[naca-compressible-flow]: https://www.grc.nasa.gov/www/k-12/airplane/Images/naca1135.pdf
[openstax-beats]: https://openstax.org/books/university-physics-volume-1/pages/17-6-beats
[openstax-doppler]: https://openstax.org/books/university-physics-volume-1/pages/17-7-the-doppler-effect
[openstax-friction]: https://openstax.org/books/university-physics-volume-1/pages/6-2-friction
[openstax-potential-energy]: https://openstax.org/books/university-physics-volume-1/pages/8-1-potential-energy-of-a-system
[openstax-sound-intensity]: https://openstax.org/books/university-physics-volume-1/pages/17-3-sound-intensity
[openstax-sound-modes]: https://openstax.org/books/university-physics-volume-1/pages/17-4-normal-modes-of-a-standing-sound-wave
[openstax-speed-of-sound]: https://openstax.org/books/university-physics-volume-1/pages/17-2-speed-of-sound
[openstax-standing-waves]: https://openstax.org/books/university-physics-volume-1/pages/16-6-standing-waves-and-resonance
[openstax-stretched-string]: https://openstax.org/books/university-physics-volume-1/pages/16-3-wave-speed-on-a-stretched-string
[usbr-friction]: https://www.usbr.gov/tsc/techreferences/mands/wmm/chap02_16.html
[usbr-froude]: https://www.usbr.gov/tsc/techreferences/mands/wmm/chap02_12.html
[nmsu-pipe-networks]: https://nmwrri.nmsu.edu/footer_pages/nm-wrri-library-database-files/wrri-library-pdfs/wrrilibrary7/007529.pdf
[openstax-stokes-drag]: https://openstax.org/books/university-physics-volume-1/pages/6-4-drag-force-and-terminal-speed
[ucsd-stokes-settling]: https://igppweb.ucsd.edu/~guy/sio224/Stokes.pdf
[mit-interfacial-pressure]: https://web.mit.edu/fluids-modules/www/potential_flows/LecturesHTML/lec04/lecture4.html
[mit-weber]: https://web.mit.edu/fluids-modules/www/potential_flows/LecturesHTML/lec06/lecture6.html
[openstax-capillary-action]: https://openstax.org/books/college-physics-2e/pages/11-8-cohesion-and-adhesion-in-liquids-surface-tension-and-capillary-action
[nasa-isentropic-compression]: https://www.grc.nasa.gov/www/k-12/airplane/compexp.html
[nasa-specific-heat-ratio]: https://www.grc.nasa.gov/www/k-12/airplane/isndrv.html
[openstax-carnot]: https://openstax.org/books/university-physics-volume-2/pages/4-5-the-carnot-cycle
[openstax-heat-engines]: https://openstax.org/books/university-physics-volume-2/pages/4-2-heat-engines
[princeton-thermal-resistance]: https://rosenreview.cbe.princeton.edu/sites/g/files/toruqf6461/files/documents/Transport_I_12_14_0.pdf
[michigan-tech-lumped-heat]: https://pages.mtu.edu/~fmorriso/cm3120/lectures/2021_CM3120_module2_6blectureV.pdf
[mit-transport-numbers]: https://ocw.mit.edu/courses/3-185-transport-phenomena-in-materials-engineering-fall-2003/a55c74dc21def01fc0054de35574d1e5_3_21.pdf
[qudt-magnetic-dipole]: https://qudt.org/vocab/quantitykind/MagneticDipoleMoment
[qudt-magnetomotive-force]: https://qudt.org/vocab/quantitykind/MagnetomotiveForce
[mit-inductors]: https://circuits.mit.edu/_static/S23/handouts/lec07a/lecture07a.pdf
[mit-capacitors-inductors]: https://ocw.mit.edu/courses/6-071j-introduction-to-electronics-signals-and-measurement-spring-2006/eac1995aca830cfd0de6bab60a8abf10_capactr_inductr.pdf
[ou-rc-rl-step]: https://www.nhn.ou.edu/~bumm/ELAB/Lect_Notes/RC_RL_transients_v1_2_3.html
[openstax-voltage-divider]: https://openstax.org/books/university-physics-volume-2/pages/10-additional-problems
[openstax-straight-wire]: https://openstax.org/books/university-physics-volume-2/pages/12-2-magnetic-field-due-to-a-thin-straight-wire
[openstax-parallel-currents]: https://openstax.org/books/university-physics-volume-2/pages/12-3-magnetic-force-between-two-parallel-currents
[openstax-ampere]: https://openstax.org/books/university-physics-volume-2/pages/12-5-amperes-law
[openstax-thin-lenses]: https://openstax.org/books/university-physics-volume-3/pages/2-4-thin-lenses
[openstax-refraction]: https://openstax.org/books/university-physics-volume-3/pages/1-3-refraction
[openstax-total-internal-reflection]: https://openstax.org/books/university-physics-volume-3/pages/1-4-total-internal-reflection
[openstax-polarization]: https://openstax.org/books/university-physics-volume-3/pages/1-7-polarization
[openstax-diffraction]: https://openstax.org/books/university-physics-volume-3/pages/4-key-equations
[openstax-interference]: https://openstax.org/books/university-physics-volume-3/pages/3-2-mathematics-of-interference
[nist-cone-solid-angle]: https://nvlpubs.nist.gov/nistpubs/Legacy/hb/nisthandbook152.pdf
[cie-photometric-distance]: https://cie.co.at/eilvterm/17-25-104
[cie-basic-radiometry]: https://cie.co.at/eilvterm/17-25-088
[bipm-mole]: https://www.bipm.org/en/si-base-units/mole
[iaea-dose-equivalent]: https://nucleus.iaea.org/sites/orpnet/training/ExternalRadiation/Shared%20Documents/2_quantities%20-%20Final.pdf
[iaea-exposure]: https://www-pub.iaea.org/mtcd/publications/pdf/pub1196_web.pdf
[nist-photon-attenuation]: https://physics.nist.gov/PhysRefData/XrayMassCoef/chap2.html
[nist-half-value-layer]: https://srd.nist.gov/NSRDS/NSRDS-NBS29.pdf
[iupac-molality]: https://goldbook.iupac.org/terms/view/M03970
[iupac-fraction]: https://goldbook.iupac.org/terms/view/F02494
[iupac-volume-fraction]: https://goldbook.iupac.org/terms/view/V06643
[openstax-molarity]: https://openstax.org/books/chemistry-2e/pages/3-3-molarity
[openstax-elasticity]: https://openstax.org/books/university-physics-volume-1/pages/12-3-stress-strain-and-elastic-modulus
[purdue-elastic-energy]: https://web.ics.purdue.edu/~gonza226/ME323/Lecture-21.pdf
[illinois-axial-stiffness]: https://mechref.engr.illinois.edu/sol/axial.html
[hawaii-elastic-moduli]: https://www.soest.hawaii.edu/martel/Courses/GG303/Lecture_20.pdf
[mit-shaft-torsion]: https://ocw.mit.edu/courses/1-050-solid-mechanics-fall-2004/emech8_04.pdf
[openstax-rotation-tables]: https://openstax.org/books/physics/pages/a-reference-tables
[openstax-relativity]: https://openstax.org/books/university-physics-volume-3/pages/5-key-equations
[openstax-damped-oscillations]: https://openstax.org/books/university-physics-volume-1/pages/15-5-damped-oscillations
[openstax-em-wave-equations]: https://openstax.org/books/university-physics-volume-2/pages/16-key-equations
[openstax-em-wave-energy]: https://openstax.org/books/university-physics-volume-2/pages/16-3-energy-carried-by-electromagnetic-waves
[openstax-radiation-pressure]: https://openstax.org/books/university-physics-volume-2/pages/16-4-momentum-and-radiation-pressure
[ou-first-order-filters]: https://www.nhn.ou.edu/~bumm/ELAB/Lect_Notes/AC_impedance_v2_1_1.html
[mit-series-rlc]: https://circuits.mit.edu/_static/S24/handouts/lec12a/lec22.pdf
[openstax-transformers]: https://openstax.org/books/university-physics-volume-2/pages/15-6-transformers
[openstax-isothermal-work]: https://openstax.org/books/university-physics-volume-2/pages/3-2-work-heat-and-internal-energy
[openstax-adiabatic-work]: https://openstax.org/books/university-physics-volume-2/pages/3-6-adiabatic-processes-for-an-ideal-gas
[openstax-wien]: https://openstax.org/books/university-physics-volume-3/pages/6-1-blackbody-radiation
[openstax-photon-equations]: https://openstax.org/books/university-physics-volume-3/pages/6-key-equations
[openstax-compton]: https://openstax.org/books/university-physics-volume-3/pages/6-3-the-compton-effect
[openstax-photoelectric]: https://openstax.org/books/university-physics-volume-3/pages/6-2-photoelectric-effect
[openstax-light-doppler]: https://openstax.org/books/university-physics-volume-3/pages/5-7-doppler-effect-for-light
[nasa-spectral-redshift]: https://www.jpl.nasa.gov/edu/resources/lesson-plan/math-of-the-expanding-universe/
[openstax-torricelli]: https://openstax.org/books/college-physics-2e/pages/12-3-the-most-general-applications-of-bernoullis-equation
[usbr-orifice-flow]: https://www.usbr.gov/tsc/techreferences/mands/wmm/chap09_08.html
[usbr-differential-flowmeters]: https://www.usbr.gov/tsc/techreferences/mands/wmm/chap14_03.html
[doe-local-flow-losses]: https://www.energy.gov/sites/default/files/2026-04/DOE-HDBK-1012-92_VOL3.pdf
[mit-bond-capillary]: https://web.mit.edu/2.25/www/225_sect_11.html
[mit-capillary-length]: https://web.mit.edu/nnf/education/wettability/gravity.html
[embry-riddle-strouhal]: https://eaglepubs.erau.edu/introductiontoaerospaceflightvehicles/chapter/unsteady-flows/
[nasa-stagnation]: https://www.grc.nasa.gov/www/winddocs/windus3.0/cfpost/appc.html
[nasa-atmosphere]: https://psg.gsfc.nasa.gov/helpatm.php
[nasa-ideal-gas-sound-speed]: https://www.grc.nasa.gov/www/k-12/VirtualAero/BottleRocket/airplane/snddrv508.html
[openstax-dalton]: https://openstax.org/books/chemistry-2e/pages/9-3-stoichiometry-of-gaseous-substances-mixtures-and-reactions
[openstax-raoult]: https://openstax.org/books/chemistry-atoms-first-2e/pages/11-4-colligative-properties
