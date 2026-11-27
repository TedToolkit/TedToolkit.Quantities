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
    [QuantityFormula("mechanics/period-and-frequency", Equation = "f = 1 / T",
        Applicability = "Period of one cycle of a periodic phenomenon.",
        Sources = new[] { FormulaSources.Period })]
    public static Frequency FromPeriod(Time period)
    {
        return FromHertz(1d / period.Second);
    }

    /// <summary>Calculates the duration of one cycle at this frequency.</summary>
    /// <returns>The period of the phenomenon.</returns>
    [QuantityFormula("mechanics/period-and-frequency")]
    public Time ToPeriod()
    {
        return Time.FromSecond(1d / Hertz);
    }
}

public readonly partial struct AngularVelocity
{
    /// <summary>Calculates the angular velocity of a motion with one revolution per cycle.</summary>
    /// <param name="frequency">The number of revolutions per unit time.</param>
    /// <returns>The corresponding angular velocity.</returns>
    [QuantityFormula("mechanics/angular-frequency", Equation = "ω = 2πf",
        Applicability = "One revolution per cycle.",
        Sources = new[] { FormulaSources.Arc })]
    public static AngularVelocity FromFrequency(Frequency frequency)
    {
        return FromRadianPerSecond(PlaneAngle.FromRevolution(1d).Radian * frequency.Hertz);
    }

    /// <summary>Calculates the rotation frequency with one cycle per revolution.</summary>
    /// <returns>The frequency corresponding to this angular velocity.</returns>
    [QuantityFormula("mechanics/angular-frequency")]
    public Frequency ToFrequency()
    {
        return Frequency.FromHertz(RadianPerSecond / PlaneAngle.FromRevolution(1d).Radian);
    }
}

public readonly partial struct PlaneAngle
{
    /// <summary>Calculates the central angle subtended by a circular arc.</summary>
    /// <param name="arcLength">The arc's signed length.</param>
    /// <param name="radius">The circle's positive radius.</param>
    /// <returns>The central angle.</returns>
    [QuantityFormula("geometry/circular-arc", Equation = "s = rθ",
        Applicability = "Circular arc with a positive radius. A signed central angle gives a signed arc length.",
        Sources = new[] { FormulaSources.Arc })]
    public static PlaneAngle FromArc(Length arcLength, Length radius)
    {
        return FromRadian((arcLength / radius).Value);
    }

    /// <summary>Calculates the length of the circular arc subtended by this central angle.</summary>
    /// <param name="radius">The circle's positive radius.</param>
    /// <returns>The signed arc length.</returns>
    [QuantityFormula("geometry/circular-arc")]
    public Length ToArcLength(Length radius)
    {
        return radius * Radian;
    }
}

public readonly partial struct Radius
{
    /// <summary>Calculates a circle's radius from its diameter.</summary>
    /// <param name="diameter">The diameter of the same circle.</param>
    /// <returns>The radius, equal to half the diameter.</returns>
    [QuantityFormula("geometry/circle-diameter", Equation = "d = 2r",
        Applicability = "The radius and diameter refer to the same circle.",
        Sources = new[] { FormulaSources.Geometry })]
    public static Radius FromDiameter(Diameter diameter)
    {
        return (Radius)(diameter.Value / 2d);
    }

    /// <summary>Calculates the circumference of a circle with this radius.</summary>
    /// <returns>The length around the circle.</returns>
    [QuantityFormula("geometry/circle-circumference", Equation = "C = 2πr",
        Applicability = "Circumference of a circle.",
        Sources = new[] { FormulaSources.Geometry })]
    public Length ToCircumference()
    {
        return (Length)(2d * Math.PI * Value);
    }
}

public readonly partial struct Diameter
{
    /// <summary>Calculates a circle's diameter from its radius.</summary>
    /// <param name="radius">The radius of the same circle.</param>
    /// <returns>The diameter, equal to twice the radius.</returns>
    [QuantityFormula("geometry/circle-diameter")]
    public static Diameter FromRadius(Radius radius)
    {
        return (Diameter)(2d * radius.Value);
    }
}

public readonly partial struct Area
{
    /// <summary>Calculates the area enclosed by a circle.</summary>
    /// <param name="radius">The radius of the circle.</param>
    /// <returns>The circle's area.</returns>
    [QuantityFormula("geometry/circle-area", Equation = "A = πr²",
        Applicability = "Area enclosed by a circle.",
        Sources = new[] { FormulaSources.CircleArea })]
    public static Area FromCircle(Radius radius)
    {
        return (Area)(Math.PI * radius.Value * radius.Value);
    }
}

