// -----------------------------------------------------------------------
// <copyright file="QuantityRelations.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Quantities;

// Formula geometry/rectangle-area
// Equation A = ab
// Applies Rectangular sides are perpendicular. The inverse solves for the other side.
// Source [OpenStax area](https://openstax.org/books/contemporary-mathematics/pages/10-6-area)
[QuantityOperator<Length, Length, Area>(Operator.MULTIPLY)]
// Formula geometry/length-ratio
// Equation Length ratio = ℓ₂ / ℓ₁
// Applies The reference length is nonzero.
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<Length, Length, DimensionlessRatio>(Operator.DIVIDE)]
[QuantityOperator<Length, Time, LinearVelocity>(Operator.DIVIDE)]
[QuantityOperator<Length, LinearVelocity, Time>(Operator.DIVIDE)]
[QuantityOperator<double, Length, InverseLength>(Operator.DIVIDE)]
// Formula geometry/inverse-length
// Equation Inverse length = 1 / ℓ
// Applies Use a nonzero length. The product with the corresponding inverse length is one.
// Source [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes)
[QuantityOperator<Length, InverseLength, DimensionlessRatio>(Operator.MULTIPLY)]
[QuantityOperator<InverseLength, Length, DimensionlessRatio>(Operator.MULTIPLY)]
public readonly partial struct Length;

// Formula geometry/prism-volume
// Equation V = Aℓ
// Applies Prism or extrusion with uniform cross section perpendicular to its length.
// Source [OpenStax prism volume](https://openstax.org/books/contemporary-mathematics/pages/10-7-volume-and-surface-area)
[QuantityOperator<Area, Length, Volume>(Operator.MULTIPLY)]
[QuantityOperator<Length, Area, Volume>(Operator.MULTIPLY)]
[QuantityOperator<Area, Length, Length>(Operator.DIVIDE)]
public readonly partial struct Area;

[QuantityOperator<Volume, Area, Length>(Operator.DIVIDE)]
[QuantityOperator<Volume, Length, Area>(Operator.DIVIDE)]
[QuantityOperator<Volume, Time, VolumePerTime>(Operator.DIVIDE)]
public readonly partial struct Volume;

// Formula mechanics/linear-displacement
// Equation Δx = v̄Δt
// Applies Constant or average velocity component along the displacement axis.
// Source [OpenStax kinematics](https://openstax.org/books/university-physics-volume-1/pages/3-key-equations)
[QuantityOperator<LinearVelocity, Time, Length>(Operator.MULTIPLY)]
[QuantityOperator<Time, LinearVelocity, Length>(Operator.MULTIPLY)]
[QuantityOperator<LinearVelocity, Time, LinearAcceleration>(Operator.DIVIDE)]
[QuantityOperator<LinearVelocity, LinearAcceleration, Time>(Operator.DIVIDE)]
public readonly partial struct LinearVelocity;

// Formula mechanics/velocity-change
// Equation Δv = āΔt
// Applies Constant or average acceleration component over the interval.
// Source [OpenStax kinematics](https://openstax.org/books/university-physics-volume-1/pages/3-key-equations)
[QuantityOperator<LinearAcceleration, Time, LinearVelocity>(Operator.MULTIPLY)]
[QuantityOperator<Time, LinearAcceleration, LinearVelocity>(Operator.MULTIPLY)]
public readonly partial struct LinearAcceleration;

// Formula mechanics/force
// Equation F = ma
// Applies Net force along an axis for a constant-mass body.
// Source [OpenStax Newton's laws](https://openstax.org/books/university-physics-volume-1/pages/5-key-equations)
[QuantityOperator<Mass, LinearAcceleration, Force>(Operator.MULTIPLY)]
[QuantityOperator<LinearAcceleration, Mass, Force>(Operator.MULTIPLY)]
// Formula mechanics/linear-momentum
// Equation p = mv
// Applies Linear momentum component for the same mass and velocity.
// Source [OpenStax impulse and momentum](https://openstax.org/books/university-physics-volume-1/pages/9-key-equations)
[QuantityOperator<Mass, LinearVelocity, LinearMomentum>(Operator.MULTIPLY)]
[QuantityOperator<LinearVelocity, Mass, LinearMomentum>(Operator.MULTIPLY)]
[QuantityOperator<Mass, Volume, Density>(Operator.DIVIDE)]
public readonly partial struct Mass;

[QuantityOperator<Force, Mass, LinearAcceleration>(Operator.DIVIDE)]
[QuantityOperator<Force, LinearAcceleration, Mass>(Operator.DIVIDE)]
public readonly partial struct Force;

// Formula fluids/density-and-specific-volume
// Equation ρ = m / V, vₛ = V / m = 1 / ρ
// Applies Density and specific volume describe the same material at the same state.
// Source [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations)
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<Density, Volume, Mass>(Operator.MULTIPLY)]
[QuantityOperator<Volume, Density, Mass>(Operator.MULTIPLY)]
public readonly partial struct Density;

// Formula mechanics/energy-from-power
// Equation E = P̄Δt
// Applies Constant or average power or heat-flow rate over the interval.
// Source [OpenStax work and energy](https://openstax.org/books/university-physics-volume-1/pages/7-key-equations)
// Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
[QuantityOperator<Power, Time, Energy>(Operator.MULTIPLY)]
[QuantityOperator<Time, Power, Energy>(Operator.MULTIPLY)]
public readonly partial struct Power;

[QuantityOperator<Energy, Time, Power>(Operator.DIVIDE)]
[QuantityOperator<Energy, Power, Time>(Operator.DIVIDE)]
// Formula geometry/squared-energy
// Equation Squared energy = E²
// Applies Algebraic square of the energy value.
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<Energy, Energy, SquareEnergy>(Operator.MULTIPLY)]
public readonly partial struct Energy;

// Formula electromagnetism/charge-from-current
// Equation ΔQ = ĪΔt
// Applies Use average electrical current through the same cross section and interval.
// Source [OpenStax current and resistance](https://openstax.org/books/university-physics-volume-2/pages/9-key-equations)
[QuantityOperator<ElectricCurrent, Time, ElectricCharge>(Operator.MULTIPLY)]
[QuantityOperator<Time, ElectricCurrent, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricCurrent;

[QuantityOperator<ElectricCharge, Time, ElectricCurrent>(Operator.DIVIDE)]
[QuantityOperator<ElectricCharge, ElectricCurrent, Time>(Operator.DIVIDE)]
public readonly partial struct ElectricCharge;

[QuantityOperator<ThermodynamicTemperature, ThermodynamicTemperature, TemperatureDifference>(Operator.SUBTRACT)]
// Formula thermal/temperature-change
// Equation ΔT = T₂ − T₁, T₂ = T₁ + ΔT
// Applies Use the temperature difference as the increment of an absolute temperature.
// Source [BIPM SI Brochure](https://www.bipm.org/en/publications/si-brochure)
[QuantityOperator<ThermodynamicTemperature, TemperatureDifference, ThermodynamicTemperature>(Operator.ADD)]
[QuantityOperator<TemperatureDifference, ThermodynamicTemperature, ThermodynamicTemperature>(Operator.ADD)]
[QuantityOperator<ThermodynamicTemperature, TemperatureDifference, ThermodynamicTemperature>(Operator.SUBTRACT)]
public readonly partial struct ThermodynamicTemperature;

public readonly partial struct DimensionlessRatio;
public readonly partial struct InverseLength;
public readonly partial struct LinearMomentum;
public readonly partial struct SquareEnergy;
public readonly partial struct TemperatureDifference;
public readonly partial struct Time;
public readonly partial struct VolumePerTime;

// Linear and rotational mechanics.

// Formula geometry/squared-time
// Equation Squared time = t²
// Applies Algebraic square of the elapsed time.
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<Time, Time, SquareTime>(Operator.MULTIPLY)]
public readonly partial struct Time;

[QuantityOperator<SquareTime, Time, Time>(Operator.DIVIDE)]
public readonly partial struct SquareTime;

[QuantityOperator<LinearMomentum, Mass, LinearVelocity>(Operator.DIVIDE)]
[QuantityOperator<LinearMomentum, LinearVelocity, Mass>(Operator.DIVIDE)]
[QuantityOperator<LinearMomentum, Force, Time>(Operator.DIVIDE)]
[QuantityOperator<LinearMomentum, Time, Force>(Operator.DIVIDE)]
public readonly partial struct LinearMomentum;

// Average force over the interval gives the change in linear momentum.
// Formula mechanics/impulse-and-momentum-change
// Equation J = Δp = F̄Δt
// Applies Use the signed component of the average force over the interval.
// Source [OpenStax impulse and momentum](https://openstax.org/books/university-physics-volume-1/pages/9-key-equations)
[QuantityOperator<Force, Time, LinearMomentum>(Operator.MULTIPLY)]
[QuantityOperator<Time, Force, LinearMomentum>(Operator.MULTIPLY)]
[QuantityOperator<Force, ForcePerArea, Area>(Operator.DIVIDE)]
[QuantityOperator<Force, Area, ForcePerArea>(Operator.DIVIDE)]
[QuantityOperator<Force, ForcePerLength, Length>(Operator.DIVIDE)]
[QuantityOperator<Force, Length, ForcePerLength>(Operator.DIVIDE)]
public readonly partial struct Force;

