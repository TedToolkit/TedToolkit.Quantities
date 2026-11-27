// -----------------------------------------------------------------------
// <copyright file="QuantityRelations.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Quantities;

[QuantityOperator<Length, Length, Area>(Operator.MULTIPLY,
    FormulaId = "geometry/rectangle-area", Equation = "A = ab",
    Applicability = "Rectangular sides are perpendicular. The inverse solves for the other side.",
    Sources = new[] { FormulaSources.CircleArea })]
[QuantityOperator<Length, Length, DimensionlessRatio>(Operator.DIVIDE,
    FormulaId = "geometry/length-ratio", Equation = "Length ratio = ℓ₂ / ℓ₁",
    Applicability = "The reference length is nonzero.",
    Sources = new[] { FormulaSources.QudtKinds })]
[QuantityOperator<Length, Time, LinearVelocity>(Operator.DIVIDE)]
[QuantityOperator<Length, LinearVelocity, Time>(Operator.DIVIDE)]
[QuantityOperator<double, Length, InverseLength>(Operator.DIVIDE)]
[QuantityOperator<Length, InverseLength, DimensionlessRatio>(Operator.MULTIPLY,
    FormulaId = "geometry/inverse-length", Equation = "Inverse length = 1 / ℓ",
    Applicability = "Use a nonzero length. The product with the corresponding inverse length is one.",
    Sources = new[] { FormulaSources.NistSi })]
[QuantityOperator<InverseLength, Length, DimensionlessRatio>(Operator.MULTIPLY)]
public readonly partial struct Length;

[QuantityOperator<Area, Length, Volume>(Operator.MULTIPLY,
    FormulaId = "geometry/prism-volume", Equation = "V = Aℓ",
    Applicability = "Prism or extrusion with uniform cross section perpendicular to its length.",
    Sources = new[] { FormulaSources.Prism })]
[QuantityOperator<Length, Area, Volume>(Operator.MULTIPLY)]
[QuantityOperator<Area, Length, Length>(Operator.DIVIDE)]
public readonly partial struct Area;

[QuantityOperator<Volume, Area, Length>(Operator.DIVIDE)]
[QuantityOperator<Volume, Length, Area>(Operator.DIVIDE)]
[QuantityOperator<Volume, Time, VolumePerTime>(Operator.DIVIDE)]
public readonly partial struct Volume;

[QuantityOperator<LinearVelocity, Time, Length>(Operator.MULTIPLY,
    FormulaId = "mechanics/linear-displacement", Equation = "Δx = v̄Δt",
    Applicability = "Constant or average velocity component along the displacement axis.",
    Sources = new[] { FormulaSources.Motion })]
[QuantityOperator<Time, LinearVelocity, Length>(Operator.MULTIPLY)]
[QuantityOperator<LinearVelocity, Time, LinearAcceleration>(Operator.DIVIDE)]
[QuantityOperator<LinearVelocity, LinearAcceleration, Time>(Operator.DIVIDE)]
public readonly partial struct LinearVelocity;

[QuantityOperator<LinearAcceleration, Time, LinearVelocity>(Operator.MULTIPLY,
    FormulaId = "mechanics/velocity-change", Equation = "Δv = āΔt",
    Applicability = "Constant or average acceleration component over the interval.",
    Sources = new[] { FormulaSources.Motion })]
[QuantityOperator<Time, LinearAcceleration, LinearVelocity>(Operator.MULTIPLY)]
public readonly partial struct LinearAcceleration;

[QuantityOperator<Mass, LinearAcceleration, Force>(Operator.MULTIPLY,
    FormulaId = "mechanics/force", Equation = "F = ma",
    Applicability = "Net force along an axis for a constant-mass body.",
    Sources = new[] { FormulaSources.Dynamics })]
[QuantityOperator<LinearAcceleration, Mass, Force>(Operator.MULTIPLY)]
[QuantityOperator<Mass, LinearVelocity, LinearMomentum>(Operator.MULTIPLY,
    FormulaId = "mechanics/linear-momentum", Equation = "p = mv",
    Applicability = "Linear momentum component for the same mass and velocity.",
    Sources = new[] { FormulaSources.Impulse })]
[QuantityOperator<LinearVelocity, Mass, LinearMomentum>(Operator.MULTIPLY)]
[QuantityOperator<Mass, Volume, Density>(Operator.DIVIDE)]
public readonly partial struct Mass;

[QuantityOperator<Force, Mass, LinearAcceleration>(Operator.DIVIDE)]
[QuantityOperator<Force, LinearAcceleration, Mass>(Operator.DIVIDE)]
public readonly partial struct Force;

[QuantityOperator<Density, Volume, Mass>(Operator.MULTIPLY,
    FormulaId = "fluids/density-and-specific-volume", Equation = "ρ = m / V, vₛ = V / m = 1 / ρ",
    Applicability = "Density and specific volume describe the same material at the same state.",
    Sources = new[] { FormulaSources.Fluids, FormulaSources.QudtKinds })]
[QuantityOperator<Volume, Density, Mass>(Operator.MULTIPLY)]
public readonly partial struct Density;

[QuantityOperator<Power, Time, Energy>(Operator.MULTIPLY,
    FormulaId = "mechanics/energy-from-power", Equation = "E = P̄Δt",
    Applicability = "Constant or average power or heat-flow rate over the interval.",
    Sources = new[] { FormulaSources.Work, FormulaSources.Thermal })]
[QuantityOperator<Time, Power, Energy>(Operator.MULTIPLY)]
public readonly partial struct Power;

[QuantityOperator<Energy, Time, Power>(Operator.DIVIDE)]
[QuantityOperator<Energy, Power, Time>(Operator.DIVIDE)]
[QuantityOperator<Energy, Energy, SquareEnergy>(Operator.MULTIPLY,
    FormulaId = "geometry/squared-energy", Equation = "Squared energy = E²",
    Applicability = "Algebraic square of the energy value.",
    Sources = new[] { FormulaSources.QudtKinds })]
public readonly partial struct Energy;

[QuantityOperator<ElectricCurrent, Time, ElectricCharge>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/charge-from-current", Equation = "ΔQ = ĪΔt",
    Applicability = "Use average electrical current through the same cross section and interval.",
    Sources = new[] { FormulaSources.Current })]
