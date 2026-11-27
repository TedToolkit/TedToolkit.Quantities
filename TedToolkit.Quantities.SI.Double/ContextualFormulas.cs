// -----------------------------------------------------------------------
// <copyright file="ContextualFormulas.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Quantities;

public readonly partial struct Frequency
{
    /// <summary>Calculates the frequency of a periodic phenomenon from the duration of one cycle.</summary>
    /// <param name="period">The duration of one cycle.</param>
    /// <returns>The frequency of the phenomenon.</returns>
    public static Frequency FromPeriod(Time period)
    {
        return FromHertz(1d / period.Second);
    }

    /// <summary>Calculates the duration of one cycle at this frequency.</summary>
    /// <returns>The period of the phenomenon.</returns>
    public Time ToPeriod()
    {
        return Time.FromSecond(1d / Hertz);
    }

    /// <summary>Calculates frequency from the speed and nonzero wavelength of the same wave.</summary>
    public static Frequency FromWaveSpeed(LinearVelocity waveSpeed, Length wavelength)
    {
        return (Frequency)(waveSpeed.Value / wavelength.Value);
    }

    /// <summary>Calculates the ordinary frequency of an ideal LC circuit with positive inductance and capacitance.</summary>
    public static Frequency FromLcResonance(Inductance inductance, Capacitance capacitance)
    {
        return AngularVelocity.FromLcResonance(inductance, capacitance).ToFrequency();
    }
}

public readonly partial struct AngularVelocity
{
    /// <summary>Calculates the angular velocity of a motion with one revolution per cycle.</summary>
    /// <param name="frequency">The number of revolutions per unit time.</param>
    /// <returns>The corresponding angular velocity.</returns>
    public static AngularVelocity FromFrequency(Frequency frequency)
    {
        return FromRadianPerSecond(PlaneAngle.FromRevolution(1d).Radian * frequency.Hertz);
    }

    /// <summary>Calculates the rotation frequency with one cycle per revolution.</summary>
    /// <returns>The frequency corresponding to this angular velocity.</returns>
    public Frequency ToFrequency()
    {
        return Frequency.FromHertz(RadianPerSecond / PlaneAngle.FromRevolution(1d).Radian);
    }

    /// <summary>Calculates the angular frequency of an ideal LC circuit with positive inductance and capacitance.</summary>
    public static AngularVelocity FromLcResonance(Inductance inductance, Capacitance capacitance)
    {
        return FromRadianPerSecond(1d / Math.Sqrt(inductance.Henry * capacitance.Farad));
    }
}

public readonly partial struct PlaneAngle
{
    /// <summary>Calculates the central angle subtended by a circular arc.</summary>
    /// <param name="arcLength">The arc's signed length.</param>
    /// <param name="radius">The circle's positive radius.</param>
    /// <returns>The central angle.</returns>
    public static PlaneAngle FromArc(Length arcLength, Length radius)
    {
        return FromRadian(arcLength / radius);
    }

    /// <summary>Calculates the length of the circular arc subtended by this central angle.</summary>
    /// <param name="radius">The circle's positive radius.</param>
    /// <returns>The signed arc length.</returns>
    public Length ToArcLength(Length radius)
    {
        return radius * Radian;
    }
}

public readonly partial struct Length
{
    /// <summary>Calculates a circle's radius from its diameter.</summary>
    /// <param name="diameter">The diameter of the same circle.</param>
    /// <returns>The radius, equal to half the diameter.</returns>
    public static Length RadiusFromDiameter(Length diameter)
    {
        return diameter / 2d;
    }

    /// <summary>Calculates the circumference of a circle given its positive radius.</summary>
    /// <param name="radius">The radius of the circle.</param>
    /// <returns>The length around the circle.</returns>
    public static Length CircumferenceFromRadius(Length radius)
    {
        return radius * (2d * Math.PI);
    }

    /// <summary>Calculates a circle's diameter from its radius.</summary>
    /// <param name="radius">The radius of the same circle.</param>
    /// <returns>The diameter, equal to twice the radius.</returns>
    public static Length DiameterFromRadius(Length radius)
    {
        return 2d * radius;
    }

    /// <summary>Calculates wavelength from the speed and nonzero frequency of the same wave.</summary>
    public static Length WavelengthFromSpeed(LinearVelocity waveSpeed, Frequency frequency)
    {
        return (Length)(waveSpeed.Value / frequency.Value);
    }
}