// Formula fluids/pressure-from-force
// Equation p = Fₙ / A
// Applies Use the normal force over the surface. The general division returns `ForcePerArea`.
// Source [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations)
[QuantityOperator<ForcePerArea, Area, Force>(Operator.MULTIPLY)]
[QuantityOperator<Area, ForcePerArea, Force>(Operator.MULTIPLY)]
// Formula fluids/hydraulic-power
// Equation P = ΔpQᵥ
// Applies Pressure difference and flow refer to the same path. Use pressures referenced to the same datum.
// Source [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations)
// Source [OpenStax pressure–volume work](https://openstax.org/books/university-physics-volume-2/pages/3-key-equations)
[QuantityOperator<ForcePerArea, VolumePerTime, Power>(Operator.MULTIPLY)]
[QuantityOperator<VolumePerTime, ForcePerArea, Power>(Operator.MULTIPLY)]
public readonly partial struct ForcePerArea;

// Formula mechanics/distributed-force
// Equation F = (F / ℓ) × ℓ
// Applies Uniform or mean force per length over the loaded span.
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
// Source [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes)
[QuantityOperator<ForcePerLength, Length, Force>(Operator.MULTIPLY)]
[QuantityOperator<Length, ForcePerLength, Force>(Operator.MULTIPLY)]
public readonly partial struct ForcePerLength;

// Formula mechanics/angular-displacement
// Equation Δθ = ω̄Δt
// Applies Fixed axis and angular velocity averaged over the interval.
// Source [OpenStax rotational variables](https://openstax.org/books/university-physics-volume-1/pages/10-1-rotational-variables)
[QuantityOperator<AngularVelocity, Time, PlaneAngle>(Operator.MULTIPLY)]
[QuantityOperator<Time, AngularVelocity, PlaneAngle>(Operator.MULTIPLY)]
[QuantityOperator<AngularVelocity, AngularAcceleration, Time>(Operator.DIVIDE)]
[QuantityOperator<AngularVelocity, Time, AngularAcceleration>(Operator.DIVIDE)]
public readonly partial struct AngularVelocity;

[QuantityOperator<PlaneAngle, AngularVelocity, Time>(Operator.DIVIDE)]
[QuantityOperator<PlaneAngle, Time, AngularVelocity>(Operator.DIVIDE)]
public readonly partial struct PlaneAngle;

// Formula mechanics/angular-velocity-change
// Equation Δω = ᾱΔt
// Applies Fixed axis and angular acceleration averaged over the interval.
// Source [OpenStax fixed-axis rotation](https://openstax.org/books/university-physics-volume-1/pages/10-key-equations)
[QuantityOperator<AngularAcceleration, Time, AngularVelocity>(Operator.MULTIPLY)]
[QuantityOperator<Time, AngularAcceleration, AngularVelocity>(Operator.MULTIPLY)]
public readonly partial struct AngularAcceleration;

// Formula mechanics/torque-from-angular-acceleration
// Equation τ = Iα
// Applies Net torque and moment of inertia about the same fixed axis.
// Source [OpenStax fixed-axis rotation](https://openstax.org/books/university-physics-volume-1/pages/10-key-equations)
[QuantityOperator<MomentOfInertia, AngularAcceleration, Torque>(Operator.MULTIPLY)]
[QuantityOperator<AngularAcceleration, MomentOfInertia, Torque>(Operator.MULTIPLY)]
// Formula mechanics/angular-momentum
// Equation L = Iω
// Applies Rigid-body angular momentum component about the specified axis.
// Source [OpenStax angular momentum](https://openstax.org/books/university-physics-volume-1/pages/11-summary)
[QuantityOperator<MomentOfInertia, AngularVelocity, AngularMomentum>(Operator.MULTIPLY)]
[QuantityOperator<AngularVelocity, MomentOfInertia, AngularMomentum>(Operator.MULTIPLY)]
public readonly partial struct MomentOfInertia;

[QuantityOperator<Torque, MomentOfInertia, AngularAcceleration>(Operator.DIVIDE)]
[QuantityOperator<Torque, AngularAcceleration, MomentOfInertia>(Operator.DIVIDE)]
// Average torque over the interval gives the change in angular momentum about the same axis.
// Formula mechanics/angular-momentum-change
// Equation ΔL = τ̄Δt
// Applies Average torque and angular momentum change about the same axis.
// Source [OpenStax angular momentum](https://openstax.org/books/university-physics-volume-1/pages/11-summary)
[QuantityOperator<Torque, Time, AngularMomentum>(Operator.MULTIPLY)]
[QuantityOperator<Time, Torque, AngularMomentum>(Operator.MULTIPLY)]
// Formula mechanics/shaft-power
// Equation P = τω
// Applies Corresponding torque and angular velocity components about the same axis.
// Source [OpenStax fixed-axis rotation](https://openstax.org/books/university-physics-volume-1/pages/10-key-equations)
[QuantityOperator<Torque, AngularVelocity, Power>(Operator.MULTIPLY)]
[QuantityOperator<AngularVelocity, Torque, Power>(Operator.MULTIPLY)]
// Formula mechanics/rotational-work
// Equation W = τ̄Δθ
// Applies Constant or angle-averaged torque along the rotation.
// Source [OpenStax fixed-axis rotation](https://openstax.org/books/university-physics-volume-1/pages/10-key-equations)
[QuantityOperator<Torque, PlaneAngle, Energy>(Operator.MULTIPLY)]
[QuantityOperator<PlaneAngle, Torque, Energy>(Operator.MULTIPLY)]
public readonly partial struct Torque;

[QuantityOperator<AngularMomentum, MomentOfInertia, AngularVelocity>(Operator.DIVIDE)]
[QuantityOperator<AngularMomentum, AngularVelocity, MomentOfInertia>(Operator.DIVIDE)]
[QuantityOperator<AngularMomentum, Torque, Time>(Operator.DIVIDE)]
[QuantityOperator<AngularMomentum, Time, Torque>(Operator.DIVIDE)]
public readonly partial struct AngularMomentum;

[QuantityOperator<Power, Torque, AngularVelocity>(Operator.DIVIDE)]
[QuantityOperator<Power, AngularVelocity, Torque>(Operator.DIVIDE)]
[QuantityOperator<Power, ForcePerArea, VolumePerTime>(Operator.DIVIDE)]
[QuantityOperator<Power, VolumePerTime, ForcePerArea>(Operator.DIVIDE)]
public readonly partial struct Power;

[QuantityOperator<Energy, Torque, PlaneAngle>(Operator.DIVIDE)]
[QuantityOperator<Energy, PlaneAngle, Torque>(Operator.DIVIDE)]
public readonly partial struct Energy;

// Mass, fluid flow, and energy distributions.

[QuantityOperator<Mass, Density, Volume>(Operator.DIVIDE)]
[QuantityOperator<Mass, MassPerLength, Length>(Operator.DIVIDE)]
[QuantityOperator<Mass, Length, MassPerLength>(Operator.DIVIDE)]
[QuantityOperator<Mass, MassPerArea, Area>(Operator.DIVIDE)]
[QuantityOperator<Mass, Area, MassPerArea>(Operator.DIVIDE)]
[QuantityOperator<Mass, MassPerTime, Time>(Operator.DIVIDE)]
[QuantityOperator<Mass, Time, MassPerTime>(Operator.DIVIDE)]
public readonly partial struct Mass;

// Formula mechanics/linear-mass-density
// Equation m = λₘℓ
// Applies Uniform or mean linear mass density λₘ over the length.
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<MassPerLength, Length, Mass>(Operator.MULTIPLY)]
[QuantityOperator<Length, MassPerLength, Mass>(Operator.MULTIPLY)]
public readonly partial struct MassPerLength;

// Formula mechanics/areal-mass-density
// Equation m = ρₐA
// Applies Uniform or mean areal mass density ρₐ over the area.
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<MassPerArea, Area, Mass>(Operator.MULTIPLY)]
[QuantityOperator<Area, MassPerArea, Mass>(Operator.MULTIPLY)]
[QuantityOperator<MassPerArea, MassPerAreaTime, Time>(Operator.DIVIDE)]
[QuantityOperator<MassPerArea, Time, MassPerAreaTime>(Operator.DIVIDE)]
public readonly partial struct MassPerArea;

// Formula fluids/mass-from-flow
// Equation Δm = ṁ̄Δt
// Applies Constant or time-averaged mass flow rate over the interval.
// Source [NASA mass flow](https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/mass-flow-rate/)
[QuantityOperator<MassPerTime, Time, Mass>(Operator.MULTIPLY)]
[QuantityOperator<Time, MassPerTime, Mass>(Operator.MULTIPLY)]
[QuantityOperator<MassPerTime, MassPerAreaTime, Area>(Operator.DIVIDE)]
[QuantityOperator<MassPerTime, Area, MassPerAreaTime>(Operator.DIVIDE)]
[QuantityOperator<MassPerTime, Density, VolumePerTime>(Operator.DIVIDE)]
[QuantityOperator<MassPerTime, VolumePerTime, Density>(Operator.DIVIDE)]
public readonly partial struct MassPerTime;