[QuantityOperator<Time, ElectricCurrent, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricCurrent;

[QuantityOperator<ElectricCharge, Time, ElectricCurrent>(Operator.DIVIDE)]
[QuantityOperator<ElectricCharge, ElectricCurrent, Time>(Operator.DIVIDE)]
public readonly partial struct ElectricCharge;

[QuantityOperator<ThermodynamicTemperature, ThermodynamicTemperature, TemperatureDifference>(Operator.SUBTRACT)]
[QuantityOperator<ThermodynamicTemperature, TemperatureDifference, ThermodynamicTemperature>(Operator.ADD,
    FormulaId = "thermal/temperature-change", Equation = "ΔT = T₂ − T₁, T₂ = T₁ + ΔT",
    Applicability = "Use the temperature difference as the increment of an absolute temperature.",
    Sources = new[] { FormulaSources.BipmSi })]
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

[QuantityOperator<Time, Time, SquareTime>(Operator.MULTIPLY,
    FormulaId = "geometry/squared-time", Equation = "Squared time = t²",
    Applicability = "Algebraic square of the elapsed time.",
    Sources = new[] { FormulaSources.QudtKinds })]
public readonly partial struct Time;

[QuantityOperator<SquareTime, Time, Time>(Operator.DIVIDE)]
public readonly partial struct SquareTime;

[QuantityOperator<LinearMomentum, Mass, LinearVelocity>(Operator.DIVIDE)]
[QuantityOperator<LinearMomentum, LinearVelocity, Mass>(Operator.DIVIDE)]
[QuantityOperator<LinearMomentum, Force, Time>(Operator.DIVIDE)]
[QuantityOperator<LinearMomentum, Time, Force>(Operator.DIVIDE)]
public readonly partial struct LinearMomentum;

// Average force over the interval gives the change in linear momentum.
[QuantityOperator<Force, Time, LinearMomentum>(Operator.MULTIPLY,
    FormulaId = "mechanics/impulse-and-momentum-change", Equation = "J = Δp = F̄Δt",
    Applicability = "Use the signed component of the average force over the interval.",
    Sources = new[] { FormulaSources.Impulse })]
[QuantityOperator<Time, Force, LinearMomentum>(Operator.MULTIPLY)]
[QuantityOperator<Force, ForcePerArea, Area>(Operator.DIVIDE)]
[QuantityOperator<Force, Area, ForcePerArea>(Operator.DIVIDE)]
[QuantityOperator<Force, ForcePerLength, Length>(Operator.DIVIDE)]
[QuantityOperator<Force, Length, ForcePerLength>(Operator.DIVIDE)]
public readonly partial struct Force;

[QuantityOperator<ForcePerArea, Area, Force>(Operator.MULTIPLY,
    FormulaId = "fluids/pressure-from-force", Equation = "p = Fₙ / A",
    Applicability = "Use the normal force over the surface. The general division returns `ForcePerArea`.",
    Sources = new[] { FormulaSources.Fluids })]
[QuantityOperator<Area, ForcePerArea, Force>(Operator.MULTIPLY)]
[QuantityOperator<ForcePerArea, VolumePerTime, Power>(Operator.MULTIPLY,
    FormulaId = "fluids/hydraulic-power", Equation = "P = ΔpQᵥ",
    Applicability = "Pressure difference and flow refer to the same path. Use pressures referenced to the same datum.",
    Sources = new[] { FormulaSources.Fluids, FormulaSources.Pv })]
[QuantityOperator<VolumePerTime, ForcePerArea, Power>(Operator.MULTIPLY)]
public readonly partial struct ForcePerArea;

[QuantityOperator<ForcePerLength, Length, Force>(Operator.MULTIPLY,
    FormulaId = "mechanics/distributed-force", Equation = "F = (F / ℓ) × ℓ",
    Applicability = "Uniform or mean force per length over the loaded span.",
    Sources = new[] { FormulaSources.QudtKinds, FormulaSources.NistSi })]
[QuantityOperator<Length, ForcePerLength, Force>(Operator.MULTIPLY)]
public readonly partial struct ForcePerLength;

[QuantityOperator<AngularVelocity, Time, PlaneAngle>(Operator.MULTIPLY,
    FormulaId = "mechanics/angular-displacement", Equation = "Δθ = ω̄Δt",
    Applicability = "Fixed axis and angular velocity averaged over the interval.",
    Sources = new[] { FormulaSources.Arc })]
[QuantityOperator<Time, AngularVelocity, PlaneAngle>(Operator.MULTIPLY)]
[QuantityOperator<AngularVelocity, AngularAcceleration, Time>(Operator.DIVIDE)]
[QuantityOperator<AngularVelocity, Time, AngularAcceleration>(Operator.DIVIDE)]
public readonly partial struct AngularVelocity;

[QuantityOperator<PlaneAngle, AngularVelocity, Time>(Operator.DIVIDE)]
[QuantityOperator<PlaneAngle, Time, AngularVelocity>(Operator.DIVIDE)]
public readonly partial struct PlaneAngle;

[QuantityOperator<AngularAcceleration, Time, AngularVelocity>(Operator.MULTIPLY,
    FormulaId = "mechanics/angular-velocity-change", Equation = "Δω = ᾱΔt",
    Applicability = "Fixed axis and angular acceleration averaged over the interval.",
    Sources = new[] { FormulaSources.Rotation })]
[QuantityOperator<Time, AngularAcceleration, AngularVelocity>(Operator.MULTIPLY)]
public readonly partial struct AngularAcceleration;

[QuantityOperator<MomentOfInertia, AngularAcceleration, Torque>(Operator.MULTIPLY,
    FormulaId = "mechanics/torque-from-angular-acceleration", Equation = "τ = Iα",
    Applicability = "Net torque and moment of inertia about the same fixed axis.",
    Sources = new[] { FormulaSources.Rotation })]
[QuantityOperator<AngularAcceleration, MomentOfInertia, Torque>(Operator.MULTIPLY)]
[QuantityOperator<MomentOfInertia, AngularVelocity, AngularMomentum>(Operator.MULTIPLY,
    FormulaId = "mechanics/angular-momentum", Equation = "L = Iω",
    Applicability = "Rigid-body angular momentum component about the specified axis.",
    Sources = new[] { FormulaSources.AngularMomentum })]
[QuantityOperator<AngularVelocity, MomentOfInertia, AngularMomentum>(Operator.MULTIPLY)]
public readonly partial struct MomentOfInertia;

[QuantityOperator<Torque, MomentOfInertia, AngularAcceleration>(Operator.DIVIDE)]
[QuantityOperator<Torque, AngularAcceleration, MomentOfInertia>(Operator.DIVIDE)]
// Average torque over the interval gives the change in angular momentum about the same axis.
[QuantityOperator<Torque, Time, AngularMomentum>(Operator.MULTIPLY,
    FormulaId = "mechanics/angular-momentum-change", Equation = "ΔL = τ̄Δt",
    Applicability = "Average torque and angular momentum change about the same axis.",
    Sources = new[] { FormulaSources.AngularMomentum })]
[QuantityOperator<Time, Torque, AngularMomentum>(Operator.MULTIPLY)]
[QuantityOperator<Torque, AngularVelocity, Power>(Operator.MULTIPLY,
    FormulaId = "mechanics/shaft-power", Equation = "P = τω",
    Applicability = "Corresponding torque and angular velocity components about the same axis.",
    Sources = new[] { FormulaSources.Rotation })]
[QuantityOperator<AngularVelocity, Torque, Power>(Operator.MULTIPLY)]
[QuantityOperator<Torque, PlaneAngle, Energy>(Operator.MULTIPLY,
    FormulaId = "mechanics/rotational-work", Equation = "W = τ̄Δθ",
    Applicability = "Constant or angle-averaged torque along the rotation.",
    Sources = new[] { FormulaSources.Rotation })]
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

[QuantityOperator<MassPerLength, Length, Mass>(Operator.MULTIPLY,
    FormulaId = "mechanics/linear-mass-density", Equation = "m = λₘℓ",
    Applicability = "Uniform or mean linear mass density λₘ over the length.",
    Sources = new[] { FormulaSources.QudtKinds })]
[QuantityOperator<Length, MassPerLength, Mass>(Operator.MULTIPLY)]
public readonly partial struct MassPerLength;

[QuantityOperator<MassPerArea, Area, Mass>(Operator.MULTIPLY,
    FormulaId = "mechanics/areal-mass-density", Equation = "m = ρₐA",
    Applicability = "Uniform or mean areal mass density ρₐ over the area.",
    Sources = new[] { FormulaSources.QudtKinds })]
[QuantityOperator<Area, MassPerArea, Mass>(Operator.MULTIPLY)]
[QuantityOperator<MassPerArea, MassPerAreaTime, Time>(Operator.DIVIDE)]
[QuantityOperator<MassPerArea, Time, MassPerAreaTime>(Operator.DIVIDE)]
public readonly partial struct MassPerArea;

[QuantityOperator<MassPerTime, Time, Mass>(Operator.MULTIPLY,
    FormulaId = "fluids/mass-from-flow", Equation = "Δm = ṁ̄Δt",
    Applicability = "Constant or time-averaged mass flow rate over the interval.",
    Sources = new[] { FormulaSources.MassFlow })]
[QuantityOperator<Time, MassPerTime, Mass>(Operator.MULTIPLY)]
[QuantityOperator<MassPerTime, MassPerAreaTime, Area>(Operator.DIVIDE)]
[QuantityOperator<MassPerTime, Area, MassPerAreaTime>(Operator.DIVIDE)]
[QuantityOperator<MassPerTime, Density, VolumePerTime>(Operator.DIVIDE)]
[QuantityOperator<MassPerTime, VolumePerTime, Density>(Operator.DIVIDE)]
public readonly partial struct MassPerTime;

[QuantityOperator<MassPerAreaTime, Area, MassPerTime>(Operator.MULTIPLY, FormulaId = "fluids/areal-mass-flow")]
[QuantityOperator<Area, MassPerAreaTime, MassPerTime>(Operator.MULTIPLY)]
[QuantityOperator<MassPerAreaTime, Time, MassPerArea>(Operator.MULTIPLY,
    FormulaId = "fluids/areal-mass-flow", Equation = "jₘ = ṁ / A = Δ(m/A) / Δt",
    Applicability = "Use the same oriented surface and interval for the areal mass flow rate jₘ.",
    Sources = new[] { FormulaSources.MassFlow, FormulaSources.QudtKinds })]
[QuantityOperator<Time, MassPerAreaTime, MassPerArea>(Operator.MULTIPLY)]
public readonly partial struct MassPerAreaTime;

[QuantityOperator<SpecificVolume, Mass, Volume>(Operator.MULTIPLY, FormulaId = "fluids/density-and-specific-volume")]
[QuantityOperator<Mass, SpecificVolume, Volume>(Operator.MULTIPLY)]
public readonly partial struct SpecificVolume;

[QuantityOperator<Volume, SpecificVolume, Mass>(Operator.DIVIDE)]
[QuantityOperator<Volume, Mass, SpecificVolume>(Operator.DIVIDE)]
[QuantityOperator<Volume, VolumePerTime, Time>(Operator.DIVIDE)]
public readonly partial struct Volume;

[QuantityOperator<Area, LinearVelocity, VolumePerTime>(Operator.MULTIPLY,
    FormulaId = "fluids/volume-flow-from-velocity", Equation = "Qᵥ = Avₙ",
    Applicability = "Area-averaged normal speed component through the section.",
    Sources = new[] { FormulaSources.Fluids, FormulaSources.MassFlow })]
[QuantityOperator<LinearVelocity, Area, VolumePerTime>(Operator.MULTIPLY)]
public readonly partial struct Area;

[QuantityOperator<VolumePerTime, Area, LinearVelocity>(Operator.DIVIDE)]
[QuantityOperator<VolumePerTime, LinearVelocity, Area>(Operator.DIVIDE)]
[QuantityOperator<VolumePerTime, Time, Volume>(Operator.MULTIPLY,
    FormulaId = "fluids/volume-from-flow", Equation = "ΔV = Q̄ᵥΔt",
    Applicability = "Constant or time-averaged volumetric flow rate.",
    Sources = new[] { FormulaSources.Fluids })]
[QuantityOperator<Time, VolumePerTime, Volume>(Operator.MULTIPLY)]
public readonly partial struct VolumePerTime;

[QuantityOperator<Density, VolumePerTime, MassPerTime>(Operator.MULTIPLY,
    FormulaId = "fluids/mass-flow-from-volume-flow", Equation = "ṁ = ρQᵥ",
    Applicability = "Use consistent density and volumetric flow at the same section.",
    Sources = new[] { FormulaSources.MassFlow })]
[QuantityOperator<VolumePerTime, Density, MassPerTime>(Operator.MULTIPLY)]
public readonly partial struct Density;

[QuantityOperator<KinematicViscosity, Density, DynamicViscosity>(Operator.MULTIPLY,
    FormulaId = "fluids/viscosity", Equation = "μ = ρν",
    Applicability = "Density, dynamic viscosity, and kinematic viscosity at the same fluid state.",
    Sources = new[] { FormulaSources.Viscosity })]
[QuantityOperator<Density, KinematicViscosity, DynamicViscosity>(Operator.MULTIPLY)]
public readonly partial struct KinematicViscosity;

[QuantityOperator<DynamicViscosity, KinematicViscosity, Density>(Operator.DIVIDE)]
[QuantityOperator<DynamicViscosity, Density, KinematicViscosity>(Operator.DIVIDE)]
public readonly partial struct DynamicViscosity;

[QuantityOperator<SpecificEnergy, Mass, Energy>(Operator.MULTIPLY,
    FormulaId = "mechanics/specific-energy", Equation = "e = E / m",
    Applicability = "Energy and mass refer to the same body or sample.",
    Sources = new[] { FormulaSources.NistSi })]
[QuantityOperator<Mass, SpecificEnergy, Energy>(Operator.MULTIPLY)]
public readonly partial struct SpecificEnergy;

[QuantityOperator<Energy, SpecificEnergy, Mass>(Operator.DIVIDE)]
[QuantityOperator<Energy, Mass, SpecificEnergy>(Operator.DIVIDE)]
[QuantityOperator<Energy, EnergyDensity, Volume>(Operator.DIVIDE)]
[QuantityOperator<Energy, Volume, EnergyDensity>(Operator.DIVIDE)]
[QuantityOperator<Energy, EnergyPerArea, Area>(Operator.DIVIDE)]
[QuantityOperator<Energy, Area, EnergyPerArea>(Operator.DIVIDE)]
public readonly partial struct Energy;

[QuantityOperator<EnergyDensity, Volume, Energy>(Operator.MULTIPLY,
    FormulaId = "mechanics/energy-density", Equation = "u = E / V",
    Applicability = "Average energy per volume of the same region.",
    Sources = new[] { FormulaSources.NistSi })]
[QuantityOperator<Volume, EnergyDensity, Energy>(Operator.MULTIPLY)]
public readonly partial struct EnergyDensity;

[QuantityOperator<EnergyPerArea, Area, Energy>(Operator.MULTIPLY,
    FormulaId = "mechanics/energy-per-area", Equation = "Eₐ = E / A",
    Applicability = "Average energy per area over the relevant surface.",
    Sources = new[] { FormulaSources.QudtKinds })]
[QuantityOperator<Area, EnergyPerArea, Energy>(Operator.MULTIPLY)]
public readonly partial struct EnergyPerArea;

[QuantityOperator<PowerPerArea, Area, Power>(Operator.MULTIPLY,
    FormulaId = "mechanics/power-per-area", Equation = "Pₐ = P / A",
    Applicability = "Average power per area over the same surface.",
    Sources = new[] { FormulaSources.NistSi })]
[QuantityOperator<Area, PowerPerArea, Power>(Operator.MULTIPLY)]
public readonly partial struct PowerPerArea;

[QuantityOperator<Power, PowerPerArea, Area>(Operator.DIVIDE)]
[QuantityOperator<Power, Area, PowerPerArea>(Operator.DIVIDE)]
public readonly partial struct Power;

// Electricity and magnetism.

[QuantityOperator<Resistance, ElectricCurrent, ElectricPotential>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/ohms-law", Equation = "V = IR, I = GV, G = 1 / R",
    Applicability = "Voltage and current refer to the same ohmic element or operating point.",
    Sources = new[] { FormulaSources.Current, FormulaSources.NistSi })]