public readonly partial struct Impulse
{
    /// <summary>Calculates the impulse component from an average force and its duration.</summary>
    /// <param name="averageForce">The signed force component averaged over the interval.</param>
    /// <param name="duration">The duration of that same interval.</param>
    /// <returns>The impulse component along the same axis.</returns>
    [QuantityFormula("mechanics/impulse-and-momentum-change")]
    public static Impulse FromAverageForce(Force averageForce, Time duration)
    {
        return (Impulse)(averageForce * duration);
    }
}

public readonly partial struct Energy
{
    /// <summary>Calculates work done by a constant force over a displacement.</summary>
    /// <param name="forceAlongDisplacement">The signed force component along the direction of displacement.</param>
    /// <param name="displacement">The distance moved in that direction.</param>
    /// <returns>The signed work done by the force.</returns>
    [QuantityFormula("mechanics/mechanical-work", Equation = "W = F∥Δx",
        Applicability = "Constant force component along the displacement.",
        Sources = new[] { FormulaSources.Work })]
    public static Energy FromMechanicalWork(Force forceAlongDisplacement, Length displacement)
    {
        return (Energy)(forceAlongDisplacement.Value * displacement.Value);
    }

    /// <summary>Calculates nonrelativistic translational kinetic energy.</summary>
    /// <param name="mass">The mass of the moving body.</param>
    /// <param name="speed">The magnitude of its velocity.</param>
    /// <returns>The translational kinetic energy.</returns>
    [QuantityFormula("mechanics/translational-kinetic-energy", Equation = "K = ½mv²",
        Applicability = "Nonrelativistic translation.",
        Sources = new[] { FormulaSources.Work })]
    public static Energy FromTranslationalKineticEnergy(Mass mass, LinearVelocity speed)
    {
        return (Energy)(0.5d * mass.Value * speed.Value * speed.Value);
    }

    /// <summary>Calculates the kinetic energy of a rigid body rotating about a fixed axis.</summary>
    /// <param name="inertia">The moment of inertia about the rotation axis.</param>
    /// <param name="angularVelocity">The angular velocity about the same axis.</param>
    /// <returns>The rotational kinetic energy.</returns>
    [QuantityFormula("mechanics/rotational-kinetic-energy", Equation = "K = ½Iω²",
        Applicability = "Rigid body rotating about a fixed axis.",
        Sources = new[] { FormulaSources.Rotation })]
    public static Energy FromRotationalKineticEnergy(MomentOfInertia inertia, AngularVelocity angularVelocity)
    {
        return (Energy)(0.5d * inertia.Value * angularVelocity.Value * angularVelocity.Value);
    }

    /// <summary>Calculates the pressure–volume work done by a system at constant external pressure.</summary>
    /// <param name="externalPressure">The pressure opposing expansion.</param>
    /// <param name="volumeChange">The final volume minus the initial volume.</param>
    /// <returns>The work done by the system, positive for expansion at positive pressure.</returns>
    [QuantityFormula("fluids/pressure-volume-work", Equation = "W = pₑₓₜΔV",
        Applicability = "Constant external pressure. Expansion gives positive work by the system.",
        Sources = new[] { FormulaSources.Pv })]
    public static Energy FromConstantPressureWork(Pressure externalPressure, Volume volumeChange)
    {
        return (Energy)(externalPressure.Value * volumeChange.Value);
    }

    /// <summary>Calculates the energy stored in a linear capacitor.</summary>
    /// <param name="capacitance">The capacitance of the capacitor.</param>
    /// <param name="voltage">The voltage across the capacitor.</param>
    /// <returns>The stored electrical energy.</returns>
    [QuantityFormula("electromagnetism/capacitor-energy", Equation = "U = ½CV²",
        Applicability = "Energy stored in a linear capacitor.",
        Sources = new[] { FormulaSources.Capacitance })]
    public static Energy FromCapacitor(Capacitance capacitance, ElectricPotential voltage)
    {
        return (capacitance * voltage) * voltage * 0.5d;
    }
}

public readonly partial struct Torque
{
    /// <summary>Calculates the magnitude of a torque from the perpendicular lever arm.</summary>
    /// <param name="force">The magnitude of the force component perpendicular to the rotation axis.</param>
    /// <param name="perpendicularLeverArm">The distance from the axis to that component's line of action in the plane of rotation.</param>
    /// <returns>The torque magnitude.</returns>
    [QuantityFormula("mechanics/lever-arm-torque", Equation = "τ = F⊥r⊥",
        Applicability = "Use the force component in the rotation plane and its perpendicular lever arm.",
        Sources = new[] { FormulaSources.Rotation })]
    public static Torque FromPerpendicularLeverArm(Force force, Length perpendicularLeverArm)
    {
        return (Torque)(force.Value * perpendicularLeverArm.Value);
    }
}