// Formula fluids/areal-mass-flow
[QuantityOperator<MassPerAreaTime, Area, MassPerTime>(Operator.MULTIPLY)]
[QuantityOperator<Area, MassPerAreaTime, MassPerTime>(Operator.MULTIPLY)]
// Formula fluids/areal-mass-flow
// Equation jₘ = ṁ / A = Δ(m/A) / Δt
// Applies Use the same oriented surface and interval for the areal mass flow rate jₘ.
// Source [NASA mass flow](https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/mass-flow-rate/)
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<MassPerAreaTime, Time, MassPerArea>(Operator.MULTIPLY)]
[QuantityOperator<Time, MassPerAreaTime, MassPerArea>(Operator.MULTIPLY)]
public readonly partial struct MassPerAreaTime;

// Formula fluids/density-and-specific-volume
[QuantityOperator<SpecificVolume, Mass, Volume>(Operator.MULTIPLY)]
[QuantityOperator<Mass, SpecificVolume, Volume>(Operator.MULTIPLY)]
public readonly partial struct SpecificVolume;

[QuantityOperator<Volume, SpecificVolume, Mass>(Operator.DIVIDE)]
[QuantityOperator<Volume, Mass, SpecificVolume>(Operator.DIVIDE)]
[QuantityOperator<Volume, VolumePerTime, Time>(Operator.DIVIDE)]
public readonly partial struct Volume;

// Formula fluids/volume-flow-from-velocity
// Equation Qᵥ = Avₙ
// Applies Area-averaged normal speed component through the section.
// Source [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations)
// Source [NASA mass flow](https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/mass-flow-rate/)
[QuantityOperator<Area, LinearVelocity, VolumePerTime>(Operator.MULTIPLY)]
[QuantityOperator<LinearVelocity, Area, VolumePerTime>(Operator.MULTIPLY)]
public readonly partial struct Area;

[QuantityOperator<VolumePerTime, Area, LinearVelocity>(Operator.DIVIDE)]
[QuantityOperator<VolumePerTime, LinearVelocity, Area>(Operator.DIVIDE)]
// Formula fluids/volume-from-flow
// Equation ΔV = Q̄ᵥΔt
// Applies Constant or time-averaged volumetric flow rate.
// Source [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations)
[QuantityOperator<VolumePerTime, Time, Volume>(Operator.MULTIPLY)]
[QuantityOperator<Time, VolumePerTime, Volume>(Operator.MULTIPLY)]
public readonly partial struct VolumePerTime;

// Formula fluids/mass-flow-from-volume-flow
// Equation ṁ = ρQᵥ
// Applies Use consistent density and volumetric flow at the same section.
// Source [NASA mass flow](https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/mass-flow-rate/)
[QuantityOperator<Density, VolumePerTime, MassPerTime>(Operator.MULTIPLY)]
[QuantityOperator<VolumePerTime, Density, MassPerTime>(Operator.MULTIPLY)]
public readonly partial struct Density;

// Formula fluids/viscosity
// Equation μ = ρν
// Applies Density, dynamic viscosity, and kinematic viscosity at the same fluid state.
// Source [NASA viscosity and flow](https://ntrs.nasa.gov/api/citations/20250001477/downloads/Paul%20Gradl_Dissertation_NASA_Title_2-11-2025.pdf)
[QuantityOperator<KinematicViscosity, Density, DynamicViscosity>(Operator.MULTIPLY)]
[QuantityOperator<Density, KinematicViscosity, DynamicViscosity>(Operator.MULTIPLY)]
public readonly partial struct KinematicViscosity;

[QuantityOperator<DynamicViscosity, KinematicViscosity, Density>(Operator.DIVIDE)]
[QuantityOperator<DynamicViscosity, Density, KinematicViscosity>(Operator.DIVIDE)]
public readonly partial struct DynamicViscosity;

// Formula mechanics/specific-energy
// Equation e = E / m
// Applies Energy and mass refer to the same body or sample.
// Source [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes)
[QuantityOperator<SpecificEnergy, Mass, Energy>(Operator.MULTIPLY)]
[QuantityOperator<Mass, SpecificEnergy, Energy>(Operator.MULTIPLY)]
public readonly partial struct SpecificEnergy;

[QuantityOperator<Energy, SpecificEnergy, Mass>(Operator.DIVIDE)]
[QuantityOperator<Energy, Mass, SpecificEnergy>(Operator.DIVIDE)]
[QuantityOperator<Energy, EnergyDensity, Volume>(Operator.DIVIDE)]
[QuantityOperator<Energy, Volume, EnergyDensity>(Operator.DIVIDE)]
[QuantityOperator<Energy, EnergyPerArea, Area>(Operator.DIVIDE)]
[QuantityOperator<Energy, Area, EnergyPerArea>(Operator.DIVIDE)]
public readonly partial struct Energy;

// Formula mechanics/energy-density
// Equation u = E / V
// Applies Average energy per volume of the same region.
// Source [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes)
[QuantityOperator<EnergyDensity, Volume, Energy>(Operator.MULTIPLY)]
[QuantityOperator<Volume, EnergyDensity, Energy>(Operator.MULTIPLY)]
public readonly partial struct EnergyDensity;

// Formula mechanics/energy-per-area
// Equation Eₐ = E / A
// Applies Average energy per area over the relevant surface.
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<EnergyPerArea, Area, Energy>(Operator.MULTIPLY)]
[QuantityOperator<Area, EnergyPerArea, Energy>(Operator.MULTIPLY)]
public readonly partial struct EnergyPerArea;

// Formula mechanics/power-per-area
// Equation Pₐ = P / A
// Applies Average power per area over the same surface.
// Source [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes)
[QuantityOperator<PowerPerArea, Area, Power>(Operator.MULTIPLY)]
[QuantityOperator<Area, PowerPerArea, Power>(Operator.MULTIPLY)]
public readonly partial struct PowerPerArea;

[QuantityOperator<Power, PowerPerArea, Area>(Operator.DIVIDE)]
[QuantityOperator<Power, Area, PowerPerArea>(Operator.DIVIDE)]
public readonly partial struct Power;

// Electricity and magnetism.

// Formula electromagnetism/ohms-law
// Equation V = IR, I = GV, G = 1 / R
// Applies Voltage and current refer to the same ohmic element or operating point.
// Source [OpenStax current and resistance](https://openstax.org/books/university-physics-volume-2/pages/9-key-equations)
// Source [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes)
[QuantityOperator<Resistance, ElectricCurrent, ElectricPotential>(Operator.MULTIPLY)]
[QuantityOperator<ElectricCurrent, Resistance, ElectricPotential>(Operator.MULTIPLY)]
// Formula electromagnetism/rc-time-constant
// Equation τ = RC
// Applies Time constant of the equivalent first-order RC circuit.
// Source [OpenStax RC circuits](https://openstax.org/books/university-physics-volume-2/pages/10-key-equations)
[QuantityOperator<Resistance, Capacitance, Time>(Operator.MULTIPLY)]
[QuantityOperator<Capacitance, Resistance, Time>(Operator.MULTIPLY)]
// Formula electromagnetism/rl-time-constant
// Equation τ = L / R
// Applies Time constant of the equivalent first-order RL circuit.
// Source [OpenStax RL circuits](https://openstax.org/books/university-physics-volume-2/pages/14-key-equations)
[QuantityOperator<Resistance, Time, Inductance>(Operator.MULTIPLY)]
[QuantityOperator<Time, Resistance, Inductance>(Operator.MULTIPLY)]
public readonly partial struct Resistance;

[QuantityOperator<Time, Resistance, Capacitance>(Operator.DIVIDE)]
[QuantityOperator<Time, Capacitance, Resistance>(Operator.DIVIDE)]
public readonly partial struct Time;

[QuantityOperator<Inductance, Resistance, Time>(Operator.DIVIDE)]
[QuantityOperator<Inductance, Time, Resistance>(Operator.DIVIDE)]
public readonly partial struct Inductance;

[QuantityOperator<ElectricPotential, Resistance, ElectricCurrent>(Operator.DIVIDE)]
[QuantityOperator<ElectricPotential, ElectricCurrent, Resistance>(Operator.DIVIDE)]
// Use corresponding instantaneous voltage and current, with the passive sign convention for absorbed power.
// Formula electromagnetism/electrical-power
// Equation P = VI
// Applies Use corresponding instantaneous voltage and current. Use the passive sign convention for absorbed power.
// Source [OpenStax current and resistance](https://openstax.org/books/university-physics-volume-2/pages/9-key-equations)
// Source [OpenStax AC power](https://openstax.org/books/university-physics-volume-2/pages/15-key-equations)
[QuantityOperator<ElectricPotential, ElectricCurrent, Power>(Operator.MULTIPLY)]
[QuantityOperator<ElectricCurrent, ElectricPotential, Power>(Operator.MULTIPLY)]
// Formula electromagnetism/potential-energy-change
// Equation ΔU = QΔV
// Applies Change in electrical potential energy for charge moved through a fixed potential difference.
// Source [OpenStax electric potential](https://openstax.org/books/university-physics-volume-2/pages/7-key-equations)
[QuantityOperator<ElectricPotential, ElectricCharge, Energy>(Operator.MULTIPLY)]
[QuantityOperator<ElectricCharge, ElectricPotential, Energy>(Operator.MULTIPLY)]
[QuantityOperator<ElectricPotential, ElectricFieldStrength, Length>(Operator.DIVIDE)]
[QuantityOperator<ElectricPotential, Length, ElectricFieldStrength>(Operator.DIVIDE)]
public readonly partial struct ElectricPotential;