[QuantityOperator<ElectricCurrent, Resistance, ElectricPotential>(Operator.MULTIPLY)]
[QuantityOperator<Resistance, Capacitance, Time>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/rc-time-constant", Equation = "τ = RC",
    Applicability = "Time constant of the equivalent first-order RC circuit.",
    Sources = new[] { FormulaSources.Dc })]
[QuantityOperator<Capacitance, Resistance, Time>(Operator.MULTIPLY)]
[QuantityOperator<Resistance, Time, Inductance>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/rl-time-constant", Equation = "τ = L / R",
    Applicability = "Time constant of the equivalent first-order RL circuit.",
    Sources = new[] { FormulaSources.Inductance })]
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
[QuantityOperator<ElectricPotential, ElectricCurrent, Power>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/electrical-power", Equation = "P = VI",
    Applicability = "Use corresponding instantaneous voltage and current. Use the passive sign convention for absorbed power.",
    Sources = new[] { FormulaSources.Current, FormulaSources.Ac })]
[QuantityOperator<ElectricCurrent, ElectricPotential, Power>(Operator.MULTIPLY)]
[QuantityOperator<ElectricPotential, ElectricCharge, Energy>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/potential-energy-change", Equation = "ΔU = QΔV",
    Applicability = "Change in electrical potential energy for charge moved through a fixed potential difference.",
    Sources = new[] { FormulaSources.Potential })]
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

