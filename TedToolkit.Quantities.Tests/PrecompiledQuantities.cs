extern alias SiDouble;

using Q = SiDouble::TedToolkit.Quantities;

internal static class PrecompiledQuantities
{
    internal static void Verify()
    {
        var distance = Q.Length.FromMillimetre(600);
        var elapsed = Q.Time.FromSecond(30);
        Q.LinearVelocity speed = distance / elapsed;
        Close(0.02, speed.MetrePerSecond);
        Close(1200, speed.MillimetrePerMinute);
        Close(20, Q.LinearVelocity.FromMillimetrePerMinute(1200).MillimetrePerSecond);
        Close(25.4, Q.Length.From(1, Q.LengthUnit.Inch_IN).Millimetre);
        Close(600, (speed * elapsed).Millimetre);
        Close(30, (distance / speed).Second);

        Q.Area area = Q.Length.FromMillimetre(2000) * Q.Length.FromMetre(3);
        Q.Volume volume = area * Q.Length.FromMetre(4);
        Close(6, area.SquareMetre);
        Q.AreaPerTime areaRate = area / elapsed;
        Close(0.2, areaRate.Value);
        Close(6, (areaRate * elapsed).SquareMetre);
        Close(30, (area / areaRate).Second);
        Close(24, volume.CubicMetre);
        Close(4, (volume / area).Metre);
        Close(2, distance / Q.Length.FromMillimetre(300));

        var amount = Q.AmountOfSubstance.FromMole(2.5);
        Close(2500, amount.Millimole);

        var startTemperature = Q.ThermodynamicTemperature.FromKelvin(280);
        var endTemperature = Q.ThermodynamicTemperature.FromKelvin(380);
        double interval = endTemperature.DifferenceInKelvins(startTemperature);
        Close(280, startTemperature.Value);
        Close(100, interval);
        Close(-100, startTemperature.DifferenceInKelvins(endTemperature));
        Close(380, startTemperature.AddKelvins(interval).Kelvin);

        Q.SquareEnergy squaredEnergy = Q.Energy.FromJoule(3) * Q.Energy.FromJoule(4);
        Close(12, squaredEnergy.Value);
        Q.SquareTime squaredTime = Q.Time.FromSecond(2) * Q.Time.FromSecond(5);
        Close(10, squaredTime.Value);
        Close(5, (squaredTime / Q.Time.FromSecond(2)).Second);

        VerifyMechanicsAndFlow();
        VerifySolidMechanics();
        VerifyRelativity();
        VerifyWaves();
        VerifyFluidTransport();
        VerifyElectromagnetism();
        VerifyCircuitNetworks();
        VerifyHeatTransfer();
        VerifyThermodynamicProcesses();
        VerifyOptics();
        VerifyElectromagneticWavesAndPhotons();
        VerifyLightChemistryAndDose();
        VerifyRadiationAndMixtures();
        VerifyQuantityConversions();
        VerifyPairRelations();

        if (typeof(Q.Length).Assembly == typeof(TedToolkit.Quantities.Length).Assembly)
        {
            throw new InvalidOperationException("Unexpected quantity type.");
        }
    }

    private static void VerifyQuantityConversions()
    {
        var stress = (Q.Stress)4d;
        Q.ForcePerArea forcePerArea = stress;
        var heatFlow = (Q.HeatFlowRate)5d;
        Q.Power power = heatFlow;
        var gravity = (Q.GravitationalAttraction)12d;
        Q.Force force = gravity;
        var light = Q.Illuminance.FromLux(7);
        Q.LuminousFluxPerArea lightPerArea = light;
        Close(4, forcePerArea.Pascal);
        Close(5, power.Watt);
        Close(12, force.Newton);
        Close(7, lightPerArea.Value);

        var energy = Q.Energy.FromJoule(5);
        Q.Torque torque = (Q.Torque)energy;
        Close(5, torque.Value);
        Close(5, ((Q.Energy)torque).Joule);
        Close(4, ((Q.Activity)Q.Frequency.FromHertz(4)).Value);

        if (HasConversion(typeof(Q.Force), typeof(Q.GravitationalAttraction), "op_Implicit")
            || HasConversion(typeof(Q.Energy), typeof(Q.Torque), "op_Implicit")
            || HasConversion(typeof(Q.Torque), typeof(Q.Energy), "op_Implicit")
            || HasConversion(typeof(Q.Length), typeof(Q.Mass), "op_Explicit"))
        {
            throw new InvalidOperationException("Unexpected quantity conversion.");
        }

        static bool HasConversion(Type source, Type target, string name)
        {
            return new[] { source, target }.SelectMany(type => type.GetMethods()).Any(method =>
                method.Name == name && method.ReturnType == target && method.GetParameters()[0].ParameterType == source);
        }
    }