public readonly partial struct Pressure
{
    /// <summary>Calculates the dynamic pressure of a moving fluid.</summary>
    /// <param name="density">The fluid density at the point of interest.</param>
    /// <param name="speed">The fluid speed at the same point.</param>
    /// <returns>The pressure-valued kinetic term ½ρv².</returns>
    [QuantityFormula("fluids/dynamic-pressure", Equation = "q = ½ρv²",
        Applicability = "Use local density and speed.",
        Sources = new[] { FormulaSources.Dynamic })]
    public static Pressure FromDynamicPressure(Density density, LinearVelocity speed)
    {
        return (Pressure)(0.5d * density.Value * speed.Value * speed.Value);
    }

    /// <summary>Calculates pressure from a force normal to a surface.</summary>
    /// <param name="normalForce">The normal component of the force, positive when compressive.</param>
    /// <param name="area">The surface area over which the force is applied.</param>
    /// <returns>The average pressure over the area.</returns>
    [QuantityFormula("fluids/pressure-from-force")]
    public static Pressure FromNormalForce(Force normalForce, Area area)
    {
        return (Pressure)(normalForce / area).Value;
    }

    /// <summary>Calculates the pressure increase below a reference level in a fluid of uniform density.</summary>
    /// <param name="density">The fluid density.</param>
    /// <param name="gravity">The magnitude of the local gravitational acceleration.</param>
    /// <param name="depth">The vertical distance below the reference level.</param>
    /// <returns>The hydrostatic pressure increase relative to the reference level.</returns>
    [QuantityFormula("fluids/hydrostatic-pressure", Equation = "Δp = ρgh",
        Applicability = "Uniform-density fluid at rest below a reference level.",
        Sources = new[] { FormulaSources.Fluids })]
    public static Pressure FromHydrostaticHead(Density density, LinearAcceleration gravity, Length depth)
    {
        return (Pressure)(density.Value * gravity.Value * depth.Value);
    }

    /// <summary>Calculates the pressure difference corresponding to power transferred to a fluid flow.</summary>
    /// <param name="hydraulicPower">The power transferred through the pressure difference.</param>
    /// <param name="flow">The corresponding volumetric flow rate.</param>
    /// <returns>The pressure difference for the given power and flow.</returns>
    [QuantityFormula("fluids/hydraulic-power")]
    public static Pressure FromHydraulicPower(Power hydraulicPower, VolumePerTime flow)
    {
        return (Pressure)(hydraulicPower / flow).Value;
    }
}

public readonly partial struct ReynoldsNumber
{
    /// <summary>Calculates the Reynolds number for a flow using kinematic viscosity.</summary>
    /// <param name="speed">The characteristic speed of the flow.</param>
    /// <param name="characteristicLength">The representative length for the flow geometry.</param>
    /// <param name="kinematicViscosity">The fluid's kinematic viscosity at the flow conditions.</param>
    /// <returns>The Reynolds number for those conditions and that length scale.</returns>
    [QuantityFormula("fluids/reynolds-number", Equation = "Re = vℓ / ν = ρvℓ / μ",
        Applicability = "Choose the characteristic length for the flow and viscosity at that state.",
        Sources = new[] { FormulaSources.Reynolds })]
    public static ReynoldsNumber FromFlow(
        LinearVelocity speed, Length characteristicLength, KinematicViscosity kinematicViscosity)
    {
        return (ReynoldsNumber)(speed.Value * characteristicLength.Value / kinematicViscosity.Value);
    }

    /// <summary>Calculates the Reynolds number for a flow using density and dynamic viscosity.</summary>
    /// <param name="speed">The characteristic speed of the flow.</param>
    /// <param name="characteristicLength">The representative length for the flow geometry.</param>
    /// <param name="density">The fluid density at the flow conditions.</param>
    /// <param name="dynamicViscosity">The fluid's dynamic viscosity at those same conditions.</param>
    /// <returns>The Reynolds number for those conditions and that length scale.</returns>
    [QuantityFormula("fluids/reynolds-number")]
    public static ReynoldsNumber FromFlow(
        LinearVelocity speed, Length characteristicLength, Density density, DynamicViscosity dynamicViscosity)
    {
        return FromFlow(speed, characteristicLength, dynamicViscosity / density);
    }
}