public readonly partial struct Area
{
    /// <summary>Calculates the area enclosed by a circle.</summary>
    /// <param name="radius">The positive radius of the circle.</param>
    /// <returns>The circle's area.</returns>
    public static Area FromCircle(Length radius)
    {
        return (Area)(Math.PI * radius.Value * radius.Value);
    }
}

public readonly partial struct Energy
{
    /// <summary>Calculates work done by a constant force over a displacement.</summary>
    /// <param name="forceAlongDisplacement">The signed force component along the direction of displacement.</param>
    /// <param name="displacement">The distance moved in that direction.</param>
    /// <returns>The signed work done by the force.</returns>
    public static Energy FromMechanicalWork(Force forceAlongDisplacement, Length displacement)
    {
        return (Energy)(forceAlongDisplacement.Value * displacement.Value);
    }

    /// <summary>Calculates nonrelativistic translational kinetic energy.</summary>
    /// <param name="mass">The mass of the moving body.</param>
    /// <param name="speed">The magnitude of its velocity.</param>
    /// <returns>The translational kinetic energy.</returns>
    public static Energy FromTranslationalKineticEnergy(Mass mass, LinearVelocity speed)
    {
        return (Energy)(0.5d * mass.Value * speed.Value * speed.Value);
    }

    /// <summary>Calculates the kinetic energy of a rigid body rotating about a fixed axis.</summary>
    /// <param name="inertia">The moment of inertia about the rotation axis.</param>
    /// <param name="angularVelocity">The angular velocity about the same axis.</param>
    /// <returns>The rotational kinetic energy.</returns>
    public static Energy FromRotationalKineticEnergy(MomentOfInertia inertia, AngularVelocity angularVelocity)
    {
        return (Energy)(0.5d * inertia.Value * angularVelocity.Value * angularVelocity.Value);
    }

    /// <summary>Calculates the pressure–volume work done by a system at constant external pressure.</summary>
    /// <param name="externalPressure">The pressure opposing expansion.</param>
    /// <param name="volumeChange">The final volume minus the initial volume.</param>
    /// <returns>The work done by the system, positive for expansion at positive pressure.</returns>
    public static Energy FromConstantPressureWork(ForcePerArea externalPressure, Volume volumeChange)
    {
        return (Energy)(externalPressure.Value * volumeChange.Value);
    }

    /// <summary>Calculates the energy stored in a linear capacitor.</summary>
    /// <param name="capacitance">The capacitance of the capacitor.</param>
    /// <param name="voltage">The voltage across the capacitor.</param>
    /// <returns>The stored electrical energy.</returns>
    public static Energy FromCapacitor(Capacitance capacitance, ElectricPotential voltage)
    {
        return (capacitance * voltage) * voltage * 0.5d;
    }

    /// <summary>Calculates the elastic potential energy relative to equilibrium for a linear spring.</summary>
    public static Energy FromSpringDisplacement(ForcePerLength stiffness, Length displacement)
    {
        return FromMechanicalWork(stiffness * displacement, displacement) * 0.5d;
    }

    /// <summary>Calculates the magnetic energy stored in a linear inductor at the given current.</summary>
    public static Energy FromInductor(Inductance inductance, ElectricCurrent current)
    {
        return (Energy)(0.5d * MagneticFlux.LinkageFromInductor(inductance, current).Value * current.Value);
    }
}

public readonly partial struct Force
{
    /// <summary>Calculates the signed restoring force of a linear spring for a displacement from equilibrium.</summary>
    public static Force FromSpringDisplacement(ForcePerLength stiffness, Length displacement)
    {
        return -(stiffness * displacement);
    }

    /// <summary>Calculates the upward buoyant force in a fluid with uniform density and gravity over the displaced volume.</summary>
    public static Force FromBuoyancy(Density fluidDensity, Volume displacedVolume, LinearAcceleration gravity)
    {
        return (fluidDensity * displacedVolume) * gravity;
    }

    /// <summary>Calculates the magnetic force magnitude on a moving charge.</summary>
    /// <param name="charge">The signed electric charge.</param>
    /// <param name="speed">The speed of the charge.</param>
    /// <param name="perpendicularFluxDensity">The magnitude of the magnetic flux density component perpendicular to the velocity.</param>
    public static Force FromMovingChargeInMagneticField(
        ElectricCharge charge, LinearVelocity speed, MagneticFluxDensity perpendicularFluxDensity)
    {
        return (Force)(Math.Abs(charge.Value) * speed.Value * perpendicularFluxDensity.Value);
    }

    /// <summary>Calculates the magnetic force magnitude on a straight current-carrying segment in a uniform field.</summary>
    /// <param name="current">The signed electric current.</param>
    /// <param name="conductorLength">The length of the segment in the field.</param>
    /// <param name="perpendicularFluxDensity">The magnitude of the magnetic flux density component perpendicular to the segment.</param>
    public static Force FromCurrentInMagneticField(
        ElectricCurrent current, Length conductorLength, MagneticFluxDensity perpendicularFluxDensity)
    {
        return (Force)(Math.Abs(current.Value) * conductorLength.Value * perpendicularFluxDensity.Value);
    }
}