    private static void VerifyMechanicsAndFlow()
    {
        Close(2, Q.Length.SchwarzschildRadius((Q.StandardGravitationalParameter)(299_792_458d * 299_792_458d)).Metre);
        Close(-2.5, Q.SpecificEnergy.FromBoundKeplerOrbit((Q.StandardGravitationalParameter)20d, Q.Length.FromMetre(4)).Value);

        Close(3, Q.Length.RadiusOfGyration((Q.MomentOfInertia)36d, Q.Mass.FromKilogram(4)).Metre);

        Close(7.5, Q.Energy.FromMomentumKineticEnergy((Q.LinearMomentum)Math.Sqrt(60d), Q.Mass.FromKilogram(4)).Joule);
        Close(5, Q.Length.FromConstantStoppingDeceleration(Q.LinearVelocity.FromMetrePerSecond(10), (Q.LinearAcceleration)10d).Metre);
        Close(-5, Q.SpecificEnergy.FromNewtonianGravitationalPotential((Q.StandardGravitationalParameter)20d, Q.Length.FromMetre(4)).Value);
        Close(3 * Math.PI, Q.Volume.FromCircularCone(Q.Length.FromMetre(1), Q.Length.FromMetre(9)).CubicMetre);
        Close(24 * Math.PI, Q.Area.FromClosedCircularCylinder(Q.Length.FromMetre(2), Q.Length.FromMetre(4)).SquareMetre);

        var frequency = Q.Frequency.FromPeriod(Q.Time.FromMillisecond(250));
        Close(4, frequency.Hertz);
        Close(0.25, frequency.ToPeriod().Second);
        var angularFrequency = Q.AngularVelocity.FromFrequency(frequency);
        Close(8 * Math.PI, angularFrequency.RadianPerSecond);
        Close(4, angularFrequency.ToFrequency().Hertz);

        var radius = Q.Length.FromMetre(2);
        var angle = Q.PlaneAngle.FromArc(Q.Length.FromMillimetre(1000), radius);
        Close(0.5, angle.Radian);
        Close(1000, angle.ToArcLength(radius).Millimetre);
        var diameter = Q.Length.FromMillimetre(200);
        var circleRadius = Q.Length.RadiusFromDiameter(diameter);
        Close(100, circleRadius.Millimetre);
        Close(200, Q.Length.DiameterFromRadius(circleRadius).Millimetre);
        Close(0.2 * Math.PI, Q.Length.CircumferenceFromRadius(circleRadius).Metre);
        Close(0.01 * Math.PI, Q.Area.FromCircle(circleRadius).SquareMetre);
        var sphereRadius = Q.Length.FromMillimetre(500);
        Close(Math.PI, Q.Area.FromSphere(sphereRadius).SquareMetre);
        Close(Math.PI / 6, Q.Volume.FromSphere(sphereRadius).CubicMetre);
        var solidAngle = Q.SolidAngle.FromSphericalPatch(Q.Area.FromSquareMetre(0.5), sphereRadius);
        Close(2, solidAngle.Steradian);
        Close(0.5, solidAngle.ToSphericalPatchArea(sphereRadius).SquareMetre);

        var wavelength = Q.Length.FromMillimetre(500);
        Q.LinearVelocity waveSpeed = Q.LinearVelocity.FromWavelength(wavelength, frequency);
        Close(2, waveSpeed.MetrePerSecond);
        Close(4, Q.Frequency.FromWaveSpeed(waveSpeed, wavelength).Hertz);
        Close(500, Q.Length.WavelengthFromSpeed(waveSpeed, frequency).Millimetre);

        var angularAcceleration = (Q.AngularAcceleration)3d;
        Q.Torque torque = (Q.MomentOfInertia)2d * angularAcceleration;
        Q.AngularVelocity angularVelocity = angularAcceleration * Q.Time.FromSecond(4);
        Q.Power shaftPower = torque * angularVelocity;
        Close(72, shaftPower.Watt);
        Close(torque.Value, (shaftPower / angularVelocity).Value);
        Close(12, (angularVelocity * Q.Time.FromSecond(1)).Radian);
        Q.LinearVelocity tangentialVelocity = radius * angularVelocity;
        Q.LinearAcceleration tangentialAcceleration = angularAcceleration * radius;
        Close(24, tangentialVelocity.MetrePerSecond);
        Close(6, tangentialAcceleration.Value);
        Close(12, (tangentialVelocity / radius).RadianPerSecond);
        Close(2, (tangentialAcceleration / angularAcceleration).Metre);
        Close(288, Q.LinearAcceleration.FromCentripetalMotion(tangentialVelocity, radius).Value);
        Close(288, Q.LinearAcceleration.FromCentripetalMotion(angularVelocity, radius).Value);

        var stiffness = (Q.ForcePerLength)200d;
        var displacement = Q.Length.FromMillimetre(40);
        Close(-8, Q.Force.FromSpringDisplacement(stiffness, displacement).Newton);
        Close(0.16, Q.Energy.FromSpringDisplacement(stiffness, displacement).Joule);
        var damping = (Q.MassPerTime)4d;
        Close(-2, Q.Force.FromViscousDamping(damping, Q.LinearVelocity.FromMetrePerSecond(0.5)).Newton);
        var gravitationalParameter = (Q.StandardGravitationalParameter)400d;
        var orbitRadius = Q.Length.FromMillimetre(4000);
        Close(10, Q.LinearVelocity.FromCircularOrbit(gravitationalParameter, orbitRadius).MetrePerSecond);
        Close(0.8 * Math.PI, Q.Time.FromCircularOrbit(gravitationalParameter, orbitRadius).Second);
        Close(2.7 * Math.PI, Q.Time.FromKeplerOrbit(gravitationalParameter, Q.Length.FromMetre(9)).Second);
        Close(25, Q.LinearAcceleration.FromGravitationalParameter(gravitationalParameter, orbitRadius).Value);

        var translatingMass = Q.Mass.FromKilogram(2);
        var speed = Q.LinearVelocity.FromMetrePerSecond(3);
        var tangentialMomentum = translatingMass * speed;
        Q.AngularMomentum angularMomentum = radius * tangentialMomentum;
        Close(12, angularMomentum.Value);
        Close(6, (angularMomentum / radius).Value);
        Close(8, Q.MomentOfInertia.FromPointMass(translatingMass, radius).Value);
        Close(0.2 * Math.PI, Q.Time.FromMassSpring(translatingMass, stiffness).Second);
        Close(10 * Math.Sqrt(2), Q.LinearVelocity.EscapeSpeed(gravitationalParameter, orbitRadius).MetrePerSecond);
        var normalForce = Q.Force.FromNewton(20);
        Close(10, Q.Force.MaximumStaticFriction(normalForce, 0.5).Newton);
        Close(4, Q.Force.FromKineticFriction(normalForce, 0.2).Newton);
        Close(9, Q.Energy.FromTranslationalKineticEnergy(translatingMass, speed).Joule);
        Close(144, Q.Energy.FromRotationalKineticEnergy((Q.MomentOfInertia)2d, angularVelocity).Joule);
        var resistingForce = Q.Force.FromNewton(-5);
        Close(-15, (resistingForce * speed).Watt);
        Close(-10, Q.Energy.FromMechanicalWork(resistingForce, radius).Joule);
        Close(10, Q.Torque.FromPerpendicularLeverArm(-resistingForce, radius).Value);
        var impulseDuration = Q.Time.FromMillisecond(200);
        Q.LinearMomentum impulse = resistingForce * impulseDuration;
        Close(-1, impulse.Value);
        Close(-5, (impulse / impulseDuration).Newton);

        var density = Q.Mass.FromKilogram(2) / Q.Volume.FromLitre(4);
        var section = Q.Area.FromSquareMetre(0.25);
        var thickness = Q.Length.FromMillimetre(4);
        Q.MassPerLength linearDensity = section * density;
        Q.MassPerArea arealDensity = thickness * density;
        Close(125, linearDensity.Value);
        Close(2, arealDensity.Value);
        Close(500, (linearDensity / section).Value);
        Close(4, (arealDensity / density).Millimetre);
        var volumeFlow = Q.Volume.FromLitre(3) / Q.Time.FromSecond(2);
        Q.MassPerTime massFlow = density * volumeFlow;
        Close(0.75, massFlow.Value);
        Close(volumeFlow.Value, (massFlow / density).Value);
        Close(0.002, ((Q.KinematicViscosity)4e-6 * density).Value);

        Q.ForcePerArea forcePerArea = Q.Force.FromNewton(20) / Q.Area.FromSquareMetre(0.5);
        var pressure = Q.ForcePerArea.FromNormalForce(Q.Force.FromNewton(20), Q.Area.FromSquareMetre(0.5));
        Q.Power hydraulicPower = pressure * volumeFlow;
        Close(40, forcePerArea.Pascal);
        Close(0.06, (forcePerArea * volumeFlow).Watt);
        Close(40, pressure.Pascal);
        Close(0.06, hydraulicPower.Watt);
        Close(40, Q.ForcePerArea.FromHydraulicPower(hydraulicPower, volumeFlow).Pascal);
        Close(0.12, Q.Energy.FromConstantPressureWork(pressure, Q.Volume.FromLitre(3)).Joule);
        var gravity = (Q.LinearAcceleration)10d;
        Close(2 * Math.PI * Math.Sqrt(0.2), Q.Time.FromSimplePendulum(radius, gravity).Second);
        Close(density.Value, (1d / (1d / density)).Value);
        Close(1, density * (1d / density));
        Close(2250, Q.ForcePerArea.FromDynamicPressure(density, speed).Pascal);
    }