[QuantityOperator<Power, ElectricPotential, ElectricCurrent>(Operator.DIVIDE)]
[QuantityOperator<Power, ElectricCurrent, ElectricPotential>(Operator.DIVIDE)]
public readonly partial struct Power;

[QuantityOperator<Energy, ElectricPotential, ElectricCharge>(Operator.DIVIDE)]
[QuantityOperator<Energy, ElectricCharge, ElectricPotential>(Operator.DIVIDE)]
public readonly partial struct Energy;

// Formula electromagnetism/capacitor-charge
// Equation Q = CV
// Applies Linear capacitor with the voltage across its terminals.
// Source [OpenStax capacitance](https://openstax.org/books/university-physics-volume-2/pages/8-key-equations)
[QuantityOperator<Capacitance, ElectricPotential, ElectricCharge>(Operator.MULTIPLY)]
[QuantityOperator<ElectricPotential, Capacitance, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct Capacitance;

[QuantityOperator<ElectricCharge, Capacitance, ElectricPotential>(Operator.DIVIDE)]
[QuantityOperator<ElectricCharge, ElectricPotential, Capacitance>(Operator.DIVIDE)]
[QuantityOperator<ElectricCharge, ElectricChargeLineDensity, Length>(Operator.DIVIDE)]
[QuantityOperator<ElectricCharge, Length, ElectricChargeLineDensity>(Operator.DIVIDE)]
[QuantityOperator<ElectricCharge, ElectricChargePerArea, Area>(Operator.DIVIDE)]
[QuantityOperator<ElectricCharge, Area, ElectricChargePerArea>(Operator.DIVIDE)]
[QuantityOperator<ElectricCharge, ElectricChargeVolumeDensity, Volume>(Operator.DIVIDE)]
[QuantityOperator<ElectricCharge, Volume, ElectricChargeVolumeDensity>(Operator.DIVIDE)]
[QuantityOperator<ElectricCharge, ElectricChargePerMass, Mass>(Operator.DIVIDE)]
[QuantityOperator<ElectricCharge, Mass, ElectricChargePerMass>(Operator.DIVIDE)]
[QuantityOperator<ElectricCharge, ElectricChargePerAmountOfSubstance, AmountOfSubstance>(Operator.DIVIDE)]
[QuantityOperator<ElectricCharge, AmountOfSubstance, ElectricChargePerAmountOfSubstance>(Operator.DIVIDE)]
// For equal and opposite charges, use the separation directed from the negative to the positive charge.
// Formula electromagnetism/dipole-moment
// Equation p = Qd
// Applies Equal and opposite charges. Separation is directed from negative charge to positive charge.
// Source [OpenStax electric fields](https://openstax.org/books/university-physics-volume-2/pages/5-key-equations)
[QuantityOperator<ElectricCharge, Length, ElectricDipoleMoment>(Operator.MULTIPLY)]
[QuantityOperator<Length, ElectricCharge, ElectricDipoleMoment>(Operator.MULTIPLY)]
public readonly partial struct ElectricCharge;

// The field component times path length gives the voltage drop, V_start - V_end.
// Formula electromagnetism/electric-field-and-voltage
// Equation Vdrop = E∥ℓ, ΔV = −E∥ℓ
// Applies Use the path-averaged field component and final-minus-initial voltage for ΔV.
// Source [OpenStax electric potential](https://openstax.org/books/university-physics-volume-2/pages/7-key-equations)
[QuantityOperator<ElectricFieldStrength, Length, ElectricPotential>(Operator.MULTIPLY)]
[QuantityOperator<Length, ElectricFieldStrength, ElectricPotential>(Operator.MULTIPLY)]
// Formula electromagnetism/electric-force
// Equation F = QE
// Applies Use the electric field component at the charge.
// Source [OpenStax electric fields](https://openstax.org/books/university-physics-volume-2/pages/5-key-equations)
[QuantityOperator<ElectricFieldStrength, ElectricCharge, Force>(Operator.MULTIPLY)]
[QuantityOperator<ElectricCharge, ElectricFieldStrength, Force>(Operator.MULTIPLY)]
// Formula electromagnetism/electric-flux
// Equation Φₑ = EₙA
// Applies Uniform or area-averaged normal electric field on the surface.
// Source [OpenStax electric fields](https://openstax.org/books/university-physics-volume-2/pages/5-key-equations)
// Source [MIT linear electromagnetic media](https://ocw.mit.edu/courses/18-325-topics-in-applied-mathematics-waves-and-imaging-fall-2015/c5db24b1a6d0d3301b1a3f21f6ceba3a_MIT18_325F15_CompleteLect.pdf)
[QuantityOperator<ElectricFieldStrength, Area, ElectricFlux>(Operator.MULTIPLY)]
[QuantityOperator<Area, ElectricFieldStrength, ElectricFlux>(Operator.MULTIPLY)]
public readonly partial struct ElectricFieldStrength;

[QuantityOperator<Force, ElectricFieldStrength, ElectricCharge>(Operator.DIVIDE)]
[QuantityOperator<Force, ElectricCharge, ElectricFieldStrength>(Operator.DIVIDE)]
public readonly partial struct Force;

[QuantityOperator<ElectricFlux, ElectricFieldStrength, Area>(Operator.DIVIDE)]
[QuantityOperator<ElectricFlux, Area, ElectricFieldStrength>(Operator.DIVIDE)]
public readonly partial struct ElectricFlux;

// Formula electromagnetism/ohmic-current-density
// Equation J = σE
// Applies Scalar conductivity in the local ohmic regime. Use corresponding components.
// Source [OpenStax resistivity](https://openstax.org/books/university-physics-volume-2/pages/9-3-resistivity-and-resistance)
[QuantityOperator<ElectricConductivity, ElectricFieldStrength, ElectricCurrentDensity>(Operator.MULTIPLY)]
[QuantityOperator<ElectricFieldStrength, ElectricConductivity, ElectricCurrentDensity>(Operator.MULTIPLY)]
public readonly partial struct ElectricConductivity;

[QuantityOperator<ElectricCurrentDensity, ElectricConductivity, ElectricFieldStrength>(Operator.DIVIDE)]
[QuantityOperator<ElectricCurrentDensity, ElectricFieldStrength, ElectricConductivity>(Operator.DIVIDE)]
// Formula electromagnetism/current-through-surface
// Equation I = JₙA
// Applies Uniform or area-averaged normal current density through the section.
// Source [OpenStax current and resistance](https://openstax.org/books/university-physics-volume-2/pages/9-key-equations)
[QuantityOperator<ElectricCurrentDensity, Area, ElectricCurrent>(Operator.MULTIPLY)]
[QuantityOperator<Area, ElectricCurrentDensity, ElectricCurrent>(Operator.MULTIPLY)]
public readonly partial struct ElectricCurrentDensity;

[QuantityOperator<ElectricCurrent, ElectricCurrentDensity, Area>(Operator.DIVIDE)]
[QuantityOperator<ElectricCurrent, Area, ElectricCurrentDensity>(Operator.DIVIDE)]
public readonly partial struct ElectricCurrent;

// Formula electromagnetism/line-charge-density
// Equation Q = λqℓ
// Applies Uniform or mean line charge density λq over the length.
// Source [OpenStax electric fields](https://openstax.org/books/university-physics-volume-2/pages/5-key-equations)
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<ElectricChargeLineDensity, Length, ElectricCharge>(Operator.MULTIPLY)]
[QuantityOperator<Length, ElectricChargeLineDensity, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricChargeLineDensity;

// Formula electromagnetism/surface-charge-density
// Equation Q = σqA
// Applies Uniform or mean surface charge density σq over the area.
// Source [OpenStax electric fields](https://openstax.org/books/university-physics-volume-2/pages/5-key-equations)
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<ElectricChargePerArea, Area, ElectricCharge>(Operator.MULTIPLY)]
[QuantityOperator<Area, ElectricChargePerArea, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricChargePerArea;

// Formula electromagnetism/volume-charge-density
// Equation Q = ρqV
// Applies Uniform or mean volume charge density ρq over the region.
// Source [MIT linear electromagnetic media](https://ocw.mit.edu/courses/18-325-topics-in-applied-mathematics-waves-and-imaging-fall-2015/c5db24b1a6d0d3301b1a3f21f6ceba3a_MIT18_325F15_CompleteLect.pdf)
[QuantityOperator<ElectricChargeVolumeDensity, Volume, ElectricCharge>(Operator.MULTIPLY)]
[QuantityOperator<Volume, ElectricChargeVolumeDensity, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricChargeVolumeDensity;

// Formula electromagnetism/charge-per-mass
// Equation Q = (Q / m)m
// Applies Charge and mass refer to the same particles or sample.
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<ElectricChargePerMass, Mass, ElectricCharge>(Operator.MULTIPLY)]
[QuantityOperator<Mass, ElectricChargePerMass, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricChargePerMass;

// Formula electromagnetism/charge-per-mole
// Equation Q = (Q / n)n
// Applies Use charge per mole for the specified entities and process.
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
// Source [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971)
[QuantityOperator<ElectricChargePerAmountOfSubstance, AmountOfSubstance, ElectricCharge>(Operator.MULTIPLY)]
[QuantityOperator<AmountOfSubstance, ElectricChargePerAmountOfSubstance, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricChargePerAmountOfSubstance;

[QuantityOperator<ElectricDipoleMoment, ElectricCharge, Length>(Operator.DIVIDE)]
[QuantityOperator<ElectricDipoleMoment, Length, ElectricCharge>(Operator.DIVIDE)]
public readonly partial struct ElectricDipoleMoment;

// Formula electromagnetism/electric-displacement
// Equation D = εE
// Applies Linear isotropic dielectric, using corresponding field components.
// Source [MIT linear electromagnetic media](https://ocw.mit.edu/courses/18-325-topics-in-applied-mathematics-waves-and-imaging-fall-2015/c5db24b1a6d0d3301b1a3f21f6ceba3a_MIT18_325F15_CompleteLect.pdf)
[QuantityOperator<Permittivity, ElectricFieldStrength, ElectricDisplacement>(Operator.MULTIPLY)]
[QuantityOperator<ElectricFieldStrength, Permittivity, ElectricDisplacement>(Operator.MULTIPLY)]
public readonly partial struct Permittivity;

[QuantityOperator<ElectricDisplacement, Permittivity, ElectricFieldStrength>(Operator.DIVIDE)]
[QuantityOperator<ElectricDisplacement, ElectricFieldStrength, Permittivity>(Operator.DIVIDE)]
public readonly partial struct ElectricDisplacement;

// Use the area-averaged field component normal to the surface.
// Formula electromagnetism/magnetic-flux
// Equation Φ = BₙA
// Applies Use the uniform or area-averaged normal flux density.
// Source [OpenStax Faraday's law](https://openstax.org/books/university-physics-volume-2/pages/13-key-equations)
[QuantityOperator<MagneticFluxDensity, Area, MagneticFlux>(Operator.MULTIPLY)]
[QuantityOperator<Area, MagneticFluxDensity, MagneticFlux>(Operator.MULTIPLY)]
[QuantityOperator<MagneticFluxDensity, Permeability, AuxillaryMagneticField>(Operator.DIVIDE)]
[QuantityOperator<MagneticFluxDensity, AuxillaryMagneticField, Permeability>(Operator.DIVIDE)]
public readonly partial struct MagneticFluxDensity;

[QuantityOperator<MagneticFlux, MagneticFluxDensity, Area>(Operator.DIVIDE)]
[QuantityOperator<MagneticFlux, Area, MagneticFluxDensity>(Operator.DIVIDE)]
[QuantityOperator<MagneticFlux, MagneticFluxPerLength, Length>(Operator.DIVIDE)]
[QuantityOperator<MagneticFlux, Length, MagneticFluxPerLength>(Operator.DIVIDE)]
public readonly partial struct MagneticFlux;

// Formula electromagnetism/magnetic-flux-per-length
// Equation Φ = (Φ / ℓ)ℓ
// Applies Uniform or mean magnetic flux per length over the interval.
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<MagneticFluxPerLength, Length, MagneticFlux>(Operator.MULTIPLY)]
[QuantityOperator<Length, MagneticFluxPerLength, MagneticFlux>(Operator.MULTIPLY)]
public readonly partial struct MagneticFluxPerLength;

// Formula electromagnetism/magnetic-field-in-material
// Equation B = μH
// Applies Linear isotropic magnetic medium. The QUDT `AuxillaryMagneticField` type represents H.
// Source [MIT linear electromagnetic media](https://ocw.mit.edu/courses/18-325-topics-in-applied-mathematics-waves-and-imaging-fall-2015/c5db24b1a6d0d3301b1a3f21f6ceba3a_MIT18_325F15_CompleteLect.pdf)
[QuantityOperator<Permeability, AuxillaryMagneticField, MagneticFluxDensity>(Operator.MULTIPLY)]
[QuantityOperator<AuxillaryMagneticField, Permeability, MagneticFluxDensity>(Operator.MULTIPLY)]
public readonly partial struct Permeability;

// Heat and temperature.

// Use a constant heat capacity or its average over the temperature interval.
// Formula thermal/sensible-heat
// Equation Q = CΔT
// Applies Sensible heat with constant or interval-averaged heat capacity.
// Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
[QuantityOperator<HeatCapacity, TemperatureDifference, Energy>(Operator.MULTIPLY)]
[QuantityOperator<TemperatureDifference, HeatCapacity, Energy>(Operator.MULTIPLY)]
[QuantityOperator<HeatCapacity, SpecificHeatCapacity, Mass>(Operator.DIVIDE)]
[QuantityOperator<HeatCapacity, Mass, SpecificHeatCapacity>(Operator.DIVIDE)]
[QuantityOperator<HeatCapacity, VolumetricHeatCapacity, Volume>(Operator.DIVIDE)]
[QuantityOperator<HeatCapacity, Volume, VolumetricHeatCapacity>(Operator.DIVIDE)]
public readonly partial struct HeatCapacity;

[QuantityOperator<Energy, HeatCapacity, TemperatureDifference>(Operator.DIVIDE)]
[QuantityOperator<Energy, TemperatureDifference, HeatCapacity>(Operator.DIVIDE)]
[QuantityOperator<Energy, HeatFlowRate, Time>(Operator.DIVIDE)]
public readonly partial struct Energy;

// Formula thermal/specific-heat-capacity
// Equation C = mc
// Applies Use mass-specific heat capacity for the specified thermodynamic process.
// Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
[QuantityOperator<SpecificHeatCapacity, Mass, HeatCapacity>(Operator.MULTIPLY)]
[QuantityOperator<Mass, SpecificHeatCapacity, HeatCapacity>(Operator.MULTIPLY)]
// Formula thermal/volumetric-capacity-from-density
// Equation cᵥ = ρc
// Applies Density and mass-specific heat capacity describe the same state.
// Source [NIST TN 622](https://nvlpubs.nist.gov/nistpubs/Legacy/TN/nbstechnicalnote622.pdf)
[QuantityOperator<SpecificHeatCapacity, Density, VolumetricHeatCapacity>(Operator.MULTIPLY)]
[QuantityOperator<Density, SpecificHeatCapacity, VolumetricHeatCapacity>(Operator.MULTIPLY)]
public readonly partial struct SpecificHeatCapacity;

// Formula thermal/volumetric-heat-capacity
// Equation C = Vcᵥ
// Applies Uniform or volume-averaged volumetric heat capacity cᵥ.
// Source [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes)
[QuantityOperator<VolumetricHeatCapacity, Volume, HeatCapacity>(Operator.MULTIPLY)]
[QuantityOperator<Volume, VolumetricHeatCapacity, HeatCapacity>(Operator.MULTIPLY)]
[QuantityOperator<VolumetricHeatCapacity, SpecificHeatCapacity, Density>(Operator.DIVIDE)]
[QuantityOperator<VolumetricHeatCapacity, Density, SpecificHeatCapacity>(Operator.DIVIDE)]
public readonly partial struct VolumetricHeatCapacity;

// Formula thermal/thermal-diffusivity
// Equation α = k / cᵥ
// Applies Thermal diffusivity for scalar conductivity and volumetric heat capacity.
// Source [NIST TN 622](https://nvlpubs.nist.gov/nistpubs/Legacy/TN/nbstechnicalnote622.pdf)
[QuantityOperator<ThermalDiffusivity, VolumetricHeatCapacity, ThermalConductivity>(Operator.MULTIPLY)]
[QuantityOperator<VolumetricHeatCapacity, ThermalDiffusivity, ThermalConductivity>(Operator.MULTIPLY)]
public readonly partial struct ThermalDiffusivity;

[QuantityOperator<ThermalConductivity, ThermalDiffusivity, VolumetricHeatCapacity>(Operator.DIVIDE)]
[QuantityOperator<ThermalConductivity, VolumetricHeatCapacity, ThermalDiffusivity>(Operator.DIVIDE)]
public readonly partial struct ThermalConductivity;

// Formula mechanics/energy-from-power
[QuantityOperator<HeatFlowRate, Time, Energy>(Operator.MULTIPLY)]
[QuantityOperator<Time, HeatFlowRate, Energy>(Operator.MULTIPLY)]
[QuantityOperator<HeatFlowRate, HeatFlowRatePerArea, Area>(Operator.DIVIDE)]
[QuantityOperator<HeatFlowRate, Area, HeatFlowRatePerArea>(Operator.DIVIDE)]
public readonly partial struct HeatFlowRate;

// Formula thermal/heat-flow-from-flux
// Equation Pheat = q″A
// Applies Use average normal heat flux over the surface.
// Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
// Source [QUDT heat-transfer coefficient](https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer)
[QuantityOperator<HeatFlowRatePerArea, Area, HeatFlowRate>(Operator.MULTIPLY)]
[QuantityOperator<Area, HeatFlowRatePerArea, HeatFlowRate>(Operator.MULTIPLY)]
[QuantityOperator<HeatFlowRatePerArea, CoefficientOfHeatTransfer, TemperatureDifference>(Operator.DIVIDE)]
[QuantityOperator<HeatFlowRatePerArea, TemperatureDifference, CoefficientOfHeatTransfer>(Operator.DIVIDE)]
public readonly partial struct HeatFlowRatePerArea;

// Formula thermal/surface-heat-transfer
// Equation q″ = hΔT
// Applies Transfer coefficient h is per unit area. Use the driving temperature difference in the indicated direction.
// Source [QUDT heat-transfer coefficient](https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer)
[QuantityOperator<CoefficientOfHeatTransfer, TemperatureDifference, HeatFlowRatePerArea>(Operator.MULTIPLY)]
[QuantityOperator<TemperatureDifference, CoefficientOfHeatTransfer, HeatFlowRatePerArea>(Operator.MULTIPLY)]
public readonly partial struct CoefficientOfHeatTransfer;

// Formula thermal/thermal-path
// Equation Pheat = ΔT / Rₜ = GₜΔT, Gₜ = 1 / Rₜ
// Applies Use total resistance or conductance for the same thermal path. A signed temperature drop gives the signed heat-flow rate.
// Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
// Source [QUDT thermal conductance](https://qudt.org/vocab/quantitykind/ThermalConductance)
[QuantityOperator<ThermalResistance, HeatFlowRate, TemperatureDifference>(Operator.MULTIPLY)]
[QuantityOperator<HeatFlowRate, ThermalResistance, TemperatureDifference>(Operator.MULTIPLY)]
// Formula thermal/areal-thermal-resistance
// Equation R″ = ARₜ
// Applies Relate total thermal resistance to area-normalized resistance for the same uniform surface.
// Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
// Source [QUDT heat-transfer coefficient](https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer)
[QuantityOperator<ThermalResistance, Area, ThermalInsulance>(Operator.MULTIPLY)]
[QuantityOperator<Area, ThermalResistance, ThermalInsulance>(Operator.MULTIPLY)]
public readonly partial struct ThermalResistance;

[QuantityOperator<TemperatureDifference, ThermalResistance, HeatFlowRate>(Operator.DIVIDE)]
[QuantityOperator<TemperatureDifference, HeatFlowRate, ThermalResistance>(Operator.DIVIDE)]
[QuantityOperator<TemperatureDifference, ThermalInsulance, HeatFlowRatePerArea>(Operator.DIVIDE)]
[QuantityOperator<TemperatureDifference, HeatFlowRatePerArea, ThermalInsulance>(Operator.DIVIDE)]
[QuantityOperator<TemperatureDifference, TemperaturePerTime, Time>(Operator.DIVIDE)]
[QuantityOperator<TemperatureDifference, Time, TemperaturePerTime>(Operator.DIVIDE)]
public readonly partial struct TemperatureDifference;

[QuantityOperator<ThermalInsulance, ThermalResistance, Area>(Operator.DIVIDE)]
[QuantityOperator<ThermalInsulance, Area, ThermalResistance>(Operator.DIVIDE)]
// Formula thermal/temperature-drop-from-heat-flux
// Equation ΔT = q″R″
// Applies Use areal thermal resistance and the corresponding heat-flux component.
// Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
// Source [QUDT heat-transfer coefficient](https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer)
[QuantityOperator<ThermalInsulance, HeatFlowRatePerArea, TemperatureDifference>(Operator.MULTIPLY)]
[QuantityOperator<HeatFlowRatePerArea, ThermalInsulance, TemperatureDifference>(Operator.MULTIPLY)]
public readonly partial struct ThermalInsulance;

// Formula thermal/temperature-change-rate
// Equation ΔT = (dT/dt)̄Δt
// Applies Temperature-change rate is constant or averaged over the interval.
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<TemperaturePerTime, Time, TemperatureDifference>(Operator.MULTIPLY)]
[QuantityOperator<Time, TemperaturePerTime, TemperatureDifference>(Operator.MULTIPLY)]
public readonly partial struct TemperaturePerTime;

// Formula thermal/thermal-expansion
// Equation ΔL = L′ΔT, ΔA = A′ΔT, ΔV = V′ΔT
// Applies Use each whole object's dimensional expansion per kelvin, incorporating its initial size. The return is a change in size.
// Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<LinearThermalExpansion, TemperatureDifference, Length>(Operator.MULTIPLY)]
[QuantityOperator<TemperatureDifference, LinearThermalExpansion, Length>(Operator.MULTIPLY)]
public readonly partial struct LinearThermalExpansion;

[QuantityOperator<Length, LinearThermalExpansion, TemperatureDifference>(Operator.DIVIDE)]
public readonly partial struct Length;

// Formula thermal/thermal-expansion
[QuantityOperator<AreaThermalExpansion, TemperatureDifference, Area>(Operator.MULTIPLY)]
[QuantityOperator<TemperatureDifference, AreaThermalExpansion, Area>(Operator.MULTIPLY)]
public readonly partial struct AreaThermalExpansion;

[QuantityOperator<Area, AreaThermalExpansion, TemperatureDifference>(Operator.DIVIDE)]
public readonly partial struct Area;

// Formula thermal/thermal-expansion
[QuantityOperator<VolumeThermalExpansion, TemperatureDifference, Volume>(Operator.MULTIPLY)]
[QuantityOperator<TemperatureDifference, VolumeThermalExpansion, Volume>(Operator.MULTIPLY)]
public readonly partial struct VolumeThermalExpansion;

[QuantityOperator<Volume, VolumeThermalExpansion, TemperatureDifference>(Operator.DIVIDE)]
public readonly partial struct Volume;

// Photometry and radiometry.

// Use the average intensity over the solid angle. For luminance, use the projected emitting area.
// Formula optics/luminous-flux
// Equation Φᵥ = IᵥΩ
// Applies Use mean luminous intensity over the solid angle.
// Source [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf)
// Source [NIST photometric equations 2–6](https://www.nist.gov/system/files/documents/calibrations/sp250-15.pdf)
[QuantityOperator<LuminousIntensity, SolidAngle, LuminousFlux>(Operator.MULTIPLY)]
[QuantityOperator<SolidAngle, LuminousIntensity, LuminousFlux>(Operator.MULTIPLY)]
[QuantityOperator<LuminousIntensity, Luminance, Area>(Operator.DIVIDE)]
[QuantityOperator<LuminousIntensity, Area, Luminance>(Operator.DIVIDE)]
public readonly partial struct LuminousIntensity;

[QuantityOperator<LuminousFlux, LuminousIntensity, SolidAngle>(Operator.DIVIDE)]
[QuantityOperator<LuminousFlux, SolidAngle, LuminousIntensity>(Operator.DIVIDE)]
[QuantityOperator<LuminousFlux, Illuminance, Area>(Operator.DIVIDE)]
// Illuminance is incident luminous flux per illuminated area.
[QuantityOperator<LuminousFlux, Area, Illuminance>(Operator.DIVIDE)]
// Formula optics/luminous-energy
// Equation Qᵥ = Φ̄ᵥΔt
// Applies Constant or time-averaged luminous flux over the interval.
// Source [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf)
[QuantityOperator<LuminousFlux, Time, LuminousEnergy>(Operator.MULTIPLY)]
[QuantityOperator<Time, LuminousFlux, LuminousEnergy>(Operator.MULTIPLY)]
[QuantityOperator<LuminousFlux, LuminousEfficacy, Power>(Operator.DIVIDE)]
[QuantityOperator<LuminousFlux, Power, LuminousEfficacy>(Operator.DIVIDE)]
public readonly partial struct LuminousFlux;

// Formula optics/illuminance
// Equation Eᵥ = Φᵥ,incident / A
// Applies Incident luminous flux averaged over the receiver area.
// Source [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf)
// Source [NIST photometric equations 2–6](https://www.nist.gov/system/files/documents/calibrations/sp250-15.pdf)
[QuantityOperator<Illuminance, Area, LuminousFlux>(Operator.MULTIPLY)]
[QuantityOperator<Area, Illuminance, LuminousFlux>(Operator.MULTIPLY)]
public readonly partial struct Illuminance;

// Formula optics/luminance
// Equation Iᵥ = LᵥAproj
// Applies Use projected emitting area as viewed from the specified direction.
// Source [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf)
[QuantityOperator<Luminance, Area, LuminousIntensity>(Operator.MULTIPLY)]
[QuantityOperator<Area, Luminance, LuminousIntensity>(Operator.MULTIPLY)]
public readonly partial struct Luminance;

[QuantityOperator<LuminousEnergy, LuminousFlux, Time>(Operator.DIVIDE)]
[QuantityOperator<LuminousEnergy, Time, LuminousFlux>(Operator.DIVIDE)]
public readonly partial struct LuminousEnergy;

// Formula optics/luminous-efficacy
// Equation Φᵥ = ηᵥP
// Applies Use the power basis for which the luminous efficacy was specified.
// Source [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf)
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<LuminousEfficacy, Power, LuminousFlux>(Operator.MULTIPLY)]
[QuantityOperator<Power, LuminousEfficacy, LuminousFlux>(Operator.MULTIPLY)]
public readonly partial struct LuminousEfficacy;