public readonly partial struct Torque
{
    /// <summary>Calculates the magnitude of a torque from the perpendicular lever arm.</summary>
    /// <param name="force">The magnitude of the force component perpendicular to the rotation axis.</param>
    /// <param name="perpendicularLeverArm">The distance from the axis to that component's line of action in the plane of rotation.</param>
    /// <returns>The torque magnitude.</returns>
    public static Torque FromPerpendicularLeverArm(Force force, Length perpendicularLeverArm)
    {
        return (Torque)(force.Value * perpendicularLeverArm.Value);
    }
}

public readonly partial struct ForcePerArea
{
    /// <summary>Calculates the dynamic pressure of a moving fluid.</summary>
    /// <param name="density">The fluid density at the point of interest.</param>
    /// <param name="speed">The fluid speed at the same point.</param>
    /// <returns>The pressure-valued kinetic term ½ρv².</returns>
    public static ForcePerArea FromDynamicPressure(Density density, LinearVelocity speed)
    {
        return (ForcePerArea)(0.5d * density.Value * speed.Value * speed.Value);
    }

    /// <summary>Calculates pressure from a force normal to a surface.</summary>
    /// <param name="normalForce">The normal component of the force, positive when compressive.</param>
    /// <param name="area">The surface area over which the force is applied.</param>
    /// <returns>The average pressure over the area.</returns>
    public static ForcePerArea FromNormalForce(Force normalForce, Area area)
    {
        return normalForce / area;
    }

    /// <summary>Calculates the pressure increase below a reference level in a static fluid of uniform density.</summary>
    /// <param name="density">The fluid density.</param>
    /// <param name="gravity">The gravitational acceleration magnitude, assumed constant over the depth.</param>
    /// <param name="depth">The vertical distance below the reference level.</param>
    /// <returns>The hydrostatic pressure increase relative to the reference level.</returns>
    public static ForcePerArea FromHydrostaticHead(Density density, LinearAcceleration gravity, Length depth)
    {
        return (ForcePerArea)(density.Value * gravity.Value * depth.Value);
    }

    /// <summary>Calculates the pressure difference corresponding to power transferred to a fluid flow.</summary>
    /// <param name="hydraulicPower">The power transferred through the pressure difference.</param>
    /// <param name="flow">The corresponding volumetric flow rate.</param>
    /// <returns>The pressure difference for the given power and flow.</returns>
    public static ForcePerArea FromHydraulicPower(Power hydraulicPower, VolumePerTime flow)
    {
        return hydraulicPower / flow;
    }

    /// <summary>Calculates static pressure along a streamline in steady, incompressible, inviscid flow.</summary>
    /// <param name="referencePressure">The static pressure at the reference point.</param>
    /// <param name="density">The constant fluid density.</param>
    /// <param name="referenceSpeed">The fluid speed at the reference point.</param>
    /// <param name="speed">The fluid speed at the result point on the same streamline.</param>
    /// <param name="gravity">The gravitational acceleration magnitude.</param>
    /// <param name="elevationChange">The elevation at the result point minus the reference elevation.</param>
    public static ForcePerArea FromBernoulli(
        ForcePerArea referencePressure, Density density, LinearVelocity referenceSpeed, LinearVelocity speed,
        LinearAcceleration gravity, Length elevationChange)
    {
        var dynamicPressureChange = FromDynamicPressure(density, referenceSpeed).Value
                                    - FromDynamicPressure(density, speed).Value;
        return (ForcePerArea)(referencePressure.Value + dynamicPressureChange
                             - density.Value * gravity.Value * elevationChange.Value);
    }
}

