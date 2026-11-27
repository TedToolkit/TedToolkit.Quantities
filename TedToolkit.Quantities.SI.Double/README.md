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
and the Froude number use `double`. A temperature interval or gradient also uses `double`,
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

The inventory lists **274 physical formulas**. Each calculation uses up to two input quantities.
Dimension-one inputs such as ratios use `double` and count as one quantity. Swapped products,
solved forms, and typed variants share a row. The conditions apply to each listed relation, and division requires a nonzero divisor.
Update the corresponding row when changing a formula's coverage.

### Geometry and algebra

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| V = πr²h/3 | `Volume` | Circular base of positive radius r and perpendicular height h. Supported methods include `Volume.FromCircularCone`. | [OpenStax cylinder and cone geometry][openstax-cone-and-cylinder] |
| A = 2πr(r + h) | `Area` | Total surface area, including both bases, of a closed right circular cylinder with positive radius and height. Supported methods include `Area.FromClosedCircularCylinder`. | [OpenStax cylinder and cone geometry][openstax-cone-and-cylinder] |
| g = ℓ/A | `Area`, `InverseLength`, `Length` | Geometric ratio for a uniform member. Use axial length and its perpendicular cross-sectional area. | [OpenStax resistivity][openstax-resistivity] |
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
| rₛ = 2μ/c² | `Length` | Schwarzschild horizon radius of an uncharged, nonrotating mass. Use its positive standard gravitational parameter. Supported methods include `Length.SchwarzschildRadius`. | [OpenStax gravitation equations][openstax-gravitation] · [NIST CODATA physical constants][nist-codata-constants] |
| εorbit = −μ/(2a) | `SpecificEnergy` | Total orbital energy per mass in a bound Newtonian two-body orbit, using the positive standard gravitational parameter and semi-major axis. Supported methods include `SpecificEnergy.FromBoundKeplerOrbit`. | [NASA introductory orbital mechanics][nasa-orbital-energy] |
| k = √(I/m) | `Length` | Radius of gyration about the same axis used for moment of inertia I. Use the positive mass of the same body. Supported methods include `Length.RadiusOfGyration`. | [MIT rigid-body dynamics][mit-radius-of-gyration] |
| dstop = v₀²/(2a) | `Length` | Straight-line stopping distance with positive constant deceleration magnitude a and nonnegative initial speed. Supported methods include `Length.FromConstantStoppingDeceleration`. | [OpenStax constant acceleration][openstax-constant-acceleration] |
| vmax = Aω, amax = Aω² | `LinearAcceleration`, `LinearVelocity` | Peak magnitudes in simple harmonic motion. Use nonnegative displacement amplitude A and positive angular frequency ω. Supported methods include `LinearVelocity.PeakSpeedInSimpleHarmonicMotion` and `LinearAcceleration.PeakAccelerationInSimpleHarmonicMotion`. | [OpenStax simple harmonic motion equations][openstax-simple-harmonic-motion] |
| φ = −μ/r | `SpecificEnergy` | Newtonian potential relative to infinity outside a spherical mass, or at positive distance r from a point mass. Use its standard gravitational parameter μ. Supported methods include `SpecificEnergy.FromNewtonianGravitationalPotential`. | [OpenStax gravitational potential energy][openstax-gravitational-energy] |
| Δφ = gΔz | `SpecificEnergy` | Potential energy change per unit mass in uniform gravity. Use positive gravity and signed upward elevation change. Supported methods include `SpecificEnergy.FromUniformGravityElevationChange`. | [OpenStax Bernoulli equation][openstax-bernoulli] |
| L = rp⊥ | `AngularMomentum`, `Length`, `LinearMomentum` | Signed angular momentum about a fixed axis. Use the perpendicular radius from the axis and the signed tangential momentum component. | [OpenStax angular momentum][openstax-angular-momentum] |
| F = −bv | `Force` | Linear viscous damping. Use nonnegative damping coefficient b in mass per time and signed relative velocity. Supported methods include `Force.FromViscousDamping`. | [OpenStax damped oscillations][openstax-damped-oscillations] |
| Fₛ,max = μₛN | `Force` | Maximum static friction for a dry contact under the Coulomb model. Use the nonnegative coefficient and compressive normal force. Supported methods include `Force.MaximumStaticFriction`. | [OpenStax friction][openstax-friction] |
| Fₖ = μₖN | `Force` | Kinetic friction magnitude for dry sliding contact under the Coulomb model. Use the nonnegative coefficient and compressive normal force. Supported methods include `Force.FromKineticFriction`. | [OpenStax friction][openstax-friction] |
| I = mr² | `MomentOfInertia` | Point mass at perpendicular distance r from the axis or a thin ring about its central symmetry axis. Supported methods include `MomentOfInertia.FromPointMass` and `MomentOfInertia.FromThinRing`. | [OpenStax moments of inertia][openstax-moments-of-inertia] · [OpenStax moments of inertia reference table][openstax-rotation-tables] |
| I = mR² / 2 | `MomentOfInertia` | Uniform solid disk or cylinder about its central symmetry axis. Supported methods include `MomentOfInertia.FromSolidCylinder`. | [OpenStax moments of inertia reference table][openstax-rotation-tables] |
| I = mℓ² / 12 | `MomentOfInertia` | Uniform thin rod about an axis through the midpoint perpendicular to its length. Supported methods include `MomentOfInertia.FromThinRod`. | [OpenStax moments of inertia reference table][openstax-rotation-tables] |
| I = 2mR² / 5 | `MomentOfInertia` | Uniform solid sphere about an axis through its centre. Supported methods include `MomentOfInertia.FromSolidSphere`. | [OpenStax moments of inertia reference table][openstax-rotation-tables] |
| T = 2π√(m / k) | `Time` | Positive mass and stiffness in an undamped, linear, massless-spring system. Supported methods include `Time.FromMassSpring`. | [OpenStax periodic motion][openstax-periodic-motion] |
| T = 2π√(ℓ / g) | `Time` | Small-angle, undamped simple pendulum of positive length in uniform gravity. Supported methods include `Time.FromSimplePendulum`. | [OpenStax pendulums][openstax-pendulums] |
| vₑ = √(2μ / r) | `LinearVelocity` | Newtonian escape speed from radius r outside an isolated spherical body. Neglect the escaping body's mass. Supported methods include `LinearVelocity.EscapeSpeed`. | [OpenStax gravitational potential and escape][openstax-gravitational-potential] |
| vₜ = rω | `AngularVelocity`, `Length`, `LinearVelocity` | Fixed-axis rotation. Radius is the perpendicular distance to the axis, and velocity is the signed tangential component. | [OpenStax circular motion][openstax-circular-motion] |
| aₜ = rα | `AngularAcceleration`, `Length`, `LinearAcceleration` | Fixed-axis rotation at a fixed radius. Acceleration is the signed tangential component. | [OpenStax circular motion][openstax-circular-motion] |
| a꜀ = vₜ² / r = rω² | `LinearAcceleration` | Circular motion at positive radius. Returns the inward acceleration magnitude. Supported methods include `LinearAcceleration.FromCentripetalMotion`. | [OpenStax circular motion][openstax-circular-motion] |
| Fₛ = −kx | `Force` | Linear spring with positive stiffness. Displacement is signed from equilibrium. Supported methods include `Force.FromSpringDisplacement`. | [OpenStax periodic motion][openstax-periodic-motion] |
| Uₛ = ½kx² | `Energy` | Elastic potential energy relative to equilibrium in the linear range of the spring. Supported methods include `Energy.FromSpringDisplacement`. | [OpenStax periodic motion][openstax-periodic-motion] |
| g = μ / r² | `LinearAcceleration` | Newtonian gravitational acceleration magnitude outside a spherical body. Use the body's gravitational parameter μ = GM and distance from its centre. Supported methods include `LinearAcceleration.FromGravitationalParameter`. | [OpenStax gravitation][openstax-gravitation] |
| v = √(μ / r) | `LinearVelocity` | Newtonian circular orbit with negligible orbiting mass. Use the central body's gravitational parameter and the orbital radius from its centre. Supported methods include `LinearVelocity.FromCircularOrbit`. | [OpenStax satellite orbits][openstax-satellite-orbits] |
| T = 2π√(a³ / μ) | `Time` | Newtonian elliptic orbit with negligible orbiting mass. Use positive semi-major axis a and the central body's gravitational parameter μ. In a circular orbit a equals the radius and T = 2πr / v. Supported methods include `Time.FromKeplerOrbit` and `Time.FromCircularOrbit`. | [OpenStax Kepler's laws][openstax-kepler] · [OpenStax satellite orbits][openstax-satellite-orbits] |
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
| e = E / m | `Energy`, `Mass`, `SpecificEnergy` | Energy and mass refer to the same body or sample. For a phase transition at constant temperature, apply the same relation as Q = mL using specific latent heat L and the transitioning mass. | [NIST SP 811][nist-sp-811] · [OpenStax phase changes][openstax-phase-changes] |
| τ = Iα | `AngularAcceleration`, `MomentOfInertia`, `Torque` | Net torque and moment of inertia about the same fixed axis. | [OpenStax fixed-axis rotation][openstax-fixed-axis-rotation] |
| K = ½mv² = p²/(2m) | `Energy` | Nonrelativistic translation with positive mass. Use speed or momentum magnitude. Supported methods include `Energy.FromTranslationalKineticEnergy` and `Energy.FromMomentumKineticEnergy`. | [OpenStax kinetic energy][openstax-kinetic-energy] · [OpenStax momentum][openstax-impulse-and-momentum] |
| Δv = āΔt | `LinearAcceleration`, `LinearVelocity`, `Time` | Constant or average acceleration component over the interval. | [OpenStax kinematics][openstax-kinematics] |
| v = λf | `Frequency`, `Length`, `LinearVelocity` | Wavelength and frequency belong to the same propagating wave. Supported methods include `Frequency.FromWaveSpeed`, `Length.WavelengthFromSpeed`, `LinearVelocity.FromWavelength`. | [OpenStax waves][openstax-waves] |