// Formula optics/radiant-flux
// Equation Φₑ = IₑΩ
// Applies Use mean radiant intensity over the solid angle.
// Source [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf)
[QuantityOperator<RadiantIntensity, SolidAngle, Power>(Operator.MULTIPLY)]
[QuantityOperator<SolidAngle, RadiantIntensity, Power>(Operator.MULTIPLY)]
public readonly partial struct RadiantIntensity;

[QuantityOperator<Power, RadiantIntensity, SolidAngle>(Operator.DIVIDE)]
[QuantityOperator<Power, SolidAngle, RadiantIntensity>(Operator.DIVIDE)]
public readonly partial struct Power;

// Amount of substance and radiation dose.

// Formula chemistry/molar-mass
// Equation M = m / n
// Applies Mass and amount describe the same substance or specified entities.
// Source [IUPAC molar mass](https://goldbook.iupac.org/terms/view/12214)
[QuantityOperator<MolarMass, AmountOfSubstance, Mass>(Operator.MULTIPLY)]
[QuantityOperator<AmountOfSubstance, MolarMass, Mass>(Operator.MULTIPLY)]
[QuantityOperator<MolarMass, Density, MolarVolume>(Operator.DIVIDE)]
[QuantityOperator<MolarMass, MolarVolume, Density>(Operator.DIVIDE)]
public readonly partial struct MolarMass;