public readonly partial struct LinearVelocity
{
    /// <summary>Calculates wave speed from the wavelength and frequency of the same wave.</summary>
    public static LinearVelocity FromWavelength(Length wavelength, Frequency frequency)
    {
        return (LinearVelocity)(wavelength.Value * frequency.Value);
    }

    /// <summary>Calculates the speed of a Newtonian circular orbit about a spherical central body.</summary>
    /// <param name="gravitationalParameter">The central body's gravitational parameter.</param>
    /// <param name="radius">The positive distance from the body's centre to the orbit.</param>
    public static LinearVelocity FromCircularOrbit(StandardGravitationalParameter gravitationalParameter, Length radius)
    {
        return FromMetrePerSecond(Math.Sqrt(gravitationalParameter.Value / radius.Metre));
    }

    /// <summary>Calculates the Reynolds number for a flow using kinematic viscosity.</summary>
    /// <param name="characteristicLength">The representative length for the flow geometry.</param>
    /// <param name="kinematicViscosity">The fluid's kinematic viscosity at the flow conditions.</param>
    /// <returns>The Reynolds number for those conditions and that length scale.</returns>
    public double ReynoldsNumber(Length characteristicLength, KinematicViscosity kinematicViscosity)
    {
        return Value * characteristicLength.Value / kinematicViscosity.Value;
    }

    /// <summary>Calculates the Reynolds number for a flow using density and dynamic viscosity.</summary>
    /// <param name="characteristicLength">The representative length for the flow geometry.</param>
    /// <param name="density">The fluid density at the flow conditions.</param>
    /// <param name="dynamicViscosity">The fluid's dynamic viscosity at those same conditions.</param>
    /// <returns>The Reynolds number for those conditions and that length scale.</returns>
    public double ReynoldsNumber(Length characteristicLength, Density density, DynamicViscosity dynamicViscosity)
    {
        return ReynoldsNumber(characteristicLength, dynamicViscosity / density);
    }
}

public readonly partial struct LinearAcceleration
{
    /// <summary>Calculates the inward acceleration magnitude of circular motion from speed and positive radius.</summary>
    public static LinearAcceleration FromCentripetalMotion(LinearVelocity tangentialSpeed, Length radius)
    {
        return (LinearAcceleration)(tangentialSpeed.Value * tangentialSpeed.Value / radius.Value);
    }

    /// <summary>Calculates the inward acceleration magnitude of circular motion from angular velocity and positive radius.</summary>
    public static LinearAcceleration FromCentripetalMotion(AngularVelocity angularVelocity, Length radius)
    {
        return (LinearAcceleration)(angularVelocity.Value * angularVelocity.Value * radius.Value);
    }

    /// <summary>Calculates the acceleration magnitude due to the gravity of a spherical body at an external point.</summary>
    /// <param name="gravitationalParameter">The body's gravitational parameter.</param>
    /// <param name="distance">The positive distance from the centre of the body.</param>
    public static LinearAcceleration FromGravitationalParameter(
        StandardGravitationalParameter gravitationalParameter, Length distance)
    {
        return (LinearAcceleration)(gravitationalParameter.Value / (distance.Value * distance.Value));
    }
}

public readonly partial struct Time
{
    /// <summary>Calculates the period of a Newtonian circular orbit about a spherical central body.</summary>
    public static Time FromCircularOrbit(StandardGravitationalParameter gravitationalParameter, Length radius)
    {
        return Length.CircumferenceFromRadius(radius) / LinearVelocity.FromCircularOrbit(gravitationalParameter, radius);
    }
}

public readonly partial struct MagneticFlux
{
    /// <summary>Calculates the signed self-flux linkage summed over all turns of a linear inductor.</summary>
    public static MagneticFlux LinkageFromInductor(Inductance inductance, ElectricCurrent current)
    {
        return (MagneticFlux)(inductance.Value * current.Value);
    }
}

public readonly partial struct ElectricFieldStrength
{
    /// <summary>Calculates the average electric field component along a straight path.</summary>
    /// <param name="potentialChange">The final electric potential minus the initial electric potential.</param>
    /// <param name="distance">The length of the path.</param>
    /// <returns>The signed field component in the direction of the path.</returns>
    public static ElectricFieldStrength FromPotentialChange(ElectricPotential potentialChange, Length distance)
    {
        return -(potentialChange / distance);
    }
}

