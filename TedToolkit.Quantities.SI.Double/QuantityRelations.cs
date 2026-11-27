// -----------------------------------------------------------------------
// <copyright file="QuantityRelations.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Quantities;

[QuantityOperator<Length, Length, Area>(Operator.MULTIPLY)]
[QuantityOperator<Length, Length, double>(Operator.DIVIDE)]
[QuantityOperator<Length, Time, LinearVelocity>(Operator.DIVIDE)]
[QuantityOperator<Length, LinearVelocity, Time>(Operator.DIVIDE)]
[QuantityOperator<double, Length, InverseLength>(Operator.DIVIDE)]
[QuantityOperator<Length, InverseLength, double>(Operator.MULTIPLY)]
[QuantityOperator<InverseLength, Length, double>(Operator.MULTIPLY)]
public readonly partial struct Length;

[QuantityOperator<Area, Length, Volume>(Operator.MULTIPLY)]
[QuantityOperator<Length, Area, Volume>(Operator.MULTIPLY)]
[QuantityOperator<Area, Length, Length>(Operator.DIVIDE)]
[QuantityOperator<Area, Time, AreaPerTime>(Operator.DIVIDE)]
[QuantityOperator<Area, AreaPerTime, Time>(Operator.DIVIDE)]
public readonly partial struct Area;

[QuantityOperator<AreaPerTime, Time, Area>(Operator.MULTIPLY)]
[QuantityOperator<Time, AreaPerTime, Area>(Operator.MULTIPLY)]
public readonly partial struct AreaPerTime;

[QuantityOperator<Volume, Area, Length>(Operator.DIVIDE)]
[QuantityOperator<Volume, Length, Area>(Operator.DIVIDE)]
[QuantityOperator<Volume, Time, VolumePerTime>(Operator.DIVIDE)]
public readonly partial struct Volume;

[QuantityOperator<LinearVelocity, Time, Length>(Operator.MULTIPLY)]
[QuantityOperator<Time, LinearVelocity, Length>(Operator.MULTIPLY)]
[QuantityOperator<LinearVelocity, Time, LinearAcceleration>(Operator.DIVIDE)]
[QuantityOperator<LinearVelocity, LinearAcceleration, Time>(Operator.DIVIDE)]
[QuantityOperator<LinearVelocity, Length, AngularVelocity>(Operator.DIVIDE)]
[QuantityOperator<LinearVelocity, AngularVelocity, Length>(Operator.DIVIDE)]
public readonly partial struct LinearVelocity;

[QuantityOperator<LinearAcceleration, Time, LinearVelocity>(Operator.MULTIPLY)]
[QuantityOperator<Time, LinearAcceleration, LinearVelocity>(Operator.MULTIPLY)]
[QuantityOperator<LinearAcceleration, Length, AngularAcceleration>(Operator.DIVIDE)]
[QuantityOperator<LinearAcceleration, AngularAcceleration, Length>(Operator.DIVIDE)]
public readonly partial struct LinearAcceleration;

[QuantityOperator<Mass, LinearAcceleration, Force>(Operator.MULTIPLY)]
[QuantityOperator<LinearAcceleration, Mass, Force>(Operator.MULTIPLY)]
[QuantityOperator<Mass, LinearVelocity, LinearMomentum>(Operator.MULTIPLY)]
[QuantityOperator<LinearVelocity, Mass, LinearMomentum>(Operator.MULTIPLY)]
[QuantityOperator<Mass, Volume, Density>(Operator.DIVIDE)]
public readonly partial struct Mass;

[QuantityOperator<Force, Mass, LinearAcceleration>(Operator.DIVIDE)]
[QuantityOperator<Force, LinearAcceleration, Mass>(Operator.DIVIDE)]
public readonly partial struct Force;

[QuantityOperator<Density, Volume, Mass>(Operator.MULTIPLY)]
[QuantityOperator<Volume, Density, Mass>(Operator.MULTIPLY)]
[QuantityOperator<Density, Area, MassPerLength>(Operator.MULTIPLY)]
[QuantityOperator<Area, Density, MassPerLength>(Operator.MULTIPLY)]
[QuantityOperator<Density, Length, MassPerArea>(Operator.MULTIPLY)]
[QuantityOperator<Length, Density, MassPerArea>(Operator.MULTIPLY)]
public readonly partial struct Density;

[QuantityOperator<Power, Time, Energy>(Operator.MULTIPLY)]
[QuantityOperator<Time, Power, Energy>(Operator.MULTIPLY)]
public readonly partial struct Power;

[QuantityOperator<Energy, Time, Power>(Operator.DIVIDE)]
[QuantityOperator<Energy, Power, Time>(Operator.DIVIDE)]
[QuantityOperator<Energy, Energy, SquareEnergy>(Operator.MULTIPLY)]
[QuantityOperator<Energy, Length, LengthEnergy>(Operator.MULTIPLY)]
[QuantityOperator<Length, Energy, LengthEnergy>(Operator.MULTIPLY)]
public readonly partial struct Energy;

