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

    /// <summary>Calculates the half-power cutoff frequency of a first-order RC or RL filter with positive time constant.</summary>
    public static Frequency FromFirstOrderCutoff(Time timeConstant)
    {
        return AngularVelocity.FromRadianPerSecond(1d / timeConstant.Second).ToFrequency();
    }

    /// <summary>Calculates the full half-power bandwidth of a linear series RLC circuit.</summary>
    /// <remarks>Use the total positive series resistance and positive inductance.</remarks>
    public static Frequency FromSeriesRlcBandwidth(Resistance resistance, Inductance inductance)
    {
        return AngularVelocity.FromRadianPerSecond(resistance.Ohm / inductance.Henry).ToFrequency();
    }

    /// <summary>Calculates the fundamental frequency of an ideal tube open at both ends using positive sound speed and effective length.</summary>
    public static Frequency FromHalfWaveResonance(LinearVelocity waveSpeed, Length resonatorLength)
    {
        return FromWaveSpeed(waveSpeed, 2d * resonatorLength);
    }

    /// <summary>Calculates the fundamental frequency of an ideal tube open at one end and closed at the other using positive sound speed and effective length.</summary>
    public static Frequency FromQuarterWaveResonance(LinearVelocity waveSpeed, Length resonatorLength)
    {
        return FromWaveSpeed(waveSpeed, 4d * resonatorLength);
    }

    /// <summary>Calculates the quality factor of a linear series RLC circuit from this resonance frequency and its positive half-power bandwidth.</summary>
    public double SeriesRlcQualityFactor(Frequency halfPowerBandwidth)
    {
        return Hertz / halfPowerBandwidth.Hertz;
    }

    /// <summary>Calculates the observed frequency for motion along the line of sight in vacuum.</summary>
    /// <param name="recessionVelocity">Signed relative velocity, positive for recession and negative for approach, with magnitude below the vacuum light speed.</param>
    public Frequency ObserveLongitudinalDoppler(LinearVelocity recessionVelocity)
    {
        var beta = recessionVelocity.Value / PhysicalConstants.VacuumLightSpeed;
        return FromHertz(Hertz * Math.Sqrt((1d - beta) / (1d + beta)));
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

    /// <summary>Calculates the approximate Rayleigh angular resolution of a circular aperture in the small-angle regime.</summary>
    public static PlaneAngle FromRayleighCriterion(Length wavelength, Length apertureDiameter)
    {
        return FromRadian(1.22d * (wavelength / apertureDiameter));
    }

    /// <summary>Calculates numerical aperture from this objective's acceptance half-angle and the positive object-space refractive index.</summary>
    public double NumericalAperture(double refractiveIndex)
    {
        return refractiveIndex * Math.Sin(Radian);
    }

    /// <summary>Calculates the principal first-order diffraction angle for normal incidence from positive wavelength and grating period supporting that order.</summary>
    public static PlaneAngle FromFirstDiffractionOrder(Length wavelength, Length gratingPeriod)
    {
        return FromRadian(Math.Asin(wavelength / gratingPeriod));
    }

    /// <summary>Calculates the first single-slit diffraction minimum angle for normal incidence from positive wavelength and a slit width at least as large.</summary>
    public static PlaneAngle FromFirstSingleSlitMinimum(Length wavelength, Length slitWidth)
    {
        return FromRadian(Math.Asin(wavelength / slitWidth));
    }

    /// <summary>Calculates the transmitted angle for a ray incident at this angle on a planar optical interface.</summary>
    /// <param name="incidentToTransmittedIndexRatio">The positive ratio of the incident and transmitted refractive indices.</param>
    /// <param name="transmittedAngle">The signed transmitted angle measured from the normal when a real angle exists.</param>
    /// <returns>True for a real transmitted angle and false for total internal reflection.</returns>
    public bool TryRefractionAngle(double incidentToTransmittedIndexRatio, out PlaneAngle transmittedAngle)
    {
        var sine = incidentToTransmittedIndexRatio * Math.Sin(Radian);
        if (Math.Abs(sine) <= 1d)
        {
            transmittedAngle = FromRadian(Math.Asin(sine));
            return true;
        }

        transmittedAngle = default;
        return false;
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

    /// <summary>Calculates hydraulic diameter from flow area and positive wetted perimeter.</summary>
    public static Length HydraulicDiameter(Area flowArea, Length wettedPerimeter)
    {
        return 4d * (flowArea / wettedPerimeter);
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

    /// <summary>Calculates approximate lateral Rayleigh resolution for a diffraction-limited microscope with positive numerical aperture.</summary>
    public static Length RayleighMicroscopeResolution(Length vacuumWavelength, double numericalAperture)
    {
        return vacuumWavelength * (0.61d / numericalAperture);
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

    /// <summary>Calculates stopping distance along a straight line from nonnegative initial speed and positive constant deceleration magnitude.</summary>
    public static Length FromConstantStoppingDeceleration(LinearVelocity initialSpeed, LinearAcceleration deceleration)
    {
        return (Length)(initialSpeed.Value * initialSpeed.Value / (2d * deceleration.Value));
    }

    /// <summary>Calculates the observed length from this proper length parallel to relative motion and a Lorentz factor of at least one.</summary>
    public Length ContractAlongMotion(double lorentzFactor)
    {
        return this / lorentzFactor;
    }

    /// <summary>Calculates the radius of gyration of a body about the axis used for its nonnegative moment of inertia, given its positive mass.</summary>
    public static Length RadiusOfGyration(MomentOfInertia momentOfInertia, Mass mass)
    {
        return FromMetre(Math.Sqrt(momentOfInertia.Value / mass.Value));
    }

    /// <summary>Calculates RMS displacement for one-dimensional unbounded diffusion with nonnegative constant diffusivity and elapsed time.</summary>
    public static Length RmsDisplacementInOneDimensionalDiffusion(AreaPerTime diffusivity, Time elapsed)
    {
        return FromMetre(Math.Sqrt(2d * diffusivity.Value * elapsed.Second));
    }

    /// <summary>Calculates the de Broglie wavelength from positive momentum magnitude using the SI Planck constant.</summary>
    public static Length FromDeBroglieMomentum(LinearMomentum momentum)
    {
        return FromMetre(PhysicalConstants.PlanckConstant / momentum.Value);
    }

    /// <summary>Calculates hydraulic depth in an open channel from the cross-sectional flow area and positive width at the water surface.</summary>
    public static Length HydraulicDepth(Area flowArea, Length topWidth)
    {
        return flowArea / topWidth;
    }

    /// <summary>Calculates the Schwarzschild horizon radius for a nonrotating, uncharged mass from its positive standard gravitational parameter.</summary>
    public static Length SchwarzschildRadius(StandardGravitationalParameter gravitationalParameter)
    {
        return FromMetre(2d * (gravitationalParameter.Value / PhysicalConstants.VacuumLightSpeed) / PhysicalConstants.VacuumLightSpeed);
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

    /// <summary>Calculates the total surface area of a closed right circular cylinder with positive radius and height.</summary>
    public static Area FromClosedCircularCylinder(Length radius, Length height)
    {
        return 2d * Area.FromCircle(radius) + Length.CircumferenceFromRadius(radius) * height;
    }

    /// <summary>Calculates the projected area of this plane surface along a direction between zero and ninety degrees from the surface normal.</summary>
    public Area ProjectAlong(PlaneAngle angleFromNormal)
    {
        return this * Math.Cos(angleFromNormal.Radian);
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

    /// <summary>Calculates the volume of a cone with a circular base of positive radius and positive perpendicular height.</summary>
    public static Volume FromCircularCone(Length radius, Length height)
    {
        return Area.FromCircle(radius) * height / 3d;
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
}

public readonly partial struct EnergyDensity
{
    /// <summary>Calculates energy density from instantaneous electric field magnitude in a linear, isotropic, nondispersive medium.</summary>
    public static EnergyDensity FromElectricField(ElectricFieldStrength field, Permittivity permittivity)
    {
        return (EnergyDensity)(0.5d * permittivity.Value * field.Value * field.Value);
    }

    /// <summary>Calculates energy density from instantaneous magnetic flux density magnitude in a linear, isotropic, nondispersive medium.</summary>
    public static EnergyDensity FromMagneticField(MagneticFluxDensity field, Permeability permeability)
    {
        return (EnergyDensity)(0.5d * field.Value * field.Value / permeability.Value);
    }

    /// <summary>Calculates recoverable energy per unit volume for a single linearly elastic stress and its conjugate strain.</summary>
    public static EnergyDensity FromElasticStrain(Stress stress, double strain)
    {
        return (EnergyDensity)(0.5d * stress.Value * strain);
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

    /// <summary>Calculates heat engine efficiency from this net work output and the positive heat input over the same cycle.</summary>
    public double HeatEngineEfficiency(Energy heatInput)
    {
        return Value / heatInput.Value;
    }

    /// <summary>Calculates the rest energy of a body using the positive vacuum speed of light.</summary>
    public static Energy FromRestMass(Mass restMass, LinearVelocity vacuumLightSpeed)
    {
        return (Energy)(restMass.Value * vacuumLightSpeed.Value * vacuumLightSpeed.Value);
    }

    /// <summary>Calculates photon energy in vacuum from momentum magnitude or signed collinear momentum.</summary>
    public static Energy FromPhotonMomentum(LinearMomentum momentum, LinearVelocity vacuumLightSpeed)
    {
        return (Energy)(Math.Abs(momentum.Value) * vacuumLightSpeed.Value);
    }

    /// <summary>Calculates photon energy in a downward bound-state transition from the upper and lower level energies.</summary>
    public static Energy FromEmissionTransition(Energy upperLevel, Energy lowerLevel)
    {
        return (Energy)(upperLevel.Value - lowerLevel.Value);
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

    /// <summary>Calculates nonrelativistic translational kinetic energy from momentum magnitude and positive mass.</summary>
    public static Energy FromMomentumKineticEnergy(LinearMomentum momentum, Mass mass)
    {
        return (Energy)(0.5d * momentum.Value * (momentum.Value / mass.Value));
    }

    /// <summary>Calculates the change in internal energy of a closed system with no change in macroscopic kinetic or potential energy.</summary>
    /// <param name="heatAdded">Signed heat transferred into the system.</param>
    /// <param name="workDoneBySystem">Signed work done by the system on its surroundings.</param>
    public static Energy FromHeatAndWork(Energy heatAdded, Energy workDoneBySystem)
    {
        return FromJoule(heatAdded.Joule - workDoneBySystem.Joule);
    }

    /// <summary>Calculates orientation-dependent potential energy from the signed parallel dipole component and a uniform electric field magnitude.</summary>
    public static Energy FromElectricDipole(ElectricDipoleMoment parallelDipoleMoment, ElectricFieldStrength field)
    {
        return (Energy)(-parallelDipoleMoment.Value * field.Value);
    }

    /// <summary>Calculates orientation-dependent potential energy from the signed parallel dipole component and a uniform magnetic field magnitude in vacuum.</summary>
    public static Energy FromMagneticDipole(MagneticDipoleMoment parallelDipoleMoment, AuxillaryMagneticField field)
    {
        return (Energy)(-parallelDipoleMoment.Value * field.Value);
    }

    /// <summary>Calculates a massive body's relativistic kinetic energy from its rest energy and Lorentz factor of at least one.</summary>
    public static Energy FromRelativisticKineticEnergy(Energy restEnergy, double lorentzFactor)
    {
        return FromJoule(restEnergy.Joule * (lorentzFactor - 1d));
    }

    /// <summary>Calculates the energy of one photon from its positive frequency using the SI Planck constant.</summary>
    public static Energy FromPhotonFrequency(Frequency frequency)
    {
        return FromJoule(PhysicalConstants.PlanckConstant * frequency.Hertz);
    }

    /// <summary>Calculates the frequency of one photon from its positive energy using the SI Planck constant.</summary>
    public Frequency ToPhotonFrequency()
    {
        return Frequency.FromHertz(Joule / PhysicalConstants.PlanckConstant);
    }

    /// <summary>Calculates the internal energy in the classical monatomic ideal-gas model from the amount and positive absolute temperature.</summary>
    public static Energy FromMonatomicIdealGas(AmountOfSubstance amount, ThermodynamicTemperature temperature)
    {
        return FromJoule(1.5d * amount.Mole * PhysicalConstants.MolarGasConstant * temperature.Kelvin);
    }
}

public readonly partial struct LinearMomentum
{
    /// <summary>Calculates photon momentum magnitude in vacuum from energy and the positive speed of light.</summary>
    public static LinearMomentum FromPhotonEnergy(Energy photonEnergy, LinearVelocity vacuumLightSpeed)
    {
        return (LinearMomentum)(photonEnergy.Value / vacuumLightSpeed.Value);
    }
}

public readonly partial struct Force
{
    /// <summary>Calculates the signed restoring force of a linear spring for a displacement from equilibrium.</summary>
    public static Force FromSpringDisplacement(ForcePerLength stiffness, Length displacement)
    {
        return -(stiffness * displacement);
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

    /// <summary>Calculates the torque magnitude on an electric dipole from its perpendicular moment and a uniform electric field magnitude.</summary>
    public static Torque FromElectricDipole(ElectricDipoleMoment perpendicularDipoleMoment, ElectricFieldStrength field)
    {
        return (Torque)(perpendicularDipoleMoment.Value * field.Value);
    }

    /// <summary>Calculates the torque magnitude on a magnetic dipole from its perpendicular moment and a uniform magnetic field magnitude in vacuum.</summary>
    public static Torque FromMagneticDipole(MagneticDipoleMoment perpendicularDipoleMoment, AuxillaryMagneticField field)
    {
        return (Torque)(perpendicularDipoleMoment.Value * field.Value);
    }
}

public readonly partial struct ForcePerArea
{
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

    /// <summary>Calculates the pressure difference corresponding to power transferred to a fluid flow.</summary>
    /// <param name="hydraulicPower">The power transferred through the pressure difference.</param>
    /// <param name="flow">The corresponding volumetric flow rate.</param>
    /// <returns>The pressure difference for the given power and flow.</returns>
    public static ForcePerArea FromHydraulicPower(Power hydraulicPower, VolumePerTime flow)
    {
        return hydraulicPower / flow;
    }

    /// <summary>Calculates the inside-to-outside pressure jump across a single spherical interface.</summary>
    public static ForcePerArea FromSphericalSurfaceTension(ForcePerLength surfaceTension, Length radius)
    {
        return (ForcePerArea)(2d * surfaceTension.Value / radius.Value);
    }

    /// <summary>Calculates radiation pressure on a perfect absorber from normally incident irradiance and positive vacuum light speed.</summary>
    public static ForcePerArea FromAbsorbedRadiation(PowerPerArea irradiance, LinearVelocity vacuumLightSpeed)
    {
        return (ForcePerArea)(irradiance.Value / vacuumLightSpeed.Value);
    }

    /// <summary>Calculates radiation pressure from normally incident irradiance reflected specularly and without loss in vacuum.</summary>
    public static ForcePerArea FromReflectedRadiation(PowerPerArea irradiance, LinearVelocity vacuumLightSpeed)
    {
        return 2d * FromAbsorbedRadiation(irradiance, vacuumLightSpeed);
    }

    /// <summary>Calculates the absolute pressure of an ideal gas at the specified amount concentration and positive absolute temperature.</summary>
    public static ForcePerArea FromIdealGasConcentration(AmountOfSubstancePerVolume concentration, ThermodynamicTemperature temperature)
    {
        return FromPascal(concentration.Value * PhysicalConstants.MolarGasConstant * temperature.Kelvin);
    }

    /// <summary>Calculates absolute pressure from signed gauge pressure and the absolute pressure used as its reference.</summary>
    public static ForcePerArea FromGaugePressure(ForcePerArea gaugePressure, ForcePerArea referencePressure)
    {
        return FromPascal(gaugePressure.Pascal + referencePressure.Pascal);
    }

    /// <summary>Calculates signed gauge pressure relative to the specified absolute reference pressure.</summary>
    public ForcePerArea GaugePressureRelativeTo(ForcePerArea referencePressure)
    {
        return FromPascal(Pascal - referencePressure.Pascal);
    }

    /// <summary>Calculates stagnation pressure from static and dynamic pressure for incompressible flow brought to rest without losses or a change in elevation.</summary>
    public static ForcePerArea FromIncompressibleStagnation(ForcePerArea staticPressure, ForcePerArea dynamicPressure)
    {
        return FromPascal(staticPressure.Pascal + dynamicPressure.Pascal);
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

    /// <summary>Calculates peak speed in simple harmonic motion from nonnegative amplitude and positive angular frequency.</summary>
    public static LinearVelocity PeakSpeedInSimpleHarmonicMotion(Length amplitude, AngularVelocity angularFrequency)
    {
        return amplitude * angularFrequency;
    }

    /// <summary>Calculates the open-channel Froude number from this mean flow speed magnitude and its positive gravity-wave speed relative to the flow.</summary>
    public double FroudeNumber(LinearVelocity relativeGravityWaveSpeed)
    {
        return Value / relativeGravityWaveSpeed.Value;
    }

    /// <summary>Calculates the molecular RMS speed at the positive absolute temperature and molar mass of a classical ideal gas in thermal equilibrium.</summary>
    public static LinearVelocity RmsSpeedOfIdealGas(ThermodynamicTemperature temperature, MolarMass molarMass)
    {
        return FromMetrePerSecond(Math.Sqrt(3d * PhysicalConstants.MolarGasConstant * temperature.Kelvin / molarMass.Value));
    }

    /// <summary>Transforms this signed velocity between collinear inertial frames using the signed velocity of the intermediate frame.</summary>
    /// <remarks>Both speeds have magnitudes below the vacuum light speed. All velocities are measured along the same positive direction.</remarks>
    public LinearVelocity AddCollinearRelativisticVelocity(LinearVelocity frameVelocity)
    {
        var beta = Value / PhysicalConstants.VacuumLightSpeed;
        var frameBeta = frameVelocity.Value / PhysicalConstants.VacuumLightSpeed;
        return FromMetrePerSecond((Value + frameVelocity.Value) / (1d + beta * frameBeta));
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

    /// <summary>Calculates peak acceleration in simple harmonic motion from nonnegative amplitude and positive angular frequency.</summary>
    public static LinearAcceleration PeakAccelerationInSimpleHarmonicMotion(Length amplitude, AngularVelocity angularFrequency)
    {
        return (LinearAcceleration)(amplitude.Value * angularFrequency.RadianPerSecond * angularFrequency.RadianPerSecond);
    }
}

public readonly partial struct Time
{
    /// <summary>Calculates the period of a Newtonian circular orbit about a spherical central body.</summary>
    public static Time FromCircularOrbit(StandardGravitationalParameter gravitationalParameter, Length radius)
    {
        return FromKeplerOrbit(gravitationalParameter, radius);
    }

    /// <summary>Calculates the period of a Newtonian elliptic orbit from positive semi-major axis and gravitational parameter.</summary>
    public static Time FromKeplerOrbit(StandardGravitationalParameter gravitationalParameter, Length semiMajorAxis)
    {
        var axis = semiMajorAxis.Metre;
        return FromSecond((2d * Math.PI * axis) * Math.Sqrt(axis / gravitationalParameter.Value));
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

    /// <summary>Calculates the inertial observer's elapsed time from this proper duration and a Lorentz factor of at least one.</summary>
    public Time Dilate(double lorentzFactor)
    {
        return this * lorentzFactor;
    }

    /// <summary>Calculates the fraction of a signal remaining after this nonnegative elapsed time with a positive exponential decay time constant.</summary>
    public double ExponentialDecayFraction(Time timeConstant)
    {
        return Math.Exp(-Second / timeConstant.Second);
    }

    /// <summary>Calculates the half-life corresponding to this positive exponential decay time constant.</summary>
    public Time HalfLifeFromDecayTimeConstant()
    {
        return this * Math.Log(2d);
    }

    /// <summary>Calculates the exponential decay time constant from a positive half-life.</summary>
    public static Time FromExponentialHalfLife(Time halfLife)
    {
        return halfLife / Math.Log(2d);
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

public readonly partial struct Power
{
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

    /// <summary>Calculates the fraction of normally incident optical power reflected at an interface between lossless nonmagnetic media.</summary>
    /// <param name="incidentIndex">The positive refractive index of the incident medium.</param>
    /// <param name="transmittedIndex">The positive refractive index of the transmitted medium.</param>
    public static double ReflectanceAtNormalIncidence(double incidentIndex, double transmittedIndex)
    {
        var amplitude = (incidentIndex - transmittedIndex) / (incidentIndex + transmittedIndex);
        return amplitude * amplitude;
    }
}

public readonly partial struct MagneticFluxDensity
{
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
}

public readonly partial struct Capacitance
{
    /// <summary>Calculates capacitance of an isolated conducting sphere in an unbounded homogeneous linear dielectric.</summary>
    public static Capacitance FromIsolatedSphere(Permittivity permittivity, Length radius)
    {
        return FromFarad((4d * Math.PI) * permittivity.Value * radius.Value);
    }

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
}

public readonly partial struct ThermodynamicTemperature
{
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

    /// <summary>Calculates the absolute temperature of an ideal gas from its positive absolute pressure and amount concentration.</summary>
    public static ThermodynamicTemperature FromIdealGasPressure(ForcePerArea absolutePressure, AmountOfSubstancePerVolume concentration)
    {
        return FromKelvin(absolutePressure.Pascal / (concentration.Value * PhysicalConstants.MolarGasConstant));
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
    /// <summary>Calculates heat flux from the signed driving temperature difference in kelvins.</summary>
    public HeatFlowRatePerArea HeatFluxForTemperatureChange(double deltaKelvins)
    {
        return (HeatFlowRatePerArea)(Value * deltaKelvins);
    }
}

public readonly partial struct ThermalResistance
{
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

public readonly partial struct LuminousIntensity
{
    /// <summary>Calculates luminous intensity at an angle to the normal of a Lambertian source, starting from its normal intensity.</summary>
    public LuminousIntensity AtLambertianAngle(PlaneAngle angleFromNormal)
    {
        return this * Math.Cos(angleFromNormal.Radian);
    }

    /// <summary>Calculates total luminous flux into one hemisphere from this Lambertian source's normal intensity.</summary>
    public LuminousFlux ToLambertianFlux()
    {
        return this * SolidAngle.FromSteradian(Math.PI);
    }
}

public readonly partial struct Luminance
{
    /// <summary>Calculates hemispherical luminous exitance from this uniform Lambertian luminance.</summary>
    public LuminousFluxPerArea ToLambertianExitance()
    {
        return (LuminousFluxPerArea)(Math.PI * Value);
    }
}

public readonly partial struct LuminousFluxPerArea
{
    /// <summary>Calculates average luminous exitance from emitted luminous flux and the emitting area.</summary>
    public static LuminousFluxPerArea FromEmittedFlux(LuminousFlux emittedFlux, Area emittingArea)
    {
        return (LuminousFluxPerArea)(emittedFlux.Value / emittingArea.Value);
    }
}

public readonly partial struct ElectricDisplacement
{
    /// <summary>Calculates average normal electric displacement from its signed flux through the area.</summary>
    public static ElectricDisplacement FromDisplacementFlux(ElectricCharge displacementFlux, Area area)
    {
        return (ElectricDisplacement)(displacementFlux.Value / area.Value);
    }
}

public readonly partial struct Permittivity
{
    /// <summary>Calculates this absolute permittivity relative to positive vacuum permittivity.</summary>
    public double RelativePermittivity(Permittivity vacuumPermittivity)
    {
        return Value / vacuumPermittivity.Value;
    }

    /// <summary>Calculates absolute permittivity from relative permittivity and vacuum permittivity.</summary>
    public static Permittivity FromRelativePermittivity(double relativePermittivity, Permittivity vacuumPermittivity)
    {
        return vacuumPermittivity * relativePermittivity;
    }
}

public readonly partial struct Permeability
{
    /// <summary>Calculates this absolute permeability relative to positive vacuum permeability.</summary>
    public double RelativePermeability(Permeability vacuumPermeability)
    {
        return Value / vacuumPermeability.Value;
    }

    /// <summary>Calculates absolute permeability from relative permeability and vacuum permeability.</summary>
    public static Permeability FromRelativePermeability(double relativePermeability, Permeability vacuumPermeability)
    {
        return vacuumPermeability * relativePermeability;
    }
}

public readonly partial struct RadiantIntensity
{
    /// <summary>Calculates radiant intensity at an angle to the normal of a Lambertian source, starting from its normal intensity.</summary>
    public RadiantIntensity AtLambertianAngle(PlaneAngle angleFromNormal)
    {
        return this * Math.Cos(angleFromNormal.Radian);
    }

    /// <summary>Calculates total radiant power into one hemisphere from this Lambertian source's normal intensity.</summary>
    public Power ToLambertianFlux()
    {
        return this * SolidAngle.FromSteradian(Math.PI);
    }
}

public readonly partial struct PowerPerArea
{
    /// <summary>Calculates irradiance transmitted through an ideal polarizer for linearly polarized incident light.</summary>
    /// <param name="axisAngle">Angle between the incident polarization and the transmission axis.</param>
    public PowerPerArea AfterIdealPolarizer(PlaneAngle axisAngle)
    {
        var cosine = Math.Cos(axisAngle.Radian);
        return this * (cosine * cosine);
    }

    /// <summary>Calculates intensity at positive distance from an isotropic source in a lossless medium.</summary>
    public static PowerPerArea FromIsotropicSource(Power emittedPower, Length distance)
    {
        return emittedPower / Area.FromSphere(distance);
    }

    /// <summary>Calculates irradiance for normal incidence from a point source at a positive distance.</summary>
    /// <param name="intensity">The radiant intensity in the direction of the receiving surface.</param>
    /// <param name="distance">The distance from the point source to the surface.</param>
    public static PowerPerArea FromNormalPointSource(RadiantIntensity intensity, Length distance)
    {
        return (PowerPerArea)(intensity.Value / (distance * distance).Value);
    }

    /// <summary>Calculates total blackbody radiant exitance from absolute temperature and the Stefan–Boltzmann constant.</summary>
    public static PowerPerArea FromBlackbodyTemperature(ThermodynamicTemperature temperature, PowerPerAreaQuarticTemperature stefanBoltzmannConstant)
    {
        return (PowerPerArea)(stefanBoltzmannConstant.Value * Math.Pow(temperature.Kelvin, 4d));
    }

    /// <summary>Calculates average intensity of a progressive plane sound wave from its in-phase RMS pressure and particle velocity.</summary>
    public static PowerPerArea FromProgressiveSoundWave(ForcePerArea rmsSoundPressure, LinearVelocity rmsParticleVelocity)
    {
        return (PowerPerArea)(rmsSoundPressure.Value * rmsParticleVelocity.Value);
    }

    /// <summary>Calculates the sound intensity level difference in decibels for two positive acoustic intensities using the second as reference.</summary>
    public static double SoundIntensityLevelDifferenceInDecibels(PowerPerArea intensity, PowerPerArea referenceIntensity)
    {
        return 10d * (Math.Log10(intensity.Value) - Math.Log10(referenceIntensity.Value));
    }

    /// <summary>Calculates the instantaneous Poynting flux magnitude from orthogonal electric and auxiliary magnetic field magnitudes.</summary>
    public static PowerPerArea FromOrthogonalElectromagneticFields(ElectricFieldStrength electricField, AuxillaryMagneticField magneticField)
    {
        return (PowerPerArea)(electricField.Value * magneticField.Value);
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

public readonly partial struct DoseEquivalent
{
    /// <summary>Calculates dose equivalent at a point from absorbed dose and its mean radiation quality factor.</summary>
    public static DoseEquivalent FromAbsorbedDose(AbsorbedDose dose, double qualityFactor)
    {
        return (DoseEquivalent)(dose.Value * qualityFactor);
    }
}

public readonly partial struct Activity
{
    /// <summary>Calculates average activity from the number of nuclear transformations in the sample over a positive interval.</summary>
    public static Activity FromDecayCount(double decayCount, Time elapsed)
    {
        return (Activity)(decayCount / elapsed.Second);
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

public readonly partial struct SpecificEnergy
{
    /// <summary>Calculates gravitational potential relative to infinity at positive distance from a point mass or outside a spherical mass.</summary>
    public static SpecificEnergy FromNewtonianGravitationalPotential(StandardGravitationalParameter gravitationalParameter, Length radius)
    {
        return (SpecificEnergy)(-gravitationalParameter.Value / radius.Value);
    }

    /// <summary>Calculates the change in gravitational potential energy per mass in a uniform field from positive gravity and signed upward elevation change.</summary>
    public static SpecificEnergy FromUniformGravityElevationChange(LinearAcceleration gravity, Length elevationChange)
    {
        return (SpecificEnergy)(gravity.Value * elevationChange.Value);
    }

    /// <summary>Calculates the total Newtonian orbital energy per mass in a bound Kepler orbit from positive gravitational parameter and semi-major axis.</summary>
    public static SpecificEnergy FromBoundKeplerOrbit(StandardGravitationalParameter gravitationalParameter, Length semiMajorAxis)
    {
        return 0.5d * FromNewtonianGravitationalPotential(gravitationalParameter, semiMajorAxis);
    }
}

public readonly partial struct ElectricPotential
{
    /// <summary>Calculates the RMS voltage of a zero-offset sinusoid from its peak magnitude.</summary>
    public static ElectricPotential FromSinusoidalPeak(ElectricPotential peakVoltage)
    {
        return FromVolt(Math.Abs(peakVoltage.Volt) / Math.Sqrt(2d));
    }

    /// <summary>Calculates the secondary voltage magnitude of an ideal transformer from this primary voltage magnitude and a positive secondary-to-primary turns ratio.</summary>
    public ElectricPotential SecondaryVoltageOfIdealTransformer(double secondaryToPrimaryTurnsRatio)
    {
        return FromVolt(Volt * secondaryToPrimaryTurnsRatio);
    }
}

public readonly partial struct ElectricCurrent
{
    /// <summary>Calculates the RMS current of a zero-offset sinusoid from its peak magnitude.</summary>
    public static ElectricCurrent FromSinusoidalPeak(ElectricCurrent peakCurrent)
    {
        return FromAmpere(Math.Abs(peakCurrent.Ampere) / Math.Sqrt(2d));
    }

    /// <summary>Calculates the secondary current magnitude of an ideal transformer from this primary current magnitude and a positive secondary-to-primary turns ratio.</summary>
    public ElectricCurrent SecondaryCurrentOfIdealTransformer(double secondaryToPrimaryTurnsRatio)
    {
        return FromAmpere(Ampere / secondaryToPrimaryTurnsRatio);
    }
}

public readonly partial struct ForcePerLength
{
    /// <summary>Calculates the magnetic force magnitude per unit length on a straight wire from nonnegative current and its perpendicular magnetic flux density magnitude.</summary>
    public static ForcePerLength FromCurrentInMagneticField(ElectricCurrent current, MagneticFluxDensity perpendicularField)
    {
        return (ForcePerLength)(current.Value * perpendicularField.Value);
    }
}

public readonly partial struct InverseLength
{
    /// <summary>Calculates the surviving intensity fraction of a monoenergetic narrow beam through homogeneous material of nonnegative thickness using this nonnegative linear attenuation coefficient.</summary>
    public double TransmissionThrough(Length thickness)
    {
        return Math.Exp(-Value * thickness.Value);
    }

    /// <summary>Calculates angular wavenumber in radians per metre from a positive wavelength.</summary>
    public static InverseLength AngularWavenumberFromWavelength(Length wavelength)
    {
        return (InverseLength)(PlaneAngle.FromRevolution(1d).Radian / wavelength.Value);
    }

    /// <summary>Calculates spectroscopic wavenumber as the reciprocal of a positive vacuum wavelength.</summary>
    public static InverseLength SpectroscopicWavenumberFromWavelength(Length vacuumWavelength)
    {
        return (InverseLength)(1d / vacuumWavelength.Value);
    }
}

public readonly partial struct ThermalDiffusivity
{
    /// <summary>Calculates the Lewis number from this thermal diffusivity and the positive molecular diffusivity of the same species and fluid state.</summary>
    public double LewisNumber(AreaPerTime molecularDiffusivity)
    {
        return Value / molecularDiffusivity.Value;
    }
}

public readonly partial struct KinematicViscosity
{
    /// <summary>Calculates the Schmidt number from this kinematic viscosity and the positive molecular diffusivity for the same fluid state.</summary>
    public double SchmidtNumber(AreaPerTime molecularDiffusivity)
    {
        return Value / molecularDiffusivity.Value;
    }
}

internal static class PhysicalConstants
{
    // Coherent SI values from the 2022 CODATA table.
    internal const double VacuumLightSpeed = 299_792_458d;
    internal const double PlanckConstant = 6.626_070_15e-34;
    internal const double MolarGasConstant = 8.314_462_618_153_24;
}

public readonly partial struct AuxillaryMagneticField
{
    /// <summary>Calculates the azimuthal magnetic field around a long straight wire carrying steady current in a homogeneous isotropic medium.</summary>
    /// <param name="current">Signed current along the wire.</param>
    /// <param name="radialDistance">Positive perpendicular distance from the wire axis.</param>
    public static AuxillaryMagneticField FromLongStraightWire(ElectricCurrent current, Length radialDistance)
    {
        return (AuxillaryMagneticField)(current.Value / (PlaneAngle.FromRevolution(1d).Radian * radialDistance.Value));
    }
}

public readonly partial struct AmountOfSubstancePerVolume
{
    /// <summary>Calculates the amount concentration of an ideal gas from its positive absolute pressure and temperature.</summary>
    public static AmountOfSubstancePerVolume FromIdealGasPressure(ForcePerArea absolutePressure, ThermodynamicTemperature temperature)
    {
        return (AmountOfSubstancePerVolume)(absolutePressure.Pascal / (PhysicalConstants.MolarGasConstant * temperature.Kelvin));
    }
}

public readonly partial struct ThermalConductivity
{
    /// <summary>Calculates one-dimensional conductive heat flux along the axis of the signed temperature gradient in kelvins per metre.</summary>
    public HeatFlowRatePerArea HeatFluxForTemperatureGradient(double kelvinsPerMetre)
    {
        return (HeatFlowRatePerArea)(-Value * kelvinsPerMetre);
    }

    /// <summary>Calculates the signed temperature gradient in kelvins per metre from heat flux along the same axis and this positive conductivity.</summary>
    public double TemperatureGradientInKelvinsPerMetre(HeatFlowRatePerArea heatFlux)
    {
        return -heatFlux.Value / Value;
    }
}

public readonly partial struct MolarHeatCapacity
{
    /// <summary>Calculates the constant-pressure molar heat capacity of an ideal gas from this constant-volume capacity at the same temperature.</summary>
    public MolarHeatCapacity ToIsobaricForIdealGas()
    {
        return (MolarHeatCapacity)(Value + PhysicalConstants.MolarGasConstant);
    }

    /// <summary>Calculates the constant-volume molar heat capacity of an ideal gas from this constant-pressure capacity at the same temperature.</summary>
    public MolarHeatCapacity ToIsochoricForIdealGas()
    {
        return (MolarHeatCapacity)(Value - PhysicalConstants.MolarGasConstant);
    }
}