public readonly partial struct ElectromotiveForce
{
    /// <summary>Calculates the average induced electromotive force using Faraday's law.</summary>
    /// <param name="fluxLinkageChange">The signed final minus initial flux linkage summed over the winding's turns.</param>
    /// <param name="elapsed">The time over which the flux linkage changes.</param>
    /// <returns>The signed induced electromotive force with Lenz's-law polarity.</returns>
    public static ElectromotiveForce FromFluxLinkageChange(MagneticFlux fluxLinkageChange, Time elapsed)
    {
        return FromVolt(-fluxLinkageChange.Weber / elapsed.Second);
    }
}

public readonly partial struct Resistance
{
    /// <summary>Calculates the resistance along a uniform conductor.</summary>
    /// <param name="conductivity">The positive electrical conductivity of the conductor material.</param>
    /// <param name="length">The conductor length in the direction of current flow.</param>
    /// <param name="crossSection">The uniform cross-sectional area perpendicular to the current.</param>
    /// <returns>The electrical resistance along the conductor.</returns>
    public static Resistance FromUniformConductor(ElectricConductivity conductivity, Length length, Area crossSection)
    {
        return (Resistance)(length.Value / (conductivity.Value * crossSection.Value));
    }
}

public readonly partial struct Capacitance
{
    /// <summary>Calculates capacitance for parallel plates with a uniform dielectric.</summary>
    /// <param name="permittivity">The absolute permittivity of the dielectric between the plates.</param>
    /// <param name="overlap">The overlapping plate area.</param>
    /// <param name="separation">The distance between the plates.</param>
    /// <returns>The capacitance in the parallel-plate approximation with negligible fringing.</returns>
    public static Capacitance FromParallelPlates(Permittivity permittivity, Area overlap, Length separation)
    {
        return (Capacitance)(permittivity.Value * overlap.Value / separation.Value);
    }
}

public readonly partial struct ThermodynamicTemperature
{
    /// <summary>Returns this absolute temperature minus the reference temperature, in kelvins.</summary>
    public double DifferenceInKelvins(ThermodynamicTemperature reference)
    {
        return Kelvin - reference.Kelvin;
    }

    /// <summary>Returns the absolute temperature after adding the signed difference in kelvins.</summary>
    public ThermodynamicTemperature AddKelvins(double deltaKelvins)
    {
        return FromKelvin(Kelvin + deltaKelvins);
    }

    /// <summary>Calculates the average gradient from the start temperature to this point at a positive distance.</summary>
    /// <returns>The signed gradient in kelvins per metre.</returns>
    public double GradientInKelvinsPerMetre(ThermodynamicTemperature start, Length distance)
    {
        return DifferenceInKelvins(start) / distance.Metre;
    }
}

public readonly partial struct HeatCapacity
{
    /// <summary>Calculates sensible heat for a signed temperature change in kelvins at constant or average heat capacity.</summary>
    public Energy EnergyForTemperatureChange(double deltaKelvins)
    {
        return (Energy)(Value * deltaKelvins);
    }

    /// <summary>Calculates the average heat capacity from sensible heat and a nonzero temperature change in kelvins.</summary>
    public static HeatCapacity FromEnergyAndTemperatureChange(Energy energy, double deltaKelvins)
    {
        return (HeatCapacity)(energy.Value / deltaKelvins);
    }
}

public readonly partial struct CoefficientOfHeatTransfer
{
    /// <summary>Calculates heat flux from the signed driving temperature difference in kelvins.</summary>
    public HeatFlowRatePerArea HeatFluxForTemperatureChange(double deltaKelvins)
    {
        return (HeatFlowRatePerArea)(Value * deltaKelvins);
    }
}

public readonly partial struct ThermalResistance
{
    /// <summary>Calculates the signed heat-flow rate from the driving temperature difference in kelvins.</summary>
    public HeatFlowRate HeatFlowForTemperatureChange(double deltaKelvins)
    {
        return (HeatFlowRate)(deltaKelvins / Value);
    }

    /// <summary>Calculates the signed temperature drop in kelvins for the heat flow along this path.</summary>
    public double TemperatureDropInKelvins(HeatFlowRate heatFlow)
    {
        return Value * heatFlow.Value;
    }

    /// <summary>Calculates thermal resistance from the temperature drop in kelvins and corresponding nonzero heat flow.</summary>
    public static ThermalResistance FromTemperatureDrop(double deltaKelvins, HeatFlowRate heatFlow)
    {
        return (ThermalResistance)(deltaKelvins / heatFlow.Value);
    }

    /// <summary>Calculates thermal resistance from the positive transfer coefficient and area of the same surface.</summary>
    public static ThermalResistance FromHeatTransferCoefficient(CoefficientOfHeatTransfer coefficient, Area area)
    {
        return (ThermalResistance)(1d / (coefficient.Value * area.Value));
    }
}

