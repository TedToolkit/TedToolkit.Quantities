// -----------------------------------------------------------------------
// <copyright file="QuantityConversions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// -----------------------------------------------------------------------

namespace TedToolkit.Quantities;

#pragma warning disable CS1591 // These casts share the value-preserving behavior described below.

// Casts preserve the coherent SI value. Implicit casts generalize the source quantity.
public readonly partial struct AbsorbedDose
{
    public static explicit operator DoseEquivalent(AbsorbedDose value) => (DoseEquivalent)value.Value;
    public static explicit operator SpecificEnergy(AbsorbedDose value) => (SpecificEnergy)value.Value;
}

public readonly partial struct Activity
{
    public static explicit operator AngularVelocity(Activity value) => (AngularVelocity)value.Value;
    public static explicit operator Frequency(Activity value) => (Frequency)value.Value;
}

public readonly partial struct AngularVelocity
{
    public static explicit operator Activity(AngularVelocity value) => (Activity)value.Value;
    public static explicit operator Frequency(AngularVelocity value) => (Frequency)value.Value;
}

public readonly partial struct Area
{
    public static explicit operator AreaAngle(Area value) => (AreaAngle)value.Value;
}

public readonly partial struct AreaAngle
{
    public static explicit operator Area(AreaAngle value) => (Area)value.Value;
}

public readonly partial struct AreaPerTime
{
    public static explicit operator KinematicViscosity(AreaPerTime value) => (KinematicViscosity)value.Value;
    public static explicit operator ThermalDiffusivity(AreaPerTime value) => (ThermalDiffusivity)value.Value;
}

public readonly partial struct AuxillaryMagneticField
{
    public static explicit operator LinearElectricCurrent(AuxillaryMagneticField value) => (LinearElectricCurrent)value.Value;
    public static explicit operator Magnetization(AuxillaryMagneticField value) => (Magnetization)value.Value;
}

public readonly partial struct DoseEquivalent
{
    public static explicit operator AbsorbedDose(DoseEquivalent value) => (AbsorbedDose)value.Value;
    public static explicit operator SpecificEnergy(DoseEquivalent value) => (SpecificEnergy)value.Value;
}

public readonly partial struct ElectricChargePerArea
{
    public static explicit operator ElectricDisplacement(ElectricChargePerArea value) => (ElectricDisplacement)value.Value;
    public static explicit operator PolarizationField(ElectricChargePerArea value) => (PolarizationField)value.Value;
}

public readonly partial struct ElectricChargePerMass
{
    public static explicit operator Exposure(ElectricChargePerMass value) => (Exposure)value.Value;
}

public readonly partial struct ElectricCurrent
{
    public static explicit operator ElectricCurrentPerAngle(ElectricCurrent value) => (ElectricCurrentPerAngle)value.Value;
    public static explicit operator MagnetomotiveForce(ElectricCurrent value) => (MagnetomotiveForce)value.Value;
}

public readonly partial struct ElectricCurrentPerAngle
{
    public static explicit operator ElectricCurrent(ElectricCurrentPerAngle value) => (ElectricCurrent)value.Value;
    public static explicit operator MagnetomotiveForce(ElectricCurrentPerAngle value) => (MagnetomotiveForce)value.Value;
}

public readonly partial struct ElectricCurrentPerEnergy
{
    public static explicit operator InverseMagneticFlux(ElectricCurrentPerEnergy value) => (InverseMagneticFlux)value.Value;
}

public readonly partial struct ElectricDisplacement
{
    public static explicit operator ElectricChargePerArea(ElectricDisplacement value) => (ElectricChargePerArea)value.Value;
    public static explicit operator PolarizationField(ElectricDisplacement value) => (PolarizationField)value.Value;
}

public readonly partial struct ElectricFieldStrength
{
    public static explicit operator ForcePerElectricCharge(ElectricFieldStrength value) => (ForcePerElectricCharge)value.Value;
}

public readonly partial struct ElectricPotential
{
    public static explicit operator ElectromotiveForce(ElectricPotential value) => (ElectromotiveForce)value.Value;
}

public readonly partial struct ElectromotiveForce
{
    public static explicit operator ElectricPotential(ElectromotiveForce value) => (ElectricPotential)value.Value;
    public static explicit operator EnergyPerElectricCharge(ElectromotiveForce value) => (EnergyPerElectricCharge)value.Value;
}