[QuantityOperator<Capacitance, ElectricPotential, ElectricCharge>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/capacitor-charge", Equation = "Q = CV",
    Applicability = "Linear capacitor with the voltage across its terminals.",
    Sources = new[] { FormulaSources.Capacitance })]
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
[QuantityOperator<ElectricCharge, Length, ElectricDipoleMoment>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/dipole-moment", Equation = "p = Qd",
    Applicability = "Equal and opposite charges. Separation is directed from negative charge to positive charge.",
    Sources = new[] { FormulaSources.Fields })]
[QuantityOperator<Length, ElectricCharge, ElectricDipoleMoment>(Operator.MULTIPLY)]
public readonly partial struct ElectricCharge;

// The field component times path length gives the voltage drop, V_start - V_end.
[QuantityOperator<ElectricFieldStrength, Length, ElectricPotential>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/electric-field-and-voltage", Equation = "Vdrop = E∥ℓ, ΔV = −E∥ℓ",
    Applicability = "Use the path-averaged field component and final-minus-initial voltage for ΔV.",
    Sources = new[] { FormulaSources.Potential })]
[QuantityOperator<Length, ElectricFieldStrength, ElectricPotential>(Operator.MULTIPLY)]
[QuantityOperator<ElectricFieldStrength, ElectricCharge, Force>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/electric-force", Equation = "F = QE",
    Applicability = "Use the electric field component at the charge.",
    Sources = new[] { FormulaSources.Fields })]
[QuantityOperator<ElectricCharge, ElectricFieldStrength, Force>(Operator.MULTIPLY)]
[QuantityOperator<ElectricFieldStrength, Area, ElectricFlux>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/electric-flux", Equation = "Φₑ = EₙA",
    Applicability = "Uniform or area-averaged normal electric field on the surface.",
    Sources = new[] { FormulaSources.Fields, FormulaSources.Constitutive })]
[QuantityOperator<Area, ElectricFieldStrength, ElectricFlux>(Operator.MULTIPLY)]
public readonly partial struct ElectricFieldStrength;

[QuantityOperator<Force, ElectricFieldStrength, ElectricCharge>(Operator.DIVIDE)]
[QuantityOperator<Force, ElectricCharge, ElectricFieldStrength>(Operator.DIVIDE)]
public readonly partial struct Force;

[QuantityOperator<ElectricFlux, ElectricFieldStrength, Area>(Operator.DIVIDE)]
[QuantityOperator<ElectricFlux, Area, ElectricFieldStrength>(Operator.DIVIDE)]
public readonly partial struct ElectricFlux;

[QuantityOperator<ElectricConductivity, ElectricFieldStrength, ElectricCurrentDensity>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/ohmic-current-density", Equation = "J = σE",
    Applicability = "Scalar conductivity in the local ohmic regime. Use corresponding components.",
    Sources = new[] { FormulaSources.Resistance })]
[QuantityOperator<ElectricFieldStrength, ElectricConductivity, ElectricCurrentDensity>(Operator.MULTIPLY)]
public readonly partial struct ElectricConductivity;

[QuantityOperator<ElectricCurrentDensity, ElectricConductivity, ElectricFieldStrength>(Operator.DIVIDE)]
[QuantityOperator<ElectricCurrentDensity, ElectricFieldStrength, ElectricConductivity>(Operator.DIVIDE)]
[QuantityOperator<ElectricCurrentDensity, Area, ElectricCurrent>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/current-through-surface", Equation = "I = JₙA",
    Applicability = "Uniform or area-averaged normal current density through the section.",
    Sources = new[] { FormulaSources.Current })]
[QuantityOperator<Area, ElectricCurrentDensity, ElectricCurrent>(Operator.MULTIPLY)]
public readonly partial struct ElectricCurrentDensity;

[QuantityOperator<ElectricCurrent, ElectricCurrentDensity, Area>(Operator.DIVIDE)]
[QuantityOperator<ElectricCurrent, Area, ElectricCurrentDensity>(Operator.DIVIDE)]
public readonly partial struct ElectricCurrent;

[QuantityOperator<ElectricChargeLineDensity, Length, ElectricCharge>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/line-charge-density", Equation = "Q = λqℓ",
    Applicability = "Uniform or mean line charge density λq over the length.",
    Sources = new[] { FormulaSources.Fields, FormulaSources.QudtKinds })]
[QuantityOperator<Length, ElectricChargeLineDensity, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricChargeLineDensity;

[QuantityOperator<ElectricChargePerArea, Area, ElectricCharge>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/surface-charge-density", Equation = "Q = σqA",
    Applicability = "Uniform or mean surface charge density σq over the area.",
    Sources = new[] { FormulaSources.Fields, FormulaSources.QudtKinds })]
[QuantityOperator<Area, ElectricChargePerArea, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricChargePerArea;

[QuantityOperator<ElectricChargeVolumeDensity, Volume, ElectricCharge>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/volume-charge-density", Equation = "Q = ρqV",
    Applicability = "Uniform or mean volume charge density ρq over the region.",
    Sources = new[] { FormulaSources.Constitutive })]
[QuantityOperator<Volume, ElectricChargeVolumeDensity, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricChargeVolumeDensity;

[QuantityOperator<ElectricChargePerMass, Mass, ElectricCharge>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/charge-per-mass", Equation = "Q = (Q / m)m",
    Applicability = "Charge and mass refer to the same particles or sample.",
    Sources = new[] { FormulaSources.QudtKinds })]
[QuantityOperator<Mass, ElectricChargePerMass, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricChargePerMass;

[QuantityOperator<ElectricChargePerAmountOfSubstance, AmountOfSubstance, ElectricCharge>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/charge-per-mole", Equation = "Q = (Q / n)n",
    Applicability = "Use charge per mole for the specified entities and process.",
    Sources = new[] { FormulaSources.QudtKinds, FormulaSources.Molar })]
[QuantityOperator<AmountOfSubstance, ElectricChargePerAmountOfSubstance, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricChargePerAmountOfSubstance;

[QuantityOperator<ElectricDipoleMoment, ElectricCharge, Length>(Operator.DIVIDE)]
[QuantityOperator<ElectricDipoleMoment, Length, ElectricCharge>(Operator.DIVIDE)]
public readonly partial struct ElectricDipoleMoment;

[QuantityOperator<Permittivity, ElectricFieldStrength, ElectricDisplacement>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/electric-displacement", Equation = "D = εE",
    Applicability = "Linear isotropic dielectric, using corresponding field components.",
    Sources = new[] { FormulaSources.Constitutive })]
[QuantityOperator<ElectricFieldStrength, Permittivity, ElectricDisplacement>(Operator.MULTIPLY)]
public readonly partial struct Permittivity;

[QuantityOperator<ElectricDisplacement, Permittivity, ElectricFieldStrength>(Operator.DIVIDE)]
[QuantityOperator<ElectricDisplacement, ElectricFieldStrength, Permittivity>(Operator.DIVIDE)]
public readonly partial struct ElectricDisplacement;

// Use the area-averaged field component normal to the surface.
[QuantityOperator<MagneticFluxDensity, Area, MagneticFlux>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/magnetic-flux", Equation = "Φ = BₙA",
    Applicability = "Use the uniform or area-averaged normal flux density.",
    Sources = new[] { FormulaSources.Induction })]
[QuantityOperator<Area, MagneticFluxDensity, MagneticFlux>(Operator.MULTIPLY)]
[QuantityOperator<MagneticFluxDensity, Permeability, AuxillaryMagneticField>(Operator.DIVIDE)]
[QuantityOperator<MagneticFluxDensity, AuxillaryMagneticField, Permeability>(Operator.DIVIDE)]
public readonly partial struct MagneticFluxDensity;

[QuantityOperator<MagneticFlux, MagneticFluxDensity, Area>(Operator.DIVIDE)]
[QuantityOperator<MagneticFlux, Area, MagneticFluxDensity>(Operator.DIVIDE)]
[QuantityOperator<MagneticFlux, MagneticFluxPerLength, Length>(Operator.DIVIDE)]
[QuantityOperator<MagneticFlux, Length, MagneticFluxPerLength>(Operator.DIVIDE)]
public readonly partial struct MagneticFlux;