public readonly partial struct ThermalInsulance
{
    /// <summary>Calculates the signed temperature drop in kelvins for the corresponding normal heat flux.</summary>
    public double TemperatureDropInKelvins(HeatFlowRatePerArea heatFlux)
    {
        return Value * heatFlux.Value;
    }
}

public readonly partial struct TemperaturePerTime
{
    /// <summary>Calculates the change in kelvins from a constant or average rate over the elapsed interval.</summary>
    public double ChangeInKelvins(Time elapsed)
    {
        return Value * elapsed.Value;
    }
}

public readonly partial struct LinearThermalExpansion
{
    /// <summary>Calculates the change in length from the whole object's expansion per kelvin and the signed temperature change.</summary>
    public Length LengthChange(double deltaKelvins)
    {
        return (Length)(Value * deltaKelvins);
    }
}

public readonly partial struct AreaThermalExpansion
{
    /// <summary>Calculates the change in area from the whole object's expansion per kelvin and the signed temperature change.</summary>
    public Area AreaChange(double deltaKelvins)
    {
        return (Area)(Value * deltaKelvins);
    }
}

public readonly partial struct VolumeThermalExpansion
{
    /// <summary>Calculates the change in volume from the whole object's expansion per kelvin and the signed temperature change.</summary>
    public Volume VolumeChange(double deltaKelvins)
    {
        return (Volume)(Value * deltaKelvins);
    }
}

public readonly partial struct HeatFlowRatePerArea
{
    /// <summary>Calculates the conductive heat-flux component along an axis using Fourier's law.</summary>
    /// <param name="conductivity">The scalar thermal conductivity of the material.</param>
    /// <param name="start">The absolute temperature at the start of the interval.</param>
    /// <param name="end">The absolute temperature at the end of the interval.</param>
    /// <param name="distance">The positive distance from start to end along the axis.</param>
    /// <returns>The signed heat flux along that axis, directed down the temperature gradient.</returns>
    public static HeatFlowRatePerArea FromConduction(
        ThermalConductivity conductivity, ThermodynamicTemperature start, ThermodynamicTemperature end, Length distance)
    {
        return (HeatFlowRatePerArea)(-conductivity.Value * end.GradientInKelvinsPerMetre(start, distance));
    }
}

public readonly partial struct Illuminance
{
    /// <summary>Calculates illuminance on a surface perpendicular to light from a point source.</summary>
    /// <param name="intensity">The luminous intensity of the source towards the surface.</param>
    /// <param name="distance">The distance from the source to the surface.</param>
    /// <returns>The illuminance on the surface.</returns>
    public static Illuminance FromNormalPointSource(LuminousIntensity intensity, Length distance)
    {
        return FromLux(intensity.Candela / (distance * distance).SquareMetre);
    }
}

public readonly partial struct PowerPerArea
{
    /// <summary>Calculates irradiance for normal incidence from a point source at a positive distance.</summary>
    /// <param name="intensity">The radiant intensity in the direction of the receiving surface.</param>
    /// <param name="distance">The distance from the point source to the surface.</param>
    public static PowerPerArea FromNormalPointSource(RadiantIntensity intensity, Length distance)
    {
        return (PowerPerArea)(intensity.Value / (distance * distance).Value);
    }
}

public readonly partial struct AbsorbedDose
{
    /// <summary>Calculates the mean absorbed dose from ionizing radiation energy deposited in matter.</summary>
    /// <param name="depositedEnergy">The total energy imparted by ionizing radiation to the specified matter.</param>
    /// <param name="mass">The mass of that same matter.</param>
    /// <returns>The mean absorbed dose for the specified matter.</returns>
    public static AbsorbedDose FromDepositedEnergy(Energy depositedEnergy, Mass mass)
    {
        return (AbsorbedDose)(depositedEnergy / mass).Value;
    }
}