    private static void VerifyWaves()
    {
        Close(Math.PI, Q.InverseLength.AngularWavenumberFromWavelength(Q.Length.FromMetre(2)).Value);
        Close(0.5, Q.InverseLength.SpectroscopicWavenumberFromWavelength(Q.Length.FromMetre(2)).Value);

        Close(6, Q.LinearVelocity.PeakSpeedInSimpleHarmonicMotion(Q.Length.FromMetre(2), Q.AngularVelocity.FromRadianPerSecond(3)).Value);
        Close(18, Q.LinearAcceleration.PeakAccelerationInSimpleHarmonicMotion(Q.Length.FromMetre(2), Q.AngularVelocity.FromRadianPerSecond(3)).Value);
        Close(85, Q.Frequency.FromHalfWaveResonance(Q.LinearVelocity.FromMetrePerSecond(340), Q.Length.FromMetre(2)).Hertz);
        Close(42.5, Q.Frequency.FromQuarterWaveResonance(Q.LinearVelocity.FromMetrePerSecond(340), Q.Length.FromMetre(2)).Hertz);
        Close(6, Q.PowerPerArea.FromProgressiveSoundWave(Q.ForcePerArea.FromPascal(2), Q.LinearVelocity.FromMetrePerSecond(3)).Value);
        Close(-20, Q.PowerPerArea.SoundIntensityLevelDifferenceInDecibels((Q.PowerPerArea)0.02d, (Q.PowerPerArea)2d));

        Close(40, Q.LinearVelocity.FromStretchedString(Q.Force.FromNewton(800), (Q.MassPerLength)0.5d).MetrePerSecond);
        Close(1500, Q.LinearVelocity.FromFluidBulkModulus(
            Q.ForcePerArea.FromPascal(2.25e9), (Q.Density)1000d).MetrePerSecond);

        var soundSpeed = Q.LinearVelocity.FromMetrePerSecond(320);
        Close(0.5, Q.LinearVelocity.FromMetrePerSecond(-160).MachNumber(soundSpeed));
        Close(4, Q.Frequency.BeatFrequency(Q.Frequency.FromHertz(444), Q.Frequency.FromHertz(440)).Hertz);
        var length = Q.Length.FromMillimetre(2000);
        Close(5, Q.PowerPerArea.FromIsotropicSource(Q.Power.FromWatt(80 * Math.PI), length).Value);
        var gravity = (Q.LinearAcceleration)10d;
        var wavelength = Q.Length.FromMetre(2 * Math.PI);
        Close(Math.Sqrt(10), Q.LinearVelocity.FromDeepWaterGravityWave(gravity, wavelength).MetrePerSecond);
        Close(2, Q.LinearVelocity.FromShallowWaterGravityWave(gravity, Q.Length.FromMillimetre(400)).MetrePerSecond);
    }

    private static void VerifySolidMechanics()
    {
        var mass = Q.Mass.FromKilogram(3);
        var radius = Q.Length.FromMillimetre(2000);
        Close(12, Q.MomentOfInertia.FromThinRing(mass, radius).Value);
        Close(6, Q.MomentOfInertia.FromSolidCylinder(mass, radius).Value);
        Close(1, Q.MomentOfInertia.FromThinRod(mass, radius).Value);
        var sphereInertia = Q.MomentOfInertia.FromSolidSphere(mass, radius);
        Close(4.8, sphereInertia.Value);
        var strain = Q.Length.FromMillimetre(1100).EngineeringStrainFrom(Q.Length.FromMetre(1));
        Close(0.1, strain);
        var youngsModulus = Q.ForcePerArea.FromPascal(1000);
        var stress = Q.Stress.FromUniaxialStrain(youngsModulus, strain);
        Close(100, stress.Value);
        Close(5, Q.EnergyDensity.FromElasticStrain(stress, strain).Value);
        var shearModulus = Q.ForcePerArea.ShearModulusFromYoungsModulus(youngsModulus, 0.25);
        Close(400, shearModulus.Pascal);
        Close(-40, Q.Stress.FromElasticShearStrain(shearModulus, -0.1).Value);
        Close(2000d / 3, Q.ForcePerArea.BulkModulusFromYoungsModulus(youngsModulus, 0.25).Pascal);
        var originalVolume = Q.Volume.FromLitre(1);
        var finalVolume = Q.Volume.FromLitre(0.9);
        Close(-0.1, finalVolume.VolumetricStrainFrom(originalVolume));
        var torque = (Q.Torque)Math.PI;
        var shaftRadius = Q.Length.FromMillimetre(500);
        Close(16, Q.Stress.FromSolidShaftTorque(torque, shaftRadius).Value);
    }