[QuantityOperator<MagneticFluxPerLength, Length, MagneticFlux>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/magnetic-flux-per-length", Equation = "Φ = (Φ / ℓ)ℓ",
    Applicability = "Uniform or mean magnetic flux per length over the interval.",
    Sources = new[] { FormulaSources.QudtKinds })]
[QuantityOperator<Length, MagneticFluxPerLength, MagneticFlux>(Operator.MULTIPLY)]
public readonly partial struct MagneticFluxPerLength;

[QuantityOperator<Permeability, AuxillaryMagneticField, MagneticFluxDensity>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/magnetic-field-in-material", Equation = "B = μH",
    Applicability = "Linear isotropic magnetic medium. The QUDT `AuxillaryMagneticField` type represents H.",
    Sources = new[] { FormulaSources.Constitutive })]
[QuantityOperator<AuxillaryMagneticField, Permeability, MagneticFluxDensity>(Operator.MULTIPLY)]
public readonly partial struct Permeability;

// Heat and temperature.

// Use a constant heat capacity or its average over the temperature interval.
[QuantityOperator<HeatCapacity, TemperatureDifference, Energy>(Operator.MULTIPLY,
    FormulaId = "thermal/sensible-heat", Equation = "Q = CΔT",
    Applicability = "Sensible heat with constant or interval-averaged heat capacity.",
    Sources = new[] { FormulaSources.Thermal })]
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

[QuantityOperator<SpecificHeatCapacity, Mass, HeatCapacity>(Operator.MULTIPLY,
    FormulaId = "thermal/specific-heat-capacity", Equation = "C = mc",
    Applicability = "Use mass-specific heat capacity for the specified thermodynamic process.",
    Sources = new[] { FormulaSources.Thermal })]
[QuantityOperator<Mass, SpecificHeatCapacity, HeatCapacity>(Operator.MULTIPLY)]
[QuantityOperator<SpecificHeatCapacity, Density, VolumetricHeatCapacity>(Operator.MULTIPLY,
    FormulaId = "thermal/volumetric-capacity-from-density", Equation = "cᵥ = ρc",
    Applicability = "Density and mass-specific heat capacity describe the same state.",
    Sources = new[] { FormulaSources.Diffusivity })]
[QuantityOperator<Density, SpecificHeatCapacity, VolumetricHeatCapacity>(Operator.MULTIPLY)]
public readonly partial struct SpecificHeatCapacity;

[QuantityOperator<VolumetricHeatCapacity, Volume, HeatCapacity>(Operator.MULTIPLY,
    FormulaId = "thermal/volumetric-heat-capacity", Equation = "C = Vcᵥ",
    Applicability = "Uniform or volume-averaged volumetric heat capacity cᵥ.",
    Sources = new[] { FormulaSources.NistSi })]
[QuantityOperator<Volume, VolumetricHeatCapacity, HeatCapacity>(Operator.MULTIPLY)]
[QuantityOperator<VolumetricHeatCapacity, SpecificHeatCapacity, Density>(Operator.DIVIDE)]
[QuantityOperator<VolumetricHeatCapacity, Density, SpecificHeatCapacity>(Operator.DIVIDE)]
public readonly partial struct VolumetricHeatCapacity;

[QuantityOperator<ThermalDiffusivity, VolumetricHeatCapacity, ThermalConductivity>(Operator.MULTIPLY,
    FormulaId = "thermal/thermal-diffusivity", Equation = "α = k / cᵥ",
    Applicability = "Thermal diffusivity for scalar conductivity and volumetric heat capacity.",
    Sources = new[] { FormulaSources.Diffusivity })]
[QuantityOperator<VolumetricHeatCapacity, ThermalDiffusivity, ThermalConductivity>(Operator.MULTIPLY)]
public readonly partial struct ThermalDiffusivity;

[QuantityOperator<ThermalConductivity, ThermalDiffusivity, VolumetricHeatCapacity>(Operator.DIVIDE)]
[QuantityOperator<ThermalConductivity, VolumetricHeatCapacity, ThermalDiffusivity>(Operator.DIVIDE)]
public readonly partial struct ThermalConductivity;

[QuantityOperator<HeatFlowRate, Time, Energy>(Operator.MULTIPLY, FormulaId = "mechanics/energy-from-power")]
[QuantityOperator<Time, HeatFlowRate, Energy>(Operator.MULTIPLY)]
[QuantityOperator<HeatFlowRate, HeatFlowRatePerArea, Area>(Operator.DIVIDE)]
[QuantityOperator<HeatFlowRate, Area, HeatFlowRatePerArea>(Operator.DIVIDE)]
public readonly partial struct HeatFlowRate;

[QuantityOperator<HeatFlowRatePerArea, Area, HeatFlowRate>(Operator.MULTIPLY,
    FormulaId = "thermal/heat-flow-from-flux", Equation = "Pheat = q″A",
    Applicability = "Use average normal heat flux over the surface.",
    Sources = new[] { FormulaSources.Thermal, FormulaSources.HeatCoefficient })]
[QuantityOperator<Area, HeatFlowRatePerArea, HeatFlowRate>(Operator.MULTIPLY)]
[QuantityOperator<HeatFlowRatePerArea, CoefficientOfHeatTransfer, TemperatureDifference>(Operator.DIVIDE)]
[QuantityOperator<HeatFlowRatePerArea, TemperatureDifference, CoefficientOfHeatTransfer>(Operator.DIVIDE)]
public readonly partial struct HeatFlowRatePerArea;

[QuantityOperator<CoefficientOfHeatTransfer, TemperatureDifference, HeatFlowRatePerArea>(Operator.MULTIPLY,
    FormulaId = "thermal/surface-heat-transfer", Equation = "q″ = hΔT",
    Applicability = "Transfer coefficient h is per unit area. Use the driving temperature difference in the indicated direction.",
    Sources = new[] { FormulaSources.HeatCoefficient })]
[QuantityOperator<TemperatureDifference, CoefficientOfHeatTransfer, HeatFlowRatePerArea>(Operator.MULTIPLY)]
public readonly partial struct CoefficientOfHeatTransfer;

[QuantityOperator<ThermalResistance, HeatFlowRate, TemperatureDifference>(Operator.MULTIPLY,
    FormulaId = "thermal/thermal-path", Equation = "Pheat = ΔT / Rₜ = GₜΔT, Gₜ = 1 / Rₜ",
    Applicability = "Use total resistance or conductance for the same thermal path. A signed temperature drop gives the signed heat-flow rate.",
    Sources = new[] { FormulaSources.Thermal, FormulaSources.ThermalConductance })]
[QuantityOperator<HeatFlowRate, ThermalResistance, TemperatureDifference>(Operator.MULTIPLY)]
[QuantityOperator<ThermalResistance, Area, ThermalInsulance>(Operator.MULTIPLY,
    FormulaId = "thermal/areal-thermal-resistance", Equation = "R″ = ARₜ",
    Applicability = "Relate total thermal resistance to area-normalized resistance for the same uniform surface.",
    Sources = new[] { FormulaSources.Thermal, FormulaSources.HeatCoefficient })]
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
[QuantityOperator<ThermalInsulance, HeatFlowRatePerArea, TemperatureDifference>(Operator.MULTIPLY,
    FormulaId = "thermal/temperature-drop-from-heat-flux", Equation = "ΔT = q″R″",
    Applicability = "Use areal thermal resistance and the corresponding heat-flux component.",
    Sources = new[] { FormulaSources.Thermal, FormulaSources.HeatCoefficient })]
[QuantityOperator<HeatFlowRatePerArea, ThermalInsulance, TemperatureDifference>(Operator.MULTIPLY)]
public readonly partial struct ThermalInsulance;

[QuantityOperator<TemperaturePerTime, Time, TemperatureDifference>(Operator.MULTIPLY,
    FormulaId = "thermal/temperature-change-rate", Equation = "ΔT = (dT/dt)̄Δt",
    Applicability = "Temperature-change rate is constant or averaged over the interval.",
    Sources = new[] { FormulaSources.QudtKinds })]
[QuantityOperator<Time, TemperaturePerTime, TemperatureDifference>(Operator.MULTIPLY)]
public readonly partial struct TemperaturePerTime;