public readonly partial struct Energy
{
    public static explicit operator Torque(Energy value) => (Torque)value.Value;
}

public readonly partial struct EnergyDensity
{
    public static explicit operator ForcePerArea(EnergyDensity value) => (ForcePerArea)value.Value;
    public static explicit operator Stress(EnergyDensity value) => (Stress)value.Value;
}

public readonly partial struct EnergyPerArea
{
    public static explicit operator ForcePerLength(EnergyPerArea value) => (ForcePerLength)value.Value;
}

public readonly partial struct EnergyPerElectricCharge
{
    public static explicit operator ElectromotiveForce(EnergyPerElectricCharge value) => (ElectromotiveForce)value.Value;
}

public readonly partial struct Exposure
{
    public static explicit operator ElectricChargePerMass(Exposure value) => (ElectricChargePerMass)value.Value;
}

public readonly partial struct Force
{
    public static explicit operator GravitationalAttraction(Force value) => (GravitationalAttraction)value.Value;
}

public readonly partial struct ForcePerArea
{
    public static explicit operator EnergyDensity(ForcePerArea value) => (EnergyDensity)value.Value;
    public static explicit operator Stress(ForcePerArea value) => (Stress)value.Value;
}

public readonly partial struct ForcePerElectricCharge
{
    public static explicit operator ElectricFieldStrength(ForcePerElectricCharge value) => (ElectricFieldStrength)value.Value;
}

public readonly partial struct ForcePerLength
{
    public static explicit operator EnergyPerArea(ForcePerLength value) => (EnergyPerArea)value.Value;
}

public readonly partial struct Frequency
{
    public static explicit operator Activity(Frequency value) => (Activity)value.Value;
    public static explicit operator AngularVelocity(Frequency value) => (AngularVelocity)value.Value;
}

public readonly partial struct GravitationalAttraction
{
    public static implicit operator Force(GravitationalAttraction value) => (Force)value.Value;
}

public readonly partial struct HeatFlowRate
{
    public static implicit operator Power(HeatFlowRate value) => (Power)value.Value;
    public static explicit operator RadiantIntensity(HeatFlowRate value) => (RadiantIntensity)value.Value;
}

public readonly partial struct HeatFlowRatePerArea
{
    public static implicit operator PowerPerArea(HeatFlowRatePerArea value) => (PowerPerArea)value.Value;
    public static explicit operator PowerPerAreaAngle(HeatFlowRatePerArea value) => (PowerPerAreaAngle)value.Value;
}

public readonly partial struct Illuminance
{
    public static explicit operator Luminance(Illuminance value) => (Luminance)value.Value;
    public static implicit operator LuminousFluxPerArea(Illuminance value) => (LuminousFluxPerArea)value.Value;
}

public readonly partial struct InverseMagneticFlux
{
    public static explicit operator ElectricCurrentPerEnergy(InverseMagneticFlux value) => (ElectricCurrentPerEnergy)value.Value;
}

public readonly partial struct KinematicViscosity
{
    public static explicit operator AreaPerTime(KinematicViscosity value) => (AreaPerTime)value.Value;
    public static explicit operator ThermalDiffusivity(KinematicViscosity value) => (ThermalDiffusivity)value.Value;
}

public readonly partial struct LinearAcceleration
{
    public static explicit operator ThrustToMassRatio(LinearAcceleration value) => (ThrustToMassRatio)value.Value;
}

public readonly partial struct LinearElectricCurrent
{
    public static explicit operator AuxillaryMagneticField(LinearElectricCurrent value) => (AuxillaryMagneticField)value.Value;
    public static explicit operator Magnetization(LinearElectricCurrent value) => (Magnetization)value.Value;
}

public readonly partial struct LinearVelocity
{
    public static explicit operator SpecificImpulseByMass(LinearVelocity value) => (SpecificImpulseByMass)value.Value;
}

public readonly partial struct Luminance
{
    public static explicit operator Illuminance(Luminance value) => (Illuminance)value.Value;
    public static explicit operator LuminousFluxPerArea(Luminance value) => (LuminousFluxPerArea)value.Value;
}

public readonly partial struct LuminousFlux
{
    public static explicit operator LuminousIntensity(LuminousFlux value) => (LuminousIntensity)value.Value;
}