### Elasticity and structural mechanics

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| ε = Δℓ/ℓ₀, εᵥ = ΔV/V₀ | `double` | Signed engineering or volumetric strain relative to the positive initial gauge length or volume. Supported methods include `Length.EngineeringStrainFrom` and `Volume.VolumetricStrainFrom`. | [OpenStax stress, strain, and elastic modulus][openstax-elasticity] |
| σ = Eε, τ = Gγ | `Stress` | Linear elasticity. Use Young's modulus E under uniaxial stress, or shear modulus G and engineering shear strain γ for simple shear. Supported methods include `Stress.FromUniaxialStrain` and `Stress.FromElasticShearStrain`. | [OpenStax stress, strain, and elastic modulus][openstax-elasticity] |
| u = ½σε | `EnergyDensity` | Recoverable energy density for a single linearly elastic stress and conjugate strain, such as uniaxial loading. Supported methods include `EnergyDensity.FromElasticStrain`. | [Purdue elastic strain energy][purdue-elastic-energy] |
| τsurface = 2T/(πr³) | `Stress` | Signed surface shear stress from elastic torsion of a solid circular shaft of positive radius. Supported methods include `Stress.FromSolidShaftTorque`. | [MIT circular shaft torsion][mit-shaft-torsion] |
| G = E/[2(1 + ν)] | `ForcePerArea` | Isotropic linear elasticity. Use positive Young's modulus and Poisson's ratio ν between −1 and ½, excluding the endpoints. Supported methods include `ForcePerArea.ShearModulusFromYoungsModulus`. | [University of Hawaiʻi elastic moduli][hawaii-elastic-moduli] |
| K = E/[3(1 − 2ν)] | `ForcePerArea` | Isotropic linear elasticity with the same conditions on Young's modulus and Poisson's ratio. Supported methods include `ForcePerArea.BulkModulusFromYoungsModulus`. | [University of Hawaiʻi elastic moduli][hawaii-elastic-moduli] |

### Special relativity

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| fobs = fₛ√((1 − β)/(1 + β)) | `Frequency` | Longitudinal Doppler shift in vacuum. Use the source-frame frequency and signed relative line-of-sight velocity v with magnitude below c. β = v/c is positive for recession. Supported methods include `Frequency.ObserveLongitudinalDoppler`. | [OpenStax longitudinal Doppler effect][openstax-longitudinal-doppler] |
| u = (u′ + v)/(1 + u′v/c²) | `LinearVelocity` | Velocity addition for inertial frames and velocities along the same axis. Use signed velocities u′ of the object in the intermediate frame and v of that frame relative to the observer, both with magnitude below c. Supported methods include `LinearVelocity.AddCollinearRelativisticVelocity`. | [OpenStax relativistic velocity transformation][openstax-relativistic-velocity] |
| Δt = γΔτ | `Time` | Inertial observers. Δτ is proper duration between events at the same point in the moving clock's frame. Supply Lorentz factor γ of at least one. Supported methods include `Time.Dilate`. | [OpenStax special relativity equations][openstax-relativity] |
| L = L₀/γ | `Length` | Inertial observers. Use proper length L₀ parallel to relative motion and Lorentz factor γ of at least one. Supported methods include `Length.ContractAlongMotion`. | [OpenStax special relativity equations][openstax-relativity] |
| K = (γ − 1)E₀ | `Energy` | Relativistic kinetic energy for a massive body, given its rest energy and Lorentz factor. Supported methods include `Energy.FromRelativisticKineticEnergy`. | [OpenStax special relativity equations][openstax-relativity] |
| z = (λobs − λemit)/λemit | `double` | Use observed and emitted wavelengths of the same spectral feature with positive emitted wavelength. Supported methods include `Length.RedshiftFrom`. | [NASA JPL spectral redshift][nasa-spectral-redshift] |
| γ = 1/√(1 − v²/c²) | `double` | Relative speed magnitude below the positive vacuum speed of light c. Supported methods include `LinearVelocity.LorentzFactor`. | [OpenStax special relativity equations][openstax-relativity] |
| E₀ = mc² | `Energy` | Rest energy of a body of rest mass m. Supply the positive vacuum speed of light. Supported methods include `Energy.FromRestMass`. | [OpenStax special relativity equations][openstax-relativity] |

### Acoustics and surface waves

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| k = 2π/λ, ṽ = 1/λ₀ | `InverseLength` | Angular wavenumber k in radians per metre uses the wavelength in the medium. Spectroscopic wavenumber ṽ is the reciprocal of vacuum wavelength. Supported methods include `InverseLength.AngularWavenumberFromWavelength` and `InverseLength.SpectroscopicWavenumberFromWavelength`. | [OpenStax mathematics of waves][openstax-wave-mathematics] · [NIST atomic spectra units][nist-spectroscopic-wavenumber] |
| f₁ = c/(2L), f₁ = c/(4L) | `Frequency` | Ideal fundamental acoustic mode in a tube open at both ends, or open at one end and closed at the other, respectively. Use positive sound speed and effective length. Supported methods include `Frequency.FromHalfWaveResonance` and `Frequency.FromQuarterWaveResonance`. | [OpenStax standing sound waves][openstax-standing-sound] |
| Ī = pᵣₘₛuᵣₘₛ | `PowerPerArea` | Progressive plane sound wave with pressure and particle velocity in phase. Use their RMS values. Supported methods include `PowerPerArea.FromProgressiveSoundWave`. | [OpenStax sound intensity][openstax-sound-intensity] |
| ΔLᵢ = 10 log₁₀(I₂/I₁) | `double` | Sound intensity level difference in decibels from positive acoustic intensities. Supported methods include `PowerPerArea.SoundIntensityLevelDifferenceInDecibels`. | [OpenStax sound intensity][openstax-sound-intensity] |
| c = √(Fₜ / λₘ) | `LinearVelocity` | Transverse waves of small amplitude on a uniform taut string. Use positive tension and linear mass density. Supported methods include `LinearVelocity.FromStretchedString`. | [OpenStax wave speed on a string][openstax-stretched-string] |
| c = √(B / ρ) | `LinearVelocity` | Small acoustic perturbations in a fluid. Use its adiabatic bulk modulus and density at the same state. Supported methods include `LinearVelocity.FromFluidBulkModulus`. | [OpenStax speed of sound][openstax-speed-of-sound] |
| I = P / (4πr²) | `PowerPerArea` | Isotropic source in a lossless medium. Use total emitted power and positive distance. Supported methods include `PowerPerArea.FromIsotropicSource`. | [OpenStax sound intensity][openstax-sound-intensity] |
| fbeat = abs(f₁ − f₂) | `Frequency` | Beat frequency for two nearby tones superposed at the observer. Supported methods include `Frequency.BeatFrequency`. | [OpenStax beats][openstax-beats] |
| c_shallow = √(gh), c_deep = √(gλ/(2π)) | `LinearVelocity` | Phase speed of small surface gravity waves in still water. Use depth h for the shallow-water limit and wavelength λ for the deep-water limit. Supported methods include `LinearVelocity.FromShallowWaterGravityWave` and `LinearVelocity.FromDeepWaterGravityWave`. | [MIT surface gravity waves][mit-surface-gravity-waves] |