    private static void VerifyRelativity()
    {
        var physicalLightSpeed = Q.LinearVelocity.FromMetrePerSecond(299_792_458d);
        var fifthOfLightSpeed = physicalLightSpeed * 0.2d;
        Close(Math.Sqrt(2d / 3d), Q.Frequency.FromHertz(1).ObserveLongitudinalDoppler(fifthOfLightSpeed).Hertz);
        Close(Math.Sqrt(1.5d), Q.Frequency.FromHertz(1).ObserveLongitudinalDoppler(-fifthOfLightSpeed).Hertz);
        Close(5d / 13d, fifthOfLightSpeed.AddCollinearRelativisticVelocity(fifthOfLightSpeed).Value / physicalLightSpeed.Value);

        Close(10, Q.Time.FromSecond(8).Dilate(1.25).Second);
        Close(8, Q.Length.FromMetre(10).ContractAlongMotion(1.25).Metre);
        Close(10, Q.Energy.FromRelativisticKineticEnergy(Q.Energy.FromJoule(40), 1.25).Joule);

        var mass = Q.Mass.FromKilogram(2);
        var lightSpeed = Q.LinearVelocity.FromMetrePerSecond(10);
        var velocity = Q.LinearVelocity.FromMetrePerSecond(6);
        Close(1.25, velocity.LorentzFactor(lightSpeed));
        Close(200, Q.Energy.FromRestMass(mass, lightSpeed).Joule);
        Close(0.5, Q.Length.FromMillimetre(1500).RedshiftFrom(Q.Length.FromMetre(1)));
    }

    private static void VerifyFluidTransport()
    {
        var gaugeReferencePressure = Q.ForcePerArea.FromPascal(100000);
        var absolutePressure = Q.ForcePerArea.FromGaugePressure(Q.ForcePerArea.FromPascal(-200), gaugeReferencePressure);
        Close(99800, absolutePressure.Pascal);
        Close(-200, absolutePressure.GaugePressureRelativeTo(gaugeReferencePressure).Pascal);
        Close(100300, Q.ForcePerArea.FromIncompressibleStagnation(gaugeReferencePressure, Q.ForcePerArea.FromPascal(300)).Pascal);

        Close(2, Q.Length.HydraulicDepth(Q.Area.FromSquareMetre(10), Q.Length.FromMetre(5)).Metre);

        Close(2, Q.LinearVelocity.FromMetrePerSecond(6).FroudeNumber(Q.LinearVelocity.FromMetrePerSecond(3)));
        Close(4, ((Q.KinematicViscosity)8e-6).SchmidtNumber((Q.AreaPerTime)2e-6));
        Close(4, Q.Length.RmsDisplacementInOneDimensionalDiffusion((Q.AreaPerTime)2d, Q.Time.FromSecond(4)).Metre);

        var potentialIncrease = Q.SpecificEnergy.FromUniformGravityElevationChange((Q.LinearAcceleration)10d, Q.Length.FromMetre(3));
        Q.ForcePerArea headPressure = (Q.Density)1000d * potentialIncrease;
        Close(30000, headPressure.Pascal);
        Close(30, (headPressure / (Q.Density)1000d).Value);
        Q.Force thrust = (Q.MassPerTime)2d * (Q.SpecificImpulseByMass)3000d;
        Close(6000, thrust.Newton);
        Close(3000, (thrust / (Q.MassPerTime)2d).Value);

        var gravity = (Q.LinearAcceleration)10d;
        var diameter = Q.Length.HydraulicDiameter(Q.Area.FromSquareMetre(0.5), Q.Length.FromMetre(2));
        Close(1, diameter.Metre);
        Close(6, Q.LinearVelocity.FromTorricelliHead(gravity, Q.Length.FromMillimetre(1800)).MetrePerSecond);
        var surfaceTension = (Q.ForcePerLength)0.075d;
        var capillaryRadius = Q.Length.FromMillimetre(10);
        Close(15, Q.ForcePerArea.FromSphericalSurfaceTension(surfaceTension, capillaryRadius).Pascal);
    }

    private static void VerifyElectromagnetism()
    {
        Close(-1d / Math.PI, Q.AuxillaryMagneticField.FromLongStraightWire(Q.ElectricCurrent.FromAmpere(-4), Q.Length.FromMetre(2)).Value);

        var magneticDipole = Q.MagneticDipoleMoment.FromWeberMetre(6);
        var auxiliaryField = (Q.AuxillaryMagneticField)2d;
        Close(1.5, Q.ForcePerLength.FromCurrentInMagneticField(Q.ElectricCurrent.FromAmpere(3), Q.MagneticFluxDensity.FromTesla(0.5)).Value);
        Close(18, Q.Torque.FromElectricDipole((Q.ElectricDipoleMoment)6d, (Q.ElectricFieldStrength)3d).Value);
        Close(-18, Q.Energy.FromElectricDipole((Q.ElectricDipoleMoment)6d, (Q.ElectricFieldStrength)3d).Joule);
        Close(12, Q.Torque.FromMagneticDipole(magneticDipole, auxiliaryField).Value);
        Close(-12, Q.Energy.FromMagneticDipole(magneticDipole, auxiliaryField).Joule);

        var resistance = Q.Resistance.FromOhm(10);
        var current = Q.ElectricCurrent.FromAmpere(4);
        Q.ElectricPotential voltage = resistance * current;
        Q.Power power = voltage * current;
        Close(40, voltage.Volt);
        Close(160, power.Watt);
        Close(160, Q.Power.FromResistiveHeating(current, resistance).Watt);
        Close(160, Q.Power.FromResistiveHeating(-voltage, resistance).Watt);
        Close(10, (voltage / current).Ohm);
        Close(4, (power / voltage).Ampere);
        var path = Q.Length.FromMetre(2);
        var field = Q.ElectricFieldStrength.FromPotentialChange(voltage, path);
        Close(-40, (field * path).Volt);
        Close(-4, Q.ElectromotiveForce.FromFluxLinkageChange(
            Q.MagneticFlux.FromWeber(2), Q.Time.FromMillisecond(500)).Volt);

        Q.MagneticFlux flux = Q.MagneticFluxDensity.FromTesla(2) * Q.Area.FromSquareMetre(0.5);
        Close(1, flux.Weber);
        Close(2, (flux / Q.Area.FromSquareMetre(0.5)).Tesla);

        var capacitance = (Q.Capacitance)0.005d;
        var inductance = Q.Inductance.FromHenry(2);
        Q.Time rcTimeConstant = resistance * capacitance;
        Q.Time rlTimeConstant = inductance / resistance;
        Close(0.05, rcTimeConstant.Second);
        Close(0.2, rlTimeConstant.Second);
        Close(2, (rlTimeConstant * resistance).Henry);
        var reverseCurrent = Q.ElectricCurrent.FromAmpere(-3);
        Close(-6, Q.MagneticFlux.LinkageFromInductor(inductance, reverseCurrent).Weber);
        Close(9, Q.Energy.FromInductor(inductance, reverseCurrent).Joule);
        Close(10, Q.AngularVelocity.FromLcResonance(inductance, capacitance).RadianPerSecond);
        Close(5 / Math.PI, Q.Frequency.FromLcResonance(inductance, capacitance).Hertz);
        var magnetomotiveForce = Q.MagnetomotiveForce.FromWinding(reverseCurrent, 200);
        Close(-600, magnetomotiveForce.Value);
        Q.AuxillaryMagneticField magneticFieldStrength = magnetomotiveForce / path;
        Close(-300, magneticFieldStrength.Value);
        Close(-600, (magneticFieldStrength * path).Value);

        var permittivity = (Q.Permittivity)(1 / (4 * Math.PI));
        var electrodeRadius = Q.Length.FromMillimetre(500);
        Close(0.5, Q.Capacitance.FromIsolatedSphere(permittivity, electrodeRadius).Farad);
    }