[QuantityOperator<LinearThermalExpansion, TemperatureDifference, Length>(Operator.MULTIPLY,
    FormulaId = "thermal/thermal-expansion", Equation = "ΔL = L′ΔT, ΔA = A′ΔT, ΔV = V′ΔT",
    Applicability = "Use each whole object's dimensional expansion per kelvin, incorporating its initial size. The return is a change in size.",
    Sources = new[] { FormulaSources.Thermal, FormulaSources.QudtKinds })]
[QuantityOperator<TemperatureDifference, LinearThermalExpansion, Length>(Operator.MULTIPLY)]
public readonly partial struct LinearThermalExpansion;

[QuantityOperator<Length, LinearThermalExpansion, TemperatureDifference>(Operator.DIVIDE)]
public readonly partial struct Length;

[QuantityOperator<AreaThermalExpansion, TemperatureDifference, Area>(Operator.MULTIPLY, FormulaId = "thermal/thermal-expansion")]
[QuantityOperator<TemperatureDifference, AreaThermalExpansion, Area>(Operator.MULTIPLY)]
public readonly partial struct AreaThermalExpansion;

[QuantityOperator<Area, AreaThermalExpansion, TemperatureDifference>(Operator.DIVIDE)]
public readonly partial struct Area;

[QuantityOperator<VolumeThermalExpansion, TemperatureDifference, Volume>(Operator.MULTIPLY, FormulaId = "thermal/thermal-expansion")]
[QuantityOperator<TemperatureDifference, VolumeThermalExpansion, Volume>(Operator.MULTIPLY)]
public readonly partial struct VolumeThermalExpansion;

[QuantityOperator<Volume, VolumeThermalExpansion, TemperatureDifference>(Operator.DIVIDE)]
public readonly partial struct Volume;

// Photometry and radiometry.

// Use the average intensity over the solid angle. For luminance, use the projected emitting area.
[QuantityOperator<LuminousIntensity, SolidAngle, LuminousFlux>(Operator.MULTIPLY,
    FormulaId = "optics/luminous-flux", Equation = "Φᵥ = IᵥΩ",
    Applicability = "Use mean luminous intensity over the solid angle.",
    Sources = new[] { FormulaSources.Photometry, FormulaSources.PointSource })]
[QuantityOperator<SolidAngle, LuminousIntensity, LuminousFlux>(Operator.MULTIPLY)]
[QuantityOperator<LuminousIntensity, Luminance, Area>(Operator.DIVIDE)]
[QuantityOperator<LuminousIntensity, Area, Luminance>(Operator.DIVIDE)]
public readonly partial struct LuminousIntensity;

[QuantityOperator<LuminousFlux, LuminousIntensity, SolidAngle>(Operator.DIVIDE)]
[QuantityOperator<LuminousFlux, SolidAngle, LuminousIntensity>(Operator.DIVIDE)]
[QuantityOperator<LuminousFlux, Illuminance, Area>(Operator.DIVIDE)]
// Illuminance is incident luminous flux per illuminated area.
[QuantityOperator<LuminousFlux, Area, Illuminance>(Operator.DIVIDE)]
[QuantityOperator<LuminousFlux, Time, LuminousEnergy>(Operator.MULTIPLY,
    FormulaId = "optics/luminous-energy", Equation = "Qᵥ = Φ̄ᵥΔt",
    Applicability = "Constant or time-averaged luminous flux over the interval.",
    Sources = new[] { FormulaSources.Photometry })]
[QuantityOperator<Time, LuminousFlux, LuminousEnergy>(Operator.MULTIPLY)]
[QuantityOperator<LuminousFlux, LuminousEfficacy, Power>(Operator.DIVIDE)]
[QuantityOperator<LuminousFlux, Power, LuminousEfficacy>(Operator.DIVIDE)]
public readonly partial struct LuminousFlux;

[QuantityOperator<Illuminance, Area, LuminousFlux>(Operator.MULTIPLY,
    FormulaId = "optics/illuminance", Equation = "Eᵥ = Φᵥ,incident / A",
    Applicability = "Incident luminous flux averaged over the receiver area.",
    Sources = new[] { FormulaSources.Photometry, FormulaSources.PointSource })]
[QuantityOperator<Area, Illuminance, LuminousFlux>(Operator.MULTIPLY)]
public readonly partial struct Illuminance;

[QuantityOperator<Luminance, Area, LuminousIntensity>(Operator.MULTIPLY,
    FormulaId = "optics/luminance", Equation = "Iᵥ = LᵥAproj",
    Applicability = "Use projected emitting area as viewed from the specified direction.",
    Sources = new[] { FormulaSources.Photometry })]
[QuantityOperator<Area, Luminance, LuminousIntensity>(Operator.MULTIPLY)]
public readonly partial struct Luminance;

[QuantityOperator<LuminousEnergy, LuminousFlux, Time>(Operator.DIVIDE)]
[QuantityOperator<LuminousEnergy, Time, LuminousFlux>(Operator.DIVIDE)]
public readonly partial struct LuminousEnergy;

[QuantityOperator<LuminousEfficacy, Power, LuminousFlux>(Operator.MULTIPLY,
    FormulaId = "optics/luminous-efficacy", Equation = "Φᵥ = ηᵥP",
    Applicability = "Use the power basis for which the luminous efficacy was specified.",
    Sources = new[] { FormulaSources.Photometry, FormulaSources.QudtKinds })]
[QuantityOperator<Power, LuminousEfficacy, LuminousFlux>(Operator.MULTIPLY)]
public readonly partial struct LuminousEfficacy;

[QuantityOperator<RadiantIntensity, SolidAngle, Power>(Operator.MULTIPLY,
    FormulaId = "optics/radiant-flux", Equation = "Φₑ = IₑΩ",
    Applicability = "Use mean radiant intensity over the solid angle.",
    Sources = new[] { FormulaSources.Photometry })]
[QuantityOperator<SolidAngle, RadiantIntensity, Power>(Operator.MULTIPLY)]
public readonly partial struct RadiantIntensity;

[QuantityOperator<Power, RadiantIntensity, SolidAngle>(Operator.DIVIDE)]
[QuantityOperator<Power, SolidAngle, RadiantIntensity>(Operator.DIVIDE)]
public readonly partial struct Power;

// Amount of substance and radiation dose.

[QuantityOperator<MolarMass, AmountOfSubstance, Mass>(Operator.MULTIPLY,
    FormulaId = "chemistry/molar-mass", Equation = "M = m / n",
    Applicability = "Mass and amount describe the same substance or specified entities.",
    Sources = new[] { FormulaSources.MolarMass })]
[QuantityOperator<AmountOfSubstance, MolarMass, Mass>(Operator.MULTIPLY)]
[QuantityOperator<MolarMass, Density, MolarVolume>(Operator.DIVIDE)]
[QuantityOperator<MolarMass, MolarVolume, Density>(Operator.DIVIDE)]
public readonly partial struct MolarMass;

[QuantityOperator<Mass, MolarMass, AmountOfSubstance>(Operator.DIVIDE)]
[QuantityOperator<Mass, AmountOfSubstance, MolarMass>(Operator.DIVIDE)]
public readonly partial struct Mass;

[QuantityOperator<MolarVolume, AmountOfSubstance, Volume>(Operator.MULTIPLY,
    FormulaId = "chemistry/molar-volume", Equation = "Vₘ = V / n",
    Applicability = "Volume and amount refer to the same sample at the same state.",
    Sources = new[] { FormulaSources.Molar })]
[QuantityOperator<AmountOfSubstance, MolarVolume, Volume>(Operator.MULTIPLY)]
[QuantityOperator<MolarVolume, Density, MolarMass>(Operator.MULTIPLY,
    FormulaId = "chemistry/molar-mass-from-density", Equation = "M = ρVₘ",
    Applicability = "Density and molar volume describe the same material at the same state.",
    Sources = new[] { FormulaSources.Molar, FormulaSources.MolarVolume })]
[QuantityOperator<Density, MolarVolume, MolarMass>(Operator.MULTIPLY)]
public readonly partial struct MolarVolume;