[QuantityOperator<LengthEnergy, Length, Energy>(Operator.DIVIDE)]
[QuantityOperator<LengthEnergy, Energy, Length>(Operator.DIVIDE)]
public readonly partial struct LengthEnergy;

[QuantityOperator<Length, ThermodynamicTemperature, LengthTemperature>(Operator.MULTIPLY)]
[QuantityOperator<ThermodynamicTemperature, Length, LengthTemperature>(Operator.MULTIPLY)]
public readonly partial struct ThermodynamicTemperature;

[QuantityOperator<LengthTemperature, ThermodynamicTemperature, Length>(Operator.DIVIDE)]
[QuantityOperator<LengthTemperature, Length, ThermodynamicTemperature>(Operator.DIVIDE)]
public readonly partial struct LengthTemperature;

[QuantityOperator<ElectricCurrent, Time, ElectricCharge>(Operator.MULTIPLY)]
[QuantityOperator<Time, ElectricCurrent, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricCurrent;

[QuantityOperator<ElectricCharge, Time, ElectricCurrent>(Operator.DIVIDE)]
[QuantityOperator<ElectricCharge, ElectricCurrent, Time>(Operator.DIVIDE)]
public readonly partial struct ElectricCharge;

[QuantityOperator<double, InverseLength, Length>(Operator.DIVIDE)]
public readonly partial struct InverseLength;
public readonly partial struct LinearMomentum;
public readonly partial struct SquareEnergy;

public readonly partial struct Time;
public readonly partial struct VolumePerTime;

// Linear and rotational mechanics.

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
[QuantityOperator<Force, Time, LinearMomentum>(Operator.MULTIPLY)]
[QuantityOperator<Time, Force, LinearMomentum>(Operator.MULTIPLY)]
[QuantityOperator<Force, ForcePerArea, Area>(Operator.DIVIDE)]
[QuantityOperator<Force, Area, ForcePerArea>(Operator.DIVIDE)]
[QuantityOperator<Force, ForcePerLength, Length>(Operator.DIVIDE)]
[QuantityOperator<Force, Length, ForcePerLength>(Operator.DIVIDE)]
public readonly partial struct Force;

[QuantityOperator<ForcePerArea, Area, Force>(Operator.MULTIPLY)]
[QuantityOperator<Area, ForcePerArea, Force>(Operator.MULTIPLY)]
[QuantityOperator<ForcePerArea, VolumePerTime, Power>(Operator.MULTIPLY)]
[QuantityOperator<VolumePerTime, ForcePerArea, Power>(Operator.MULTIPLY)]
public readonly partial struct ForcePerArea;

[QuantityOperator<ForcePerLength, Length, Force>(Operator.MULTIPLY)]
[QuantityOperator<Length, ForcePerLength, Force>(Operator.MULTIPLY)]
public readonly partial struct ForcePerLength;

[QuantityOperator<AngularVelocity, Time, PlaneAngle>(Operator.MULTIPLY)]
[QuantityOperator<Time, AngularVelocity, PlaneAngle>(Operator.MULTIPLY)]
[QuantityOperator<AngularVelocity, AngularAcceleration, Time>(Operator.DIVIDE)]
[QuantityOperator<AngularVelocity, Time, AngularAcceleration>(Operator.DIVIDE)]
[QuantityOperator<AngularVelocity, Length, LinearVelocity>(Operator.MULTIPLY)]
[QuantityOperator<Length, AngularVelocity, LinearVelocity>(Operator.MULTIPLY)]
public readonly partial struct AngularVelocity;

[QuantityOperator<PlaneAngle, AngularVelocity, Time>(Operator.DIVIDE)]
[QuantityOperator<PlaneAngle, Time, AngularVelocity>(Operator.DIVIDE)]
public readonly partial struct PlaneAngle;

[QuantityOperator<AngularAcceleration, Time, AngularVelocity>(Operator.MULTIPLY)]
[QuantityOperator<Time, AngularAcceleration, AngularVelocity>(Operator.MULTIPLY)]
[QuantityOperator<AngularAcceleration, Length, LinearAcceleration>(Operator.MULTIPLY)]
[QuantityOperator<Length, AngularAcceleration, LinearAcceleration>(Operator.MULTIPLY)]
public readonly partial struct AngularAcceleration;

[QuantityOperator<MomentOfInertia, AngularAcceleration, Torque>(Operator.MULTIPLY)]
[QuantityOperator<AngularAcceleration, MomentOfInertia, Torque>(Operator.MULTIPLY)]
[QuantityOperator<MomentOfInertia, AngularVelocity, AngularMomentum>(Operator.MULTIPLY)]
[QuantityOperator<AngularVelocity, MomentOfInertia, AngularMomentum>(Operator.MULTIPLY)]
public readonly partial struct MomentOfInertia;

[QuantityOperator<Torque, MomentOfInertia, AngularAcceleration>(Operator.DIVIDE)]
[QuantityOperator<Torque, AngularAcceleration, MomentOfInertia>(Operator.DIVIDE)]
// Average torque over the interval gives the change in angular momentum about the same axis.
[QuantityOperator<Torque, Time, AngularMomentum>(Operator.MULTIPLY)]
[QuantityOperator<Time, Torque, AngularMomentum>(Operator.MULTIPLY)]
[QuantityOperator<Torque, AngularVelocity, Power>(Operator.MULTIPLY)]
[QuantityOperator<AngularVelocity, Torque, Power>(Operator.MULTIPLY)]
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

[QuantityOperator<MassPerLength, Area, Density>(Operator.DIVIDE)]
[QuantityOperator<MassPerLength, Density, Area>(Operator.DIVIDE)]
[QuantityOperator<MassPerLength, Length, Mass>(Operator.MULTIPLY)]
[QuantityOperator<Length, MassPerLength, Mass>(Operator.MULTIPLY)]
public readonly partial struct MassPerLength;

[QuantityOperator<MassPerArea, Length, Density>(Operator.DIVIDE)]
[QuantityOperator<MassPerArea, Density, Length>(Operator.DIVIDE)]
[QuantityOperator<MassPerArea, Area, Mass>(Operator.MULTIPLY)]
[QuantityOperator<Area, MassPerArea, Mass>(Operator.MULTIPLY)]
[QuantityOperator<MassPerArea, MassPerAreaTime, Time>(Operator.DIVIDE)]
[QuantityOperator<MassPerArea, Time, MassPerAreaTime>(Operator.DIVIDE)]
public readonly partial struct MassPerArea;

[QuantityOperator<MassPerTime, Time, Mass>(Operator.MULTIPLY)]
[QuantityOperator<Time, MassPerTime, Mass>(Operator.MULTIPLY)]
[QuantityOperator<MassPerTime, MassPerAreaTime, Area>(Operator.DIVIDE)]
[QuantityOperator<MassPerTime, Area, MassPerAreaTime>(Operator.DIVIDE)]
[QuantityOperator<MassPerTime, Density, VolumePerTime>(Operator.DIVIDE)]
[QuantityOperator<MassPerTime, VolumePerTime, Density>(Operator.DIVIDE)]
public readonly partial struct MassPerTime;

[QuantityOperator<MassPerAreaTime, Area, MassPerTime>(Operator.MULTIPLY)]
[QuantityOperator<Area, MassPerAreaTime, MassPerTime>(Operator.MULTIPLY)]
[QuantityOperator<MassPerAreaTime, Time, MassPerArea>(Operator.MULTIPLY)]
[QuantityOperator<Time, MassPerAreaTime, MassPerArea>(Operator.MULTIPLY)]
public readonly partial struct MassPerAreaTime;

[QuantityOperator<SpecificVolume, Mass, Volume>(Operator.MULTIPLY)]
[QuantityOperator<Mass, SpecificVolume, Volume>(Operator.MULTIPLY)]
public readonly partial struct SpecificVolume;

[QuantityOperator<Volume, SpecificVolume, Mass>(Operator.DIVIDE)]
[QuantityOperator<Volume, Mass, SpecificVolume>(Operator.DIVIDE)]
[QuantityOperator<Volume, VolumePerTime, Time>(Operator.DIVIDE)]
public readonly partial struct Volume;

[QuantityOperator<Area, LinearVelocity, VolumePerTime>(Operator.MULTIPLY)]
[QuantityOperator<LinearVelocity, Area, VolumePerTime>(Operator.MULTIPLY)]
public readonly partial struct Area;

[QuantityOperator<VolumePerTime, Area, LinearVelocity>(Operator.DIVIDE)]
[QuantityOperator<VolumePerTime, LinearVelocity, Area>(Operator.DIVIDE)]
[QuantityOperator<VolumePerTime, Time, Volume>(Operator.MULTIPLY)]
[QuantityOperator<Time, VolumePerTime, Volume>(Operator.MULTIPLY)]
public readonly partial struct VolumePerTime;

[QuantityOperator<Density, VolumePerTime, MassPerTime>(Operator.MULTIPLY)]
[QuantityOperator<VolumePerTime, Density, MassPerTime>(Operator.MULTIPLY)]
public readonly partial struct Density;

[QuantityOperator<KinematicViscosity, Density, DynamicViscosity>(Operator.MULTIPLY)]
[QuantityOperator<Density, KinematicViscosity, DynamicViscosity>(Operator.MULTIPLY)]
[QuantityOperator<KinematicViscosity, ThermalDiffusivity, double>(Operator.DIVIDE)]
public readonly partial struct KinematicViscosity;

[QuantityOperator<DynamicViscosity, KinematicViscosity, Density>(Operator.DIVIDE)]
[QuantityOperator<DynamicViscosity, Density, KinematicViscosity>(Operator.DIVIDE)]
public readonly partial struct DynamicViscosity;

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

[QuantityOperator<EnergyDensity, Volume, Energy>(Operator.MULTIPLY)]
[QuantityOperator<Volume, EnergyDensity, Energy>(Operator.MULTIPLY)]
public readonly partial struct EnergyDensity;

[QuantityOperator<EnergyPerArea, Area, Energy>(Operator.MULTIPLY)]
[QuantityOperator<Area, EnergyPerArea, Energy>(Operator.MULTIPLY)]
public readonly partial struct EnergyPerArea;

[QuantityOperator<PowerPerArea, Area, Power>(Operator.MULTIPLY)]
[QuantityOperator<Area, PowerPerArea, Power>(Operator.MULTIPLY)]
public readonly partial struct PowerPerArea;

[QuantityOperator<Power, PowerPerArea, Area>(Operator.DIVIDE)]
[QuantityOperator<Power, Area, PowerPerArea>(Operator.DIVIDE)]
public readonly partial struct Power;

// Electricity and magnetism.

[QuantityOperator<Resistance, ElectricCurrent, ElectricPotential>(Operator.MULTIPLY)]
[QuantityOperator<ElectricCurrent, Resistance, ElectricPotential>(Operator.MULTIPLY)]
[QuantityOperator<Resistance, Capacitance, Time>(Operator.MULTIPLY)]
[QuantityOperator<Capacitance, Resistance, Time>(Operator.MULTIPLY)]
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
[QuantityOperator<ElectricPotential, ElectricCurrent, Power>(Operator.MULTIPLY)]
[QuantityOperator<ElectricCurrent, ElectricPotential, Power>(Operator.MULTIPLY)]
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
[QuantityOperator<ElectricCharge, Length, ElectricDipoleMoment>(Operator.MULTIPLY)]
[QuantityOperator<Length, ElectricCharge, ElectricDipoleMoment>(Operator.MULTIPLY)]
public readonly partial struct ElectricCharge;

// The field component times path length gives the voltage drop, V_start - V_end.
[QuantityOperator<ElectricFieldStrength, Length, ElectricPotential>(Operator.MULTIPLY)]
[QuantityOperator<Length, ElectricFieldStrength, ElectricPotential>(Operator.MULTIPLY)]
[QuantityOperator<ElectricFieldStrength, ElectricCharge, Force>(Operator.MULTIPLY)]
[QuantityOperator<ElectricCharge, ElectricFieldStrength, Force>(Operator.MULTIPLY)]
[QuantityOperator<ElectricFieldStrength, Area, ElectricFlux>(Operator.MULTIPLY)]
[QuantityOperator<Area, ElectricFieldStrength, ElectricFlux>(Operator.MULTIPLY)]
public readonly partial struct ElectricFieldStrength;

[QuantityOperator<Force, ElectricFieldStrength, ElectricCharge>(Operator.DIVIDE)]
[QuantityOperator<Force, ElectricCharge, ElectricFieldStrength>(Operator.DIVIDE)]
public readonly partial struct Force;

[QuantityOperator<ElectricFlux, ElectricFieldStrength, Area>(Operator.DIVIDE)]
[QuantityOperator<ElectricFlux, Area, ElectricFieldStrength>(Operator.DIVIDE)]
public readonly partial struct ElectricFlux;

[QuantityOperator<ElectricConductivity, ElectricFieldStrength, ElectricCurrentDensity>(Operator.MULTIPLY)]
[QuantityOperator<ElectricFieldStrength, ElectricConductivity, ElectricCurrentDensity>(Operator.MULTIPLY)]
public readonly partial struct ElectricConductivity;

[QuantityOperator<ElectricCurrentDensity, ElectricConductivity, ElectricFieldStrength>(Operator.DIVIDE)]
[QuantityOperator<ElectricCurrentDensity, ElectricFieldStrength, ElectricConductivity>(Operator.DIVIDE)]
[QuantityOperator<ElectricCurrentDensity, Area, ElectricCurrent>(Operator.MULTIPLY)]
[QuantityOperator<Area, ElectricCurrentDensity, ElectricCurrent>(Operator.MULTIPLY)]
public readonly partial struct ElectricCurrentDensity;

[QuantityOperator<ElectricCurrent, ElectricCurrentDensity, Area>(Operator.DIVIDE)]
[QuantityOperator<ElectricCurrent, Area, ElectricCurrentDensity>(Operator.DIVIDE)]
public readonly partial struct ElectricCurrent;

[QuantityOperator<ElectricChargeLineDensity, Length, ElectricCharge>(Operator.MULTIPLY)]
[QuantityOperator<Length, ElectricChargeLineDensity, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricChargeLineDensity;

[QuantityOperator<ElectricChargePerArea, Area, ElectricCharge>(Operator.MULTIPLY)]
[QuantityOperator<Area, ElectricChargePerArea, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricChargePerArea;

[QuantityOperator<ElectricChargeVolumeDensity, Volume, ElectricCharge>(Operator.MULTIPLY)]
[QuantityOperator<Volume, ElectricChargeVolumeDensity, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricChargeVolumeDensity;

[QuantityOperator<ElectricChargePerMass, Mass, ElectricCharge>(Operator.MULTIPLY)]
[QuantityOperator<Mass, ElectricChargePerMass, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricChargePerMass;

[QuantityOperator<ElectricChargePerAmountOfSubstance, AmountOfSubstance, ElectricCharge>(Operator.MULTIPLY)]
[QuantityOperator<AmountOfSubstance, ElectricChargePerAmountOfSubstance, ElectricCharge>(Operator.MULTIPLY)]
public readonly partial struct ElectricChargePerAmountOfSubstance;

[QuantityOperator<ElectricDipoleMoment, ElectricCharge, Length>(Operator.DIVIDE)]
[QuantityOperator<ElectricDipoleMoment, Length, ElectricCharge>(Operator.DIVIDE)]
public readonly partial struct ElectricDipoleMoment;

[QuantityOperator<Permittivity, ElectricFieldStrength, ElectricDisplacement>(Operator.MULTIPLY)]
[QuantityOperator<ElectricFieldStrength, Permittivity, ElectricDisplacement>(Operator.MULTIPLY)]
public readonly partial struct Permittivity;

[QuantityOperator<ElectricDisplacement, Permittivity, ElectricFieldStrength>(Operator.DIVIDE)]
[QuantityOperator<ElectricDisplacement, ElectricFieldStrength, Permittivity>(Operator.DIVIDE)]
public readonly partial struct ElectricDisplacement;

// Use the area-averaged field component normal to the surface.
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

[QuantityOperator<MagneticFluxPerLength, Length, MagneticFlux>(Operator.MULTIPLY)]
[QuantityOperator<Length, MagneticFluxPerLength, MagneticFlux>(Operator.MULTIPLY)]
public readonly partial struct MagneticFluxPerLength;

[QuantityOperator<Permeability, AuxillaryMagneticField, MagneticFluxDensity>(Operator.MULTIPLY)]
[QuantityOperator<AuxillaryMagneticField, Permeability, MagneticFluxDensity>(Operator.MULTIPLY)]
public readonly partial struct Permeability;

[QuantityOperator<AuxillaryMagneticField, Length, MagnetomotiveForce>(Operator.MULTIPLY)]
[QuantityOperator<Length, AuxillaryMagneticField, MagnetomotiveForce>(Operator.MULTIPLY)]
public readonly partial struct AuxillaryMagneticField;

[QuantityOperator<MagnetomotiveForce, Length, AuxillaryMagneticField>(Operator.DIVIDE)]
[QuantityOperator<MagnetomotiveForce, AuxillaryMagneticField, Length>(Operator.DIVIDE)]
public readonly partial struct MagnetomotiveForce;

// Heat and temperature.

// Use a constant heat capacity or its average over the temperature interval.
[QuantityOperator<HeatCapacity, SpecificHeatCapacity, Mass>(Operator.DIVIDE)]
[QuantityOperator<HeatCapacity, Mass, SpecificHeatCapacity>(Operator.DIVIDE)]
[QuantityOperator<HeatCapacity, VolumetricHeatCapacity, Volume>(Operator.DIVIDE)]
[QuantityOperator<HeatCapacity, Volume, VolumetricHeatCapacity>(Operator.DIVIDE)]
public readonly partial struct HeatCapacity;

[QuantityOperator<Energy, HeatFlowRate, Time>(Operator.DIVIDE)]
public readonly partial struct Energy;

[QuantityOperator<SpecificHeatCapacity, Mass, HeatCapacity>(Operator.MULTIPLY)]
[QuantityOperator<Mass, SpecificHeatCapacity, HeatCapacity>(Operator.MULTIPLY)]
[QuantityOperator<SpecificHeatCapacity, Density, VolumetricHeatCapacity>(Operator.MULTIPLY)]
[QuantityOperator<Density, SpecificHeatCapacity, VolumetricHeatCapacity>(Operator.MULTIPLY)]
public readonly partial struct SpecificHeatCapacity;

[QuantityOperator<VolumetricHeatCapacity, Volume, HeatCapacity>(Operator.MULTIPLY)]
[QuantityOperator<Volume, VolumetricHeatCapacity, HeatCapacity>(Operator.MULTIPLY)]
[QuantityOperator<VolumetricHeatCapacity, SpecificHeatCapacity, Density>(Operator.DIVIDE)]
[QuantityOperator<VolumetricHeatCapacity, Density, SpecificHeatCapacity>(Operator.DIVIDE)]
public readonly partial struct VolumetricHeatCapacity;

[QuantityOperator<ThermalDiffusivity, VolumetricHeatCapacity, ThermalConductivity>(Operator.MULTIPLY)]
[QuantityOperator<VolumetricHeatCapacity, ThermalDiffusivity, ThermalConductivity>(Operator.MULTIPLY)]
public readonly partial struct ThermalDiffusivity;

[QuantityOperator<ThermalConductivity, ThermalDiffusivity, VolumetricHeatCapacity>(Operator.DIVIDE)]
[QuantityOperator<ThermalConductivity, VolumetricHeatCapacity, ThermalDiffusivity>(Operator.DIVIDE)]
public readonly partial struct ThermalConductivity;

[QuantityOperator<HeatFlowRate, Time, Energy>(Operator.MULTIPLY)]
[QuantityOperator<Time, HeatFlowRate, Energy>(Operator.MULTIPLY)]
[QuantityOperator<HeatFlowRate, HeatFlowRatePerArea, Area>(Operator.DIVIDE)]
[QuantityOperator<HeatFlowRate, Area, HeatFlowRatePerArea>(Operator.DIVIDE)]
public readonly partial struct HeatFlowRate;

[QuantityOperator<HeatFlowRatePerArea, Area, HeatFlowRate>(Operator.MULTIPLY)]
[QuantityOperator<Area, HeatFlowRatePerArea, HeatFlowRate>(Operator.MULTIPLY)]
public readonly partial struct HeatFlowRatePerArea;

[QuantityOperator<ThermalResistance, Area, ThermalInsulance>(Operator.MULTIPLY)]
[QuantityOperator<Area, ThermalResistance, ThermalInsulance>(Operator.MULTIPLY)]
[QuantityOperator<ThermalResistance, HeatCapacity, Time>(Operator.MULTIPLY)]
[QuantityOperator<HeatCapacity, ThermalResistance, Time>(Operator.MULTIPLY)]
public readonly partial struct ThermalResistance;

[QuantityOperator<Time, ThermalResistance, HeatCapacity>(Operator.DIVIDE)]
[QuantityOperator<Time, HeatCapacity, ThermalResistance>(Operator.DIVIDE)]
public readonly partial struct Time;

[QuantityOperator<ThermalInsulance, ThermalResistance, Area>(Operator.DIVIDE)]
[QuantityOperator<ThermalInsulance, Area, ThermalResistance>(Operator.DIVIDE)]
public readonly partial struct ThermalInsulance;

// Photometry and radiometry.

// Use the average intensity over the solid angle. For luminance, use the projected emitting area.
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
[QuantityOperator<LuminousFlux, Time, LuminousEnergy>(Operator.MULTIPLY)]
[QuantityOperator<Time, LuminousFlux, LuminousEnergy>(Operator.MULTIPLY)]
[QuantityOperator<LuminousFlux, LuminousEfficacy, Power>(Operator.DIVIDE)]
[QuantityOperator<LuminousFlux, Power, LuminousEfficacy>(Operator.DIVIDE)]
public readonly partial struct LuminousFlux;

[QuantityOperator<Illuminance, Area, LuminousFlux>(Operator.MULTIPLY)]
[QuantityOperator<Area, Illuminance, LuminousFlux>(Operator.MULTIPLY)]
public readonly partial struct Illuminance;

[QuantityOperator<Luminance, Area, LuminousIntensity>(Operator.MULTIPLY)]
[QuantityOperator<Area, Luminance, LuminousIntensity>(Operator.MULTIPLY)]
public readonly partial struct Luminance;

[QuantityOperator<LuminousEnergy, LuminousFlux, Time>(Operator.DIVIDE)]
[QuantityOperator<LuminousEnergy, Time, LuminousFlux>(Operator.DIVIDE)]
public readonly partial struct LuminousEnergy;

[QuantityOperator<LuminousEfficacy, Power, LuminousFlux>(Operator.MULTIPLY)]
[QuantityOperator<Power, LuminousEfficacy, LuminousFlux>(Operator.MULTIPLY)]
public readonly partial struct LuminousEfficacy;

[QuantityOperator<RadiantIntensity, SolidAngle, Power>(Operator.MULTIPLY)]
[QuantityOperator<SolidAngle, RadiantIntensity, Power>(Operator.MULTIPLY)]
public readonly partial struct RadiantIntensity;

[QuantityOperator<Power, RadiantIntensity, SolidAngle>(Operator.DIVIDE)]
[QuantityOperator<Power, SolidAngle, RadiantIntensity>(Operator.DIVIDE)]
public readonly partial struct Power;

// Amount of substance and radiation dose.

[QuantityOperator<InverseAmountOfSubstance, AmountOfSubstance, double>(Operator.MULTIPLY)]
[QuantityOperator<AmountOfSubstance, InverseAmountOfSubstance, double>(Operator.MULTIPLY)]
[QuantityOperator<double, InverseAmountOfSubstance, AmountOfSubstance>(Operator.DIVIDE)]
public readonly partial struct InverseAmountOfSubstance;

[QuantityOperator<double, AmountOfSubstance, InverseAmountOfSubstance>(Operator.DIVIDE)]
public readonly partial struct AmountOfSubstance;

[QuantityOperator<Density, MolarMass, AmountOfSubstancePerVolume>(Operator.DIVIDE)]
[QuantityOperator<Density, AmountOfSubstancePerVolume, MolarMass>(Operator.DIVIDE)]
public readonly partial struct Density;

[QuantityOperator<MolarMass, AmountOfSubstance, Mass>(Operator.MULTIPLY)]
[QuantityOperator<AmountOfSubstance, MolarMass, Mass>(Operator.MULTIPLY)]
[QuantityOperator<MolarMass, AmountOfSubstancePerVolume, Density>(Operator.MULTIPLY)]
[QuantityOperator<AmountOfSubstancePerVolume, MolarMass, Density>(Operator.MULTIPLY)]
[QuantityOperator<MolarMass, Density, MolarVolume>(Operator.DIVIDE)]
[QuantityOperator<MolarMass, MolarVolume, Density>(Operator.DIVIDE)]
public readonly partial struct MolarMass;

[QuantityOperator<Mass, MolarMass, AmountOfSubstance>(Operator.DIVIDE)]
[QuantityOperator<Mass, AmountOfSubstance, MolarMass>(Operator.DIVIDE)]
public readonly partial struct Mass;

[QuantityOperator<MolarVolume, AmountOfSubstance, Volume>(Operator.MULTIPLY)]
[QuantityOperator<AmountOfSubstance, MolarVolume, Volume>(Operator.MULTIPLY)]
[QuantityOperator<MolarVolume, Density, MolarMass>(Operator.MULTIPLY)]
[QuantityOperator<Density, MolarVolume, MolarMass>(Operator.MULTIPLY)]
public readonly partial struct MolarVolume;

[QuantityOperator<Volume, MolarVolume, AmountOfSubstance>(Operator.DIVIDE)]
[QuantityOperator<Volume, AmountOfSubstance, MolarVolume>(Operator.DIVIDE)]
public readonly partial struct Volume;

[QuantityOperator<MolarEnergy, AmountOfSubstance, Energy>(Operator.MULTIPLY)]
[QuantityOperator<AmountOfSubstance, MolarEnergy, Energy>(Operator.MULTIPLY)]
public readonly partial struct MolarEnergy;

[QuantityOperator<Energy, MolarEnergy, AmountOfSubstance>(Operator.DIVIDE)]
[QuantityOperator<Energy, AmountOfSubstance, MolarEnergy>(Operator.DIVIDE)]
[QuantityOperator<Energy, AbsorbedDose, Mass>(Operator.DIVIDE)]
public readonly partial struct Energy;

[QuantityOperator<MolarHeatCapacity, AmountOfSubstance, HeatCapacity>(Operator.MULTIPLY)]
[QuantityOperator<AmountOfSubstance, MolarHeatCapacity, HeatCapacity>(Operator.MULTIPLY)]
public readonly partial struct MolarHeatCapacity;

[QuantityOperator<HeatCapacity, MolarHeatCapacity, AmountOfSubstance>(Operator.DIVIDE)]
[QuantityOperator<HeatCapacity, AmountOfSubstance, MolarHeatCapacity>(Operator.DIVIDE)]
public readonly partial struct HeatCapacity;

[QuantityOperator<AmountOfSubstancePerVolume, Volume, AmountOfSubstance>(Operator.MULTIPLY)]
[QuantityOperator<Volume, AmountOfSubstancePerVolume, AmountOfSubstance>(Operator.MULTIPLY)]
public readonly partial struct AmountOfSubstancePerVolume;

[QuantityOperator<AmountOfSubstance, AmountOfSubstancePerVolume, Volume>(Operator.DIVIDE)]
[QuantityOperator<AmountOfSubstance, Volume, AmountOfSubstancePerVolume>(Operator.DIVIDE)]
[QuantityOperator<AmountOfSubstance, AmountOfSubstancePerMass, Mass>(Operator.DIVIDE)]
[QuantityOperator<AmountOfSubstance, Mass, AmountOfSubstancePerMass>(Operator.DIVIDE)]
[QuantityOperator<AmountOfSubstance, CatalyticActivity, Time>(Operator.DIVIDE)]
public readonly partial struct AmountOfSubstance;

[QuantityOperator<AmountOfSubstancePerMass, Mass, AmountOfSubstance>(Operator.MULTIPLY)]
[QuantityOperator<Mass, AmountOfSubstancePerMass, AmountOfSubstance>(Operator.MULTIPLY)]
public readonly partial struct AmountOfSubstancePerMass;

[QuantityOperator<AbsorbedDoseRate, Time, AbsorbedDose>(Operator.MULTIPLY)]
[QuantityOperator<Time, AbsorbedDoseRate, AbsorbedDose>(Operator.MULTIPLY)]
public readonly partial struct AbsorbedDoseRate;

[QuantityOperator<AbsorbedDose, AbsorbedDoseRate, Time>(Operator.DIVIDE)]
[QuantityOperator<AbsorbedDose, Time, AbsorbedDoseRate>(Operator.DIVIDE)]
// The dose is the mean over the same mass to which the deposited energy refers.
[QuantityOperator<AbsorbedDose, Mass, Energy>(Operator.MULTIPLY)]
[QuantityOperator<Mass, AbsorbedDose, Energy>(Operator.MULTIPLY)]
public readonly partial struct AbsorbedDose;

[QuantityOperator<CatalyticActivity, Time, AmountOfSubstance>(Operator.MULTIPLY)]
[QuantityOperator<Time, CatalyticActivity, AmountOfSubstance>(Operator.MULTIPLY)]
public readonly partial struct CatalyticActivity;

// Mechanical power and reciprocal density.

// Supply the force component along the direction represented by the velocity.
[QuantityOperator<Force, LinearVelocity, Power>(Operator.MULTIPLY)]
[QuantityOperator<LinearVelocity, Force, Power>(Operator.MULTIPLY)]
public readonly partial struct Force;

[QuantityOperator<Power, Force, LinearVelocity>(Operator.DIVIDE)]
[QuantityOperator<Power, LinearVelocity, Force>(Operator.DIVIDE)]
public readonly partial struct Power;

[QuantityOperator<double, Density, SpecificVolume>(Operator.DIVIDE)]
[QuantityOperator<Density, SpecificVolume, double>(Operator.MULTIPLY)]
[QuantityOperator<SpecificVolume, Density, double>(Operator.MULTIPLY)]
public readonly partial struct Density;

[QuantityOperator<double, SpecificVolume, Density>(Operator.DIVIDE)]
public readonly partial struct SpecificVolume;

// Mass-to-charge ratio.

[QuantityOperator<MassPerElectricCharge, ElectricCharge, Mass>(Operator.MULTIPLY)]
[QuantityOperator<ElectricCharge, MassPerElectricCharge, Mass>(Operator.MULTIPLY)]
public readonly partial struct MassPerElectricCharge;

[QuantityOperator<Mass, MassPerElectricCharge, ElectricCharge>(Operator.DIVIDE)]
[QuantityOperator<Mass, ElectricCharge, MassPerElectricCharge>(Operator.DIVIDE)]
public readonly partial struct Mass;

// Thermal material properties.

[QuantityOperator<double, ThermalConductivity, ThermalResistivity>(Operator.DIVIDE)]
[QuantityOperator<ThermalConductivity, ThermalResistivity, double>(Operator.MULTIPLY)]
[QuantityOperator<ThermalResistivity, ThermalConductivity, double>(Operator.MULTIPLY)]
public readonly partial struct ThermalConductivity;

[QuantityOperator<double, ThermalResistivity, ThermalConductivity>(Operator.DIVIDE)]
[QuantityOperator<ThermalResistivity, Length, ThermalInsulance>(Operator.MULTIPLY)]
[QuantityOperator<Length, ThermalResistivity, ThermalInsulance>(Operator.MULTIPLY)]
public readonly partial struct ThermalResistivity;

[QuantityOperator<ThermalInsulance, ThermalConductivity, Length>(Operator.MULTIPLY)]
[QuantityOperator<ThermalConductivity, ThermalInsulance, Length>(Operator.MULTIPLY)]
[QuantityOperator<ThermalInsulance, ThermalResistivity, Length>(Operator.DIVIDE)]
[QuantityOperator<ThermalInsulance, Length, ThermalResistivity>(Operator.DIVIDE)]
public readonly partial struct ThermalInsulance;

[QuantityOperator<Length, ThermalInsulance, ThermalConductivity>(Operator.DIVIDE)]
[QuantityOperator<Length, ThermalConductivity, ThermalInsulance>(Operator.DIVIDE)]
public readonly partial struct Length;