public readonly partial struct LuminousFluxPerArea
{
    public static explicit operator Illuminance(LuminousFluxPerArea value) => (Illuminance)value.Value;
    public static explicit operator Luminance(LuminousFluxPerArea value) => (Luminance)value.Value;
}

public readonly partial struct LuminousIntensity
{
    public static explicit operator LuminousFlux(LuminousIntensity value) => (LuminousFlux)value.Value;
}

public readonly partial struct Magnetization
{
    public static explicit operator AuxillaryMagneticField(Magnetization value) => (AuxillaryMagneticField)value.Value;
    public static explicit operator LinearElectricCurrent(Magnetization value) => (LinearElectricCurrent)value.Value;
}

public readonly partial struct MagnetomotiveForce
{
    public static explicit operator ElectricCurrent(MagnetomotiveForce value) => (ElectricCurrent)value.Value;
    public static explicit operator ElectricCurrentPerAngle(MagnetomotiveForce value) => (ElectricCurrentPerAngle)value.Value;
}

public readonly partial struct PlaneAngle
{
    public static explicit operator SolidAngle(PlaneAngle value) => (SolidAngle)value.Value;
}

public readonly partial struct PolarizationField
{
    public static explicit operator ElectricChargePerArea(PolarizationField value) => (ElectricChargePerArea)value.Value;
    public static explicit operator ElectricDisplacement(PolarizationField value) => (ElectricDisplacement)value.Value;
}

public readonly partial struct Power
{
    public static explicit operator HeatFlowRate(Power value) => (HeatFlowRate)value.Value;
    public static explicit operator RadiantIntensity(Power value) => (RadiantIntensity)value.Value;
}

public readonly partial struct PowerArea
{
    public static explicit operator PowerAreaPerSolidAngle(PowerArea value) => (PowerAreaPerSolidAngle)value.Value;
}

public readonly partial struct PowerAreaPerSolidAngle
{
    public static explicit operator PowerArea(PowerAreaPerSolidAngle value) => (PowerArea)value.Value;
}

public readonly partial struct PowerPerArea
{
    public static explicit operator HeatFlowRatePerArea(PowerPerArea value) => (HeatFlowRatePerArea)value.Value;
    public static explicit operator PowerPerAreaAngle(PowerPerArea value) => (PowerPerAreaAngle)value.Value;
}

public readonly partial struct PowerPerAreaAngle
{
    public static explicit operator HeatFlowRatePerArea(PowerPerAreaAngle value) => (HeatFlowRatePerArea)value.Value;
    public static explicit operator PowerPerArea(PowerPerAreaAngle value) => (PowerPerArea)value.Value;
}

public readonly partial struct RadiantIntensity
{
    public static explicit operator HeatFlowRate(RadiantIntensity value) => (HeatFlowRate)value.Value;
    public static explicit operator Power(RadiantIntensity value) => (Power)value.Value;
}

public readonly partial struct SolidAngle
{
    public static explicit operator PlaneAngle(SolidAngle value) => (PlaneAngle)value.Value;
}

public readonly partial struct SpecificEnergy
{
    public static explicit operator AbsorbedDose(SpecificEnergy value) => (AbsorbedDose)value.Value;
    public static explicit operator DoseEquivalent(SpecificEnergy value) => (DoseEquivalent)value.Value;
}

public readonly partial struct SpecificImpulseByMass
{
    public static explicit operator LinearVelocity(SpecificImpulseByMass value) => (LinearVelocity)value.Value;
}

public readonly partial struct Stress
{
    public static explicit operator EnergyDensity(Stress value) => (EnergyDensity)value.Value;
    public static implicit operator ForcePerArea(Stress value) => (ForcePerArea)value.Value;
}

public readonly partial struct ThermalDiffusivity
{
    public static explicit operator AreaPerTime(ThermalDiffusivity value) => (AreaPerTime)value.Value;
    public static explicit operator KinematicViscosity(ThermalDiffusivity value) => (KinematicViscosity)value.Value;
}

public readonly partial struct ThrustToMassRatio
{
    public static explicit operator LinearAcceleration(ThrustToMassRatio value) => (LinearAcceleration)value.Value;
}

public readonly partial struct Torque
{
    public static explicit operator Energy(Torque value) => (Energy)value.Value;
}