    private static void VerifyCircuitNetworks()
    {
        Close(5, Q.Frequency.FromHertz(20).SeriesRlcQualityFactor(Q.Frequency.FromHertz(4)));
        Close(2, Q.ElectricPotential.FromSinusoidalPeak(Q.ElectricPotential.FromVolt(Math.Sqrt(8d))).Volt);
        Close(3, Q.ElectricCurrent.FromSinusoidalPeak(Q.ElectricCurrent.FromAmpere(3 * Math.Sqrt(2d))).Ampere);
        Close(24, Q.ElectricPotential.FromVolt(12).SecondaryVoltageOfIdealTransformer(2).Volt);
        Close(3, Q.ElectricCurrent.FromAmpere(6).SecondaryCurrentOfIdealTransformer(2).Ampere);

        var resistance = Q.Resistance.FromOhm(10);
        var otherResistance = Q.Resistance.FromOhm(15);
        Close(25, resistance.InSeriesWith(otherResistance).Ohm);
        Close(6, resistance.InParallelWith(otherResistance).Ohm);
        var capacitance = Q.Capacitance.FromFarad(0.02);
        var otherCapacitance = Q.Capacitance.FromFarad(0.03);
        Close(0.012, capacitance.InSeriesWith(otherCapacitance).Farad);
        Close(0.05, capacitance.InParallelWith(otherCapacitance).Farad);
        var inductance = Q.Inductance.FromHenry(2);
        var otherInductance = Q.Inductance.FromHenry(3);
        Close(5, inductance.InSeriesWith(otherInductance).Henry);
        Close(1.2, inductance.InParallelWith(otherInductance).Henry);

        Close(1 / Math.PI, Q.Frequency.FromFirstOrderCutoff(Q.Time.FromMillisecond(500)).Hertz);
        Close(2.5 / Math.PI, Q.Frequency.FromSeriesRlcBandwidth(resistance, inductance).Hertz);
    }

    private static void VerifyHeatTransfer()
    {
        var conductingMaterial = (Q.ThermalConductivity)4d;
        var conductionFlux = conductingMaterial.HeatFluxForTemperatureGradient(-3);
        Close(12, conductionFlux.Value);
        Close(-3, conductingMaterial.TemperatureGradientInKelvinsPerMetre(conductionFlux));

        Close(48, Q.PowerPerArea.FromBlackbodyTemperature(Q.ThermodynamicTemperature.FromKelvin(2), (Q.PowerPerAreaQuarticTemperature)3d).Value);
        Close(7, Q.Energy.FromHeatAndWork(Q.Energy.FromJoule(10), Q.Energy.FromJoule(3)).Joule);

        var room = Q.ThermodynamicTemperature.FromKelvin(290);
        var heated = Q.ThermodynamicTemperature.FromKelvin(310);
        double temperatureRise = heated.DifferenceInKelvins(room);
        Q.HeatCapacity capacity = (Q.SpecificHeatCapacity)1000d * Q.Mass.FromKilogram(2);
        Q.Energy heating = capacity.EnergyForTemperatureChange(temperatureRise);
        Close(40, heating.Kilojoule);
        Close(2000, Q.HeatCapacity.FromEnergyAndTemperatureChange(heating, temperatureRise).Value);
        Close(20, ((Q.TemperaturePerTime)2d).ChangeInKelvins(Q.Time.FromSecond(10)));

        var coefficient = (Q.CoefficientOfHeatTransfer)0.5d;
        var surface = Q.Area.FromSquareMetre(10);
        Q.HeatFlowRatePerArea heatFlux = coefficient.HeatFluxForTemperatureChange(temperatureRise);
        Q.HeatFlowRate heatFlow = heatFlux * surface;
        Q.ThermalResistance resistance = Q.ThermalResistance.FromTemperatureDrop(temperatureRise, heatFlow);
        Close(100, heatFlow.Value);
        Close(0.2, resistance.Value);
        var surfaceInsulance = resistance * surface;
        Close(2, surfaceInsulance.Value);
        Close(20, surfaceInsulance.TemperatureDropInKelvins(heatFlux));
        Close(20, resistance.TemperatureDropInKelvins(heatFlow));
        Close(100, resistance.HeatFlowForTemperatureChange(temperatureRise).Value);
        Close(0.2, Q.ThermalResistance.FromHeatTransferCoefficient(coefficient, surface).Value);
        Close(2, ((Q.LinearThermalExpansion)1e-4).LengthChange(temperatureRise).Millimetre);
        Close(0.002, ((Q.AreaThermalExpansion)1e-4).AreaChange(temperatureRise).Value);
        Close(0.004, ((Q.VolumeThermalExpansion)2e-4).VolumeChange(temperatureRise).Value);

        var thickness = Q.Length.FromMillimetre(100);
        var conductivity = (Q.ThermalConductivity)0.5d;
        Q.ThermalInsulance insulance = thickness / conductivity;
        Close(0.2, insulance.Value);
        Close(0.2, ((1d / conductivity) * thickness).Value);
    }