[QuantityOperator<Mass, MolarMass, AmountOfSubstance>(Operator.DIVIDE)]
[QuantityOperator<Mass, AmountOfSubstance, MolarMass>(Operator.DIVIDE)]
public readonly partial struct Mass;

// Formula chemistry/molar-volume
// Equation Vₘ = V / n
// Applies Volume and amount refer to the same sample at the same state.
// Source [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971)
[QuantityOperator<MolarVolume, AmountOfSubstance, Volume>(Operator.MULTIPLY)]
[QuantityOperator<AmountOfSubstance, MolarVolume, Volume>(Operator.MULTIPLY)]
// Formula chemistry/molar-mass-from-density
// Equation M = ρVₘ
// Applies Density and molar volume describe the same material at the same state.
// Source [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971)
// Source [QUDT molar volume](https://qudt.org/vocab/quantitykind/MolarVolume)
[QuantityOperator<MolarVolume, Density, MolarMass>(Operator.MULTIPLY)]
[QuantityOperator<Density, MolarVolume, MolarMass>(Operator.MULTIPLY)]
public readonly partial struct MolarVolume;

[QuantityOperator<Volume, MolarVolume, AmountOfSubstance>(Operator.DIVIDE)]
[QuantityOperator<Volume, AmountOfSubstance, MolarVolume>(Operator.DIVIDE)]
public readonly partial struct Volume;