### Fluids and mass flow

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| pabs = pg + pref | `ForcePerArea` | Gauge and absolute pressure for the same pressure reference. Use signed gauge pressure and absolute reference pressure, for example the ambient atmospheric pressure. Supported methods include `ForcePerArea.FromGaugePressure` and `ForcePerArea.GaugePressureRelativeTo`. | [OpenStax fluid mechanics][openstax-fluid-mechanics] |
| p₀ = p + q | `ForcePerArea` | Stagnation pressure for incompressible flow brought to rest without losses or elevation change. Use static pressure and dynamic pressure from the same point. Supported methods include `ForcePerArea.FromIncompressibleStagnation`. | [OpenStax Bernoulli equation][openstax-bernoulli] |
| D = A/T | `Length` | Hydraulic depth of an open channel. Use cross-sectional flow area A and positive surface width T at the same section. Supported methods include `Length.HydraulicDepth`. | [San Diego State University open-channel hydraulics][sdsu-froude] |
| Fr = U/cg | `double` | Open-channel flow. Use mean flow speed magnitude U and the positive shallow-water gravity-wave speed cg relative to that flow. Supported methods include `LinearVelocity.FroudeNumber`. | [San Diego State University open-channel hydraulics][sdsu-froude] |
| Sc = ν/D | `double` | Same species and fluid state. D is its positive molecular mass diffusivity in square metres per second, represented by `AreaPerTime`. Supported methods include `KinematicViscosity.SchmidtNumber`. | [MIT convective heat and mass transfer, equation 105][mit-schmidt-number] |
| xrms = √(2Dt) | `Length` | RMS displacement along one axis in unbounded diffusion with nonnegative constant diffusivity D and elapsed time t. D is represented by `AreaPerTime`. Supported methods include `Length.RmsDisplacementInOneDimensionalDiffusion`. | [MIT diffusion and random walks][mit-diffusion-displacement] |
| Δp = ρΔφhead | `Density`, `ForcePerArea`, `SpecificEnergy` | Pressure change in a static constant-density fluid. Δφhead is the signed decrease in specific gravitational potential when moving from the reference level to the location. Also represents the pressure term p/ρ in Bernoulli's equation. | [OpenStax Bernoulli equation][openstax-bernoulli] |
| F = ṁ Isp,m | `Force`, `MassPerTime`, `SpecificImpulseByMass` | Use positive propellant mass flow and mass-specific impulse in N·s/kg for the same operating condition. Isp,m includes pressure thrust through equivalent exhaust velocity. | [NASA specific impulse][nasa-specific-impulse] · [QUDT quantity definitions][qudt-quantity-definitions] |
| ṗavg = Δp/Δt | `ForcePerArea`, `ForcePerAreaTime`, `Time` | Signed pressure change per elapsed time. Use average or constant rate over the interval when recovering the pressure change. | [QUDT quantity definitions][qudt-quantity-definitions] |
| v = √(2gh) | `LinearVelocity` | Ideal outflow from a small opening at depth h below the free surface of a large vessel. The two pressures are the same and losses are negligible. Supported methods include `LinearVelocity.FromTorricelliHead`. | [OpenStax Torricelli's theorem][openstax-torricelli] |
| Dₕ = 4A / Pwet | `Length` | Use the flow cross section and its positive wetted perimeter. The result is four times the hydraulic radius. Supported methods include `Length.HydraulicDiameter`. | [USBR friction head loss][usbr-friction] · [NMSU pipe flow manual][nmsu-pipe-networks] |
| pinside − poutside = 2σ / r | `ForcePerArea` | Pressure jump across one spherical interface of positive radius r and uniform surface tension σ. Supported methods include `ForcePerArea.FromSphericalSurfaceTension`. | [MIT interfacial pressure][mit-interfacial-pressure] |
| Ma = v / c | `double` | Use the flow speed magnitude relative to the body and the speed of sound at the relevant local fluid state. Supported methods include `LinearVelocity.MachNumber`. | [NACA compressible flow report][naca-compressible-flow] |
| λₘ = ρA | `Area`, `Density`, `MassPerLength` | Use cross-sectional area and the mean mass density over that section. Follows from m = ρV and V = Aℓ for a uniform section. | [OpenStax fluids, density, and pressure][openstax-fluids-density-and-pressure] · [QUDT quantity definitions][qudt-quantity-definitions] |
| ρₐ = ρℓ | `Density`, `Length`, `MassPerArea` | Use uniform thickness perpendicular to the surface and mean mass density through the thickness. Follows from m = ρV and V = Aℓ. | [OpenStax fluids, density, and pressure][openstax-fluids-density-and-pressure] · [QUDT quantity definitions][qudt-quantity-definitions] |
| jₘ = ṁ / A = Δ(m/A) / Δt | `Area`, `MassPerArea`, `MassPerAreaTime`, `MassPerTime`, `Time` | Use the same oriented surface and interval for the areal mass flow rate jₘ. | [NASA mass flow][nasa-mass-flow] · [QUDT quantity definitions][qudt-quantity-definitions] |
| ρ = m / V, vₛ = V / m = 1 / ρ | `Density`, `Mass`, `SpecificVolume`, `Volume`, `double` | Density and specific volume describe the same material at the same state. | [OpenStax fluid mechanics][openstax-fluid-mechanics] · [QUDT quantity definitions][qudt-quantity-definitions] |
| q = ½ρv² | `ForcePerArea` | Use local density and speed. Supported methods include `ForcePerArea.FromDynamicPressure`. | [NASA dynamic pressure][nasa-dynamic-pressure] |
| P = ΔpQᵥ | `ForcePerArea`, `Power`, `VolumePerTime` | Pressure difference and flow refer to the same path. Use pressures referenced to the same datum. Supported methods include `ForcePerArea.FromHydraulicPower`. | [OpenStax fluid mechanics][openstax-fluid-mechanics] · [OpenStax pressure–volume work][openstax-pressure-volume-work] |
| ṁ = ρQᵥ | `Density`, `MassPerTime`, `VolumePerTime` | Use consistent density and volumetric flow at the same section. | [NASA mass flow][nasa-mass-flow] |
| Δm = ṁ̄Δt | `Mass`, `MassPerTime`, `Time` | Constant or time-averaged mass flow rate over the interval. | [NASA mass flow][nasa-mass-flow] |
| p = Fₙ / A | `Area`, `Force`, `ForcePerArea` | Use the normal force over the surface. The result is the average pressure represented by ForcePerArea. Supported methods include `ForcePerArea.FromNormalForce`. | [OpenStax fluid mechanics][openstax-fluid-mechanics] |
| W = pₑₓₜΔV | `Energy` | Constant external pressure. Expansion gives positive work by the system. Supported methods include `Energy.FromConstantPressureWork`. | [OpenStax pressure–volume work][openstax-pressure-volume-work] |
| μ = ρν | `Density`, `DynamicViscosity`, `KinematicViscosity` | Density, dynamic viscosity, and kinematic viscosity at the same fluid state. | [NASA viscosity and flow][nasa-viscosity-and-flow] |
| Qᵥ = Avₙ | `Area`, `LinearVelocity`, `VolumePerTime` | Area-averaged normal speed component through the section. | [OpenStax fluid mechanics][openstax-fluid-mechanics] · [NASA mass flow][nasa-mass-flow] |
| ΔV = Q̄ᵥΔt | `Time`, `Volume`, `VolumePerTime` | Constant or time-averaged volumetric flow rate. | [OpenStax fluid mechanics][openstax-fluid-mechanics] |

### Electricity and magnetism

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| Hφ = I/(2πr) | `AuxillaryMagneticField` | Azimuthal magnetic field around a long straight wire in an unbounded homogeneous isotropic medium, at positive distance r from the axis. Use the steady signed current I along the axis. Supported methods include `AuxillaryMagneticField.FromLongStraightWire`. | [MIT magnetic fundamentals][mit-straight-wire-field] |
| Q = f₀/Δf | `double` | Quality factor of a linear series RLC circuit. Use its positive resonance frequency and full half-power bandwidth. Supported methods include `Frequency.SeriesRlcQualityFactor`. | [MIT resonant power conversion][mit-rlc-quality-factor] |
| Xᵣₘₛ = Xpeak/√2 | `ElectricCurrent`, `ElectricPotential` | Use the peak voltage or peak current magnitude of a zero-offset sinusoid. Supported methods include `ElectricPotential.FromSinusoidalPeak` and `ElectricCurrent.FromSinusoidalPeak`. | [OpenStax AC power][openstax-ac-power] |
| Vₛ = aVₚ, Iₛ = Iₚ/a | `ElectricCurrent`, `ElectricPotential` | Voltage and current magnitudes in an ideal transformer using its positive turns ratio a = Nₛ/Nₚ. Supported methods include `ElectricPotential.SecondaryVoltageOfIdealTransformer` and `ElectricCurrent.SecondaryCurrentOfIdealTransformer`. | [OpenStax ideal transformers][openstax-transformers] |
| F/ℓ = IB⊥ | `ForcePerLength` | Uniform magnetic field and straight wire. Use current magnitude and the magnitude of the perpendicular magnetic flux density component. Supported methods include `ForcePerLength.FromCurrentInMagneticField`. | [OpenStax magnetic force on a conductor][openstax-magnetic-force-on-wire] |
| τ = p⊥E, τ = j⊥H | `Torque` | Torque magnitude in a uniform field. Use the perpendicular dipole component magnitude and the field magnitude. The magnetic form uses the QUDT dipole moment j in Wb·m and the auxiliary field H in vacuum. Supported methods include `Torque.FromElectricDipole` and `Torque.FromMagneticDipole`. | [OpenStax electric dipoles][openstax-electric-dipoles] · [OpenStax magnetic dipoles][openstax-magnetic-dipoles] · [QUDT quantity definitions][qudt-quantity-definitions] |
| U = −p∥E, U = −j∥H | `Energy` | Orientation-dependent potential energy in a uniform field, zero at perpendicular alignment. Use the signed parallel dipole component and field magnitude. The magnetic form uses the QUDT dipole moment j in Wb·m and the auxiliary field H in vacuum. Supported methods include `Energy.FromElectricDipole` and `Energy.FromMagneticDipole`. | [OpenStax electric dipoles][openstax-electric-dipoles] · [OpenStax magnetic dipoles][openstax-magnetic-dipoles] · [QUDT quantity definitions][qudt-quantity-definitions] |
| Δf = R/(2πL) | `Frequency` | Full width between the half-power frequencies of a linear series RLC circuit. Use the total positive series resistance R and positive inductance L. Supported methods include `Frequency.FromSeriesRlcBandwidth`. | [MIT series RLC bandwidth][mit-rlc-bandwidth] |
| R = g/σ | `ElectricConductivity`, `InverseLength`, `Resistance` | Uniform ohmic conductor with positive conductivity. The geometric factor g is length per cross-sectional area. | [OpenStax resistivity][openstax-resistivity] |
| C = εg | `Capacitance`, `Length`, `Permittivity` | Ideal parallel plates with negligible fringing and a uniform dielectric. The geometric length g is the ratio of overlap area to plate separation. | [OpenStax capacitance of electrode geometries][openstax-capacitor-geometry] |
| Φₑ = q_free/ε | `ElectricCharge`, `ElectricFlux`, `Permittivity` | Electric flux through a closed surface in a homogeneous linear dielectric of constant permittivity. Use the signed enclosed free charge. | [OpenStax Gauss's law][openstax-gauss-law] · [MIT Electromagnetics and Applications, chapter 2][mit-fields-and-media] |
| p = αE | `ElectricDipoleMoment`, `ElectricFieldStrength`, `Polarizability` | Induced dipole in the linear, isotropic response regime. Use signed components along the same axis and the scalar electric polarizability. | [MIT dielectric polarization][mit-dielectric-polarization] |
| P = p/V | `ElectricDipoleMoment`, `PolarizationField`, `Volume` | Use total electric dipole moment in a representative volume. The polarization component uses the same axis as the moment. | [MIT dielectric polarization][mit-dielectric-polarization] |
| Φ_D = DₙA | `Area`, `ElectricCharge`, `ElectricDisplacement` | Flux of electric displacement through a surface. Use the signed mean normal displacement. On a closed surface the net flux is the enclosed free charge. The named method recovers the mean normal component. Supported methods include `ElectricDisplacement.FromDisplacementFlux`. | [MIT Electromagnetics and Applications, chapter 2][mit-fields-and-media] |
| K = I/ℓ | `ElectricCurrent`, `Length`, `LinearElectricCurrent` | Sheet current per unit length measured across a line perpendicular to the current, using current through that line. | [MIT Electromagnetics and Applications, chapter 2][mit-fields-and-media] |
| εᵣ = ε/ε₀, μᵣ = μ/μ₀ | `Permeability`, `Permittivity`, `double` | Use the absolute scalar property of a linear medium and its corresponding positive vacuum value. Supported methods include `Permittivity.RelativePermittivity`, `Permittivity.FromRelativePermittivity`, `Permeability.RelativePermeability`, `Permeability.FromRelativePermeability`. | [MIT dielectric polarization][mit-dielectric-polarization] · [MIT magnetic fundamentals][mit-magnetic-fundamentals] |
| C = 4πεR | `Capacitance` | An isolated conducting sphere of positive radius in an unbounded homogeneous linear dielectric. Supported methods include `Capacitance.FromIsolatedSphere`. | [OpenStax capacitance of electrode geometries][openstax-capacitor-geometry] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| fc = 1/(2πτ) | `Frequency` | Half-power cutoff of a first-order RC or RL filter. Use positive time constant RC or L/R as appropriate. Supported methods include `Frequency.FromFirstOrderCutoff`. | [University of Oklahoma AC filters][ou-first-order-filters] |
| Rseries = R₁ + R₂ | `Resistance` | Ohmic resistors in series carry the same current. Supported methods include `Resistance.InSeriesWith`. | [OpenStax RC circuits][openstax-rc-circuits] |
| 1/Rparallel = 1/R₁ + 1/R₂ | `Resistance` | Two positive ohmic resistances connected between the same nodes. Supported methods include `Resistance.InParallelWith`. | [OpenStax RC circuits][openstax-rc-circuits] |
| 1/Cseries = 1/C₁ + 1/C₂ | `Capacitance` | Two positive ideal capacitors in series. Supported methods include `Capacitance.InSeriesWith`. | [OpenStax capacitance][openstax-capacitance] |
| Cparallel = C₁ + C₂ | `Capacitance` | Two ideal capacitors connected across the same nodes. Supported methods include `Capacitance.InParallelWith`. | [OpenStax capacitance][openstax-capacitance] |
| Lseries = L₁ + L₂ | `Inductance` | Linear series inductors with negligible mutual coupling. Supported methods include `Inductance.InSeriesWith`. | [MIT circuit energy storage][mit-inductors] |
| 1/Lparallel = 1/L₁ + 1/L₂ | `Inductance` | Two positive linear inductors in parallel with negligible mutual coupling. Supported methods include `Inductance.InParallelWith`. | [MIT circuit energy storage][mit-inductors] |
| ℱ = NI | `MagnetomotiveForce` | Winding with N series turns sharing the same signed current and orientation. Supported methods include `MagnetomotiveForce.FromWinding`. | [OpenStax Ampère's law][openstax-ampere] · [QUDT magnetomotive force][qudt-magnetomotive-force] |
| ℱ = H∥ℓ | `AuxillaryMagneticField`, `Length`, `MagnetomotiveForce` | Constant tangential component of magnetic field strength along the path, or the average tangential component over the path. | [OpenStax Ampère's law][openstax-ampere] · [QUDT magnetomotive force][qudt-magnetomotive-force] |
| P = I²R = V² / R | `Power` | Joule dissipation in a positive ohmic resistance. Use instantaneous values for instantaneous power, or RMS current or voltage for average power. Supported methods include `Power.FromResistiveHeating`. | [OpenStax electrical power][openstax-electrical-power] · [OpenStax AC power][openstax-ac-power] |
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
| ΔU = QΔV | `ElectricCharge`, `ElectricPotential`, `Energy` | Change in electrical potential energy for charge moved through a fixed potential difference. | [OpenStax electric potential][openstax-electric-potential] |
| τ = RC | `Capacitance`, `Resistance`, `Time` | Time constant of the equivalent first-order RC circuit. | [OpenStax RC circuits][openstax-rc-circuits] |
| τ = L / R | `Inductance`, `Resistance`, `Time` | Time constant of the equivalent first-order RL circuit. | [OpenStax RL circuits][openstax-rl-circuits] |
| Λ = LI | `MagneticFlux` | Signed self-flux linkage summed over all turns of a linear inductor. Supported methods include `MagneticFlux.LinkageFromInductor`. | [OpenStax self-inductance][openstax-self-inductance] |
| U = ½LI² | `Energy` | Magnetic energy stored in a linear inductor of fixed inductance. Supported methods include `Energy.FromInductor`. | [OpenStax magnetic energy][openstax-magnetic-energy] |
| ω₀ = 1 / √(LC), f₀ = ω₀ / (2π) | `AngularVelocity`, `Frequency` | Ideal lossless LC circuit with positive inductance and capacitance. Supported methods include `AngularVelocity.FromLcResonance` and `Frequency.FromLcResonance`. | [OpenStax LC oscillations][openstax-lc-oscillations] |
| Q = σqA | `Area`, `ElectricCharge`, `ElectricChargePerArea` | Uniform or mean surface charge density σq over the area. | [OpenStax electric fields][openstax-electric-fields] · [QUDT quantity definitions][qudt-quantity-definitions] |
| Q = ρqV | `ElectricCharge`, `ElectricChargeVolumeDensity`, `Volume` | Uniform or mean volume charge density ρq over the region. | [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |

The QUDT SI catalog exposes `MagneticDipoleMoment` in Wb·m with j = μ₀m.
The current-loop relation m = IA and the magnetization relation M = m/V use magnetic moment m in A·m², which has no quantity type in this SI catalog.
[QUDT magnetic quantity definitions][qudt-quantity-definitions]

### Heat and temperature

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| q″ = −k∇T | `HeatFlowRatePerArea`, `double` | One-dimensional conduction with positive scalar thermal conductivity k. Use signed heat flux and temperature gradient along the same axis. The gradient is measured in kelvins per metre. Supported methods include `ThermalConductivity.HeatFluxForTemperatureGradient` and `ThermalConductivity.TemperatureGradientInKelvinsPerMetre`. | [MIT Fourier law of conduction][mit-fourier-conduction] |
| Cₚ − Cᵥ = R | `MolarHeatCapacity` | Mayer's relation for constant-pressure and constant-volume molar heat capacities of an ideal gas at the same temperature. Uses the fixed molar gas constant. Supported methods include `MolarHeatCapacity.ToIsobaricForIdealGas` and `MolarHeatCapacity.ToIsochoricForIdealGas`. | [OpenStax ideal-gas heat capacity][openstax-ideal-gas-heat-capacity] |
| U = ³⁄₂nRT | `Energy` | Classical monatomic ideal gas with internal thermal energy due to translational motion. Use amount n, positive absolute temperature T and the fixed molar gas constant. Supported methods include `Energy.FromMonatomicIdealGas`. | [OpenStax ideal-gas heat capacity][openstax-ideal-gas-heat-capacity] · [NIST CODATA physical constants][nist-codata-constants] |
| p = cRT | `AmountOfSubstancePerVolume`, `ForcePerArea`, `ThermodynamicTemperature` | Ideal gas in equilibrium. Relate absolute pressure, amount concentration c, and positive absolute temperature T using the fixed molar gas constant R. Supported methods include `ForcePerArea.FromIdealGasConcentration`, `AmountOfSubstancePerVolume.FromIdealGasPressure`, and `ThermodynamicTemperature.FromIdealGasPressure`. | [OpenStax ideal gas law][openstax-ideal-gas] · [NIST CODATA physical constants][nist-codata-constants] |
| vrms = √(3RT/M) | `LinearVelocity` | Molecular RMS speed of a classical ideal gas in thermal equilibrium. Use positive absolute temperature T and molar mass M. The molar gas constant R is fixed. Supported methods include `LinearVelocity.RmsSpeedOfIdealGas`. | [OpenStax molecular speed][openstax-rms-gas-speed] · [NIST CODATA physical constants][nist-codata-constants] |
| Le = α/D | `double` | Same species and fluid state. Use thermal diffusivity α and positive molecular mass diffusivity D, represented by `AreaPerTime`. Supported methods include `ThermalDiffusivity.LewisNumber`. | [Portland State University heat and mass transfer, table 6.2][psu-transport-groups] |
| Mₑ = σT⁴ | `PowerPerArea` | Total radiant exitance of a blackbody. Supply the Stefan–Boltzmann constant as `PowerPerAreaQuarticTemperature` and absolute temperature in kelvins. Supported methods include `PowerPerArea.FromBlackbodyTemperature`. | [OpenStax blackbody radiation][openstax-wien] |
| ΔU = Q − W | `Energy` | First law for a closed system with no change in macroscopic kinetic or potential energy. Use signed heat entering the system and work it does on its surroundings. Supported methods include `Energy.FromHeatAndWork`. | [OpenStax first law of thermodynamics][openstax-first-law] |
| λmax T = b | `Length`, `LengthTemperature`, `ThermodynamicTemperature` | Blackbody spectral radiance per unit wavelength at positive absolute temperature. Use the wavelength form of the Wien displacement constant b. | [OpenStax blackbody radiation][openstax-wien] |
| γ = cₚ / cᵥ | `double` | Use two `SpecificHeatCapacity` inputs with units of J/(kg·K), measured at constant pressure and constant volume for the same gas state. Supported methods include `SpecificHeatCapacity.HeatCapacityRatio`. | [NASA specific heat ratio][nasa-specific-heat-ratio] |
| η = Wnet / Qhot | `double` | Net work delivered and positive heat absorbed from the hot reservoir over the same engine cycle. Supported methods include `Energy.HeatEngineEfficiency`. | [OpenStax heat engines][openstax-heat-engines] |
| ηCarnot = 1 − Tcold / Thot | `double` | Reversible engine between two reservoirs at positive absolute temperatures, with the hot reservoir warmer. Supported methods include `ThermodynamicTemperature.CarnotEfficiency`. | [OpenStax Carnot cycle][openstax-carnot] |
| COPcool = Tcold / (Thot − Tcold) | `double` | Reversible refrigerator between the given positive absolute temperatures. The numerator uses the cold reservoir. Supported methods include `ThermodynamicTemperature.CarnotRefrigeratorCoefficientOfPerformance`. | [OpenStax Carnot cycle][openstax-carnot] |
| COPheat = Thot / (Thot − Tcold) | `double` | Reversible heat pump between the given positive absolute temperatures. The numerator uses the hot reservoir. Supported methods include `ThermodynamicTemperature.CarnotHeatPumpCoefficientOfPerformance`. | [OpenStax Carnot cycle][openstax-carnot] |
| τ = RₜC | `HeatCapacity`, `ThermalResistance`, `Time` | Single thermal lump with positive constant thermal resistance and heat capacity. For convection, Rₜ = 1/(hA). | [Michigan Tech lumped heat transfer][michigan-tech-lumped-heat] |
| Rseries = R₁ + R₂ | `ThermalResistance` | Series thermal paths with the same rate of heat transfer through each resistance. Supported methods include `ThermalResistance.InSeriesWith`. | [Princeton thermal resistance notes, §§8.7–8.8][princeton-thermal-resistance] |
| 1 / Rparallel = 1 / R₁ + 1 / R₂ | `ThermalResistance` | Two positive thermal resistances connecting the same pair of temperature nodes. Supported methods include `ThermalResistance.InParallelWith`. | [Princeton thermal resistance notes, §§8.7–8.8][princeton-thermal-resistance] |
| Pr = ν / α | `double` | Use kinematic viscosity and thermal diffusivity for the same fluid state. | [MIT heat and mass transfer dimensionless numbers][mit-transport-numbers] |
| R″ = ARₜ | `Area`, `ThermalInsulance`, `ThermalResistance` | Relate total thermal resistance to area-normalized resistance for the same uniform surface. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient] |
| Pheat = q″A | `Area`, `HeatFlowRate`, `HeatFlowRatePerArea` | Use average normal heat flux over the surface. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient] |
| Rₜ = 1 / (hA) | `ThermalResistance` | Use a positive uniform or area-averaged heat-transfer coefficient h and the area A of the same surface. Supported methods include `ThermalResistance.FromHeatTransferCoefficient`. | [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient] · [QUDT thermal conductance][qudt-thermal-conductance] |
| R″ = ℓ/k = ℓρₜ | `Length`, `ThermalConductivity`, `ThermalInsulance`, `ThermalResistivity` | Uniform plane layer. Use thickness along the heat-flow direction. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT thermal resistivity][qudt-thermal-resistivity] |
| Q = CΔT | `Energy`, `HeatCapacity` | Sensible heat with constant or interval-averaged heat capacity. Supported methods include `HeatCapacity.EnergyForTemperatureChange`, `HeatCapacity.FromEnergyAndTemperatureChange`. | [OpenStax heat and conduction][openstax-heat-and-conduction] |
| C = mc | `HeatCapacity`, `Mass`, `SpecificHeatCapacity` | Use mass-specific heat capacity for the specified thermodynamic process. | [OpenStax heat and conduction][openstax-heat-and-conduction] |
| q″ = hΔT | `HeatFlowRatePerArea` | Transfer coefficient h is per unit area. Use the driving temperature difference in the indicated direction. Supported methods include `CoefficientOfHeatTransfer.HeatFluxForTemperatureChange`. | [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient] |
| ΔT = T₂ − T₁, T₂ = T₁ + ΔT | `ThermodynamicTemperature`, `double` | Subtract two absolute temperatures to get a signed difference in kelvins. Add that difference to an absolute temperature to get another absolute temperature. Supported methods include `ThermodynamicTemperature.AddKelvins`, `ThermodynamicTemperature.DifferenceInKelvins`. | [BIPM SI Brochure][bipm-si-brochure] |
| ΔT = (dT/dt)̄Δt | `double` | Temperature-change rate is constant or averaged over the interval. Supported methods include `TemperaturePerTime.ChangeInKelvins`. | [QUDT quantity definitions][qudt-quantity-definitions] |
| ΔT = q″R″ | `double` | Use areal thermal resistance and the corresponding heat-flux component. Supported methods include `ThermalInsulance.TemperatureDropInKelvins`. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT heat-transfer coefficient][qudt-heat-transfer-coefficient] |
| α = k / cᵥ | `ThermalConductivity`, `ThermalDiffusivity`, `VolumetricHeatCapacity` | Thermal diffusivity for scalar conductivity and volumetric heat capacity. | [NIST TN 622][nist-tn-622] |
| ΔL = L′ΔT, ΔA = A′ΔT, ΔV = V′ΔT | `Area`, `Length`, `Volume` | Use the object's average dimensional expansion per kelvin over the interval, or the local slope for a small temperature change. The result is the change in size. Supported methods include `AreaThermalExpansion.AreaChange`, `LinearThermalExpansion.LengthChange`, `VolumeThermalExpansion.VolumeChange`. | [OpenStax thermal expansion][openstax-thermal-expansion] · [QUDT quantity definitions][qudt-quantity-definitions] |
| Pheat = ΔT / Rₜ | `HeatFlowRate`, `ThermalResistance`, `double` | Use total thermal resistance and a signed temperature drop in kelvins for the same thermal path. Supported methods include `ThermalResistance.FromTemperatureDrop`, `ThermalResistance.HeatFlowForTemperatureChange`, `ThermalResistance.TemperatureDropInKelvins`. | [OpenStax heat and conduction][openstax-heat-and-conduction] · [QUDT thermal conductance][qudt-thermal-conductance] |
| ρₜ = 1 / k | `ThermalConductivity`, `ThermalResistivity`, `double` | Thermal resistivity and conductivity describe the same material and direction. | [QUDT thermal resistivity][qudt-thermal-resistivity] |
| cᵥ = ρc | `Density`, `SpecificHeatCapacity`, `VolumetricHeatCapacity` | Density and mass-specific heat capacity describe the same state. | [NIST TN 622][nist-tn-622] |
| C = Vcᵥ | `HeatCapacity`, `Volume`, `VolumetricHeatCapacity` | Uniform or volume-averaged volumetric heat capacity cᵥ. | [NIST SP 811][nist-sp-811] |

### Optics and diffraction

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| sin θₜ = (nᵢ/nₜ) sin θᵢ | `PlaneAngle`, `bool` | Snell's law at a planar interface. Supply the positive refractive-index ratio as one dimension-one quantity and the signed incident angle within ±π/2 from the normal. `PlaneAngle.TryRefractionAngle` returns false when no real transmitted angle exists. | [OpenStax law of refraction][openstax-snell-law] |
| θ₁ = asin(λ/d) | `PlaneAngle` | For normal incidence use the grating period d for the first principal order, or slit width d for the first single-slit minimum. Use positive wavelength λ in the medium and d at least as large. Supported methods include `PlaneAngle.FromFirstDiffractionOrder` and `PlaneAngle.FromFirstSingleSlitMinimum`. | [OpenStax diffraction gratings][openstax-diffraction-gratings] · [OpenStax single-slit diffraction][openstax-single-slit] |
| R = ((n₁ − n₂)/(n₁ + n₂))² | `double` | Power reflectance for normal incidence at one uncoated interface between lossless nonmagnetic media with positive real refractive indices. Supported methods include `Power.ReflectanceAtNormalIncidence`. | [MIT ray optics, lecture 4][mit-normal-incidence-reflectance] |
| NA = n sin θ | `double` | Object-space numerical aperture. Use the refractive index and acceptance half-angle of the objective. Supported methods include `PlaneAngle.NumericalAperture`. | [Columbia optical microscopy][columbia-microscopy] |
| δ ≈ 0.61λ₀/NA | `Length` | Lateral Rayleigh resolution of a diffraction-limited microscope with circular pupil. Use the positive object-space numerical aperture and vacuum wavelength. Supported methods include `Length.RayleighMicroscopeResolution`. | [Columbia optical microscopy][columbia-microscopy] |
| 1/f = 1/dₒ + 1/dᵢ | `Length` | Paraxial thin lens with signed distances. Under the real-is-positive convention, real object and real image distances are positive. For a virtual image, dᵢ is negative. Supported methods include `Length.FocalLengthFromConjugates` and `Length.ImageDistanceFromThinLens`. | [OpenStax thin lenses][openstax-thin-lenses] |
| m = −dᵢ / dₒ | `double` | Paraxial thin-lens lateral magnification with the same distance convention. Negative magnification gives an inverted image. Supported methods include `Length.LateralMagnification`. | [OpenStax thin lenses][openstax-thin-lenses] |
| n = c / vphase | `LinearVelocity`, `double` | Phase refractive index at a specified wavelength. Use the vacuum light speed c and the positive phase speed in the medium. Supported methods include `LinearVelocity.FromRefractiveIndex` and `LinearVelocity.RefractiveIndex`. | [OpenStax refraction][openstax-refraction] |
| θ꜀ = asin(n₂/n₁) | `PlaneAngle` | Critical angle from the normal when the incident medium has the higher positive refractive index. Supported methods include `PlaneAngle.CriticalAngle`. | [OpenStax total internal reflection][openstax-total-internal-reflection] |
| θᵦ = atan(n₂/n₁) | `PlaneAngle` | Brewster incidence angle from the normal for an interface between lossless, nonmagnetic media. Supported methods include `PlaneAngle.BrewsterAngle`. | [OpenStax polarization][openstax-polarization] |
| I = I₀ cos² θ | `PowerPerArea` | Malus's law for linearly polarized incident light and an ideal polarizer. θ is the angle from the polarization to the transmission axis. Supported methods include `PowerPerArea.AfterIdealPolarizer`. | [OpenStax polarization][openstax-polarization] |
| θmin ≈ 1.22 λ / D | `PlaneAngle` | Small-angle Rayleigh criterion for a circular aperture of positive diameter D and wavelength λ. Supported methods include `PlaneAngle.FromRayleighCriterion`. | [OpenStax diffraction equations][openstax-diffraction] |
| Δφ = 2πΔLopt / λ₀ | `PlaneAngle` | Coherent monochromatic waves. Use signed optical path difference and positive vacuum wavelength, with no additional phase shifts. Supported methods include `PlaneAngle.FromOpticalPathDifference`. | [OpenStax mathematics of interference][openstax-interference] |

### Photometry and radiometry

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| Aproj = A cos θ | `Area` | Planar surface with the projection direction at an angle θ of zero to π/2 from the surface normal. Supported methods include `Area.ProjectAlong`. | [CIE radiance definition][cie-radiance] |
| G = AprojΩ | `Area`, `AreaAngle`, `SolidAngle` | Geometric extent of a ray bundle in the uniform projected-area approximation. Use the projected area and the solid angle subtended by the bundle. | [CIE geometric extent][cie-geometric-extent] |
| Lₑ = Φₑ/G | `AreaAngle`, `Power`, `PowerPerAreaAngle` | Average radiance over the optical bundle. Divide radiant flux by the geometric extent of the same bundle. `PowerPerAreaAngle` uses W/(m²·sr). | [CIE radiance definition][cie-radiance] |
| Hₑ = ĒₑΔt | `EnergyPerArea`, `PowerPerArea`, `Time` | Radiant exposure on a surface. Use average incident irradiance and the duration of the same interval. | [CIE radiant exposure][cie-radiant-exposure] |
| Mᵥ = Φᵥ,emitted/A | `Area`, `LuminousFlux`, `LuminousFluxPerArea` | Average luminous exitance. Use exiting flux and the corresponding emitting surface area. Supported methods include `LuminousFluxPerArea.FromEmittedFlux`. | [CIE luminous exitance][cie-luminous-exitance] |
| Mᵥ = π sr × Lᵥ | `LuminousFluxPerArea` | Hemispherical luminous exitance of a uniform Lambertian surface with luminance Lᵥ. Supported methods include `Luminance.ToLambertianExitance`. | [CIE Lambertian surface][cie-lambert-surface] |
| I(θ) = Iₙ cos θ | `LuminousIntensity`, `RadiantIntensity` | Lambertian surface element. Use the intensity in the normal direction and angle from zero to π/2 from that normal. Supported methods include `LuminousIntensity.AtLambertianAngle` and `RadiantIntensity.AtLambertianAngle`. | [CIE Lambert cosine law][cie-lambert-cosine] |
| Φ = π sr × Iₙ | `LuminousFlux`, `Power` | Total luminous flux or radiant power emitted into one hemisphere by a Lambertian surface. From M = πL and the normal intensity Iₙ = LA for a uniform emitting surface. Supported methods include `LuminousIntensity.ToLambertianFlux` and `RadiantIntensity.ToLambertianFlux`. | [CIE Lambertian surface][cie-lambert-surface] · [NIST photometric quantities][nist-photometric-quantities] |
| Eᵥ = Φᵥ,incident / A | `Area`, `Illuminance`, `LuminousFlux` | Incident luminous flux averaged over the receiver area. | [NIST photometric quantities][nist-photometric-quantities] · [NIST photometric equations 2–6][nist-photometric-equations-2-6] |
| Iᵥ = LᵥAproj, Iₑ = LₑAproj | `Area`, `Luminance`, `LuminousIntensity`, `PowerPerAreaAngle`, `RadiantIntensity` | Use luminance or radiance averaged over the projected area, with intensity in the same direction. `PowerPerAreaAngle` represents radiance in W/(m²·sr). | [CIE radiance definition][cie-radiance] · [NIST photometric quantities][nist-photometric-quantities] |
| Φᵥ = ηᵥP | `LuminousEfficacy`, `LuminousFlux`, `Power` | Use the power basis for which the luminous efficacy was specified. | [NIST photometric quantities][nist-photometric-quantities] · [QUDT quantity definitions][qudt-quantity-definitions] |
| Qᵥ = Φ̄ᵥΔt | `LuminousEnergy`, `LuminousFlux`, `Time` | Constant or time-averaged luminous flux over the interval. | [NIST photometric quantities][nist-photometric-quantities] |
| Φᵥ = IᵥΩ | `LuminousFlux`, `LuminousIntensity`, `SolidAngle` | Use mean luminous intensity over the solid angle. | [NIST photometric quantities][nist-photometric-quantities] · [NIST photometric equations 2–6][nist-photometric-equations-2-6] |
| Eᵥ = Iᵥ / r² | `Illuminance` | Point source at positive distance with the receiver perpendicular to the incident ray. Use luminous intensity in the direction of the receiver. Supported methods include `Illuminance.FromNormalPointSource`. | [CIE photometric distance law][cie-photometric-distance] |
| Eₑ = Iₑ / r² | `PowerPerArea` | Point source at positive distance with the receiver perpendicular to the incident ray. Use radiant intensity in the direction of the receiver. Supported methods include `PowerPerArea.FromNormalPointSource`. | [CIE irradiance][cie-irradiance] · [CIE radiant intensity][cie-radiant-intensity] · [CIE basic law of radiometry and photometry][cie-basic-radiometry] |
| Φₑ = IₑΩ | `Power`, `RadiantIntensity`, `SolidAngle` | Use mean radiant intensity over the solid angle. | [NIST photometric quantities][nist-photometric-quantities] |

### Chemistry and radiation dose

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| N/N₀ = exp(−t/τ) | `double` | Single exponential decay or a first-order transient returning to equilibrium. Use nonnegative elapsed time and positive time constant. Supported methods include `Time.ExponentialDecayFraction`. | [OpenStax radioactive decay][openstax-radioactive-decay] · [OpenStax RC circuits][openstax-rc-transient] |
| t½ = τ ln 2 | `Time` | Relate the half-life and positive e-folding time constant of a single exponential decay. Supported methods include `Time.HalfLifeFromDecayTimeConstant` and `Time.FromExponentialHalfLife`. | [OpenStax radioactive decay][openstax-radioactive-decay] |
| I/I₀ = exp(−μx) | `double` | Monoenergetic narrow beam through homogeneous material. Use nonnegative linear attenuation coefficient μ and path length x. Supported methods include `InverseLength.TransmissionThrough`. | [NIST photon attenuation, equation 1][nist-attenuation-law] |
| Aavg = Ndecay/Δt | `Activity` | Average activity over a positive time interval. Use the number of nuclear transformations in the sample during that interval. Supported methods include `Activity.FromDecayCount`. | [OpenStax radioactive decay][openstax-radioactive-decay] · [BIPM SI Brochure][bipm-si-brochure] |
| pᵢ = xᵢptotal | `ForcePerArea` | Dalton's law for a constituent of an ideal gas mixture. Use its gas-phase mole fraction and the total absolute pressure. Supported methods include `ForcePerArea.PartialPressureInIdealGas`. | [OpenStax partial pressures][openstax-dalton] |
| pᵢ = xᵢpᵢ* | `ForcePerArea` | Raoult's law for a constituent of an ideal liquid solution in vapour equilibrium. Use its liquid mole fraction and the pure-component saturation pressure at the same temperature. Supported methods include `ForcePerArea.VapourPartialPressureInIdealSolution`. | [OpenStax Raoult's law][openstax-raoult] |
| N = nNₐ | `AmountOfSubstance`, `InverseAmountOfSubstance`, `double` | Use amount of substance for the specified entities and the Avogadro constant in reciprocal moles. The count is a dimension-one scalar. | [BIPM definition of the mole][bipm-mole] |
| H = DQ | `DoseEquivalent` | Operational dose equivalent at a specified point. Use the absorbed dose and the mean radiation quality factor for that same point. Supported methods include `DoseEquivalent.FromAbsorbedDose`. | [IAEA operational dose quantities][iaea-dose-equivalent] |
| X = abs(Qion) / mₐᵢᵣ | `Exposure` | Use the total charge of ions of one sign per mass of air when electrons liberated by the photons are fully stopped in air. Supported methods include `Exposure.FromIonizationInAir`. | [IAEA radiation dosimetry handbook][iaea-exposure] |
| x½ = ln 2 / μ | `Length` | Half-value thickness for a narrow monoenergetic beam in a homogeneous material. Use the positive linear attenuation coefficient at the photon energy. Supported methods include `Length.HalfValueThickness`. | [NIST half-value layer][nist-half-value-layer] |
| wᵢ = mᵢ/m, xᵢ = nᵢ/n, φᵢ = Vᵢ/ΣVⱼ | `double` | Use the mass, amount or volume of the component and the corresponding positive total over all components. Volume fractions use each volume before mixing. Supported methods include `Mass.MassFraction`, `AmountOfSubstance.AmountFraction`, `Volume.VolumeFraction`. | [IUPAC fractions][iupac-fraction] · [IUPAC volume fraction][iupac-volume-fraction] |
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
| S = EH | `PowerPerArea` | Instantaneous Poynting flux magnitude for mutually orthogonal electric field and auxiliary magnetic field magnitudes. Supported methods include `PowerPerArea.FromOrthogonalElectromagneticFields`. | [MIT Poynting theorem][mit-fields-and-media] |
| pᵣ = I/c, pᵣ = 2I/c | `ForcePerArea` | Normally incident radiation in vacuum with positive light speed c, absorbed perfectly or reflected specularly without loss, respectively. Supported methods include `ForcePerArea.FromAbsorbedRadiation` and `ForcePerArea.FromReflectedRadiation`. | [OpenStax electromagnetic radiation pressure][openstax-radiation-pressure] |
| uₑ = ½εE² | `EnergyDensity` | Electric field contribution to energy density in a linear, isotropic, nondispersive medium. Use instantaneous field magnitude E and positive absolute permittivity ε. Supported methods include `EnergyDensity.FromElectricField`. | [MIT Physics of Waves, equation 12.24][mit-field-energy] |
| uₘ = B²/(2μ) | `EnergyDensity` | Magnetic field contribution to energy density in a linear, isotropic, nondispersive medium. Use instantaneous magnetic flux density magnitude B and positive absolute permeability μ. Supported methods include `EnergyDensity.FromMagneticField`. | [MIT Physics of Waves, equation 12.24][mit-field-energy] |
| v = 1/√(εμ) | `LinearVelocity` | Phase speed in a lossless, isotropic, nondispersive linear medium with positive absolute permittivity and permeability. Supported methods include `LinearVelocity.FromElectromagneticMedium`. | [OpenStax electromagnetic wave equations][openstax-em-wave-equations] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |
| E = vB | `ElectricFieldStrength`, `MagneticFluxDensity` | Progressive plane wave in a lossless medium. Use matching electric and magnetic magnitudes and the phase speed. Supported methods include `ElectricFieldStrength.FromPlaneWave` and `MagneticFluxDensity.FromPlaneWave`. | [OpenStax electromagnetic wave energy][openstax-em-wave-energy] · [MIT linear electromagnetic media][mit-linear-electromagnetic-media] |

