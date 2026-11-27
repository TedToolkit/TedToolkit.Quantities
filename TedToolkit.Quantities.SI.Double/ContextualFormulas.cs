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

    /// <summary>Calculates the beat frequency produced by two nearby frequencies.</summary>
    public static Frequency BeatFrequency(Frequency first, Frequency second)
    {
        return FromHertz(Math.Abs(first.Hertz - second.Hertz));
    }

    /// <summary>Calculates a standing-wave frequency with identical end conditions and positive mode number.</summary>
    /// <remarks>Use a string fixed at both ends or an ideal pipe open at both ends or closed at both ends.</remarks>
    public static Frequency FromStandingWave(LinearVelocity waveSpeed, Length resonatorLength, int mode)
    {
        return FromHertz((0.5d * mode) * waveSpeed.MetrePerSecond / resonatorLength.Metre);
    }

    /// <summary>Calculates the frequency of a positive mode number in an ideal pipe open at one end and closed at the other.</summary>
    public static Frequency FromQuarterWaveResonance(LinearVelocity soundSpeed, Length pipeLength, int mode)
    {
        return FromHertz((0.5d * mode - 0.25d) * soundSpeed.MetrePerSecond / pipeLength.Metre);
    }

    /// <summary>Calculates the classical Doppler shift of this emitted frequency for collinear sound propagation.</summary>
    /// <param name="soundSpeed">The positive sound speed in the uniform medium.</param>
    /// <param name="sourceVelocityTowardObserver">The signed source velocity relative to the medium, positive toward the observer.</param>
    /// <param name="observerVelocityTowardSource">The signed observer velocity relative to the medium, positive toward the source.</param>
    /// <remarks>Both speeds must be below the sound speed.</remarks>
    public Frequency ObservedWithDopplerShift(
        LinearVelocity soundSpeed, LinearVelocity sourceVelocityTowardObserver, LinearVelocity observerVelocityTowardSource)
    {
        var speed = soundSpeed.Value;
        return this * ((speed + observerVelocityTowardSource.Value) / (speed - sourceVelocityTowardObserver.Value));
    }

    /// <summary>Calculates the observed frequency of this light for collinear relative motion in vacuum.</summary>
    /// <param name="recessionVelocity">The signed subluminal relative velocity, positive for recession.</param>
    /// <param name="vacuumLightSpeed">The positive vacuum speed of light.</param>
    public Frequency ObservedWithRelativisticDopplerShift(LinearVelocity recessionVelocity, LinearVelocity vacuumLightSpeed)
    {
        var beta = recessionVelocity.Value / vacuumLightSpeed.Value;
        return this * Math.Sqrt((1d - beta) / (1d + beta));
    }

    /// <summary>Calculates the half-power cutoff frequency of a first-order RC or RL filter with positive time constant.</summary>
    public static Frequency FromFirstOrderCutoff(Time timeConstant)
    {
        return AngularVelocity.FromRadianPerSecond(1d / timeConstant.Second).ToFrequency();
    }

    /// <summary>Calculates the Strouhal number from this oscillation frequency, a characteristic flow length, and a positive reference speed.</summary>
    public double StrouhalNumber(Length characteristicLength, LinearVelocity referenceSpeed)
    {
        return Value * characteristicLength.Value / referenceSpeed.Value;
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

    /// <summary>Calculates angular frequency for a linear mass–spring system with positive subcritical viscous damping.</summary>
    public static AngularVelocity FromUnderdampedSpring(Mass mass, ForcePerLength stiffness, MassPerTime dampingCoefficient)
    {
        var decayRate = 0.5d * dampingCoefficient.Value / mass.Value;
        return FromRadianPerSecond(Math.Sqrt(stiffness.Value / mass.Value - decayRate * decayRate));
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

    /// <summary>Calculates signed angular displacement about a fixed axis under constant angular acceleration.</summary>
    public static PlaneAngle FromConstantAngularAcceleration(
        AngularVelocity initialAngularVelocity, AngularAcceleration acceleration, Time elapsed)
    {
        return FromRadian((initialAngularVelocity.Value + 0.5d * acceleration.Value * elapsed.Value) * elapsed.Value);
    }

    /// <summary>Calculates the transmitted ray angle for refraction between media with positive refractive indices.</summary>
    /// <param name="incidence">The signed incident angle from the interface normal, between −π/2 and π/2 radians.</param>
    /// <param name="incidentIndex">The refractive index of the incident medium at this wavelength.</param>
    /// <param name="transmittedIndex">The refractive index of the transmitted medium at this wavelength.</param>
    /// <param name="refraction">The signed transmitted angle, or the default angle on failure.</param>
    /// <returns>True if Snell's law admits a propagating transmitted ray.</returns>
    public static bool TryFromRefraction(
        PlaneAngle incidence, double incidentIndex, double transmittedIndex, out PlaneAngle refraction)
    {
        var sine = (incidentIndex / transmittedIndex) * Math.Sin(incidence.Radian);
        if (!(Math.Abs(sine) <= 1d))
        {
            refraction = default;
            return false;
        }

        refraction = FromRadian(Math.Asin(sine));
        return true;
    }

    /// <summary>Calculates the critical incidence angle for light passing from a higher to a lower positive refractive index.</summary>
    public static PlaneAngle CriticalAngle(double incidentIndex, double transmittedIndex)
    {
        return FromRadian(Math.Asin(transmittedIndex / incidentIndex));
    }

    /// <summary>Calculates the Brewster incidence angle at a lossless interface between nonmagnetic media.</summary>
    public static PlaneAngle BrewsterAngle(double incidentIndex, double transmittedIndex)
    {
        return FromRadian(Math.Atan(transmittedIndex / incidentIndex));
    }

    /// <summary>Calculates the signed phase difference from optical path difference and positive vacuum wavelength.</summary>
    public static PlaneAngle FromOpticalPathDifference(Length pathDifference, Length vacuumWavelength)
    {
        return FromRevolution(pathDifference / vacuumWavelength);
    }

    /// <summary>Calculates a single-slit diffraction minimum at positive order in the far field.</summary>
    /// <remarks>The light is normally incident and the order times wavelength cannot exceed the slit width.</remarks>
    public static PlaneAngle FromSingleSlitMinimum(Length wavelength, Length slitWidth, int order)
    {
        return FromRadian(Math.Asin(order * (wavelength / slitWidth)));
    }

    /// <summary>Calculates the approximate Rayleigh angular resolution of a circular aperture in the small-angle regime.</summary>
    public static PlaneAngle FromRayleighCriterion(Length wavelength, Length apertureDiameter)
    {
        return FromRadian(1.22d * (wavelength / apertureDiameter));
    }

    /// <summary>Calculates signed elastic twist under constant torque along a uniform solid circular shaft.</summary>
    public static PlaneAngle TwistOfSolidCircularShaft(Torque torque, ForcePerArea shearModulus, Length radius, Length shaftLength)
    {
        var sectionArea = Area.FromCircle(radius).Value;
        var polarAreaMoment = 0.5d * sectionArea * (radius * radius).Value;
        return FromRadian(torque.Value * shaftLength.Value / (shearModulus.Value * polarAreaMoment));
    }
}

public readonly partial struct SolidAngle
{
    /// <summary>Calculates the solid angle subtended at the centre by a patch on a sphere of positive radius.</summary>
    public static SolidAngle FromSphericalPatch(Area patchArea, Length radius)
    {
        return FromSteradian(patchArea.Value / (radius * radius).Value);
    }

    /// <summary>Calculates the area cut out by this solid angle on a sphere of positive radius.</summary>
    public Area ToSphericalPatchArea(Length radius)
    {
        return (radius * radius) * Steradian;
    }

    /// <summary>Calculates the solid angle inside a circular cone whose half-angle lies between zero and π radians.</summary>
    public static SolidAngle FromCone(PlaneAngle halfAngle)
    {
        var sine = Math.Sin(0.5d * halfAngle.Radian);
        return FromSteradian((4d * Math.PI) * sine * sine);
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

    /// <summary>Calculates signed displacement from initial velocity and constant acceleration along the same axis.</summary>
    public static Length FromConstantAcceleration(LinearVelocity initialVelocity, LinearAcceleration acceleration, Time elapsed)
    {
        return (Length)((initialVelocity.Value + 0.5d * acceleration.Value * elapsed.Value) * elapsed.Value);
    }

    /// <summary>Calculates hydraulic diameter from flow area and positive wetted perimeter.</summary>
    public static Length HydraulicDiameter(Area flowArea, Length wettedPerimeter)
    {
        return 4d * (flowArea / wettedPerimeter);
    }

    /// <summary>Calculates the signed equilibrium rise of a liquid in a vertical circular capillary tube.</summary>
    /// <remarks>Use the liquid density when the gas density is negligible and measure the contact angle through the liquid.</remarks>
    public static Length CapillaryRise(
        ForcePerLength surfaceTension, Density liquidDensity, LinearAcceleration gravity, Length tubeRadius, PlaneAngle contactAngle)
    {
        return (Length)(2d * surfaceTension.Value * Math.Cos(contactAngle.Radian)
                        / (liquidDensity.Value * gravity.Value * tubeRadius.Value));
    }

    /// <summary>Calculates paraxial focal length from signed object and image distances.</summary>
    public static Length FocalLengthFromConjugates(Length objectDistance, Length imageDistance)
    {
        return FromMetre(1d / (1d / objectDistance.Metre + 1d / imageDistance.Metre));
    }

    /// <summary>Calculates signed paraxial image distance from focal length and signed object distance for a thin lens.</summary>
    public static Length ImageDistanceFromThinLens(Length focalLength, Length objectDistance)
    {
        return FromMetre(1d / (1d / focalLength.Metre - 1d / objectDistance.Metre));
    }

    /// <summary>Calculates signed lateral magnification from paraxial object and image distances.</summary>
    public static double LateralMagnification(Length objectDistance, Length imageDistance)
    {
        return -(imageDistance / objectDistance);
    }

    /// <summary>Calculates paraxial focal length of a thin lens with the same medium on both sides.</summary>
    /// <remarks>Use signed radii, positive where the centre of curvature is downstream. A planar surface has infinite radius.</remarks>
    public static Length FocalLengthFromLensMaker(
        double lensIndex, double surroundingIndex, Length firstRadius, Length secondRadius)
    {
        var curvatureDifference = 1d / firstRadius.Metre - 1d / secondRadius.Metre;
        return FromMetre(1d / ((lensIndex / surroundingIndex - 1d) * curvatureDifference));
    }

    /// <summary>Calculates wavelength from a nonzero diffraction grating order with normal incidence.</summary>
    public static Length WavelengthFromDiffractionGrating(Length grooveSpacing, PlaneAngle angle, int order)
    {
        return grooveSpacing * (Math.Sin(angle.Radian) / order);
    }

    /// <summary>Calculates wavelength from a Bragg reflection at positive order and glancing angle measured from the lattice planes.</summary>
    public static Length WavelengthFromBraggDiffraction(Length latticeSpacing, PlaneAngle glancingAngle, int order)
    {
        return latticeSpacing * (2d * Math.Sin(glancingAngle.Radian) / order);
    }

    /// <summary>Calculates paraxial double-slit fringe spacing with normally incident coherent light.</summary>
    public static Length FringeSpacingForDoubleSlit(Length wavelength, Length screenDistance, Length slitSeparation)
    {
        return screenDistance * (wavelength / slitSeparation);
    }

    /// <summary>Calculates the half-value thickness for narrow-beam exponential attenuation at a fixed photon energy.</summary>
    public static Length HalfValueThickness(InverseLength linearAttenuationCoefficient)
    {
        return Math.Log(2d) / linearAttenuationCoefficient;
    }

    /// <summary>Calculates signed engineering strain from this final length and a positive original gauge length.</summary>
    public double EngineeringStrainFrom(Length originalLength)
    {
        return (Value - originalLength.Value) / originalLength.Value;
    }

    /// <summary>Calculates the length measured along motion when this proper length moves at the given relative velocity.</summary>
    public Length ContractedAlongMotion(LinearVelocity relativeVelocity, LinearVelocity vacuumLightSpeed)
    {
        return this / relativeVelocity.LorentzFactor(vacuumLightSpeed);
    }

    /// <summary>Calculates the wavelength increase for Compton scattering from an initially stationary electron.</summary>
    public static Length ComptonWavelengthShift(Length electronComptonWavelength, PlaneAngle scatteringAngle)
    {
        var sine = Math.Sin(0.5d * scatteringAngle.Radian);
        return electronComptonWavelength * (2d * sine * sine);
    }

    /// <summary>Calculates the signed fractional shift of this observed wavelength from a positive emitted wavelength.</summary>
    public double RedshiftFrom(Length emittedWavelength)
    {
        return (Value - emittedWavelength.Value) / emittedWavelength.Value;
    }

    /// <summary>Calculates the amplitude envelope of an underdamped linear spring with viscous damping.</summary>
    /// <remarks>Use this initial amplitude, positive mass and damping coefficient, and nonnegative elapsed time.</remarks>
    public Length AfterViscousDamping(Mass mass, MassPerTime dampingCoefficient, Time elapsed)
    {
        var decayRate = 0.5d * dampingCoefficient.Value / mass.Value;
        return this * Math.Exp(-decayRate * elapsed.Value);
    }

    /// <summary>Calculates the capillary length of an interface between two fluids of different density under uniform gravity.</summary>
    public static Length CapillaryLength(
        ForcePerLength surfaceTension, Density heavierFluid, Density lighterFluid, LinearAcceleration gravity)
    {
        var densityDifference = heavierFluid.Value - lighterFluid.Value;
        return FromMetre(Math.Sqrt(surfaceTension.Value / (densityDifference * gravity.Value)));
    }

    /// <summary>Calculates the Bond number for this characteristic length at a fluid interface.</summary>
    public double BondNumber(ForcePerLength surfaceTension, Density heavierFluid, Density lighterFluid, LinearAcceleration gravity)
    {
        var scaleRatio = this / CapillaryLength(surfaceTension, heavierFluid, lighterFluid, gravity);
        return scaleRatio * scaleRatio;
    }

    /// <summary>Calculates the constant scale height of a hydrostatic, isothermal ideal gas under uniform gravity.</summary>
    public static Length IsothermalScaleHeight(ForcePerArea absolutePressure, Density density, LinearAcceleration gravity)
    {
        return FromMetre(absolutePressure.Value / (density.Value * gravity.Value));
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

    /// <summary>Calculates the surface area of a sphere of positive radius.</summary>
    public static Area FromSphere(Length radius)
    {
        return 4d * FromCircle(radius);
    }
}

public readonly partial struct Volume
{
    /// <summary>Calculates signed volumetric strain from this final volume and a positive original volume.</summary>
    public double VolumetricStrainFrom(Volume originalVolume)
    {
        return (Value - originalVolume.Value) / originalVolume.Value;
    }

    /// <summary>Calculates the volume fraction of this component using the total component volume before mixing.</summary>
    public double VolumeFraction(Volume totalBeforeMixing)
    {
        return Value / totalBeforeMixing.Value;
    }

    /// <summary>Calculates the volume enclosed by a sphere of positive radius.</summary>
    public static Volume FromSphere(Length radius)
    {
        return Area.FromSphere(radius) * (radius / 3d);
    }
}

public readonly partial struct MomentOfInertia
{
    /// <summary>Calculates a point mass's moment of inertia about an axis at the given perpendicular distance.</summary>
    public static MomentOfInertia FromPointMass(Mass mass, Length perpendicularDistance)
    {
        return (MomentOfInertia)(mass.Value * (perpendicularDistance * perpendicularDistance).Value);
    }

    /// <summary>Calculates inertia of a uniform thin ring about its central symmetry axis.</summary>
    public static MomentOfInertia FromThinRing(Mass mass, Length radius)
    {
        return FromPointMass(mass, radius);
    }

    /// <summary>Calculates inertia of a uniform solid disk or solid cylinder about its central symmetry axis.</summary>
    public static MomentOfInertia FromSolidCylinder(Mass mass, Length radius)
    {
        return 0.5d * FromPointMass(mass, radius);
    }

    /// <summary>Calculates inertia of a uniform thin rod about its midpoint, around an axis perpendicular to its length.</summary>
    public static MomentOfInertia FromThinRod(Mass mass, Length length)
    {
        return FromPointMass(mass, length) / 12d;
    }

    /// <summary>Calculates inertia of a uniform solid sphere about an axis through its centre.</summary>
    public static MomentOfInertia FromSolidSphere(Mass mass, Length radius)
    {
        return (2d / 5d) * FromPointMass(mass, radius);
    }

    /// <summary>Transfers this centre-of-mass inertia to a parallel axis at the specified perpendicular distance.</summary>
    public MomentOfInertia AboutParallelAxis(Mass mass, Length axisSeparation)
    {
        return (MomentOfInertia)(Value + FromPointMass(mass, axisSeparation).Value);
    }
}

public readonly partial struct EnergyDensity
{
    /// <summary>Calculates recoverable energy per unit volume for a single linearly elastic stress and its conjugate strain.</summary>
    public static EnergyDensity FromElasticStrain(Stress stress, double strain)
    {
        return (EnergyDensity)(0.5d * stress.Value * strain);
    }

    /// <summary>Calculates instantaneous field energy density in a lossless, linear, isotropic, nondispersive medium.</summary>
    public static EnergyDensity FromElectromagneticFields(
        ElectricFieldStrength electricField, MagneticFluxDensity magneticField, Permittivity permittivity, Permeability permeability)
    {
        var electric = electricField.Value;
        var magnetic = magneticField.Value;
        return (EnergyDensity)(0.5d * (permittivity.Value * electric * electric + magnetic * magnetic / permeability.Value));
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

    /// <summary>Calculates gravitational potential energy outside a spherical body, with zero energy at infinity.</summary>
    /// <param name="gravitationalParameter">The central body's gravitational parameter.</param>
    /// <param name="mass">The mass of the other body.</param>
    /// <param name="distance">The positive distance between the centres.</param>
    public static Energy FromGravitationalPotential(
        StandardGravitationalParameter gravitationalParameter, Mass mass, Length distance)
    {
        return (Energy)(-(gravitationalParameter.Value / distance.Value) * mass.Value);
    }

    /// <summary>Calculates the signed change in gravitational potential energy in a uniform field.</summary>
    /// <param name="mass">The mass of the body.</param>
    /// <param name="gravity">The positive magnitude of gravitational acceleration.</param>
    /// <param name="heightChange">The signed height change, positive upward.</param>
    public static Energy FromUniformGravity(Mass mass, LinearAcceleration gravity, Length heightChange)
    {
        return FromMechanicalWork(mass * gravity, heightChange);
    }

    /// <summary>Calculates the total mechanical energy of a Newtonian circular orbit with negligible orbiting mass.</summary>
    public static Energy FromCircularOrbit(StandardGravitationalParameter gravitationalParameter, Mass orbitingMass, Length radius)
    {
        return 0.5d * FromGravitationalPotential(gravitationalParameter, orbitingMass, radius);
    }

    /// <summary>Calculates heat engine efficiency from this net work output and the positive heat input over the same cycle.</summary>
    public double HeatEngineEfficiency(Energy heatInput)
    {
        return Value / heatInput.Value;
    }

    /// <summary>Calculates potential energy of an electric dipole in a uniform field, relative to perpendicular orientation.</summary>
    public static Energy FromElectricDipole(ElectricDipoleMoment dipoleMoment, ElectricFieldStrength field, PlaneAngle angle)
    {
        return (Energy)(-dipoleMoment.Value * field.Value * Math.Cos(angle.Radian));
    }

    /// <summary>Calculates potential energy of a magnetic dipole in a uniform field, relative to perpendicular orientation.</summary>
    public static Energy FromMagneticDipole(
        MagneticDipoleMoment dipoleMoment, MagneticFluxDensity field, Permeability vacuumPermeability, PlaneAngle angle)
    {
        return (Energy)(-(dipoleMoment.Value / vacuumPermeability.Value) * field.Value * Math.Cos(angle.Radian));
    }

    /// <summary>Calculates the rest energy of a body using the positive vacuum speed of light.</summary>
    public static Energy FromRestMass(Mass restMass, LinearVelocity vacuumLightSpeed)
    {
        return (Energy)(restMass.Value * vacuumLightSpeed.Value * vacuumLightSpeed.Value);
    }

    /// <summary>Calculates total relativistic energy from rest mass and velocity measured in the same inertial frame.</summary>
    public static Energy FromRelativisticMotion(Mass restMass, LinearVelocity velocity, LinearVelocity vacuumLightSpeed)
    {
        return FromRestMass(restMass, vacuumLightSpeed) * velocity.LorentzFactor(vacuumLightSpeed);
    }

    /// <summary>Calculates relativistic kinetic energy from rest mass and subluminal velocity.</summary>
    public static Energy FromRelativisticKineticEnergy(Mass restMass, LinearVelocity velocity, LinearVelocity vacuumLightSpeed)
    {
        var beta = velocity.Value / vacuumLightSpeed.Value;
        var gamma = velocity.LorentzFactor(vacuumLightSpeed);
        var gammaTimesBeta = gamma * beta;
        return FromRestMass(restMass, vacuumLightSpeed) * (gammaTimesBeta * gammaTimesBeta / (gamma + 1d));
    }

    /// <summary>Calculates positive total relativistic energy from rest mass and momentum magnitude or a signed collinear momentum.</summary>
    public static Energy FromRelativisticMomentum(Mass restMass, LinearMomentum momentum, LinearVelocity vacuumLightSpeed)
    {
        var restEnergy = FromRestMass(restMass, vacuumLightSpeed).Value;
        var momentumTimesLightSpeed = momentum.Value * vacuumLightSpeed.Value;
        return (Energy)Math.Sqrt(restEnergy * restEnergy + momentumTimesLightSpeed * momentumTimesLightSpeed);
    }

    /// <summary>Calculates maximum photoelectron kinetic energy from photon energy and a positive work function.</summary>
    /// <returns>True when the photon energy reaches the emission threshold.</returns>
    public static bool TryPhotoelectronKineticEnergy(Energy photonEnergy, Energy workFunction, out Energy kineticEnergy)
    {
        var difference = photonEnergy.Value - workFunction.Value;
        if (!(difference >= 0d))
        {
            kineticEnergy = default;
            return false;
        }

        kineticEnergy = (Energy)difference;
        return true;
    }

    /// <summary>Calculates work done by a fixed amount of ideal gas during a reversible isothermal volume change.</summary>
    public static Energy FromIsothermalWork(ForcePerArea initialPressure, Volume initialVolume, Volume finalVolume)
    {
        return (Energy)(initialPressure.Value * initialVolume.Value * Math.Log(finalVolume.Value / initialVolume.Value));
    }

    /// <summary>Calculates work done by a calorically perfect ideal gas during a reversible adiabatic volume change.</summary>
    public static Energy FromIsentropicWork(
        ForcePerArea initialPressure, Volume initialVolume, Volume finalVolume, double heatCapacityRatio)
    {
        var finalPressure = initialPressure.AfterIsentropicVolumeChange(initialVolume, finalVolume, heatCapacityRatio);
        return (Energy)((initialPressure.Value * initialVolume.Value - finalPressure.Value * finalVolume.Value)
                        / (heatCapacityRatio - 1d));
    }
}

public readonly partial struct LinearMomentum
{
    /// <summary>Calculates signed relativistic momentum along the given subluminal velocity in an inertial frame.</summary>
    public static LinearMomentum FromRelativisticMotion(Mass restMass, LinearVelocity velocity, LinearVelocity vacuumLightSpeed)
    {
        return (restMass * velocity) * velocity.LorentzFactor(vacuumLightSpeed);
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

    /// <summary>Calculates the radial Coulomb force on a point charge in a homogeneous, isotropic medium.</summary>
    /// <returns>The signed radial force, positive for repulsion.</returns>
    public static Force FromPointCharges(
        ElectricCharge sourceCharge, ElectricCharge testCharge, Permittivity permittivity, Length separation)
    {
        return ElectricFieldStrength.FromPointCharge(sourceCharge, permittivity, separation) * testCharge;
    }

    /// <summary>Calculates the maximum static friction magnitude in the dry Coulomb friction model.</summary>
    public static Force MaximumStaticFriction(Force normalForce, double coefficientOfStaticFriction)
    {
        return normalForce * coefficientOfStaticFriction;
    }

    /// <summary>Calculates the kinetic friction magnitude for sliding surfaces in the dry Coulomb friction model.</summary>
    public static Force FromKineticFriction(Force normalForce, double coefficientOfKineticFriction)
    {
        return normalForce * coefficientOfKineticFriction;
    }

    /// <summary>Calculates drag magnitude with the coefficient and reference area for the given flow condition.</summary>
    public static Force FromDrag(Density density, LinearVelocity relativeSpeed, Area referenceArea, double dragCoefficient)
    {
        return (ForcePerArea.FromDynamicPressure(density, relativeSpeed) * referenceArea) * dragCoefficient;
    }

    /// <summary>Calculates signed lift with the coefficient and reference area for the given flow condition.</summary>
    public static Force FromLift(Density density, LinearVelocity relativeSpeed, Area referenceArea, double liftCoefficient)
    {
        return (ForcePerArea.FromDynamicPressure(density, relativeSpeed) * referenceArea) * liftCoefficient;
    }

    /// <summary>Calculates Stokes drag magnitude on an isolated rigid sphere in creeping Newtonian flow.</summary>
    public static Force FromStokesDrag(DynamicViscosity viscosity, Length sphereRadius, LinearVelocity relativeSpeed)
    {
        return (Force)((6d * Math.PI) * viscosity.Value * sphereRadius.Value * Math.Abs(relativeSpeed.Value));
    }

    /// <summary>Calculates the signed force opposing relative motion under linear viscous damping.</summary>
    public static Force FromViscousDamping(MassPerTime dampingCoefficient, LinearVelocity relativeVelocity)
    {
        return (Force)(-dampingCoefficient.Value * relativeVelocity.Value);
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

    /// <summary>Calculates the torque magnitude on an electric dipole in a uniform electric field.</summary>
    /// <param name="dipoleMoment">The electric dipole moment magnitude.</param>
    /// <param name="field">The electric field magnitude.</param>
    /// <param name="angle">The angle between the dipole and the field, from zero to π radians.</param>
    public static Torque FromElectricDipole(ElectricDipoleMoment dipoleMoment, ElectricFieldStrength field, PlaneAngle angle)
    {
        return (Torque)(dipoleMoment.Value * field.Value * Math.Sin(angle.Radian));
    }

    /// <summary>Calculates the torque magnitude on a magnetic dipole in a uniform magnetic field.</summary>
    /// <param name="dipoleMoment">The QUDT magnetic dipole moment magnitude in weber metres.</param>
    /// <param name="field">The magnetic flux density magnitude.</param>
    /// <param name="vacuumPermeability">The vacuum permeability used to recover the magnetic area moment.</param>
    /// <param name="angle">The angle between the dipole and the field, from zero to π radians.</param>
    public static Torque FromMagneticDipole(
        MagneticDipoleMoment dipoleMoment, MagneticFluxDensity field, Permeability vacuumPermeability, PlaneAngle angle)
    {
        return (Torque)((dipoleMoment.Value / vacuumPermeability.Value) * field.Value * Math.Sin(angle.Radian));
    }
}

public readonly partial struct ForcePerLength
{
    /// <summary>Calculates axial stiffness of a straight uniform bar in the linear elastic regime.</summary>
    public static ForcePerLength AxialStiffness(ForcePerArea youngsModulus, Area crossSection, Length barLength)
    {
        return (youngsModulus * crossSection) / barLength;
    }

    /// <summary>Calculates force per unit length between long parallel conductors in a uniform linear medium.</summary>
    /// <returns>The signed force per unit length, positive for currents in the same direction and attraction.</returns>
    public static ForcePerLength FromParallelCurrents(
        Permeability permeability, ElectricCurrent firstCurrent, ElectricCurrent secondCurrent, Length separation)
    {
        var field = MagneticFluxDensity.FromLongStraightWire(permeability, firstCurrent, separation);
        return (ForcePerLength)(field.Value * secondCurrent.Value);
    }
}

public readonly partial struct ForcePerArea
{
    /// <summary>Calculates this static absolute pressure after isentropic deceleration of a calorically perfect gas to rest.</summary>
    public ForcePerArea ToIsentropicStagnation(double machNumber, double heatCapacityRatio)
    {
        var ratio = ThermodynamicTemperature.StagnationTemperatureRatio(machNumber, heatCapacityRatio);
        return this * Math.Pow(ratio, heatCapacityRatio / (heatCapacityRatio - 1d));
    }

    /// <summary>Calculates hydrostatic absolute pressure after a signed height change in an isothermal ideal gas.</summary>
    /// <remarks>Positive height change is upward. Use constant scale height and a positive reference pressure.</remarks>
    public ForcePerArea AfterIsothermalElevationChange(Length heightChange, Length scaleHeight)
    {
        return this * Math.Exp(-(heightChange / scaleHeight));
    }

    /// <summary>Calculates a constituent's partial pressure from this total pressure of an ideal gas mixture.</summary>
    public ForcePerArea PartialPressureInIdealGas(double moleFraction)
    {
        return this * moleFraction;
    }

    /// <summary>Calculates a constituent's vapour partial pressure from this pure-component saturation pressure in an ideal solution.</summary>
    public ForcePerArea VapourPartialPressureInIdealSolution(double liquidMoleFraction)
    {
        return this * liquidMoleFraction;
    }

    /// <summary>Calculates mechanical head loss as pressure across a fitting in steady incompressible flow.</summary>
    public static ForcePerArea FromLocalFlowLoss(Density density, LinearVelocity referenceSpeed, double lossCoefficient)
    {
        return FromDynamicPressure(density, referenceSpeed) * lossCoefficient;
    }

    /// <summary>Calculates the signed pressure change for a small volumetric strain with constant bulk modulus.</summary>
    public static ForcePerArea PressureChangeFromBulkModulus(ForcePerArea bulkModulus, Volume originalVolume, Volume finalVolume)
    {
        return bulkModulus * (-finalVolume.VolumetricStrainFrom(originalVolume));
    }

    /// <summary>Calculates shear modulus from Young's modulus and Poisson's ratio for stable isotropic linear elasticity.</summary>
    public static ForcePerArea ShearModulusFromYoungsModulus(ForcePerArea youngsModulus, double poissonsRatio)
    {
        return youngsModulus / (2d * (1d + poissonsRatio));
    }

    /// <summary>Calculates bulk modulus from Young's modulus and Poisson's ratio for stable isotropic linear elasticity.</summary>
    public static ForcePerArea BulkModulusFromYoungsModulus(ForcePerArea youngsModulus, double poissonsRatio)
    {
        return youngsModulus / (3d * (1d - 2d * poissonsRatio));
    }

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

    /// <summary>Calculates the new absolute pressure of a fixed amount of ideal gas.</summary>
    /// <remarks>This pressure, both volumes, and both absolute temperatures must be positive.</remarks>
    public ForcePerArea AfterIdealGasStateChange(
        Volume initialVolume, ThermodynamicTemperature initialTemperature,
        Volume finalVolume, ThermodynamicTemperature finalTemperature)
    {
        return this * ((initialVolume.Value / finalVolume.Value) * (finalTemperature.Kelvin / initialTemperature.Kelvin));
    }

    /// <summary>Calculates the absolute pressure after a reversible adiabatic volume change of a calorically perfect ideal gas.</summary>
    public ForcePerArea AfterIsentropicVolumeChange(Volume initialVolume, Volume finalVolume, double heatCapacityRatio)
    {
        return this * Math.Pow(initialVolume.Value / finalVolume.Value, heatCapacityRatio);
    }

    /// <summary>Calculates the Darcy–Weisbach friction pressure drop for fully developed flow in a straight duct.</summary>
    /// <param name="density">The fluid density, assumed constant along the duct.</param>
    /// <param name="meanSpeed">The cross-sectional mean flow speed.</param>
    /// <param name="ductLength">The length of the constant cross-section duct.</param>
    /// <param name="hydraulicDiameter">The positive hydraulic diameter.</param>
    /// <param name="darcyFrictionFactor">The Darcy friction factor for the given duct and flow conditions.</param>
    public static ForcePerArea FromDarcyWeisbach(
        Density density, LinearVelocity meanSpeed, Length ductLength, Length hydraulicDiameter, double darcyFrictionFactor)
    {
        return FromDynamicPressure(density, meanSpeed) * (darcyFrictionFactor * (ductLength / hydraulicDiameter));
    }

    /// <summary>Calculates the inside-to-outside pressure jump across a single spherical interface.</summary>
    public static ForcePerArea FromSphericalSurfaceTension(ForcePerLength surfaceTension, Length radius)
    {
        return (ForcePerArea)(2d * surfaceTension.Value / radius.Value);
    }

    /// <summary>Calculates pressure on an opaque surface illuminated at normal incidence in vacuum.</summary>
    /// <remarks>Specular reflectance is a fraction from zero to one, and the remaining irradiance is absorbed.</remarks>
    public static ForcePerArea FromRadiationPressure(
        PowerPerArea irradiance, LinearVelocity vacuumLightSpeed, double specularReflectance)
    {
        return (ForcePerArea)((1d + specularReflectance) * irradiance.Value / vacuumLightSpeed.Value);
    }
}

public readonly partial struct VolumePerTime
{
    /// <summary>Calculates incompressible discharge through an orifice with negligible approach velocity.</summary>
    /// <param name="pressureDrop">The nonnegative driving pressure difference, with elevation head accounted for.</param>
    /// <param name="density">The positive fluid density.</param>
    /// <param name="orificeArea">The area of the opening.</param>
    /// <param name="dischargeCoefficient">The discharge coefficient defined for this orifice and flow condition.</param>
    public static VolumePerTime FromOrificeFlow(
        ForcePerArea pressureDrop, Density density, Area orificeArea, double dischargeCoefficient)
    {
        var idealSpeed = LinearVelocity.FromMetrePerSecond(Math.Sqrt(2d * pressureDrop.Value / density.Value));
        return (orificeArea * idealSpeed) * dischargeCoefficient;
    }

    /// <summary>Calculates incompressible discharge from a Venturi meter with steady flow and a known discharge coefficient.</summary>
    /// <remarks>Use elevation-corrected differential pressure, positive density, and a throat area smaller than the upstream area.</remarks>
    public static VolumePerTime FromVenturi(
        ForcePerArea pressureDrop, Density density, Area upstreamArea, Area throatArea, double dischargeCoefficient)
    {
        var areaRatio = throatArea.Value / upstreamArea.Value;
        var throatSpeed = LinearVelocity.FromMetrePerSecond(
            Math.Sqrt(2d * pressureDrop.Value / (density.Value * (1d - areaRatio * areaRatio))));
        return (throatArea * throatSpeed) * dischargeCoefficient;
    }

    /// <summary>Calculates Hagen–Poiseuille flow through a straight, rigid, horizontal circular pipe.</summary>
    /// <remarks>Applies to steady, fully developed, laminar flow of an incompressible Newtonian fluid with no wall slip.</remarks>
    /// <param name="pressureDrop">The inlet pressure minus the outlet pressure.</param>
    /// <param name="viscosity">The positive dynamic viscosity.</param>
    /// <param name="radius">The positive internal pipe radius.</param>
    /// <param name="pipeLength">The positive length over which the pressure drop is measured.</param>
    public static VolumePerTime FromLaminarPipeFlow(
        ForcePerArea pressureDrop, DynamicViscosity viscosity, Length radius, Length pipeLength)
    {
        var radiusSquared = (radius * radius).Value;
        return (VolumePerTime)(Math.PI * radiusSquared * radiusSquared * pressureDrop.Value
                               / (8d * viscosity.Value * pipeLength.Value));
    }
}

public readonly partial struct Stress
{
    /// <summary>Calculates signed normal stress under uniaxial linear elasticity.</summary>
    public static Stress FromUniaxialStrain(ForcePerArea youngsModulus, double longitudinalStrain)
    {
        return (Stress)(youngsModulus.Value * longitudinalStrain);
    }

    /// <summary>Calculates signed shear stress from engineering shear strain under linear elasticity.</summary>
    public static Stress FromElasticShearStrain(ForcePerArea shearModulus, double shearStrain)
    {
        return (Stress)(shearModulus.Value * shearStrain);
    }

    /// <summary>Calculates signed surface shear stress in elastic torsion of a solid circular shaft.</summary>
    public static Stress FromSolidShaftTorque(Torque torque, Length radius)
    {
        return (Stress)(2d * torque.Value / (Area.FromCircle(radius) * radius).Value);
    }

    /// <summary>Calculates signed Newtonian shear stress for a linear velocity profile.</summary>
    /// <param name="viscosity">The dynamic viscosity of the fluid.</param>
    /// <param name="velocityDifference">The signed difference of the velocity component parallel to the layers.</param>
    /// <param name="separation">The positive distance between the layers, perpendicular to them.</param>
    public static Stress FromNewtonianShear(DynamicViscosity viscosity, LinearVelocity velocityDifference, Length separation)
    {
        return (Stress)(viscosity.Value * (velocityDifference.Value / separation.Value));
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

    /// <summary>Calculates Newtonian escape speed at a given positive radius outside a spherical body.</summary>
    public static LinearVelocity EscapeSpeed(StandardGravitationalParameter gravitationalParameter, Length radius)
    {
        return Math.Sqrt(2d) * FromCircularOrbit(gravitationalParameter, radius);
    }

    /// <summary>Calculates final speed from signed one-dimensional kinematics under constant acceleration.</summary>
    /// <remarks>The inputs must describe a reachable displacement, so the squared final speed is nonnegative.</remarks>
    public static LinearVelocity SpeedAfterConstantAcceleration(
        LinearVelocity initialVelocity, LinearAcceleration acceleration, Length displacement)
    {
        return FromMetrePerSecond(Math.Sqrt(initialVelocity.Value * initialVelocity.Value + 2d * acceleration.Value * displacement.Value));
    }

    /// <summary>Calculates transverse wave speed along a uniform taut string under small displacement.</summary>
    public static LinearVelocity FromStretchedString(Force tension, MassPerLength linearDensity)
    {
        return FromMetrePerSecond(Math.Sqrt(tension.Value / linearDensity.Value));
    }

    /// <summary>Calculates small-signal sound speed from a fluid's positive adiabatic bulk modulus and density.</summary>
    public static LinearVelocity FromFluidBulkModulus(ForcePerArea bulkModulus, Density density)
    {
        return FromMetrePerSecond(Math.Sqrt(bulkModulus.Value / density.Value));
    }

    /// <summary>Calculates phase speed for linear surface gravity waves in still water of uniform positive depth.</summary>
    /// <remarks>Use a positive wavelength in the gravity-dominated regime, where surface tension is negligible.</remarks>
    public static LinearVelocity FromSurfaceGravityWave(LinearAcceleration gravity, Length wavelength, Length depth)
    {
        var waveNumber = 2d * Math.PI / wavelength.Metre;
        return FromMetrePerSecond(Math.Sqrt((gravity.Value / waveNumber) * Math.Tanh(waveNumber * depth.Metre)));
    }

    /// <summary>Calculates phase speed for linear surface gravity waves when depth is small relative to wavelength.</summary>
    public static LinearVelocity FromShallowWaterGravityWave(LinearAcceleration gravity, Length depth)
    {
        return FromMetrePerSecond(Math.Sqrt(gravity.Value * depth.Value));
    }

    /// <summary>Calculates phase speed for linear surface gravity waves when depth is large relative to wavelength.</summary>
    public static LinearVelocity FromDeepWaterGravityWave(LinearAcceleration gravity, Length wavelength)
    {
        return FromMetrePerSecond(Math.Sqrt(gravity.Value * wavelength.Value / (2d * Math.PI)));
    }

    /// <summary>Calculates the ratio of this speed magnitude to the positive local sound speed.</summary>
    public double MachNumber(LinearVelocity soundSpeed)
    {
        return Math.Abs(Value) / soundSpeed.Value;
    }

    /// <summary>Calculates the Froude number for open-channel flow using this mean velocity and positive hydraulic depth.</summary>
    public double FroudeNumber(LinearAcceleration gravity, Length hydraulicDepth)
    {
        return Math.Abs(Value) / FromShallowWaterGravityWave(gravity, hydraulicDepth).Value;
    }

    /// <summary>Calculates the Weber number using this relative speed, the fluid density, reference length, and positive surface tension.</summary>
    public double WeberNumber(Density density, Length characteristicLength, ForcePerLength surfaceTension)
    {
        return density.Value * Value * Value * characteristicLength.Value / surfaceTension.Value;
    }

    /// <summary>Calculates the Darcy friction factor from this positive mean speed in fully developed laminar circular-pipe flow.</summary>
    public double DarcyFrictionFactorForLaminarPipe(Length pipeDiameter, KinematicViscosity kinematicViscosity)
    {
        return 64d / ReynoldsNumber(pipeDiameter, kinematicViscosity);
    }

    /// <summary>Calculates a rigid sphere's signed terminal velocity in an unbounded Newtonian fluid at very low Reynolds number.</summary>
    /// <returns>The terminal velocity, positive downward and negative for a rising sphere.</returns>
    public static LinearVelocity StokesTerminalVelocity(
        Density sphereDensity, Density fluidDensity, DynamicViscosity viscosity, LinearAcceleration gravity, Length sphereRadius)
    {
        var radiusSquared = (sphereRadius * sphereRadius).Value;
        return FromMetrePerSecond((2d / 9d) * (sphereDensity.Value - fluidDensity.Value) * gravity.Value * radiusSquared / viscosity.Value);
    }

    /// <summary>Calculates the phase speed of light in a medium from its positive refractive index at the wavelength of interest.</summary>
    public static LinearVelocity FromRefractiveIndex(LinearVelocity vacuumLightSpeed, double refractiveIndex)
    {
        return vacuumLightSpeed / refractiveIndex;
    }

    /// <summary>Calculates refractive index from the vacuum light speed and this positive phase speed in a medium.</summary>
    public double RefractiveIndex(LinearVelocity vacuumLightSpeed)
    {
        return vacuumLightSpeed.Value / Value;
    }

    /// <summary>Calculates the Lorentz factor of this relative velocity with magnitude below the positive vacuum light speed.</summary>
    public double LorentzFactor(LinearVelocity vacuumLightSpeed)
    {
        var beta = Math.Abs(Value) / vacuumLightSpeed.Value;
        return 1d / Math.Sqrt((1d - beta) * (1d + beta));
    }

    /// <summary>Composes this velocity in a moving inertial frame with that frame's collinear velocity in a reference frame.</summary>
    /// <remarks>Both signed velocities must have magnitudes below the positive vacuum light speed.</remarks>
    public LinearVelocity ComposeRelativistically(LinearVelocity frameVelocity, LinearVelocity vacuumLightSpeed)
    {
        var vacuumSpeed = vacuumLightSpeed.Value;
        var normalizedProduct = (Value / vacuumSpeed) * (frameVelocity.Value / vacuumSpeed);
        return FromMetrePerSecond((Value + frameVelocity.Value) / (1d + normalizedProduct));
    }

    /// <summary>Calculates electromagnetic phase speed in a lossless, isotropic, nondispersive linear medium.</summary>
    public static LinearVelocity FromElectromagneticMedium(Permittivity permittivity, Permeability permeability)
    {
        return FromMetrePerSecond(1d / Math.Sqrt(permittivity.Value * permeability.Value));
    }

    /// <summary>Calculates ideal discharge speed from a small opening below the free surface of a large vessel.</summary>
    /// <remarks>Use positive head and uniform gravity, with equal surface and outlet pressures and negligible losses.</remarks>
    public static LinearVelocity FromTorricelliHead(LinearAcceleration gravity, Length head)
    {
        return FromMetrePerSecond(Math.Sqrt(2d * gravity.Value * head.Value));
    }

    /// <summary>Calculates sound speed from absolute static pressure, density, and heat capacity ratio for a calorically perfect ideal gas.</summary>
    public static LinearVelocity FromIdealGasSoundSpeed(ForcePerArea pressure, Density density, double heatCapacityRatio)
    {
        return FromFluidBulkModulus(pressure * heatCapacityRatio, density);
    }

    /// <summary>Calculates the capillary number from this flow speed, dynamic viscosity, and positive surface tension.</summary>
    public double CapillaryNumber(DynamicViscosity viscosity, ForcePerLength surfaceTension)
    {
        return viscosity.Value * Math.Abs(Value) / surfaceTension.Value;
    }

    /// <summary>Calculates the thermal Péclet number from this flow speed, characteristic length, and positive thermal diffusivity.</summary>
    public double ThermalPecletNumber(Length characteristicLength, ThermalDiffusivity diffusivity)
    {
        return Math.Abs(Value) * characteristicLength.Value / diffusivity.Value;
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

public readonly partial struct Density
{
    /// <summary>Calculates this static density after isentropic deceleration of a calorically perfect gas to rest.</summary>
    public Density ToIsentropicStagnation(double machNumber, double heatCapacityRatio)
    {
        var ratio = ThermodynamicTemperature.StagnationTemperatureRatio(machNumber, heatCapacityRatio);
        return this * Math.Pow(ratio, 1d / (heatCapacityRatio - 1d));
    }
}

public readonly partial struct Time
{
    /// <summary>Calculates the coordinate duration of this proper interval for a clock moving at the given relative velocity.</summary>
    public Time DilatedForRelativeVelocity(LinearVelocity relativeVelocity, LinearVelocity vacuumLightSpeed)
    {
        return this * relativeVelocity.LorentzFactor(vacuumLightSpeed);
    }

    /// <summary>Calculates the period of a Newtonian circular orbit about a spherical central body.</summary>
    public static Time FromCircularOrbit(StandardGravitationalParameter gravitationalParameter, Length radius)
    {
        return Length.CircumferenceFromRadius(radius) / LinearVelocity.FromCircularOrbit(gravitationalParameter, radius);
    }

    /// <summary>Calculates the period of a positive mass on an undamped, massless spring with positive stiffness.</summary>
    public static Time FromMassSpring(Mass mass, ForcePerLength stiffness)
    {
        return FromSecond((2d * Math.PI) * Math.Sqrt(mass.Value / stiffness.Value));
    }

    /// <summary>Calculates the small-angle period of a simple pendulum in uniform gravity.</summary>
    /// <param name="length">The positive distance from the pivot to the point mass.</param>
    /// <param name="gravity">The positive magnitude of gravitational acceleration.</param>
    public static Time FromSimplePendulum(Length length, LinearAcceleration gravity)
    {
        return FromSecond((2d * Math.PI) * Math.Sqrt(length.Value / gravity.Value));
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
    /// <summary>Calculates the signed radial electric field of a point charge in a homogeneous, isotropic medium.</summary>
    /// <returns>The radial field component, positive outward from the source charge.</returns>
    public static ElectricFieldStrength FromPointCharge(ElectricCharge charge, Permittivity permittivity, Length distance)
    {
        return (ElectricFieldStrength)(ElectricPotential.FromPointCharge(charge, permittivity, distance).Value / distance.Value);
    }

    /// <summary>Calculates the signed radial field of an infinite uniform line charge in a homogeneous linear dielectric.</summary>
    public static ElectricFieldStrength FromLineCharge(
        ElectricChargeLineDensity lineCharge, Permittivity permittivity, Length distance)
    {
        return (ElectricFieldStrength)(lineCharge.Value / ((2d * Math.PI) * permittivity.Value * distance.Value));
    }

    /// <summary>Calculates the electric field magnitude from the matching magnetic magnitude in a progressive plane wave.</summary>
    public static ElectricFieldStrength FromPlaneWave(MagneticFluxDensity magneticField, LinearVelocity phaseSpeed)
    {
        return (ElectricFieldStrength)(magneticField.Value * phaseSpeed.Value);
    }

    /// <summary>Calculates the average electric field component along a straight path.</summary>
    /// <param name="potentialChange">The final electric potential minus the initial electric potential.</param>
    /// <param name="distance">The length of the path.</param>
    /// <returns>The signed field component in the direction of the path.</returns>
    public static ElectricFieldStrength FromPotentialChange(ElectricPotential potentialChange, Length distance)
    {
        return -(potentialChange / distance);
    }
}

public readonly partial struct ElectricPotential
{
    /// <summary>Calculates the potential of a point charge in a homogeneous, isotropic medium, relative to infinity.</summary>
    /// <param name="charge">The signed source charge.</param>
    /// <param name="permittivity">The medium's positive absolute permittivity.</param>
    /// <param name="distance">The positive distance to the charge.</param>
    public static ElectricPotential FromPointCharge(ElectricCharge charge, Permittivity permittivity, Length distance)
    {
        return (ElectricPotential)(charge.Value / (4d * Math.PI * permittivity.Value * distance.Value));
    }

    /// <summary>Calculates the voltage across the lower resistor of an unloaded two-resistor divider.</summary>
    public static ElectricPotential FromVoltageDivider(
        ElectricPotential supply, Resistance upperResistance, Resistance lowerResistance)
    {
        return supply * (lowerResistance.Value / upperResistance.InSeriesWith(lowerResistance).Value);
    }

    /// <summary>Calculates this capacitor voltage after a first-order RC step toward a constant final voltage.</summary>
    public ElectricPotential AfterRcStep(ElectricPotential finalVoltage, Resistance resistance, Capacitance capacitance, Time elapsed)
    {
        var decay = Math.Exp(-elapsed.Value / (resistance * capacitance).Value);
        return FromVolt(finalVoltage.Volt + (Volt - finalVoltage.Volt) * decay);
    }

    /// <summary>Calculates RMS voltage across an ideal capacitor from RMS current at a positive sinusoidal frequency.</summary>
    public static ElectricPotential FromSinusoidalCapacitor(ElectricCurrent rmsCurrent, Capacitance capacitance, Frequency frequency)
    {
        var angularFrequency = AngularVelocity.FromFrequency(frequency).RadianPerSecond;
        return FromVolt(rmsCurrent.Ampere / (angularFrequency * capacitance.Farad));
    }

    /// <summary>Calculates RMS voltage across an ideal inductor from RMS current at a positive sinusoidal frequency.</summary>
    public static ElectricPotential FromSinusoidalInductor(ElectricCurrent rmsCurrent, Inductance inductance, Frequency frequency)
    {
        return FromVolt(rmsCurrent.Ampere * AngularVelocity.FromFrequency(frequency).RadianPerSecond * inductance.Henry);
    }

    /// <summary>Calculates average passive terminal voltage from the change of current through a linear ideal inductor.</summary>
    public static ElectricPotential FromInductorCurrentChange(Inductance inductance, ElectricCurrent currentChange, Time elapsed)
    {
        return FromVolt(inductance.Henry * currentChange.Ampere / elapsed.Second);
    }

    /// <summary>Calculates secondary RMS voltage from this primary RMS voltage in an ideal transformer.</summary>
    public ElectricPotential AfterIdealTransformer(int primaryTurns, int secondaryTurns)
    {
        return this * ((double)secondaryTurns / primaryTurns);
    }
}

public readonly partial struct ElectricCurrent
{
    /// <summary>Calculates this inductor current after a first-order RL step toward a constant final current.</summary>
    public ElectricCurrent AfterRlStep(ElectricCurrent finalCurrent, Resistance resistance, Inductance inductance, Time elapsed)
    {
        var decay = Math.Exp(-elapsed.Value / (inductance / resistance).Value);
        return FromAmpere(finalCurrent.Ampere + (Ampere - finalCurrent.Ampere) * decay);
    }

    /// <summary>Calculates the RMS current of a series RLC circuit in sinusoidal steady state.</summary>
    public static ElectricCurrent FromSeriesRlc(
        ElectricPotential rmsVoltage, Resistance resistance, Inductance inductance, Capacitance capacitance, Frequency frequency)
    {
        var angularFrequency = AngularVelocity.FromFrequency(frequency).RadianPerSecond;
        var reactance = angularFrequency * inductance.Henry - 1d / (angularFrequency * capacitance.Farad);
        return FromAmpere(rmsVoltage.Volt / Math.Sqrt(resistance.Ohm * resistance.Ohm + reactance * reactance));
    }

    /// <summary>Calculates average passive terminal current from the voltage change across a linear ideal capacitor.</summary>
    public static ElectricCurrent FromCapacitorVoltageChange(Capacitance capacitance, ElectricPotential voltageChange, Time elapsed)
    {
        return (capacitance * voltageChange) / elapsed;
    }

    /// <summary>Calculates secondary RMS current from this primary RMS current in an ideal loaded transformer.</summary>
    public ElectricCurrent AfterIdealTransformer(int primaryTurns, int secondaryTurns)
    {
        return this * ((double)primaryTurns / secondaryTurns);
    }
}

public readonly partial struct Power
{
    /// <summary>Calculates average active power from RMS voltage and current in sinusoidal steady state.</summary>
    /// <remarks>Measure the phase difference between terminal voltage and current entering its positive terminal.</remarks>
    public static Power FromSinusoidalAc(ElectricPotential rmsVoltage, ElectricCurrent rmsCurrent, PlaneAngle phaseDifference)
    {
        return (rmsVoltage * rmsCurrent) * Math.Cos(phaseDifference.Radian);
    }

    /// <summary>Calculates Joule heating from the current through a positive ohmic resistance.</summary>
    public static Power FromResistiveHeating(ElectricCurrent current, Resistance resistance)
    {
        return (resistance * current) * current;
    }

    /// <summary>Calculates Joule heating from the voltage across a positive ohmic resistance.</summary>
    public static Power FromResistiveHeating(ElectricPotential voltage, Resistance resistance)
    {
        return voltage * (voltage / resistance);
    }
}

public readonly partial struct MagneticFluxDensity
{
    /// <summary>Calculates the signed axial flux density near the centre of a long, uniformly wound solenoid.</summary>
    /// <param name="permeability">The uniform absolute permeability of the linear medium.</param>
    /// <param name="current">The signed winding current, with positive direction given by the winding orientation.</param>
    /// <param name="turns">The number of turns.</param>
    /// <param name="axialLength">The positive length of the winding, large compared with its diameter.</param>
    public static MagneticFluxDensity FromLongSolenoid(
        Permeability permeability, ElectricCurrent current, int turns, Length axialLength)
    {
        return permeability * (MagnetomotiveForce.FromWinding(current, turns) / axialLength);
    }

    /// <summary>Calculates the signed tangential magnetic flux density outside a long straight current in a uniform linear medium.</summary>
    public static MagneticFluxDensity FromLongStraightWire(Permeability permeability, ElectricCurrent current, Length distance)
    {
        return FromTesla(permeability.Value * current.Ampere / ((2d * Math.PI) * distance.Metre));
    }

    /// <summary>Calculates magnetic flux density magnitude from the matching electric magnitude in a progressive plane wave.</summary>
    public static MagneticFluxDensity FromPlaneWave(ElectricFieldStrength electricField, LinearVelocity phaseSpeed)
    {
        return FromTesla(electricField.Value / phaseSpeed.Value);
    }
}

public readonly partial struct MagnetomotiveForce
{
    /// <summary>Calculates signed magnetomotive force from a winding's current and number of series turns.</summary>
    public static MagnetomotiveForce FromWinding(ElectricCurrent current, int turns)
    {
        return (MagnetomotiveForce)(turns * current.Value);
    }
}

public readonly partial struct MagneticDipoleMoment
{
    /// <summary>Calculates the signed QUDT magnetic dipole moment in weber metres of a planar current loop winding.</summary>
    /// <param name="vacuumPermeability">Vacuum permeability, which scales the current-area moment to the QUDT quantity.</param>
    /// <param name="current">Signed current with respect to the winding orientation.</param>
    /// <param name="loopArea">The area enclosed by each planar turn.</param>
    /// <param name="turns">The number of turns with the same orientation.</param>
    public static MagneticDipoleMoment FromCurrentLoop(
        Permeability vacuumPermeability, ElectricCurrent current, Area loopArea, int turns)
    {
        return (MagneticDipoleMoment)(vacuumPermeability.Value * turns * current.Value * loopArea.Value);
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
    /// <summary>Calculates effective resistance of two series resistors.</summary>
    public Resistance InSeriesWith(Resistance other)
    {
        return FromOhm(Ohm + other.Ohm);
    }

    /// <summary>Calculates effective resistance of two positive parallel resistors.</summary>
    public Resistance InParallelWith(Resistance other)
    {
        var smaller = Math.Min(Ohm, other.Ohm);
        var larger = Math.Max(Ohm, other.Ohm);
        return FromOhm(smaller / (1d + smaller / larger));
    }

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
    /// <summary>Calculates effective capacitance of two positive ideal capacitors in series.</summary>
    public Capacitance InSeriesWith(Capacitance other)
    {
        var smaller = Math.Min(Farad, other.Farad);
        var larger = Math.Max(Farad, other.Farad);
        return FromFarad(smaller / (1d + smaller / larger));
    }

    /// <summary>Calculates effective capacitance of two ideal capacitors in parallel.</summary>
    public Capacitance InParallelWith(Capacitance other)
    {
        return FromFarad(Farad + other.Farad);
    }

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

public readonly partial struct Inductance
{
    /// <summary>Calculates effective inductance of two series inductors with negligible mutual coupling.</summary>
    public Inductance InSeriesWith(Inductance other)
    {
        return FromHenry(Henry + other.Henry);
    }

    /// <summary>Calculates effective inductance of two positive parallel inductors with negligible mutual coupling.</summary>
    public Inductance InParallelWith(Inductance other)
    {
        var smaller = Math.Min(Henry, other.Henry);
        var larger = Math.Max(Henry, other.Henry);
        return FromHenry(smaller / (1d + smaller / larger));
    }

    /// <summary>Calculates self-inductance of a long uniform solenoid in a linear medium with negligible fringing.</summary>
    public static Inductance FromLongSolenoid(Permeability permeability, Area crossSection, Length axialLength, int turns)
    {
        return FromHenry(permeability.Value * turns * turns * crossSection.Value / axialLength.Value);
    }

    /// <summary>Calculates the quality factor of a series RLC circuit from this inductance and positive capacitance and resistance.</summary>
    public double SeriesQualityFactor(Capacitance capacitance, Resistance seriesResistance)
    {
        return AngularVelocity.FromLcResonance(this, capacitance).Value * Value / seriesResistance.Value;
    }
}

public readonly partial struct ThermodynamicTemperature
{
    /// <summary>Calculates this static temperature after isentropic deceleration of a calorically perfect gas to rest.</summary>
    public ThermodynamicTemperature ToIsentropicStagnation(double machNumber, double heatCapacityRatio)
    {
        return FromKelvin(Kelvin * StagnationTemperatureRatio(machNumber, heatCapacityRatio));
    }

    internal static double StagnationTemperatureRatio(double machNumber, double heatCapacityRatio)
    {
        return 1d + 0.5d * (heatCapacityRatio - 1d) * machNumber * machNumber;
    }

    /// <summary>Calculates the absolute temperature after a reversible adiabatic volume change of a calorically perfect ideal gas.</summary>
    public ThermodynamicTemperature AfterIsentropicVolumeChange(Volume initialVolume, Volume finalVolume, double heatCapacityRatio)
    {
        return FromKelvin(Kelvin * Math.Pow(initialVolume.Value / finalVolume.Value, heatCapacityRatio - 1d));
    }

    /// <summary>Calculates Carnot engine efficiency with this hot reservoir and a colder reservoir.</summary>
    public double CarnotEfficiency(ThermodynamicTemperature coldReservoir)
    {
        return DifferenceInKelvins(coldReservoir) / Kelvin;
    }

    /// <summary>Calculates the Carnot refrigerator coefficient of performance with this hot reservoir and a colder reservoir.</summary>
    public double CarnotRefrigeratorCoefficientOfPerformance(ThermodynamicTemperature coldReservoir)
    {
        return coldReservoir.Kelvin / DifferenceInKelvins(coldReservoir);
    }

    /// <summary>Calculates the Carnot heat pump coefficient of performance with this hot reservoir and a colder reservoir.</summary>
    public double CarnotHeatPumpCoefficientOfPerformance(ThermodynamicTemperature coldReservoir)
    {
        return Kelvin / DifferenceInKelvins(coldReservoir);
    }

    /// <summary>Calculates this body's temperature after lumped heating or cooling against constant surroundings.</summary>
    /// <remarks>Use constant, positive thermal resistance and heat capacity, negligible internal temperature gradients, and nonnegative elapsed time.</remarks>
    public ThermodynamicTemperature AfterLumpedHeatTransfer(
        ThermodynamicTemperature surroundings, ThermalResistance resistance, HeatCapacity heatCapacity, Time elapsed)
    {
        var decay = Math.Exp(-elapsed.Value / (resistance * heatCapacity).Value);
        return surroundings.AddKelvins(DifferenceInKelvins(surroundings) * decay);
    }

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

public readonly partial struct SpecificHeatCapacity
{
    /// <summary>Calculates the ratio of specific heat at constant pressure to specific heat at constant volume.</summary>
    /// <remarks>Both mass-specific heat capacities must refer to the same gas state.</remarks>
    public static double HeatCapacityRatio(SpecificHeatCapacity atConstantPressure, SpecificHeatCapacity atConstantVolume)
    {
        return atConstantPressure.Value / atConstantVolume.Value;
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
    /// <summary>Calculates the Biot number using this surface coefficient and the solid's characteristic length and conductivity.</summary>
    public double BiotNumber(Length characteristicLength, ThermalConductivity solidConductivity)
    {
        return Value * characteristicLength.Value / solidConductivity.Value;
    }

    /// <summary>Calculates the Nusselt number using this convection coefficient and the fluid's characteristic length and conductivity.</summary>
    public double NusseltNumber(Length characteristicLength, ThermalConductivity fluidConductivity)
    {
        return Value * characteristicLength.Value / fluidConductivity.Value;
    }

    /// <summary>Calculates heat flux from the signed driving temperature difference in kelvins.</summary>
    public HeatFlowRatePerArea HeatFluxForTemperatureChange(double deltaKelvins)
    {
        return (HeatFlowRatePerArea)(Value * deltaKelvins);
    }
}

public readonly partial struct ThermalResistance
{
    /// <summary>Calculates steady radial conduction resistance of a homogeneous cylindrical shell with constant conductivity.</summary>
    public static ThermalResistance FromCylindricalConduction(
        ThermalConductivity conductivity, Length innerRadius, Length outerRadius, Length axialLength)
    {
        return (ThermalResistance)(Math.Log(outerRadius / innerRadius) / ((2d * Math.PI) * conductivity.Value * axialLength.Value));
    }

    /// <summary>Calculates steady radial conduction resistance of a homogeneous spherical shell with constant conductivity.</summary>
    public static ThermalResistance FromSphericalConduction(ThermalConductivity conductivity, Length innerRadius, Length outerRadius)
    {
        var inverseRadiusDifference = (outerRadius.Value - innerRadius.Value) / outerRadius.Value / innerRadius.Value;
        return (ThermalResistance)(inverseRadiusDifference / ((4d * Math.PI) * conductivity.Value));
    }

    /// <summary>Calculates equivalent resistance for two thermal paths in series, with the same heat flow through both.</summary>
    public ThermalResistance InSeriesWith(ThermalResistance other)
    {
        return (ThermalResistance)(Value + other.Value);
    }

    /// <summary>Calculates equivalent resistance for two positive thermal resistances between the same temperature nodes.</summary>
    public ThermalResistance InParallelWith(ThermalResistance other)
    {
        var smaller = Math.Min(Value, other.Value);
        var larger = Math.Max(Value, other.Value);
        return (ThermalResistance)(smaller / (1d + smaller / larger));
    }

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

public readonly partial struct ThermalDiffusivity
{
    /// <summary>Calculates the Fourier number for this thermal diffusivity, elapsed time, and positive characteristic length.</summary>
    public double FourierNumber(Time elapsed, Length characteristicLength)
    {
        return Value * elapsed.Value / (characteristicLength * characteristicLength).Value;
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
    /// <summary>Calculates net outward thermal radiation per unit area from a grey surface to large isothermal surroundings.</summary>
    /// <param name="stefanBoltzmannConstant">The Stefan–Boltzmann constant as a quantity in power per area per kelvin to the fourth power.</param>
    /// <param name="emissivity">The surface emissivity, a ratio from zero to one.</param>
    /// <param name="surfaceTemperature">The absolute surface temperature.</param>
    /// <param name="surroundingsTemperature">The absolute temperature of the surrounding black enclosure.</param>
    public static HeatFlowRatePerArea FromThermalRadiation(
        PowerPerAreaQuarticTemperature stefanBoltzmannConstant, double emissivity,
        ThermodynamicTemperature surfaceTemperature, ThermodynamicTemperature surroundingsTemperature)
    {
        var surface = surfaceTemperature.Kelvin;
        var surroundings = surroundingsTemperature.Kelvin;
        var fourthPowerDifference = (surface - surroundings) * (surface + surroundings)
                                    * (surface * surface + surroundings * surroundings);
        return (HeatFlowRatePerArea)(emissivity * stefanBoltzmannConstant.Value * fourthPowerDifference);
    }

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
    /// <summary>Calculates point-source illuminance at a surface whose normal faces the source.</summary>
    /// <param name="intensity">Luminous intensity in the direction of the surface.</param>
    /// <param name="distance">Positive distance from the point source to the surface.</param>
    /// <param name="incidenceAngle">Angle between the surface normal and the direction to the source, from zero to π/2 radians.</param>
    public static Illuminance FromPointSource(LuminousIntensity intensity, Length distance, PlaneAngle incidenceAngle)
    {
        return FromNormalPointSource(intensity, distance) * Math.Cos(incidenceAngle.Radian);
    }

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
    /// <summary>Calculates average intensity of a progressive sinusoidal plane wave in a lossless linear medium.</summary>
    public static PowerPerArea FromPlaneWave(
        ElectricFieldStrength rmsElectricField, MagneticFluxDensity rmsMagneticField, Permeability permeability)
    {
        return (PowerPerArea)(rmsElectricField.Value * rmsMagneticField.Value / permeability.Value);
    }

    /// <summary>Calculates transmitted irradiance for a narrow monoenergetic beam crossing a uniform material.</summary>
    /// <remarks>Use the linear attenuation coefficient at the photon energy and the path length through the material.</remarks>
    public PowerPerArea AfterExponentialAttenuation(InverseLength linearAttenuationCoefficient, Length pathLength)
    {
        return this * Math.Exp(-(linearAttenuationCoefficient * pathLength));
    }

    /// <summary>Calculates irradiance transmitted through an ideal polarizer for linearly polarized incident light.</summary>
    /// <param name="axisAngle">Angle between the incident polarization and the transmission axis.</param>
    public PowerPerArea AfterIdealPolarizer(PlaneAngle axisAngle)
    {
        var cosine = Math.Cos(axisAngle.Radian);
        return this * (cosine * cosine);
    }

    /// <summary>Calculates point-source irradiance at a surface whose normal faces the source.</summary>
    /// <param name="intensity">Radiant intensity in the direction of the surface.</param>
    /// <param name="distance">Positive distance from the point source to the surface.</param>
    /// <param name="incidenceAngle">Angle between the surface normal and the direction to the source, from zero to π/2 radians.</param>
    public static PowerPerArea FromPointSource(RadiantIntensity intensity, Length distance, PlaneAngle incidenceAngle)
    {
        return FromNormalPointSource(intensity, distance) * Math.Cos(incidenceAngle.Radian);
    }

    /// <summary>Calculates intensity at positive distance from an isotropic source in a lossless medium.</summary>
    public static PowerPerArea FromIsotropicSource(Power emittedPower, Length distance)
    {
        return emittedPower / Area.FromSphere(distance);
    }

    /// <summary>Calculates average intensity from the peak acoustic pressure of a progressive sinusoidal plane wave.</summary>
    public static PowerPerArea FromPeakSoundPressure(ForcePerArea peakPressure, Density density, LinearVelocity soundSpeed)
    {
        return (PowerPerArea)(0.5d * peakPressure.Value * peakPressure.Value / (density.Value * soundSpeed.Value));
    }

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

public readonly partial struct Activity
{
    /// <summary>Calculates the activity of a single radionuclide from its amount and positive half-life.</summary>
    public static Activity FromAmountAndHalfLife(
        AmountOfSubstance radionuclideAmount, Time halfLife, InverseAmountOfSubstance avogadroConstant)
    {
        return (Activity)((radionuclideAmount * avogadroConstant) * (Math.Log(2d) / halfLife.Value));
    }

    /// <summary>Calculates the remaining activity of a single radionuclide after radioactive decay.</summary>
    /// <remarks>The half-life must be positive, elapsed time must be nonnegative, and the nuclide is neither replenished nor removed by other processes.</remarks>
    public Activity AfterDecay(Time elapsed, Time halfLife)
    {
        return this * Math.Pow(0.5d, elapsed.Value / halfLife.Value);
    }
}

public readonly partial struct DoseEquivalent
{
    /// <summary>Calculates dose equivalent at a point from absorbed dose and its mean radiation quality factor.</summary>
    public static DoseEquivalent FromAbsorbedDose(AbsorbedDose dose, double qualityFactor)
    {
        return (DoseEquivalent)(dose.Value * qualityFactor);
    }
}

public readonly partial struct Exposure
{
    /// <summary>Calculates photon exposure from the total charge of ions of one sign and the mass of air.</summary>
    /// <remarks>Use the ion charge produced when the electrons liberated by the photons are fully stopped in air.</remarks>
    public static Exposure FromIonizationInAir(ElectricCharge ionChargeOfOneSign, Mass airMass)
    {
        return (Exposure)Math.Abs((ionChargeOfOneSign / airMass).Value);
    }
}

public readonly partial struct Mass
{
    /// <summary>Calculates this component's mass fraction in a mixture of positive total mass.</summary>
    public double MassFraction(Mass totalMass)
    {
        return Value / totalMass.Value;
    }
}

public readonly partial struct AmountOfSubstance
{
    /// <summary>Calculates this component's amount fraction using the positive total amount of all mixture components.</summary>
    public double AmountFraction(AmountOfSubstance totalAmount)
    {
        return Value / totalAmount.Value;
    }
}

public readonly partial struct AmountOfSubstancePerVolume
{
    /// <summary>Calculates solute concentration after dilution at constant solute amount.</summary>
    public AmountOfSubstancePerVolume AfterDilution(Volume initialSolutionVolume, Volume finalSolutionVolume)
    {
        return (this * initialSolutionVolume) / finalSolutionVolume;
    }

    /// <summary>Calculates the concentration of the same solute after mixing two solutions without reaction or loss.</summary>
    /// <remarks>Use the actual positive final solution volume.</remarks>
    public static AmountOfSubstancePerVolume FromMixing(
        AmountOfSubstancePerVolume firstConcentration, Volume firstVolume,
        AmountOfSubstancePerVolume secondConcentration, Volume secondVolume, Volume finalSolutionVolume)
    {
        var firstAmount = firstConcentration * firstVolume;
        var secondAmount = secondConcentration * secondVolume;
        return (AmountOfSubstance)(firstAmount.Value + secondAmount.Value) / finalSolutionVolume;
    }

    /// <summary>Calculates solute concentration from its mass fraction and molar mass and the density of the complete solution.</summary>
    public static AmountOfSubstancePerVolume FromMassFraction(double soluteMassFraction, Density solutionDensity, MolarMass soluteMolarMass)
    {
        return (solutionDensity * soluteMassFraction) / soluteMolarMass;
    }

    /// <summary>Calculates amount concentration from the molality of a single solute dissolved in a solvent.</summary>
    public static AmountOfSubstancePerVolume FromMolality(
        AmountOfSubstancePerMass molality, Density solutionDensity, MolarMass soluteMolarMass)
    {
        return (AmountOfSubstancePerVolume)(molality.Value * solutionDensity.Value / (1d + molality.Value * soluteMolarMass.Value));
    }
}

public readonly partial struct AmountOfSubstancePerMass
{
    /// <summary>Calculates molality from the amount concentration of a single solute dissolved in a solvent.</summary>
    /// <remarks>The complete solution density must exceed the mass of solute per unit solution volume.</remarks>
    public static AmountOfSubstancePerMass FromAmountConcentration(
        AmountOfSubstancePerVolume concentration, Density solutionDensity, MolarMass soluteMolarMass)
    {
        var soluteDensity = concentration * soluteMolarMass;
        return (AmountOfSubstancePerMass)(concentration.Value / (solutionDensity.Value - soluteDensity.Value));
    }
}
