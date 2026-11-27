// -----------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.Quantities;

// Select all QUDT SI quantity kinds and the additional kinds used by the formulas.
[assembly: Quantities<double>(QuantitySystems.ALL,
    "AbsorbedDose", "AbsorbedDoseRate", "Activity", "AmountOfSubstance",
    "AmountOfSubstancePerMass", "AmountOfSubstancePerVolume", "AngularAcceleration", "AngularMomentum",
    "AngularVelocity", "Area", "AreaAngle", "AreaPerTime",
    "AreaTemperature", "AreaThermalExpansion", "AreaTime", "AuxillaryMagneticField",
    "Capacitance", "CatalyticActivity", "CoefficientOfHeatTransfer", "Conductance",
    "CubicElectricDipoleMomentPerSquareEnergy", "Density", "Depth", "Diameter",
    "DimensionlessRatio", "Distance", "DoseEquivalent", "DynamicViscosity",
    "ElectricCharge", "ElectricChargeLineDensity", "ElectricChargePerAmountOfSubstance", "ElectricChargePerArea",
    "ElectricChargePerMass", "ElectricChargeVolumeDensity", "ElectricConductivity", "ElectricCurrent",
    "ElectricCurrentDensity", "ElectricCurrentPerAngle", "ElectricCurrentPerEnergy", "ElectricDipoleMoment",
    "ElectricDisplacement", "ElectricFieldStrength", "ElectricFlux", "ElectricPotential",
    "ElectricQuadrupoleMoment", "ElectromotiveForce", "Energy", "EnergyDensity",
    "EnergyPerArea", "EnergyPerAreaElectricCharge", "EnergyPerElectricCharge", "EnergyPerSquareMagneticFluxDensity",
    "Exposure", "Force", "ForcePerArea", "ForcePerAreaTime",
    "ForcePerElectricCharge", "ForcePerLength", "Frequency", "GravitationalAttraction",
    "HeatCapacity", "HeatFlowRate", "HeatFlowRatePerArea", "Height",
    "Illuminance", "Impulse", "Inductance", "InverseAmountOfSubstance",
    "InverseEnergy", "InverseLength", "InverseLengthTemperature", "InverseMagneticFlux",
    "InversePermittivity", "InverseSquareEnergy", "InverseTimeTemperature", "InverseVolume",
    "KinematicViscosity", "Length", "LengthEnergy", "LengthMass",
    "LengthMolarEnergy", "LengthPerElectricCurrent", "LengthTemperature", "LinearAcceleration",
    "LinearElectricCurrent", "LinearMomentum", "LinearThermalExpansion", "LinearVelocity",
    "Luminance", "LuminousEfficacy", "LuminousEnergy", "LuminousFlux",
    "LuminousFluxPerArea", "LuminousIntensity", "MagneticDipoleMoment", "MagneticFlux",
    "MagneticFluxDensity", "MagneticFluxPerLength", "MagneticReluctivity", "Magnetization",
    "MagnetomotiveForce", "Mass", "MassPerArea", "MassPerAreaTime",
    "MassPerElectricCharge", "MassPerLength", "MassPerTime", "MassTemperature",
    "MolarAngularMomentum", "MolarEnergy", "MolarHeatCapacity", "MolarMass",
    "MolarVolume", "MomentOfInertia", "PathLength", "Permeability",
    "Permittivity", "PlaneAngle", "Polarizability", "PolarizationField",
    "Power", "PowerArea", "PowerAreaPerSolidAngle", "PowerPerArea",
    "PowerPerAreaAngle", "PowerPerAreaQuarticTemperature", "PowerPerElectricCharge", "Pressure",
    "QuarticElectricDipoleMomentPerCubicEnergy", "RadiantIntensity", "Radius", "Resistance",
    "Resistivity", "ReynoldsNumber", "SolidAngle", "SpecificEnergy",
    "SpecificHeatCapacity", "SpecificHeatPressure", "SpecificHeatVolume", "SpecificImpulseByMass",
    "SpecificVolume", "SquareEnergy", "SquareTime", "StandardGravitationalParameter",
    "Stress", "TemperatureAmountOfSubstance", "TemperatureDifference", "TemperatureGradient",
    "TemperaturePerMagneticFluxDensity", "TemperaturePerTime", "ThermalConductance", "ThermalConductivity",
    "ThermalDiffusivity", "ThermalInsulance", "ThermalResistance", "ThermalResistivity",
    "ThermodynamicTemperature", "ThrustToMassRatio", "Time", "TimeTemperature",
    "Torque", "Volume", "VolumePerTime", "VolumeThermalExpansion",
    "VolumetricHeatCapacity", "Wavelength", "Width")]