// Formula chemistry/molar-energy
// Equation Eₘ = E / n
// Applies Use energy for the same specified amount of substance.
// Source [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971)
[QuantityOperator<MolarEnergy, AmountOfSubstance, Energy>(Operator.MULTIPLY)]
[QuantityOperator<AmountOfSubstance, MolarEnergy, Energy>(Operator.MULTIPLY)]
public readonly partial struct MolarEnergy;

[QuantityOperator<Energy, MolarEnergy, AmountOfSubstance>(Operator.DIVIDE)]
[QuantityOperator<Energy, AmountOfSubstance, MolarEnergy>(Operator.DIVIDE)]
[QuantityOperator<Energy, AbsorbedDose, Mass>(Operator.DIVIDE)]
public readonly partial struct Energy;

// Formula chemistry/molar-heat-capacity
// Equation Cₘ = C / n
// Applies Use heat capacity for the same amount and thermodynamic process.
// Source [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971)
// Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
[QuantityOperator<MolarHeatCapacity, AmountOfSubstance, HeatCapacity>(Operator.MULTIPLY)]
[QuantityOperator<AmountOfSubstance, MolarHeatCapacity, HeatCapacity>(Operator.MULTIPLY)]
public readonly partial struct MolarHeatCapacity;

[QuantityOperator<HeatCapacity, MolarHeatCapacity, AmountOfSubstance>(Operator.DIVIDE)]
[QuantityOperator<HeatCapacity, AmountOfSubstance, MolarHeatCapacity>(Operator.DIVIDE)]
public readonly partial struct HeatCapacity;

// Formula chemistry/amount-concentration
// Equation c = n / V
// Applies Amount of the constituent divided by the volume of the mixture.
// Source [IUPAC amount concentration](https://goldbook.iupac.org/terms/view/A00295)
[QuantityOperator<AmountOfSubstancePerVolume, Volume, AmountOfSubstance>(Operator.MULTIPLY)]
[QuantityOperator<Volume, AmountOfSubstancePerVolume, AmountOfSubstance>(Operator.MULTIPLY)]
public readonly partial struct AmountOfSubstancePerVolume;

[QuantityOperator<AmountOfSubstance, AmountOfSubstancePerVolume, Volume>(Operator.DIVIDE)]
[QuantityOperator<AmountOfSubstance, Volume, AmountOfSubstancePerVolume>(Operator.DIVIDE)]
[QuantityOperator<AmountOfSubstance, AmountOfSubstancePerMass, Mass>(Operator.DIVIDE)]
[QuantityOperator<AmountOfSubstance, Mass, AmountOfSubstancePerMass>(Operator.DIVIDE)]
[QuantityOperator<AmountOfSubstance, CatalyticActivity, Time>(Operator.DIVIDE)]
public readonly partial struct AmountOfSubstance;

// Formula chemistry/amount-per-mass
// Equation b = n / m
// Applies Use the reference mass for the specified amount-per-mass quantity, such as solvent mass when calculating molality.
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
// Source [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971)
[QuantityOperator<AmountOfSubstancePerMass, Mass, AmountOfSubstance>(Operator.MULTIPLY)]
[QuantityOperator<Mass, AmountOfSubstancePerMass, AmountOfSubstance>(Operator.MULTIPLY)]
public readonly partial struct AmountOfSubstancePerMass;

// Formula chemistry/absorbed-dose-rate
// Equation ΔD = Ḋ̄Δt
// Applies Constant or interval-averaged absorbed dose rate for the same matter.
// Source [IAEA dosimetry](https://www-pub.iaea.org/MTCD/Publications/PDF/te_1540_web.pdf)
[QuantityOperator<AbsorbedDoseRate, Time, AbsorbedDose>(Operator.MULTIPLY)]
[QuantityOperator<Time, AbsorbedDoseRate, AbsorbedDose>(Operator.MULTIPLY)]
public readonly partial struct AbsorbedDoseRate;

[QuantityOperator<AbsorbedDose, AbsorbedDoseRate, Time>(Operator.DIVIDE)]
[QuantityOperator<AbsorbedDose, Time, AbsorbedDoseRate>(Operator.DIVIDE)]
// The dose is the mean over the same mass to which the deposited energy refers.
// Formula chemistry/absorbed-dose
// Equation D = Edeposited / m
// Applies Mean energy deposited by ionizing radiation in the specified matter.
// Source [IAEA dosimetry](https://www-pub.iaea.org/MTCD/Publications/PDF/te_1540_web.pdf)
[QuantityOperator<AbsorbedDose, Mass, Energy>(Operator.MULTIPLY)]
[QuantityOperator<Mass, AbsorbedDose, Energy>(Operator.MULTIPLY)]
public readonly partial struct AbsorbedDose;

// Formula chemistry/catalytic-activity
// Equation Δn = ācΔt
// Applies Use the constant or time-averaged catalytic activity under the specified assay conditions.
// Source [BIPM SI Brochure](https://www.bipm.org/en/publications/si-brochure)
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<CatalyticActivity, Time, AmountOfSubstance>(Operator.MULTIPLY)]
[QuantityOperator<Time, CatalyticActivity, AmountOfSubstance>(Operator.MULTIPLY)]
public readonly partial struct CatalyticActivity;

// Pressure, power, and reciprocal density.

// Supply the force component along the direction represented by the velocity.
// Formula mechanics/mechanical-power
// Equation P = F∥v
// Applies Use the signed force component along the velocity axis.
// Source [OpenStax work and energy](https://openstax.org/books/university-physics-volume-1/pages/7-key-equations)
[QuantityOperator<Force, LinearVelocity, Power>(Operator.MULTIPLY)]
[QuantityOperator<LinearVelocity, Force, Power>(Operator.MULTIPLY)]
[QuantityOperator<Force, Pressure, Area>(Operator.DIVIDE)]
public readonly partial struct Force;

[QuantityOperator<Power, Force, LinearVelocity>(Operator.DIVIDE)]
[QuantityOperator<Power, LinearVelocity, Force>(Operator.DIVIDE)]
[QuantityOperator<Power, Pressure, VolumePerTime>(Operator.DIVIDE)]
public readonly partial struct Power;

// Formula fluids/pressure-from-force
[QuantityOperator<Pressure, Area, Force>(Operator.MULTIPLY)]
[QuantityOperator<Area, Pressure, Force>(Operator.MULTIPLY)]
// For hydraulic power, the pressure operand is the signed difference across the flow path.
// Formula fluids/hydraulic-power
[QuantityOperator<Pressure, VolumePerTime, Power>(Operator.MULTIPLY)]
[QuantityOperator<VolumePerTime, Pressure, Power>(Operator.MULTIPLY)]
public readonly partial struct Pressure;