### Photons and matter waves

| Formula | Result types | Applicability and named entry points | Source |
| --- | --- | --- | --- |
| λ = h/p | `Length` | De Broglie wavelength of a particle of positive momentum magnitude. Uses the SI Planck constant. Supported methods include `Length.FromDeBroglieMomentum`. | [OpenStax de Broglie matter waves][openstax-de-broglie] · [NIST CODATA physical constants][nist-codata-constants] |
| Eₘλ = Nₐhc | `Length`, `LengthMolarEnergy`, `MolarEnergy` | Energy per mole of photons at a single vacuum wavelength. Supply the molar energy–wavelength product as a `LengthMolarEnergy` when solving for either input. | [OpenStax photon equations][openstax-photon-equations] · [BIPM definition of the mole][bipm-mole] |
| p = E/c | `Energy`, `LinearMomentum` | Photon in vacuum. Use energy and momentum magnitude and the positive vacuum light speed c. Supported methods include `LinearMomentum.FromPhotonEnergy` and `Energy.FromPhotonMomentum`. | [OpenStax photon equations][openstax-photon-equations] |
| Eγ = Eupper − Elower | `Energy` | Photon energy emitted in a downward transition between two bound levels under the bound-state model. Supported methods include `Energy.FromEmissionTransition`. | [OpenStax one-electron atom][openstax-bohr] |
| E = hf, Eλ = hc | `Energy`, `Frequency`, `Length`, `LengthEnergy` | Single-photon energy. Use `Energy.FromPhotonFrequency` or `Energy.ToPhotonFrequency` with the fixed SI Planck constant, or supply hc as `LengthEnergy` when solving for photon energy or vacuum wavelength. | [OpenStax photon equations][openstax-photon-equations] · [NIST CODATA physical constants][nist-codata-constants] |
| Δλ = λ꜀(1 − cos θ) | `Length` | Compton scattering from an initially stationary free electron. Use the electron Compton wavelength and photon scattering angle. Supported methods include `Length.ComptonWavelengthShift`. | [OpenStax Compton scattering][openstax-compton] |
| Kmax = Eγ − W | `Energy`, `bool` | Single-photon photoemission with material work function W. The method returns false below the emission threshold. Supported methods include `Energy.TryPhotoelectronKineticEnergy`. | [OpenStax photoelectric effect][openstax-photoelectric] |