    private static void VerifyLightChemistryAndDose()
    {
        var projectedArea = Q.Area.FromSquareMetre(4).ProjectAlong(Q.PlaneAngle.FromRadian(Math.PI / 3));
        Q.AreaAngle geometricExtent = projectedArea * Q.SolidAngle.FromSteradian(1.5);
        Q.PowerPerAreaAngle meanRadiance = Q.Power.FromWatt(6) / geometricExtent;
        Q.RadiantIntensity normalRadianceIntensity = meanRadiance * projectedArea;
        Q.EnergyPerArea radiantExposure = (Q.PowerPerArea)7d * Q.Time.FromSecond(3);
        Close(2, projectedArea.SquareMetre);
        Close(3, geometricExtent.Value);
        Close(2, meanRadiance.Value);
        Close(4, normalRadianceIntensity.Value);
        Close(21, radiantExposure.Value);

        Q.LuminousFlux light = Q.LuminousIntensity.FromCandela(20) * Q.SolidAngle.FromSteradian(3);
        Q.Illuminance illuminance = light / Q.Area.FromSquareMetre(2);
        Close(60, light.Lumen);
        Close(30, illuminance.Lux);
        Close(5, Q.Illuminance.FromNormalPointSource(
            Q.LuminousIntensity.FromCandela(20), Q.Length.FromMetre(2)).Lux);
        Close(5, Q.PowerPerArea.FromNormalPointSource(
            (Q.RadiantIntensity)20d, Q.Length.FromMillimetre(2000)).Value);

        var amount = Q.AmountOfSubstance.FromMole(3);
        Q.Mass mass = (Q.MolarMass)0.028d * amount;
        Q.AmountOfSubstancePerVolume concentration = amount / Q.Volume.FromLitre(2);
        Close(0.084, mass.Kilogram);
        Close(1500, concentration.Value);
        Close(3, (concentration * Q.Volume.FromLitre(2)).Mole);
        var molarMass = (Q.MolarMass)0.028d;
        var density = (Q.Density)1.25d;
        Q.AmountOfSubstancePerVolume pureSubstanceConcentration = density / molarMass;
        Close(1.25 / 0.028, pureSubstanceConcentration.Value);
        Close(1.25, (pureSubstanceConcentration * molarMass).Value);
        Close(0.028, (density / pureSubstanceConcentration).Value);
        Q.MolarVolume molarVolume = molarMass / density;
        Close(0.0224, molarVolume.Value);

        Q.AbsorbedDose dose = (Q.AbsorbedDoseRate)0.4d * Q.Time.FromSecond(5);
        Q.Energy deposited = dose * Q.Mass.FromKilogram(3);
        Q.SpecificEnergy perMass = deposited / Q.Mass.FromKilogram(3);
        Close(2, dose.Gray);
        Close(6, deposited.Joule);
        Close(2, perMass.Value);
        Close(2, Q.AbsorbedDose.FromDepositedEnergy(deposited, Q.Mass.FromKilogram(3)).Gray);
    }

    private static void VerifyOptics()
    {
        var incidenceAngle = Q.PlaneAngle.FromRadian(Math.PI / 6);
        if (!incidenceAngle.TryRefractionAngle(0.5, out var transmittedAngle))
        {
            throw new InvalidOperationException("Expected refraction into the higher-index medium.");
        }
        Close(Math.Asin(0.25), transmittedAngle.Radian);
        if (Q.PlaneAngle.FromRadian(Math.PI / 3).TryRefractionAngle(1.5, out _))
        {
            throw new InvalidOperationException("Expected total internal reflection.");
        }

        Close(Math.PI / 6, Q.PlaneAngle.FromFirstDiffractionOrder(Q.Length.FromMillimetre(1), Q.Length.FromMillimetre(2)).Radian);
        Close(Math.PI / 6, Q.PlaneAngle.FromFirstSingleSlitMinimum(Q.Length.FromMillimetre(1), Q.Length.FromMillimetre(2)).Radian);
        Close(0.04, Q.Power.ReflectanceAtNormalIncidence(1, 1.5));

        var objectDistance = Q.Length.FromMillimetre(2000);
        var focalLength = Q.Length.FocalLengthFromConjugates(objectDistance, Q.Length.FromMetre(2));
        Close(1, focalLength.Metre);
        var virtualImage = Q.Length.ImageDistanceFromThinLens(-focalLength, Q.Length.FromMetre(1));
        Close(-0.5, virtualImage.Metre);
        Close(0.5, Q.Length.LateralMagnification(Q.Length.FromMetre(1), virtualImage));
        var vacuumLightSpeed = Q.LinearVelocity.FromMetrePerSecond(3e8);
        var mediumLightSpeed = Q.LinearVelocity.FromRefractiveIndex(vacuumLightSpeed, 2);
        Close(1.5e8, mediumLightSpeed.MetrePerSecond);
        Close(2, mediumLightSpeed.RefractiveIndex(vacuumLightSpeed));
        var incidence = Q.PlaneAngle.FromRadian(Math.PI / 6);
        var steepIncidence = Q.PlaneAngle.FromRadian(Math.PI / 3);
        Close(Math.PI / 6, Q.PlaneAngle.CriticalAngle(2, 1).Radian);
        Close(Math.Atan(1.5), Q.PlaneAngle.BrewsterAngle(1, 1.5).Radian);
        Close(5, ((Q.PowerPerArea)20d).AfterIdealPolarizer(steepIncidence).Value);
        Close(2 * Math.PI, Q.SolidAngle.FromCone(Q.PlaneAngle.FromRadian(Math.PI / 2)).Steradian);

        var wavelength = Q.Length.FromMillimetre(0.001);
        Close(1.22e-4, Q.PlaneAngle.FromRayleighCriterion(wavelength, Q.Length.FromMillimetre(10)).Radian);
        Close(Math.PI / 2, Q.PlaneAngle.FromOpticalPathDifference(wavelength / 4d, wavelength).Radian);
        var numericalAperture = incidence.NumericalAperture(1.5);
        Close(0.75, numericalAperture);
        Close(0.00061 / 0.75, Q.Length.RayleighMicroscopeResolution(wavelength, numericalAperture).Millimetre);
        var normalLuminousIntensity = Q.LuminousIntensity.FromCandela(20);
        var normalRadiantIntensity = (Q.RadiantIntensity)20d;
        Close(10, normalLuminousIntensity.AtLambertianAngle(steepIncidence).Candela);
        Close(10, normalRadiantIntensity.AtLambertianAngle(steepIncidence).Value);
        Close(20 * Math.PI, normalLuminousIntensity.ToLambertianFlux().Lumen);
        Close(20 * Math.PI, normalRadiantIntensity.ToLambertianFlux().Watt);
    }