public readonly partial struct ElectricFieldStrength
{
    /// <summary>Calculates the average electric field component along a straight path.</summary>
    /// <param name="potentialChange">The final electric potential minus the initial electric potential.</param>
    /// <param name="distance">The length of the path.</param>
    /// <returns>The signed field component in the direction of the path.</returns>
    [QuantityFormula("electromagnetism/electric-field-and-voltage")]
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
    [QuantityFormula("electromagnetism/faraday-induction", Equation = "ℰ = −ΔΛ / Δt",
        Applicability = "Signed final-minus-initial flux linkage summed over all turns.",
        Sources = new[] { FormulaSources.Induction, FormulaSources.Inductance })]
    public static ElectromotiveForce FromFluxLinkageChange(MagneticFlux fluxLinkageChange, Time elapsed)
    {
        return FromVolt(-fluxLinkageChange.Weber / elapsed.Second);
    }
}

public readonly partial struct Resistance
{
    /// <summary>Calculates the resistance along a uniform conductor.</summary>
    /// <param name="resistivity">The resistivity of the conductor material.</param>
    /// <param name="length">The conductor length in the direction of current flow.</param>
    /// <param name="crossSection">The uniform cross-sectional area perpendicular to the current.</param>
    /// <returns>The electrical resistance along the conductor.</returns>
    [QuantityFormula("electromagnetism/uniform-conductor-resistance", Equation = "R = ρₑℓ / A",
        Applicability = "Uniform conductor with constant cross section and material resistivity.",
        Sources = new[] { FormulaSources.Resistance })]
    public static Resistance FromUniformConductor(Resistivity resistivity, Length length, Area crossSection)
    {
        return (Resistance)(resistivity.Value * length.Value / crossSection.Value);
    }
}

public readonly partial struct Capacitance
{
    /// <summary>Calculates capacitance for parallel plates with a uniform dielectric.</summary>
    /// <param name="permittivity">The absolute permittivity of the dielectric between the plates.</param>
    /// <param name="overlap">The overlapping plate area.</param>
    /// <param name="separation">The distance between the plates.</param>
    /// <returns>The capacitance in the parallel-plate approximation with negligible fringing.</returns>
    [QuantityFormula("electromagnetism/parallel-plate-capacitance", Equation = "C = εA / d",
        Applicability = "Parallel plates with uniform dielectric, using overlap area and negligible fringing.",
        Sources = new[] { FormulaSources.Capacitance })]
    public static Capacitance FromParallelPlates(Permittivity permittivity, Area overlap, Length separation)
    {
        return (Capacitance)(permittivity.Value * overlap.Value / separation.Value);
    }
}

public readonly partial struct HeatFlowRatePerArea
{
    /// <summary>Calculates the conductive heat-flux component along an axis using Fourier's law.</summary>
    /// <param name="conductivity">The scalar thermal conductivity of the material.</param>
    /// <param name="temperatureGradient">The signed temperature gradient along the axis.</param>
    /// <returns>The signed heat flux along that axis, directed down the temperature gradient.</returns>
    [QuantityFormula("thermal/fourier-conduction", Equation = "qₓ″ = −k∂T/∂x",
        Applicability = "Signed temperature gradient along the axis with scalar conductivity.",
        Sources = new[] { FormulaSources.Thermal, FormulaSources.HeatFlow })]
    public static HeatFlowRatePerArea FromConduction(
        ThermalConductivity conductivity, TemperatureGradient temperatureGradient)
    {
        return (HeatFlowRatePerArea)(-conductivity.Value * temperatureGradient.Value);
    }
}

public readonly partial struct Illuminance
{
    /// <summary>Calculates illuminance on a surface perpendicular to light from a point source.</summary>
    /// <param name="intensity">The luminous intensity of the source towards the surface.</param>
    /// <param name="distance">The distance from the source to the surface.</param>
    /// <returns>The illuminance on the surface.</returns>
    [QuantityFormula("optics/point-source-illuminance", Equation = "Eᵥ = Iᵥ / r²",
        Applicability = "Point source and receiving surface perpendicular to the incident ray.",
        Sources = new[] { FormulaSources.PointSource })]
    public static Illuminance FromNormalPointSource(LuminousIntensity intensity, Length distance)
    {
        return FromLux(intensity.Candela / (distance * distance).SquareMetre);
    }
}

public readonly partial struct AbsorbedDose
{
    /// <summary>Calculates the mean absorbed dose from ionizing radiation energy deposited in matter.</summary>
    /// <param name="depositedEnergy">The total energy imparted by ionizing radiation to the specified matter.</param>
    /// <param name="mass">The mass of that same matter.</param>
    /// <returns>The mean absorbed dose for the specified matter.</returns>
    [QuantityFormula("chemistry/absorbed-dose")]
    public static AbsorbedDose FromDepositedEnergy(Energy depositedEnergy, Mass mass)
    {
        return (AbsorbedDose)(depositedEnergy / mass).Value;
    }
}