[QuantityOperator<Volume, MolarVolume, AmountOfSubstance>(Operator.DIVIDE)]
[QuantityOperator<Volume, AmountOfSubstance, MolarVolume>(Operator.DIVIDE)]
public readonly partial struct Volume;

[QuantityOperator<MolarEnergy, AmountOfSubstance, Energy>(Operator.MULTIPLY,
    FormulaId = "chemistry/molar-energy", Equation = "Eₘ = E / n",
    Applicability = "Use energy for the same specified amount of substance.",
    Sources = new[] { FormulaSources.Molar })]
[QuantityOperator<AmountOfSubstance, MolarEnergy, Energy>(Operator.MULTIPLY)]
public readonly partial struct MolarEnergy;

[QuantityOperator<Energy, MolarEnergy, AmountOfSubstance>(Operator.DIVIDE)]
[QuantityOperator<Energy, AmountOfSubstance, MolarEnergy>(Operator.DIVIDE)]
[QuantityOperator<Energy, AbsorbedDose, Mass>(Operator.DIVIDE)]
public readonly partial struct Energy;

[QuantityOperator<MolarHeatCapacity, AmountOfSubstance, HeatCapacity>(Operator.MULTIPLY,
    FormulaId = "chemistry/molar-heat-capacity", Equation = "Cₘ = C / n",
    Applicability = "Use heat capacity for the same amount and thermodynamic process.",
    Sources = new[] { FormulaSources.Molar, FormulaSources.Thermal })]
[QuantityOperator<AmountOfSubstance, MolarHeatCapacity, HeatCapacity>(Operator.MULTIPLY)]
public readonly partial struct MolarHeatCapacity;

[QuantityOperator<HeatCapacity, MolarHeatCapacity, AmountOfSubstance>(Operator.DIVIDE)]
[QuantityOperator<HeatCapacity, AmountOfSubstance, MolarHeatCapacity>(Operator.DIVIDE)]
public readonly partial struct HeatCapacity;

[QuantityOperator<AmountOfSubstancePerVolume, Volume, AmountOfSubstance>(Operator.MULTIPLY,
    FormulaId = "chemistry/amount-concentration", Equation = "c = n / V",
    Applicability = "Amount of the constituent divided by the volume of the mixture.",
    Sources = new[] { FormulaSources.Concentration })]
[QuantityOperator<Volume, AmountOfSubstancePerVolume, AmountOfSubstance>(Operator.MULTIPLY)]
public readonly partial struct AmountOfSubstancePerVolume;

[QuantityOperator<AmountOfSubstance, AmountOfSubstancePerVolume, Volume>(Operator.DIVIDE)]
[QuantityOperator<AmountOfSubstance, Volume, AmountOfSubstancePerVolume>(Operator.DIVIDE)]
[QuantityOperator<AmountOfSubstance, AmountOfSubstancePerMass, Mass>(Operator.DIVIDE)]
[QuantityOperator<AmountOfSubstance, Mass, AmountOfSubstancePerMass>(Operator.DIVIDE)]
[QuantityOperator<AmountOfSubstance, CatalyticActivity, Time>(Operator.DIVIDE)]
public readonly partial struct AmountOfSubstance;

[QuantityOperator<AmountOfSubstancePerMass, Mass, AmountOfSubstance>(Operator.MULTIPLY,
    FormulaId = "chemistry/amount-per-mass", Equation = "b = n / m",
    Applicability = "Use the reference mass for the specified amount-per-mass quantity, such as solvent mass when calculating molality.",
    Sources = new[] { FormulaSources.QudtKinds, FormulaSources.Molar })]
[QuantityOperator<Mass, AmountOfSubstancePerMass, AmountOfSubstance>(Operator.MULTIPLY)]
public readonly partial struct AmountOfSubstancePerMass;

[QuantityOperator<AbsorbedDoseRate, Time, AbsorbedDose>(Operator.MULTIPLY,
    FormulaId = "chemistry/absorbed-dose-rate", Equation = "ΔD = Ḋ̄Δt",
    Applicability = "Constant or interval-averaged absorbed dose rate for the same matter.",
    Sources = new[] { FormulaSources.Dose })]
[QuantityOperator<Time, AbsorbedDoseRate, AbsorbedDose>(Operator.MULTIPLY)]
public readonly partial struct AbsorbedDoseRate;

[QuantityOperator<AbsorbedDose, AbsorbedDoseRate, Time>(Operator.DIVIDE)]
[QuantityOperator<AbsorbedDose, Time, AbsorbedDoseRate>(Operator.DIVIDE)]
// The dose is the mean over the same mass to which the deposited energy refers.
[QuantityOperator<AbsorbedDose, Mass, Energy>(Operator.MULTIPLY,
    FormulaId = "chemistry/absorbed-dose", Equation = "D = Edeposited / m",
    Applicability = "Mean energy deposited by ionizing radiation in the specified matter.",
    Sources = new[] { FormulaSources.Dose })]
[QuantityOperator<Mass, AbsorbedDose, Energy>(Operator.MULTIPLY)]
public readonly partial struct AbsorbedDose;

[QuantityOperator<CatalyticActivity, Time, AmountOfSubstance>(Operator.MULTIPLY,
    FormulaId = "chemistry/catalytic-activity", Equation = "Δn = ācΔt",
    Applicability = "Use the constant or time-averaged catalytic activity under the specified assay conditions.",
    Sources = new[] { FormulaSources.BipmSi, FormulaSources.QudtKinds })]
[QuantityOperator<Time, CatalyticActivity, AmountOfSubstance>(Operator.MULTIPLY)]
public readonly partial struct CatalyticActivity;

// Pressure, power, and reciprocal density.

// Supply the force component along the direction represented by the velocity.
[QuantityOperator<Force, LinearVelocity, Power>(Operator.MULTIPLY,
    FormulaId = "mechanics/mechanical-power", Equation = "P = F∥v",
    Applicability = "Use the signed force component along the velocity axis.",
    Sources = new[] { FormulaSources.Work })]
[QuantityOperator<LinearVelocity, Force, Power>(Operator.MULTIPLY)]
[QuantityOperator<Force, Pressure, Area>(Operator.DIVIDE)]
public readonly partial struct Force;

[QuantityOperator<Power, Force, LinearVelocity>(Operator.DIVIDE)]
[QuantityOperator<Power, LinearVelocity, Force>(Operator.DIVIDE)]
[QuantityOperator<Power, Pressure, VolumePerTime>(Operator.DIVIDE)]
public readonly partial struct Power;

[QuantityOperator<Pressure, Area, Force>(Operator.MULTIPLY, FormulaId = "fluids/pressure-from-force")]
[QuantityOperator<Area, Pressure, Force>(Operator.MULTIPLY)]
// For hydraulic power, the pressure operand is the signed difference across the flow path.
[QuantityOperator<Pressure, VolumePerTime, Power>(Operator.MULTIPLY, FormulaId = "fluids/hydraulic-power")]
[QuantityOperator<VolumePerTime, Pressure, Power>(Operator.MULTIPLY)]
public readonly partial struct Pressure;

[QuantityOperator<double, Density, SpecificVolume>(Operator.DIVIDE)]
[QuantityOperator<Density, SpecificVolume, DimensionlessRatio>(Operator.MULTIPLY, FormulaId = "fluids/density-and-specific-volume")]
[QuantityOperator<SpecificVolume, Density, DimensionlessRatio>(Operator.MULTIPLY)]
public readonly partial struct Density;

[QuantityOperator<double, SpecificVolume, Density>(Operator.DIVIDE)]
public readonly partial struct SpecificVolume;

// Conductance, resistivity, and the mass-to-charge ratio.

[QuantityOperator<Conductance, ElectricPotential, ElectricCurrent>(Operator.MULTIPLY, FormulaId = "electromagnetism/ohms-law")]
[QuantityOperator<ElectricPotential, Conductance, ElectricCurrent>(Operator.MULTIPLY)]
[QuantityOperator<double, Conductance, Resistance>(Operator.DIVIDE)]
public readonly partial struct Conductance;

[QuantityOperator<ElectricCurrent, Conductance, ElectricPotential>(Operator.DIVIDE)]
[QuantityOperator<ElectricCurrent, ElectricPotential, Conductance>(Operator.DIVIDE)]
public readonly partial struct ElectricCurrent;