    private static void VerifyThermodynamicProcesses()
    {
        var isochoricMolarCapacity = (Q.MolarHeatCapacity)12d;
        var isobaricMolarCapacity = isochoricMolarCapacity.ToIsobaricForIdealGas();
        Close(20.31446261815324, isobaricMolarCapacity.Value);
        Close(12, isobaricMolarCapacity.ToIsochoricForIdealGas().Value);

        var idealGasTemperature = Q.ThermodynamicTemperature.FromKelvin(300);
        var idealGasConcentration = (Q.AmountOfSubstancePerVolume)4d;
        var idealGasPressure = Q.ForcePerArea.FromIdealGasConcentration(idealGasConcentration, idealGasTemperature);
        Close(300, Q.ThermodynamicTemperature.FromIdealGasPressure(idealGasPressure, idealGasConcentration).Kelvin);
        Close(4, Q.AmountOfSubstancePerVolume.FromIdealGasPressure(idealGasPressure, idealGasTemperature).Value);
        Close(7483.0163563379, Q.Energy.FromMonatomicIdealGas(Q.AmountOfSubstance.FromMole(2), idealGasTemperature).Joule);

        var gasTemperature = Q.ThermodynamicTemperature.FromKelvin(300);
        Close(4988.6775708919, Q.ForcePerArea.FromIdealGasConcentration((Q.AmountOfSubstancePerVolume)2d, gasTemperature).Pascal);
        Close(516.9628460102, Q.LinearVelocity.RmsSpeedOfIdealGas(gasTemperature, (Q.MolarMass)0.028d).MetrePerSecond);
        Close(2, ((Q.ThermalDiffusivity)4e-6).LewisNumber((Q.AreaPerTime)2e-6));

        var hot = Q.ThermodynamicTemperature.FromKelvin(300);
        var cold = Q.ThermodynamicTemperature.FromKelvin(200);
        var gamma = Q.SpecificHeatCapacity.HeatCapacityRatio((Q.SpecificHeatCapacity)1000d, (Q.SpecificHeatCapacity)500d);
        Close(2, gamma);
        var initialPressure = Q.ForcePerArea.FromPascal(100000);
        var displacementConstant = (Q.LengthTemperature)0.003d;
        var peakWavelength = displacementConstant / hot;
        Close(0.01, peakWavelength.Millimetre);
        Close(300, (displacementConstant / peakWavelength).Kelvin);
        Close(20000, initialPressure.PartialPressureInIdealGas(0.2).Pascal);
        Close(10000, Q.ForcePerArea.FromPascal(40000).VapourPartialPressureInIdealSolution(0.25).Pascal);
        Close(0.4, Q.Energy.FromJoule(40).HeatEngineEfficiency(Q.Energy.FromJoule(100)));
        Close(1d / 3, hot.CarnotEfficiency(cold));
        Close(2, hot.CarnotRefrigeratorCoefficientOfPerformance(cold));
        Close(3, hot.CarnotHeatPumpCoefficientOfPerformance(cold));

        var resistance = (Q.ThermalResistance)0.2d;
        var capacity = (Q.HeatCapacity)10d;
        Q.Time timeConstant = capacity * resistance;
        Close(2, timeConstant.Second);
        Close(10, (timeConstant / resistance).Value);
        Close(0.2, (timeConstant / capacity).Value);
        Close(0.5, resistance.InSeriesWith((Q.ThermalResistance)0.3d).Value);
        Close(0.12, resistance.InParallelWith((Q.ThermalResistance)0.3d).Value);

        var diffusivity = (Q.ThermalDiffusivity)0.1d;
        Close(2, (Q.KinematicViscosity)0.2d / diffusivity);
    }

    private static void VerifyElectromagneticWavesAndPhotons()
    {
        var frequency25 = Q.Frequency.FromHertz(2.5e15);
        var energy25 = Q.Energy.FromPhotonFrequency(frequency25);
        Close(1.6565175375, energy25.Joule / 1e-18);
        Close(2.5, energy25.ToPhotonFrequency().Hertz / 1e15);
        Close(0.662607015, Q.Length.FromDeBroglieMomentum((Q.LinearMomentum)1e-24).Metre / 1e-9);

        Close(15, Q.PowerPerArea.FromOrthogonalElectromagneticFields((Q.ElectricFieldStrength)3d, (Q.AuxillaryMagneticField)5d).Value);
        Close(3, Q.ForcePerArea.FromAbsorbedRadiation((Q.PowerPerArea)12d, Q.LinearVelocity.FromMetrePerSecond(4)).Pascal);
        Close(6, Q.ForcePerArea.FromReflectedRadiation((Q.PowerPerArea)12d, Q.LinearVelocity.FromMetrePerSecond(4)).Pascal);

        var permittivity = (Q.Permittivity)0.5d;
        var permeability = (Q.Permeability)0.5d;
        var speed = Q.LinearVelocity.FromElectromagneticMedium(permittivity, permeability);
        Close(2, speed.MetrePerSecond);
        var magneticField = Q.MagneticFluxDensity.FromTesla(3);
        var electricField = Q.ElectricFieldStrength.FromPlaneWave(magneticField, speed);
        Close(6, electricField.Value);
        Close(3, Q.MagneticFluxDensity.FromPlaneWave(electricField, speed).Tesla);
        Close(9, Q.EnergyDensity.FromElectricField(electricField, permittivity).Value);
        Close(18, Q.EnergyDensity.FromMagneticField(magneticField, (Q.Permeability)0.25d).Value);
        var photonProduct = (Q.LengthEnergy)2d;
        var wavelength = Q.Length.FromMillimetre(500);
        Q.Energy photonEnergy = photonProduct / wavelength;
        Close(4, photonEnergy.Joule);
        Close(0.5, (photonProduct / photonEnergy).Metre);
        Close(2, (wavelength * photonEnergy).Value);
        var vacuumLightSpeed = Q.LinearVelocity.FromMetrePerSecond(10);
        var photonMomentum = Q.LinearMomentum.FromPhotonEnergy(photonEnergy, vacuumLightSpeed);
        Close(0.4, photonMomentum.Value);
        Close(4, Q.Energy.FromPhotonMomentum(-photonMomentum, vacuumLightSpeed).Joule);
        Close(2, Q.Length.ComptonWavelengthShift(
            Q.Length.FromMillimetre(4), Q.PlaneAngle.FromRadian(Math.PI / 3)).Millimetre);
        if (!Q.Energy.TryPhotoelectronKineticEnergy(photonEnergy, Q.Energy.FromJoule(1), out var kineticEnergy))
        {
            throw new InvalidOperationException("Expected a photoelectron above the emission threshold.");
        }

        Close(3, kineticEnergy.Joule);
        if (Q.Energy.TryPhotoelectronKineticEnergy(photonEnergy, Q.Energy.FromJoule(5), out _))
        {
            throw new InvalidOperationException("Expected no photoelectron below the emission threshold.");
        }
    }