## Formula references

- [MIT series RLC bandwidth][mit-rlc-bandwidth]
- [MIT Physics of Waves, equation 12.24][mit-field-energy]
- [BIPM SI Brochure][bipm-si-brochure]
- [BIPM katal resolution][bipm-katal-resolution]
- [CIE irradiance][cie-irradiance]
- [CIE radiant intensity][cie-radiant-intensity]
- [IAEA dosimetry][iaea-dosimetry]
- [IUPAC amount concentration][iupac-amount-concentration]
- [IUPAC molar mass][iupac-molar-mass]
- [IUPAC molar quantities][iupac-molar-quantities]
- [MIT linear electromagnetic media][mit-linear-electromagnetic-media]
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
- [OpenStax electrical power][openstax-electrical-power]
- [MIT surface gravity waves][mit-surface-gravity-waves]
- [NACA compressible flow report][naca-compressible-flow]
- [OpenStax beats][openstax-beats]
- [OpenStax friction][openstax-friction]
- [OpenStax sound intensity][openstax-sound-intensity]
- [OpenStax speed of sound][openstax-speed-of-sound]
- [OpenStax wave speed on a string][openstax-stretched-string]
- [USBR friction head loss][usbr-friction]
- [NMSU pipe flow manual][nmsu-pipe-networks]
- [MIT interfacial pressure][mit-interfacial-pressure]
- [NASA specific heat ratio][nasa-specific-heat-ratio]
- [OpenStax Carnot cycle][openstax-carnot]
- [OpenStax heat engines][openstax-heat-engines]
- [Princeton thermal resistance notes, §§8.7–8.8][princeton-thermal-resistance]
- [Michigan Tech lumped heat transfer][michigan-tech-lumped-heat]
- [MIT heat and mass transfer dimensionless numbers][mit-transport-numbers]
- [QUDT magnetomotive force][qudt-magnetomotive-force]
- [MIT circuit energy storage][mit-inductors]
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
- [NIST half-value layer][nist-half-value-layer]
- [IUPAC fractions][iupac-fraction]
- [IUPAC volume fraction][iupac-volume-fraction]
- [OpenStax stress, strain, and elastic modulus][openstax-elasticity]
- [Purdue elastic strain energy][purdue-elastic-energy]
- [University of Hawaiʻi elastic moduli][hawaii-elastic-moduli]
- [MIT circular shaft torsion][mit-shaft-torsion]
- [OpenStax moments of inertia reference table][openstax-rotation-tables]
- [OpenStax special relativity equations][openstax-relativity]
- [OpenStax damped oscillations][openstax-damped-oscillations]
- [OpenStax electromagnetic wave equations][openstax-em-wave-equations]
- [OpenStax electromagnetic wave energy][openstax-em-wave-energy]
- [University of Oklahoma AC filters][ou-first-order-filters]
- [OpenStax blackbody radiation][openstax-wien]
- [OpenStax photon equations][openstax-photon-equations]
- [OpenStax Compton scattering][openstax-compton]
- [OpenStax photoelectric effect][openstax-photoelectric]
- [NASA JPL spectral redshift][nasa-spectral-redshift]
- [OpenStax Torricelli's theorem][openstax-torricelli]
- [OpenStax partial pressures][openstax-dalton]
- [OpenStax Raoult's law][openstax-raoult]
- [OpenStax Kepler's laws][openstax-kepler]
- [OpenStax capacitance of electrode geometries][openstax-capacitor-geometry]
- [OpenStax one-electron atom][openstax-bohr]
- [Columbia optical microscopy][columbia-microscopy]
- [CIE Lambert cosine law][cie-lambert-cosine]
- [CIE Lambertian surface][cie-lambert-surface]
- [MIT dielectric polarization][mit-dielectric-polarization]
- [MIT Electromagnetics and Applications, chapter 2][mit-fields-and-media]
- [MIT magnetic fundamentals][mit-magnetic-fundamentals]
- [OpenStax Gauss's law][openstax-gauss-law]
- [CIE luminous exitance][cie-luminous-exitance]
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
[openstax-electrical-power]: https://openstax.org/books/university-physics-volume-2/pages/9-5-electrical-energy-and-power
[mit-surface-gravity-waves]: https://ocw.mit.edu/courses/12-802-wave-motion-in-the-ocean-and-the-atmosphere-spring-2008/fca533a5edb5f5d3e975ec4c2e63019f_MIT12_802S08_lec03.pdf
[naca-compressible-flow]: https://www.grc.nasa.gov/www/k-12/airplane/Images/naca1135.pdf
[openstax-beats]: https://openstax.org/books/university-physics-volume-1/pages/17-6-beats
[openstax-friction]: https://openstax.org/books/university-physics-volume-1/pages/6-2-friction
[openstax-sound-intensity]: https://openstax.org/books/university-physics-volume-1/pages/17-3-sound-intensity
[openstax-speed-of-sound]: https://openstax.org/books/university-physics-volume-1/pages/17-2-speed-of-sound
[openstax-stretched-string]: https://openstax.org/books/university-physics-volume-1/pages/16-3-wave-speed-on-a-stretched-string
[usbr-friction]: https://www.usbr.gov/tsc/techreferences/mands/wmm/chap02_16.html
[nmsu-pipe-networks]: https://nmwrri.nmsu.edu/footer_pages/nm-wrri-library-database-files/wrri-library-pdfs/wrrilibrary7/007529.pdf
[mit-interfacial-pressure]: https://web.mit.edu/fluids-modules/www/potential_flows/LecturesHTML/lec04/lecture4.html
[nasa-specific-heat-ratio]: https://www.grc.nasa.gov/www/k-12/airplane/isndrv.html
[openstax-carnot]: https://openstax.org/books/university-physics-volume-2/pages/4-5-the-carnot-cycle
[openstax-heat-engines]: https://openstax.org/books/university-physics-volume-2/pages/4-2-heat-engines
[princeton-thermal-resistance]: https://rosenreview.cbe.princeton.edu/sites/g/files/toruqf6461/files/documents/Transport_I_12_14_0.pdf
[michigan-tech-lumped-heat]: https://pages.mtu.edu/~fmorriso/cm3120/lectures/2021_CM3120_module2_6blectureV.pdf
[mit-transport-numbers]: https://ocw.mit.edu/courses/3-185-transport-phenomena-in-materials-engineering-fall-2003/a55c74dc21def01fc0054de35574d1e5_3_21.pdf
[qudt-magnetomotive-force]: https://qudt.org/vocab/quantitykind/MagnetomotiveForce
[mit-inductors]: https://circuits.mit.edu/_static/S23/handouts/lec07a/lecture07a.pdf
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
[nist-half-value-layer]: https://srd.nist.gov/NSRDS/NSRDS-NBS29.pdf
[iupac-fraction]: https://goldbook.iupac.org/terms/view/F02494
[iupac-volume-fraction]: https://goldbook.iupac.org/terms/view/V06643
[openstax-elasticity]: https://openstax.org/books/university-physics-volume-1/pages/12-3-stress-strain-and-elastic-modulus
[purdue-elastic-energy]: https://web.ics.purdue.edu/~gonza226/ME323/Lecture-21.pdf
[hawaii-elastic-moduli]: https://www.soest.hawaii.edu/martel/Courses/GG303/Lecture_20.pdf
[mit-shaft-torsion]: https://ocw.mit.edu/courses/1-050-solid-mechanics-fall-2004/emech8_04.pdf
[openstax-rotation-tables]: https://openstax.org/books/physics/pages/a-reference-tables
[openstax-relativity]: https://openstax.org/books/university-physics-volume-3/pages/5-key-equations
[openstax-damped-oscillations]: https://openstax.org/books/university-physics-volume-1/pages/15-5-damped-oscillations
[openstax-em-wave-equations]: https://openstax.org/books/university-physics-volume-2/pages/16-key-equations
[openstax-em-wave-energy]: https://openstax.org/books/university-physics-volume-2/pages/16-3-energy-carried-by-electromagnetic-waves
[ou-first-order-filters]: https://www.nhn.ou.edu/~bumm/ELAB/Lect_Notes/AC_impedance_v2_1_1.html
[openstax-wien]: https://openstax.org/books/university-physics-volume-3/pages/6-1-blackbody-radiation
[openstax-photon-equations]: https://openstax.org/books/university-physics-volume-3/pages/6-key-equations
[openstax-compton]: https://openstax.org/books/university-physics-volume-3/pages/6-3-the-compton-effect
[openstax-photoelectric]: https://openstax.org/books/university-physics-volume-3/pages/6-2-photoelectric-effect
[nasa-spectral-redshift]: https://www.jpl.nasa.gov/edu/resources/lesson-plan/math-of-the-expanding-universe/
[openstax-torricelli]: https://openstax.org/books/college-physics-2e/pages/12-3-the-most-general-applications-of-bernoullis-equation
[openstax-dalton]: https://openstax.org/books/chemistry-2e/pages/9-3-stoichiometry-of-gaseous-substances-mixtures-and-reactions
[openstax-raoult]: https://openstax.org/books/chemistry-atoms-first-2e/pages/11-4-colligative-properties
[openstax-kepler]: https://openstax.org/books/university-physics-volume-1/pages/13-5-keplers-laws-of-planetary-motion
[openstax-capacitor-geometry]: https://openstax.org/books/university-physics-volume-2/pages/8-1-capacitors-and-capacitance
[openstax-bohr]: https://openstax.org/books/university-physics-volume-3/pages/6-4-bohrs-model-of-the-hydrogen-atom
[columbia-microscopy]: https://www.columbia.edu/~leonard/BMELab/Optics.html
[cie-lambert-cosine]: https://cie.co.at/eilvterm/17-24-062
[cie-lambert-surface]: https://cie.co.at/eilvterm/17-24-063
[mit-dielectric-polarization]: https://ocw.mit.edu/courses/8-07-electromagnetism-ii-fall-2012/b40de5afd5f2bee0d3ae9352184db51f_MIT8_07F12_formsheet2.pdf
[mit-fields-and-media]: https://ocw.mit.edu/courses/6-013-electromagnetics-and-applications-spring-2009/MIT6_013S09_chap02.pdf
[mit-magnetic-fundamentals]: https://ocw.mit.edu/courses/3-15-electrical-optical-magnetic-materials-and-devices-fall-2006/7009df3d9c177d5865038f9d6a3377f4_ho9_magnetic_fundamentals.pdf
[openstax-gauss-law]: https://openstax.org/books/university-physics-volume-2/pages/6-2-explaining-gausss-law
[cie-luminous-exitance]: https://cie.co.at/eilvterm/17-21-081
[openstax-radioactive-decay]: https://openstax.org/books/university-physics-volume-3/pages/10-3-radioactive-decay
[mit-rlc-bandwidth]: https://web.mit.edu/6.101/www/s2019/handouts/L02_4.pdf
[mit-field-energy]: https://ocw.mit.edu/courses/8-03sc-physics-iii-vibrations-and-waves-fall-2016/ef731c1b91d77a6db003f6c27e300d25_MIT8_03SCF16_Textbook.pdf
[openstax-cone-and-cylinder]: https://openstax.org/books/prealgebra-2e/pages/9-6-solve-geometry-applications-volume-and-surface-area
[openstax-constant-acceleration]: https://openstax.org/books/university-physics-volume-1/pages/3-4-motion-with-constant-acceleration
[openstax-simple-harmonic-motion]: https://openstax.org/books/university-physics-volume-1/pages/15-key-equations
[openstax-gravitational-energy]: https://openstax.org/books/university-physics-volume-1/pages/13-3-gravitational-potential-energy-and-total-energy
[openstax-bernoulli]: https://openstax.org/books/university-physics-volume-1/pages/14-6-bernoullis-equation
[openstax-standing-sound]: https://openstax.org/books/university-physics-volume-1/pages/17-4-normal-modes-of-a-standing-sound-wave
[nasa-specific-impulse]: https://www.grc.nasa.gov/www/BGH/specimp.html
[openstax-first-law]: https://openstax.org/books/physics/pages/12-2-first-law-of-thermodynamics-thermal-energy-and-work
[openstax-kinetic-energy]: https://openstax.org/books/university-physics-volume-1/pages/7-2-kinetic-energy
[mit-rlc-quality-factor]: https://ocw.mit.edu/courses/6-622-power-electronics-spring-2023/mit6_622_s23_lec35_hand.pdf
[openstax-transformers]: https://openstax.org/books/university-physics-volume-2/pages/15-6-transformers
[openstax-magnetic-force-on-wire]: https://openstax.org/books/university-physics-volume-2/pages/11-4-magnetic-force-on-a-current-carrying-conductor
[openstax-magnetic-dipoles]: https://openstax.org/books/university-physics-volume-2/pages/11-5-force-and-torque-on-a-current-loop
[openstax-electric-dipoles]: https://openstax.org/books/university-physics-volume-2/pages/5-7-electric-dipoles
[openstax-diffraction-gratings]: https://openstax.org/books/university-physics-volume-3/pages/4-4-diffraction-gratings
[openstax-single-slit]: https://openstax.org/books/university-physics-volume-3/pages/4-1-single-slit-diffraction
[mit-normal-incidence-reflectance]: https://ocw.mit.edu/courses/3-46-photonic-materials-and-devices-spring-2006/c002c8429ae1f88a78982a53f0f9bb86_3_46l4_rayoptics.pdf
[openstax-radiation-pressure]: https://openstax.org/books/university-physics-volume-2/pages/16-4-momentum-and-radiation-pressure
[openstax-rc-transient]: https://openstax.org/books/university-physics-volume-2/pages/10-5-rc-circuits
[nist-attenuation-law]: https://physics.nist.gov/PhysRefData/XrayMassCoef/chap2.html
[mit-radius-of-gyration]: https://ocw.mit.edu/courses/16-07-dynamics-fall-2009/befffaf20475c1a379c3ac52e91a78cb_MIT16_07F09_Lec21.pdf
[sdsu-froude]: https://ponce.sdsu.edu/froude_and_vedernikov_pillars_of_open_channel_hydraulics.html
[mit-schmidt-number]: https://ocw.mit.edu/courses/3-185-transport-phenomena-in-materials-engineering-fall-2003/a55c74dc21def01fc0054de35574d1e5_3_21.pdf
[mit-diffusion-displacement]: https://ocw.mit.edu/courses/12-086-modeling-environmental-complexity-fall-2014/32303be6bc9b21df2280be1f41eb5c24_MIT12_086F14_twolevels.pdf
[openstax-ideal-gas]: https://openstax.org/books/university-physics-volume-2/pages/2-1-molecular-model-of-an-ideal-gas
[openstax-rms-gas-speed]: https://openstax.org/books/university-physics-volume-2/pages/2-2-pressure-temperature-and-rms-speed
[nist-codata-constants]: https://physics.nist.gov/cuu/pdf/all.pdf
[psu-transport-groups]: https://web.cecs.pdx.edu/~derekt/Classes/Governing%20Equations.pdf
[cie-radiance]: https://cie.co.at/eilvterm/17-21-049
[cie-geometric-extent]: https://cie.co.at/eilvterm/17-21-047
[cie-radiant-exposure]: https://cie.co.at/eilvterm/17-21-071
[openstax-de-broglie]: https://openstax.org/books/university-physics-volume-3/pages/6-5-de-broglies-matter-waves
[openstax-longitudinal-doppler]: https://openstax.org/books/university-physics-volume-3/pages/5-7-doppler-effect-for-light
[openstax-relativistic-velocity]: https://openstax.org/books/university-physics-volume-3/pages/5-6-relativistic-velocity-transformation
[openstax-wave-mathematics]: https://openstax.org/books/university-physics-volume-1/pages/16-2-mathematics-of-waves
[nist-spectroscopic-wavenumber]: https://physics.nist.gov/PhysRefData/ASD/Html/help.html
[mit-straight-wire-field]: https://ocw.mit.edu/courses/3-15-electrical-optical-magnetic-materials-and-devices-fall-2006/7009df3d9c177d5865038f9d6a3377f4_ho9_magnetic_fundamentals.pdf
[openstax-ideal-gas-heat-capacity]: https://openstax.org/books/university-physics-volume-2/pages/3-5-heat-capacities-of-an-ideal-gas
[openstax-phase-changes]: https://openstax.org/books/university-physics-volume-2/pages/1-5-phase-changes
[mit-fourier-conduction]: https://ocw.mit.edu/courses/2-43-advanced-thermodynamics-spring-2024/mit2_43_s24_lec21.pdf
[openstax-snell-law]: https://openstax.org/books/college-physics/pages/25-3-the-law-of-refraction
[nasa-orbital-energy]: https://ntrs.nasa.gov/api/citations/20205004141/downloads/Introduction%20to%20Orbitalaft%20Engineers%20CHARTS%20REV%20A.pdf?attachment=true