[QuantityOperator<double, Resistance, Conductance>(Operator.DIVIDE)]
[QuantityOperator<Resistance, Conductance, DimensionlessRatio>(Operator.MULTIPLY, FormulaId = "electromagnetism/ohms-law")]
[QuantityOperator<Conductance, Resistance, DimensionlessRatio>(Operator.MULTIPLY)]
public readonly partial struct Resistance;

[QuantityOperator<double, ElectricConductivity, Resistivity>(Operator.DIVIDE)]
[QuantityOperator<ElectricConductivity, Resistivity, DimensionlessRatio>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/conductivity-and-resistivity", Equation = "ρₑ = 1 / σ",
    Applicability = "Resistivity and conductivity refer to the same ohmic material and state.",
    Sources = new[] { FormulaSources.Resistance })]
[QuantityOperator<Resistivity, ElectricConductivity, DimensionlessRatio>(Operator.MULTIPLY)]
public readonly partial struct ElectricConductivity;

[QuantityOperator<double, Resistivity, ElectricConductivity>(Operator.DIVIDE)]
public readonly partial struct Resistivity;

[QuantityOperator<MassPerElectricCharge, ElectricCharge, Mass>(Operator.MULTIPLY,
    FormulaId = "electromagnetism/mass-per-charge", Equation = "m = (m / Q)Q",
    Applicability = "Mass and signed charge refer to the same charged entities.",
    Sources = new[] { FormulaSources.QudtKinds })]
[QuantityOperator<ElectricCharge, MassPerElectricCharge, Mass>(Operator.MULTIPLY)]
public readonly partial struct MassPerElectricCharge;

[QuantityOperator<Mass, MassPerElectricCharge, ElectricCharge>(Operator.DIVIDE)]
[QuantityOperator<Mass, ElectricCharge, MassPerElectricCharge>(Operator.DIVIDE)]
public readonly partial struct Mass;

// Temperature gradients and thermal material properties.

[QuantityOperator<TemperatureGradient, Length, TemperatureDifference>(Operator.MULTIPLY,
    FormulaId = "thermal/temperature-gradient", Equation = "∂T/∂x ≈ ΔT / Δx",
    Applicability = "Signed temperature difference over a distance along the axis. The quotient gives the interval average.",
    Sources = new[] { FormulaSources.Thermal })]
[QuantityOperator<Length, TemperatureGradient, TemperatureDifference>(Operator.MULTIPLY)]
public readonly partial struct TemperatureGradient;

[QuantityOperator<TemperatureDifference, TemperatureGradient, Length>(Operator.DIVIDE)]
[QuantityOperator<TemperatureDifference, Length, TemperatureGradient>(Operator.DIVIDE)]
public readonly partial struct TemperatureDifference;

[QuantityOperator<double, ThermalConductivity, ThermalResistivity>(Operator.DIVIDE)]
[QuantityOperator<ThermalConductivity, ThermalResistivity, DimensionlessRatio>(Operator.MULTIPLY,
    FormulaId = "thermal/thermal-resistivity", Equation = "ρₜ = 1 / k",
    Applicability = "Thermal resistivity and conductivity describe the same material and direction.",
    Sources = new[] { FormulaSources.ThermalResistivity })]
[QuantityOperator<ThermalResistivity, ThermalConductivity, DimensionlessRatio>(Operator.MULTIPLY)]
public readonly partial struct ThermalConductivity;

[QuantityOperator<double, ThermalResistivity, ThermalConductivity>(Operator.DIVIDE)]
// A uniform plane layer's depth times thermal resistivity gives its areal resistance.
[QuantityOperator<ThermalResistivity, Length, ThermalInsulance>(Operator.MULTIPLY,
    FormulaId = "thermal/plane-layer-insulance", Equation = "R″ = ℓ/k = ℓρₜ",
    Applicability = "Uniform plane layer. Use thickness along the heat-flow direction.",
    Sources = new[] { FormulaSources.Thermal, FormulaSources.ThermalResistivity })]
[QuantityOperator<Length, ThermalResistivity, ThermalInsulance>(Operator.MULTIPLY)]
public readonly partial struct ThermalResistivity;

[QuantityOperator<ThermalInsulance, ThermalConductivity, Length>(Operator.MULTIPLY, FormulaId = "thermal/plane-layer-insulance")]
[QuantityOperator<ThermalConductivity, ThermalInsulance, Length>(Operator.MULTIPLY)]
[QuantityOperator<ThermalInsulance, ThermalResistivity, Length>(Operator.DIVIDE)]
[QuantityOperator<ThermalInsulance, Length, ThermalResistivity>(Operator.DIVIDE)]
public readonly partial struct ThermalInsulance;

[QuantityOperator<Length, ThermalInsulance, ThermalConductivity>(Operator.DIVIDE)]
[QuantityOperator<Length, ThermalConductivity, ThermalInsulance>(Operator.DIVIDE)]
public readonly partial struct Length;

// Wave propagation uses wavelength and frequency for the same wave.
[QuantityOperator<Wavelength, Frequency, LinearVelocity>(Operator.MULTIPLY,
    FormulaId = "mechanics/wave-speed", Equation = "v = λf",
    Applicability = "Wavelength and frequency belong to the same propagating wave.",
    Sources = new[] { FormulaSources.Waves })]
[QuantityOperator<Frequency, Wavelength, LinearVelocity>(Operator.MULTIPLY)]
public readonly partial struct Wavelength;

[QuantityOperator<LinearVelocity, Wavelength, Frequency>(Operator.DIVIDE)]
[QuantityOperator<LinearVelocity, Frequency, Wavelength>(Operator.DIVIDE)]
public readonly partial struct LinearVelocity;

[QuantityOperator<Impulse, Time, Force>(Operator.DIVIDE, FormulaId = "mechanics/impulse-and-momentum-change")]
[QuantityOperator<Impulse, Force, Time>(Operator.DIVIDE)]
public readonly partial struct Impulse;

// Conductance is for the whole thermal path. The heat-transfer coefficient is per area.
[QuantityOperator<ThermalConductance, TemperatureDifference, HeatFlowRate>(Operator.MULTIPLY, FormulaId = "thermal/thermal-path")]
[QuantityOperator<TemperatureDifference, ThermalConductance, HeatFlowRate>(Operator.MULTIPLY)]
[QuantityOperator<double, ThermalConductance, ThermalResistance>(Operator.DIVIDE)]
public readonly partial struct ThermalConductance;

[QuantityOperator<HeatFlowRate, TemperatureDifference, ThermalConductance>(Operator.DIVIDE)]
[QuantityOperator<HeatFlowRate, ThermalConductance, TemperatureDifference>(Operator.DIVIDE)]
public readonly partial struct HeatFlowRate;

[QuantityOperator<CoefficientOfHeatTransfer, Area, ThermalConductance>(Operator.MULTIPLY,
    FormulaId = "thermal/heat-transfer-conductance", Equation = "Gₜ = hA",
    Applicability = "Uniform or area-averaged heat-transfer coefficient for the same thermal path and driving temperature difference.",
    Sources = new[] { FormulaSources.HeatCoefficient, FormulaSources.ThermalConductance })]
[QuantityOperator<Area, CoefficientOfHeatTransfer, ThermalConductance>(Operator.MULTIPLY)]
public readonly partial struct CoefficientOfHeatTransfer;

[QuantityOperator<ThermalConductance, Area, CoefficientOfHeatTransfer>(Operator.DIVIDE)]
[QuantityOperator<ThermalConductance, CoefficientOfHeatTransfer, Area>(Operator.DIVIDE)]
public readonly partial struct ThermalConductance;

[QuantityOperator<double, ThermalResistance, ThermalConductance>(Operator.DIVIDE)]
[QuantityOperator<ThermalResistance, ThermalConductance, DimensionlessRatio>(Operator.MULTIPLY, FormulaId = "thermal/thermal-path")]
[QuantityOperator<ThermalConductance, ThermalResistance, DimensionlessRatio>(Operator.MULTIPLY)]
public readonly partial struct ThermalResistance;
