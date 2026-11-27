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
        VerifyWaves();
        VerifyFluidTransport();
        VerifyElectromagnetism();
        VerifyCircuitNetworks();
        VerifyHeatTransfer();
        VerifyThermodynamicProcesses();
        VerifyOptics();
        VerifyLightChemistryAndDose();
        VerifyQuantityConversions();

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
        Close(30, Q.PlaneAngle.FromConstantAngularAcceleration(
            angularVelocity, angularAcceleration, Q.Time.FromMillisecond(2000)).Radian);

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

        var gravitationalParameter = (Q.StandardGravitationalParameter)400d;
        var orbitRadius = Q.Length.FromMillimetre(4000);
        Close(10, Q.LinearVelocity.FromCircularOrbit(gravitationalParameter, orbitRadius).MetrePerSecond);
        Close(0.8 * Math.PI, Q.Time.FromCircularOrbit(gravitationalParameter, orbitRadius).Second);
        Close(25, Q.LinearAcceleration.FromGravitationalParameter(gravitationalParameter, orbitRadius).Value);

        var translatingMass = Q.Mass.FromKilogram(2);
        var speed = Q.LinearVelocity.FromMetrePerSecond(3);
        Close(8, Q.MomentOfInertia.FromPointMass(translatingMass, radius).Value);
        Close(0.2 * Math.PI, Q.Time.FromMassSpring(translatingMass, stiffness).Second);
        Close(-200, Q.Energy.FromGravitationalPotential(gravitationalParameter, translatingMass, orbitRadius).Joule);
        Close(-100, Q.Energy.FromCircularOrbit(gravitationalParameter, translatingMass, orbitRadius).Joule);
        Close(10 * Math.Sqrt(2), Q.LinearVelocity.EscapeSpeed(gravitationalParameter, orbitRadius).MetrePerSecond);
        Close(-2, Q.Length.FromConstantAcceleration(-speed, (Q.LinearAcceleration)2d, Q.Time.FromSecond(2)).Metre);
        Close(1, Q.LinearVelocity.SpeedAfterConstantAcceleration(-speed, (Q.LinearAcceleration)2d, -radius).MetrePerSecond);
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
        Close(-40, Q.Energy.FromUniformGravity(translatingMass, gravity, -radius).Joule);
        Close(2 * Math.PI * Math.Sqrt(0.2), Q.Time.FromSimplePendulum(radius, gravity).Second);
        Close(10000, Q.ForcePerArea.FromHydrostaticHead(density, gravity, radius).Pascal);
        Close(15, Q.Force.FromBuoyancy(density, Q.Volume.FromLitre(3), gravity).Newton);
        Close(100250, Q.ForcePerArea.FromBernoulli(
            Q.ForcePerArea.FromPascal(100000), density, speed, Q.LinearVelocity.FromMetrePerSecond(2),
            gravity, Q.Length.FromMillimetre(200)).Pascal);
        Close(density.Value, (1d / (1d / density)).Value);
        Close(1, density * (1d / density));
        Close(2250, Q.ForcePerArea.FromDynamicPressure(density, speed).Pascal);
        var referenceArea = Q.Area.FromSquareMetre(0.5);
        Close(450, Q.Force.FromDrag(density, speed, referenceArea, 0.4).Newton);
        Close(-225, Q.Force.FromLift(density, speed, referenceArea, -0.2).Newton);
        var kinematicViscosity = (Q.KinematicViscosity)4e-6;
        var reynolds = speed.ReynoldsNumber(radius, kinematicViscosity);
        Close(1500000, reynolds);
        Close(reynolds, speed.ReynoldsNumber(radius, density, kinematicViscosity * density));
        Close(-0.4, Q.Stress.FromNewtonianShear(kinematicViscosity * density,
            Q.LinearVelocity.FromMetrePerSecond(-0.4), Q.Length.FromMillimetre(2)).Value);
        Close(Math.PI * 2.5e-5, Q.VolumePerTime.FromLaminarPipeFlow(
            Q.ForcePerArea.FromPascal(100), (Q.DynamicViscosity)0.01d,
            Q.Length.FromMillimetre(10), Q.Length.FromMillimetre(500)).Value);
    }

    private static void VerifyWaves()
    {
        Close(40, Q.LinearVelocity.FromStretchedString(Q.Force.FromNewton(800), (Q.MassPerLength)0.5d).MetrePerSecond);
        Close(1500, Q.LinearVelocity.FromFluidBulkModulus(
            Q.ForcePerArea.FromPascal(2.25e9), (Q.Density)1000d).MetrePerSecond);

        var soundSpeed = Q.LinearVelocity.FromMetrePerSecond(320);
        var frequency = Q.Frequency.FromHertz(480);
        var sourceVelocity = Q.LinearVelocity.FromMetrePerSecond(20);
        var observerVelocity = Q.LinearVelocity.FromMetrePerSecond(-5);
        Close(504, frequency.ObservedWithDopplerShift(soundSpeed, sourceVelocity, observerVelocity).Hertz);
        Close(0.5, Q.LinearVelocity.FromMetrePerSecond(-160).MachNumber(soundSpeed));
        Close(4, Q.Frequency.BeatFrequency(Q.Frequency.FromHertz(444), Q.Frequency.FromHertz(440)).Hertz);
        var length = Q.Length.FromMillimetre(2000);
        Close(240, Q.Frequency.FromStandingWave(soundSpeed, length, 3).Hertz);
        Close(120, Q.Frequency.FromQuarterWaveResonance(soundSpeed, length, 2).Hertz);
        Close(5, Q.PowerPerArea.FromIsotropicSource(Q.Power.FromWatt(80 * Math.PI), length).Value);
        Close(0.005, Q.PowerPerArea.FromPeakSoundPressure(
            Q.ForcePerArea.FromPascal(2), (Q.Density)1.25d, soundSpeed).Value);

        var gravity = (Q.LinearAcceleration)10d;
        var wavelength = Q.Length.FromMetre(2 * Math.PI);
        var depth = Q.Length.FromMillimetre(1000);
        Close(Math.Sqrt(10 * Math.Tanh(1)), Q.LinearVelocity.FromSurfaceGravityWave(gravity, wavelength, depth).MetrePerSecond);
        Close(Math.Sqrt(10), Q.LinearVelocity.FromDeepWaterGravityWave(gravity, wavelength).MetrePerSecond);
        Close(2, Q.LinearVelocity.FromShallowWaterGravityWave(gravity, Q.Length.FromMillimetre(400)).MetrePerSecond);
    }

    private static void VerifyFluidTransport()
    {
        var density = (Q.Density)500d;
        var gravity = (Q.LinearAcceleration)10d;
        var speed = Q.LinearVelocity.FromMetrePerSecond(3);
        var diameter = Q.Length.HydraulicDiameter(Q.Area.FromSquareMetre(0.5), Q.Length.FromMetre(2));
        Close(1, diameter.Metre);
        Close(900, Q.ForcePerArea.FromDarcyWeisbach(density, speed, Q.Length.FromMetre(10), diameter, 0.04).Pascal);
        Close(0.64, Q.LinearVelocity.FromMetrePerSecond(0.004).DarcyFrictionFactorForLaminarPipe(
            Q.Length.FromMillimetre(100), (Q.KinematicViscosity)4e-6));
        Close(1.5, speed.FroudeNumber(gravity, Q.Length.FromMillimetre(400)));

        var surfaceTension = (Q.ForcePerLength)0.075d;
        var capillaryRadius = Q.Length.FromMillimetre(10);
        Close(15, Q.ForcePerArea.FromSphericalSurfaceTension(surfaceTension, capillaryRadius).Pascal);
        Close(3, Q.Length.CapillaryRise(
            surfaceTension, density, gravity, capillaryRadius, Q.PlaneAngle.FromRadian(0)).Millimetre);
        Close(-3, Q.Length.CapillaryRise(
            surfaceTension, density, gravity, capillaryRadius, Q.PlaneAngle.FromRadian(Math.PI)).Millimetre);
        Close(600, speed.WeberNumber(density, capillaryRadius, surfaceTension));

        var sphereRadius = Q.Length.FromMillimetre(1);
        var viscosity = (Q.DynamicViscosity)0.1d;
        Close(6e-6 * Math.PI, Q.Force.FromStokesDrag(
            viscosity, sphereRadius, Q.LinearVelocity.FromMetrePerSecond(-0.01)).Newton);
        Close(-2e-3 / 9, Q.LinearVelocity.StokesTerminalVelocity(
            (Q.Density)490d, density, viscosity, gravity, sphereRadius).MetrePerSecond);
    }

    private static void VerifyElectromagnetism()
    {
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
        Close(1, Q.Resistance.FromUniformConductor(
            (Q.ElectricConductivity)2d, Q.Length.FromMetre(4), Q.Area.FromSquareMetre(2)).Ohm);

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
        var plates = Q.Capacitance.FromParallelPlates(
            (Q.Permittivity)2d, Q.Area.FromSquareMetre(0.5), Q.Length.FromMillimetre(250));
        Close(4, plates.Value);
        Close(8, Q.Energy.FromCapacitor(plates, Q.ElectricPotential.FromVolt(2)).Joule);

        var reverseCurrent = Q.ElectricCurrent.FromAmpere(-3);
        Close(-6, Q.MagneticFlux.LinkageFromInductor(inductance, reverseCurrent).Weber);
        Close(9, Q.Energy.FromInductor(inductance, reverseCurrent).Joule);
        Close(10, Q.AngularVelocity.FromLcResonance(inductance, capacitance).RadianPerSecond);
        Close(5 / Math.PI, Q.Frequency.FromLcResonance(inductance, capacitance).Hertz);
        var transverseField = Q.MagneticFluxDensity.FromTesla(0.5);
        Close(3, Q.Force.FromMovingChargeInMagneticField(
            (Q.ElectricCharge)(-2d), Q.LinearVelocity.FromMetrePerSecond(3), transverseField).Newton);
        Close(0.375, Q.Force.FromCurrentInMagneticField(
            reverseCurrent, Q.Length.FromMillimetre(250), transverseField).Newton);
        Close(-0.03, Q.MagneticFluxDensity.FromLongSolenoid((Q.Permeability)1e-4d, reverseCurrent, 200, path).Tesla);
        var mediumPermeability = (Q.Permeability)1e-4d;
        Close(-3e-4 / (4 * Math.PI), Q.MagneticFluxDensity.FromLongStraightWire(
            mediumPermeability, reverseCurrent, path).Tesla);
        Close(-3e-4 / Math.PI, Q.ForcePerLength.FromParallelCurrents(
            mediumPermeability, reverseCurrent, current, path).Value);
        Close(1, Q.Inductance.FromLongSolenoid(
            mediumPermeability, Q.Area.FromSquareMetre(0.5), path, 200).Henry);
        var magnetomotiveForce = Q.MagnetomotiveForce.FromWinding(reverseCurrent, 200);
        Close(-600, magnetomotiveForce.Value);
        Q.AuxillaryMagneticField magneticFieldStrength = magnetomotiveForce / path;
        Close(-300, magneticFieldStrength.Value);
        Close(-600, (magneticFieldStrength * path).Value);

        var permittivity = (Q.Permittivity)(1 / (4 * Math.PI));
        var sourceCharge = (Q.ElectricCharge)(-16d);
        Close(-8, Q.ElectricPotential.FromPointCharge(sourceCharge, permittivity, path).Volt);
        Close(-4, Q.ElectricFieldStrength.FromPointCharge(sourceCharge, permittivity, path).Value);
        Close(-12, Q.Force.FromPointCharges(sourceCharge, (Q.ElectricCharge)3d, permittivity, path).Newton);
        Close(-4, Q.ElectricFieldStrength.FromLineCharge((Q.ElectricChargeLineDensity)(-4d), permittivity, path).Value);

        var dipoleAngle = Q.PlaneAngle.FromRadian(Math.PI / 6);
        Close(5, Q.Torque.FromElectricDipole((Q.ElectricDipoleMoment)2d, (Q.ElectricFieldStrength)5d, dipoleAngle).Value);
        Close(-5 * Math.Sqrt(3), Q.Energy.FromElectricDipole(
            (Q.ElectricDipoleMoment)2d, (Q.ElectricFieldStrength)5d, dipoleAngle).Joule);
        var vacuumPermeability = (Q.Permeability)1.25e-6d;
        var magneticDipole = Q.MagneticDipoleMoment.FromCurrentLoop(
            vacuumPermeability, Q.ElectricCurrent.FromAmpere(3), Q.Area.FromSquareMetre(0.5), 4);
        Close(7.5e-6, magneticDipole.Value);
        Close(1.5, Q.Torque.FromMagneticDipole(magneticDipole, transverseField, vacuumPermeability, dipoleAngle).Value);
        Close(-1.5 * Math.Sqrt(3), Q.Energy.FromMagneticDipole(magneticDipole, transverseField, vacuumPermeability, dipoleAngle).Joule);
    }

    private static void VerifyCircuitNetworks()
    {
        var resistance = Q.Resistance.FromOhm(10);
        var otherResistance = Q.Resistance.FromOhm(15);
        Close(25, resistance.InSeriesWith(otherResistance).Ohm);
        Close(6, resistance.InParallelWith(otherResistance).Ohm);
        Close(6, Q.ElectricPotential.FromVoltageDivider(
            Q.ElectricPotential.FromVolt(10), resistance, otherResistance).Volt);

        var capacitance = Q.Capacitance.FromFarad(0.02);
        var otherCapacitance = Q.Capacitance.FromFarad(0.03);
        Close(0.012, capacitance.InSeriesWith(otherCapacitance).Farad);
        Close(0.05, capacitance.InParallelWith(otherCapacitance).Farad);
        var inductance = Q.Inductance.FromHenry(2);
        var otherInductance = Q.Inductance.FromHenry(3);
        Close(5, inductance.InSeriesWith(otherInductance).Henry);
        Close(1.2, inductance.InParallelWith(otherInductance).Henry);

        var elapsed = Q.Time.FromMillisecond(200);
        Close(12 - 10 / Math.E, Q.ElectricPotential.FromVolt(2).AfterRcStep(
            Q.ElectricPotential.FromVolt(12), resistance, capacitance, elapsed).Volt);
        Close(9 - 10 / Math.E, Q.ElectricCurrent.FromAmpere(-1).AfterRlStep(
            Q.ElectricCurrent.FromAmpere(9), resistance, inductance, elapsed).Ampere);
        Close(-1, Q.ElectricCurrent.FromCapacitorVoltageChange(
            capacitance, Q.ElectricPotential.FromVolt(-10), elapsed).Ampere);
        Close(-4, Q.ElectricPotential.FromInductorCurrentChange(
            inductance, Q.ElectricCurrent.FromAmpere(-1), Q.Time.FromMillisecond(500)).Volt);

        var frequency = Q.Frequency.FromHertz(5 / Math.PI);
        var rmsCurrent = Q.ElectricCurrent.FromAmpere(3);
        var acInductance = Q.Inductance.FromHenry(0.4);
        var acCapacitance = Q.Capacitance.FromFarad(0.05);
        Close(12, Q.ElectricPotential.FromSinusoidalInductor(rmsCurrent, acInductance, frequency).Volt);
        Close(6, Q.ElectricPotential.FromSinusoidalCapacitor(rmsCurrent, acCapacitance, frequency).Volt);
        Close(2, Q.ElectricCurrent.FromSeriesRlc(Q.ElectricPotential.FromVolt(2 * Math.Sqrt(13)),
            Q.Resistance.FromOhm(3), acInductance, acCapacitance, frequency).Ampere);
    }

    private static void VerifyHeatTransfer()
    {
        var warm = Q.ThermodynamicTemperature.FromKelvin(300);
        var cool = Q.ThermodynamicTemperature.FromKelvin(200);
        var radiationCoefficient = (Q.PowerPerAreaQuarticTemperature)2e-8d;
        Close(65, Q.HeatFlowRatePerArea.FromThermalRadiation(radiationCoefficient, 0.5, warm, cool).Value);
        Close(-65, Q.HeatFlowRatePerArea.FromThermalRadiation(radiationCoefficient, 0.5, cool, warm).Value);
        var gasPressure = Q.ForcePerArea.FromPascal(100000).AfterIdealGasStateChange(
            Q.Volume.FromLitre(4), warm, Q.Volume.FromLitre(1), warm.AddKelvins(300));
        Close(800000, gasPressure.Pascal);

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
        Close(200, heated.GradientInKelvinsPerMetre(room, thickness));
        Close(-100, Q.HeatFlowRatePerArea.FromConduction(conductivity, room, heated, thickness).Value);
        Close(100, Q.HeatFlowRatePerArea.FromConduction(conductivity, heated, room, thickness).Value);
        Q.ThermalInsulance insulance = thickness / conductivity;
        Close(0.2, insulance.Value);
        Close(0.2, ((1d / conductivity) * thickness).Value);
    }

    private static void VerifyLightChemistryAndDose()
    {
        var initialActivity = (Q.Activity)120d;
        Close(30, initialActivity.AfterDecay(Q.Time.FromSecond(120), Q.Time.FromMillisecond(60000)).Value);

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
        var objectDistance = Q.Length.FromMillimetre(2000);
        var focalLength = Q.Length.FocalLengthFromConjugates(objectDistance, Q.Length.FromMetre(2));
        Close(1, focalLength.Metre);
        var virtualImage = Q.Length.ImageDistanceFromThinLens(-focalLength, Q.Length.FromMetre(1));
        Close(-0.5, virtualImage.Metre);
        Close(0.5, Q.Length.LateralMagnification(Q.Length.FromMetre(1), virtualImage));
        var lensRadius = Q.Length.FromMillimetre(250);
        Close(0.25, Q.Length.FocalLengthFromLensMaker(1.5, 1, lensRadius, -lensRadius).Metre);

        var vacuumLightSpeed = Q.LinearVelocity.FromMetrePerSecond(3e8);
        var mediumLightSpeed = Q.LinearVelocity.FromRefractiveIndex(vacuumLightSpeed, 2);
        Close(1.5e8, mediumLightSpeed.MetrePerSecond);
        Close(2, mediumLightSpeed.RefractiveIndex(vacuumLightSpeed));
        var incidence = Q.PlaneAngle.FromRadian(Math.PI / 6);
        if (!Q.PlaneAngle.TryFromRefraction(-incidence, 1, 2, out var refraction))
        {
            throw new InvalidOperationException("Expected a transmitted ray.");
        }

        Close(-Math.Asin(0.25), refraction.Radian);
        var steepIncidence = Q.PlaneAngle.FromRadian(Math.PI / 3);
        if (Q.PlaneAngle.TryFromRefraction(steepIncidence, 2, 1, out _))
        {
            throw new InvalidOperationException("Expected total internal reflection.");
        }

        Close(Math.PI / 6, Q.PlaneAngle.CriticalAngle(2, 1).Radian);
        Close(Math.Atan(1.5), Q.PlaneAngle.BrewsterAngle(1, 1.5).Radian);
        Close(5, ((Q.PowerPerArea)20d).AfterIdealPolarizer(steepIncidence).Value);
        Close(2 * Math.PI, Q.SolidAngle.FromCone(Q.PlaneAngle.FromRadian(Math.PI / 2)).Steradian);

        var wavelength = Q.Length.FromMillimetre(0.001);
        var slitWidth = Q.Length.FromMillimetre(0.004);
        Close(0.001, Q.Length.WavelengthFromDiffractionGrating(slitWidth, -incidence, -2).Millimetre);
        Close(Math.Asin(0.25), Q.PlaneAngle.FromSingleSlitMinimum(wavelength, slitWidth, 1).Radian);
        Close(2e-7, Q.Length.WavelengthFromBraggDiffraction(Q.Length.FromMetre(4e-10), incidence, 2).Millimetre);
        Close(1.22e-4, Q.PlaneAngle.FromRayleighCriterion(wavelength, Q.Length.FromMillimetre(10)).Radian);
        Close(Math.PI / 2, Q.PlaneAngle.FromOpticalPathDifference(wavelength / 4d, wavelength).Radian);
        Close(1, Q.Length.FringeSpacingForDoubleSlit(wavelength, objectDistance, Q.Length.FromMillimetre(2)).Millimetre);

        Close(2.5, Q.Illuminance.FromPointSource(
            Q.LuminousIntensity.FromCandela(20), objectDistance, steepIncidence).Lux);
        Close(2.5, Q.PowerPerArea.FromPointSource((Q.RadiantIntensity)20d, objectDistance, steepIncidence).Value);
    }

    private static void VerifyThermodynamicProcesses()
    {
        var hot = Q.ThermodynamicTemperature.FromKelvin(300);
        var cold = Q.ThermodynamicTemperature.FromKelvin(200);
        var gamma = Q.SpecificHeatCapacity.HeatCapacityRatio((Q.SpecificHeatCapacity)1000d, (Q.SpecificHeatCapacity)500d);
        Close(2, gamma);
        var initialVolume = Q.Volume.FromLitre(2);
        var finalVolume = Q.Volume.FromLitre(1);
        Close(400000, Q.ForcePerArea.FromPascal(100000).AfterIsentropicVolumeChange(
            initialVolume, finalVolume, gamma).Pascal);
        Close(600, hot.AfterIsentropicVolumeChange(initialVolume, finalVolume, gamma).Kelvin);
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
        Close(200 + 100 / Math.E, hot.AfterLumpedHeatTransfer(cold, resistance, capacity, Q.Time.FromSecond(2)).Kelvin);
        Close(0.5, resistance.InSeriesWith((Q.ThermalResistance)0.3d).Value);
        Close(0.12, resistance.InParallelWith((Q.ThermalResistance)0.3d).Value);

        var conductivity = (Q.ThermalConductivity)2d;
        var innerRadius = Q.Length.FromMillimetre(500);
        var outerRadius = Q.Length.FromMetre(1);
        Close(Math.Log(2) / (4 * Math.PI), Q.ThermalResistance.FromCylindricalConduction(
            conductivity, innerRadius, outerRadius, Q.Length.FromMetre(1)).Value);
        Close(1 / (8 * Math.PI), Q.ThermalResistance.FromSphericalConduction(conductivity, innerRadius, outerRadius).Value);
        var convection = (Q.CoefficientOfHeatTransfer)5d;
        var characteristicLength = Q.Length.FromMillimetre(100);
        Close(0.25, convection.BiotNumber(characteristicLength, conductivity));
        Close(1, convection.NusseltNumber(characteristicLength, (Q.ThermalConductivity)0.5d));
        var diffusivity = (Q.ThermalDiffusivity)0.1d;
        Close(2, (Q.KinematicViscosity)0.2d / diffusivity);
        Close(0.8, diffusivity.FourierNumber(Q.Time.FromSecond(2), innerRadius));
    }

    private static void Close(double expected, double actual)
    {
        if (!(Math.Abs(expected - actual) <= 1e-9))
        {
            throw new InvalidOperationException($"Expected {expected}, got {actual}.");
        }
    }
}