[QuantityOperator<double, Density, SpecificVolume>(Operator.DIVIDE)]
// Formula fluids/density-and-specific-volume
[QuantityOperator<Density, SpecificVolume, DimensionlessRatio>(Operator.MULTIPLY)]
[QuantityOperator<SpecificVolume, Density, DimensionlessRatio>(Operator.MULTIPLY)]
public readonly partial struct Density;

[QuantityOperator<double, SpecificVolume, Density>(Operator.DIVIDE)]
public readonly partial struct SpecificVolume;

// Conductance, resistivity, and the mass-to-charge ratio.

// Formula electromagnetism/ohms-law
[QuantityOperator<Conductance, ElectricPotential, ElectricCurrent>(Operator.MULTIPLY)]
[QuantityOperator<ElectricPotential, Conductance, ElectricCurrent>(Operator.MULTIPLY)]
[QuantityOperator<double, Conductance, Resistance>(Operator.DIVIDE)]
public readonly partial struct Conductance;

[QuantityOperator<ElectricCurrent, Conductance, ElectricPotential>(Operator.DIVIDE)]
[QuantityOperator<ElectricCurrent, ElectricPotential, Conductance>(Operator.DIVIDE)]
public readonly partial struct ElectricCurrent;

[QuantityOperator<double, Resistance, Conductance>(Operator.DIVIDE)]
// Formula electromagnetism/ohms-law
[QuantityOperator<Resistance, Conductance, DimensionlessRatio>(Operator.MULTIPLY)]
[QuantityOperator<Conductance, Resistance, DimensionlessRatio>(Operator.MULTIPLY)]
public readonly partial struct Resistance;

[QuantityOperator<double, ElectricConductivity, Resistivity>(Operator.DIVIDE)]
// Formula electromagnetism/conductivity-and-resistivity
// Equation ρₑ = 1 / σ
// Applies Resistivity and conductivity refer to the same ohmic material and state.
// Source [OpenStax resistivity](https://openstax.org/books/university-physics-volume-2/pages/9-3-resistivity-and-resistance)
[QuantityOperator<ElectricConductivity, Resistivity, DimensionlessRatio>(Operator.MULTIPLY)]
[QuantityOperator<Resistivity, ElectricConductivity, DimensionlessRatio>(Operator.MULTIPLY)]
public readonly partial struct ElectricConductivity;

[QuantityOperator<double, Resistivity, ElectricConductivity>(Operator.DIVIDE)]
public readonly partial struct Resistivity;

// Formula electromagnetism/mass-per-charge
// Equation m = (m / Q)Q
// Applies Mass and signed charge refer to the same charged entities.
// Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
[QuantityOperator<MassPerElectricCharge, ElectricCharge, Mass>(Operator.MULTIPLY)]
[QuantityOperator<ElectricCharge, MassPerElectricCharge, Mass>(Operator.MULTIPLY)]
public readonly partial struct MassPerElectricCharge;

[QuantityOperator<Mass, MassPerElectricCharge, ElectricCharge>(Operator.DIVIDE)]
[QuantityOperator<Mass, ElectricCharge, MassPerElectricCharge>(Operator.DIVIDE)]
public readonly partial struct Mass;

// Temperature gradients and thermal material properties.

// Formula thermal/temperature-gradient
// Equation ∂T/∂x ≈ ΔT / Δx
// Applies Signed temperature difference over a distance along the axis. The quotient gives the interval average.
// Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
[QuantityOperator<TemperatureGradient, Length, TemperatureDifference>(Operator.MULTIPLY)]
[QuantityOperator<Length, TemperatureGradient, TemperatureDifference>(Operator.MULTIPLY)]
public readonly partial struct TemperatureGradient;

[QuantityOperator<TemperatureDifference, TemperatureGradient, Length>(Operator.DIVIDE)]
[QuantityOperator<TemperatureDifference, Length, TemperatureGradient>(Operator.DIVIDE)]
public readonly partial struct TemperatureDifference;

[QuantityOperator<double, ThermalConductivity, ThermalResistivity>(Operator.DIVIDE)]
// Formula thermal/thermal-resistivity
// Equation ρₜ = 1 / k
// Applies Thermal resistivity and conductivity describe the same material and direction.
// Source [QUDT thermal resistivity](https://qudt.org/vocab/quantitykind/ThermalResistivity)
[QuantityOperator<ThermalConductivity, ThermalResistivity, DimensionlessRatio>(Operator.MULTIPLY)]
[QuantityOperator<ThermalResistivity, ThermalConductivity, DimensionlessRatio>(Operator.MULTIPLY)]
public readonly partial struct ThermalConductivity;

[QuantityOperator<double, ThermalResistivity, ThermalConductivity>(Operator.DIVIDE)]
// A uniform plane layer's depth times thermal resistivity gives its areal resistance.
// Formula thermal/plane-layer-insulance
// Equation R″ = ℓ/k = ℓρₜ
// Applies Uniform plane layer. Use thickness along the heat-flow direction.
// Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
// Source [QUDT thermal resistivity](https://qudt.org/vocab/quantitykind/ThermalResistivity)
[QuantityOperator<ThermalResistivity, Length, ThermalInsulance>(Operator.MULTIPLY)]
[QuantityOperator<Length, ThermalResistivity, ThermalInsulance>(Operator.MULTIPLY)]
public readonly partial struct ThermalResistivity;

// Formula thermal/plane-layer-insulance
[QuantityOperator<ThermalInsulance, ThermalConductivity, Length>(Operator.MULTIPLY)]
[QuantityOperator<ThermalConductivity, ThermalInsulance, Length>(Operator.MULTIPLY)]
[QuantityOperator<ThermalInsulance, ThermalResistivity, Length>(Operator.DIVIDE)]
[QuantityOperator<ThermalInsulance, Length, ThermalResistivity>(Operator.DIVIDE)]
public readonly partial struct ThermalInsulance;

[QuantityOperator<Length, ThermalInsulance, ThermalConductivity>(Operator.DIVIDE)]
[QuantityOperator<Length, ThermalConductivity, ThermalInsulance>(Operator.DIVIDE)]
public readonly partial struct Length;

// Wave propagation uses wavelength and frequency for the same wave.
// Formula mechanics/wave-speed
// Equation v = λf
// Applies Wavelength and frequency belong to the same propagating wave.
// Source [OpenStax waves](https://openstax.org/books/university-physics-volume-1/pages/16-1-traveling-waves)
[QuantityOperator<Wavelength, Frequency, LinearVelocity>(Operator.MULTIPLY)]
[QuantityOperator<Frequency, Wavelength, LinearVelocity>(Operator.MULTIPLY)]
public readonly partial struct Wavelength;

[QuantityOperator<LinearVelocity, Wavelength, Frequency>(Operator.DIVIDE)]
[QuantityOperator<LinearVelocity, Frequency, Wavelength>(Operator.DIVIDE)]
public readonly partial struct LinearVelocity;

// Formula mechanics/impulse-and-momentum-change
[QuantityOperator<Impulse, Time, Force>(Operator.DIVIDE)]
[QuantityOperator<Impulse, Force, Time>(Operator.DIVIDE)]
public readonly partial struct Impulse;

// Conductance is for the whole thermal path. The heat-transfer coefficient is per area.
// Formula thermal/thermal-path
[QuantityOperator<ThermalConductance, TemperatureDifference, HeatFlowRate>(Operator.MULTIPLY)]
[QuantityOperator<TemperatureDifference, ThermalConductance, HeatFlowRate>(Operator.MULTIPLY)]
[QuantityOperator<double, ThermalConductance, ThermalResistance>(Operator.DIVIDE)]
public readonly partial struct ThermalConductance;

[QuantityOperator<HeatFlowRate, TemperatureDifference, ThermalConductance>(Operator.DIVIDE)]
[QuantityOperator<HeatFlowRate, ThermalConductance, TemperatureDifference>(Operator.DIVIDE)]
public readonly partial struct HeatFlowRate;

// Formula thermal/heat-transfer-conductance
// Equation Gₜ = hA
// Applies Uniform or area-averaged heat-transfer coefficient for the same thermal path and driving temperature difference.
// Source [QUDT heat-transfer coefficient](https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer)
// Source [QUDT thermal conductance](https://qudt.org/vocab/quantitykind/ThermalConductance)
[QuantityOperator<CoefficientOfHeatTransfer, Area, ThermalConductance>(Operator.MULTIPLY)]
[QuantityOperator<Area, CoefficientOfHeatTransfer, ThermalConductance>(Operator.MULTIPLY)]
public readonly partial struct CoefficientOfHeatTransfer;

[QuantityOperator<ThermalConductance, Area, CoefficientOfHeatTransfer>(Operator.DIVIDE)]
[QuantityOperator<ThermalConductance, CoefficientOfHeatTransfer, Area>(Operator.DIVIDE)]
public readonly partial struct ThermalConductance;

[QuantityOperator<double, ThermalResistance, ThermalConductance>(Operator.DIVIDE)]
// Formula thermal/thermal-path
[QuantityOperator<ThermalResistance, ThermalConductance, DimensionlessRatio>(Operator.MULTIPLY)]
[QuantityOperator<ThermalConductance, ThermalResistance, DimensionlessRatio>(Operator.MULTIPLY)]
public readonly partial struct ThermalResistance;