    private static void VerifyRadiationAndMixtures()
    {
        var halfLife = Q.Time.FromSecond(12);
        var decayTimeConstant = Q.Time.FromExponentialHalfLife(halfLife);
        Close(12, decayTimeConstant.HalfLifeFromDecayTimeConstant().Second);
        Close(0.125, Q.Time.FromSecond(36).ExponentialDecayFraction(decayTimeConstant));
        Close(0.25, ((Q.InverseLength)Math.Log(2d)).TransmissionThrough(Q.Length.FromMetre(2)));

        var avogadroConstant = (Q.InverseAmountOfSubstance)6.02214076e23;
        Q.AmountOfSubstance radionuclide = 120d / avogadroConstant;
        Close(120, radionuclide * avogadroConstant);
        Close(1, (120d / radionuclide).Value / avogadroConstant.Value);
        Close(6, Q.DoseEquivalent.FromAbsorbedDose(Q.AbsorbedDose.FromGray(2), 3).Value);
        Close(4e-6, Q.Exposure.FromIonizationInAir((Q.ElectricCharge)(-2e-6), Q.Mass.FromKilogram(0.5)).Value);

        var coefficient = (Q.InverseLength)2d;
        Close(Math.Log(2) / 2, Q.Length.HalfValueThickness(coefficient).Metre);
        Close(0.25, Q.Mass.FromKilogram(0.5).MassFraction(Q.Mass.FromKilogram(2)));
        Close(1d / 3, Q.AmountOfSubstance.FromMole(2).AmountFraction(Q.AmountOfSubstance.FromMole(6)));
        Close(0.25, Q.Volume.FromLitre(1).VolumeFraction(Q.Volume.FromLitre(4)));
    }

    private static void VerifyPairRelations()
    {
        var elapsed = Q.Time.FromMillisecond(500);
        Q.ForcePerAreaTime pressureRate = Q.ForcePerArea.FromPascal(-20) / elapsed;
        Close(-40, pressureRate.Value);
        Close(-20, (pressureRate * elapsed).Pascal);
        Close(60, Q.Activity.FromDecayCount(30, elapsed).Value);

        var photonWavelength = Q.Length.FromMillimetre(2);
        Q.LengthMolarEnergy product = (Q.MolarEnergy)2000d * photonWavelength;
        Close(4, product.Value);
        Close(2000, (product / photonWavelength).Value);

        var length = Q.Length.FromMillimetre(4000);
        var crossSection = Q.Area.FromSquareMetre(0.5);
        Q.InverseLength conductorGeometry = length / crossSection;
        Close(8, conductorGeometry.Value);
        var conductivity = (Q.ElectricConductivity)4d;
        Q.Resistance resistance = conductorGeometry / conductivity;
        Close(2, resistance.Ohm);
        Close(4, (conductorGeometry / resistance).Value);

        var permittivity = (Q.Permittivity)0.5d;
        Q.Capacitance capacitance = permittivity * Q.Length.FromMillimetre(200);
        Close(0.1, capacitance.Farad);
        var charge = (Q.ElectricCharge)2d;
        Q.ElectricFlux flux = charge / permittivity;
        Close(4, flux.Value);
        Close(2, (permittivity * flux).Value);
        var area = Q.Area.FromSquareMetre(2);
        Q.ElectricDisplacement displacement = Q.ElectricDisplacement.FromDisplacementFlux(charge, area);
        Close(1, displacement.Value);
        Close(2, (displacement * area).Value);

        var polarizability = (Q.Polarizability)2d;
        var field = (Q.ElectricFieldStrength)3d;
        Q.ElectricDipoleMoment dipole = polarizability * field;
        Close(6, dipole.Value);
        Q.PolarizationField polarization = dipole / Q.Volume.FromCubicMetre(2);
        Close(3, polarization.Value);
        var current = Q.ElectricCurrent.FromAmpere(2);
        Q.LinearElectricCurrent sheetCurrent = current / Q.Length.FromMillimetre(500);
        Close(4, sheetCurrent.Value);
        var dielectric = Q.Permittivity.FromRelativePermittivity(2, permittivity);
        Close(2, dielectric.RelativePermittivity(permittivity));
        var vacuumPermeability = (Q.Permeability)0.5d;
        var material = Q.Permeability.FromRelativePermeability(3, vacuumPermeability);
        Close(3, material.RelativePermeability(vacuumPermeability));

        Q.LuminousFluxPerArea exitance = ((Q.Luminance)5d).ToLambertianExitance();
        Close(5 * Math.PI, exitance.Value);
        Q.LuminousFlux emitted = exitance * area;
        Close(10 * Math.PI, emitted.Lumen);
        Close(5 * Math.PI, Q.LuminousFluxPerArea.FromEmittedFlux(emitted, area).Value);
    }

    private static void Close(double expected, double actual)
    {
        if (!(Math.Abs(expected - actual) <= 1e-9))
        {
            throw new InvalidOperationException($"Expected {expected}, got {actual}.");
        }
    }
}
