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
        Close(24, volume.CubicMetre);
        Close(4, (volume / area).Metre);
        Close(2, (distance / Q.Length.FromMillimetre(300)).Value);

        var amount = Q.AmountOfSubstance.FromMole(2.5);
        Close(2500, amount.Millimole);

        var freezing = Q.ThermodynamicTemperature.FromDegreeCelsius(0);
        var boiling = Q.ThermodynamicTemperature.FromKelvin(373.15);
        Q.TemperatureDifference interval = boiling - freezing;
        Close(273.15, freezing.Kelvin);
        Close(100, interval.Kelvin);
        Close(100, (freezing + interval).DegreeCelsius);

        Q.SquareEnergy squaredEnergy = Q.Energy.FromJoule(3) * Q.Energy.FromJoule(4);
        Close(12, squaredEnergy.Value);
        Q.SquareTime squaredTime = Q.Time.FromSecond(2) * Q.Time.FromSecond(5);
        Close(10, squaredTime.Value);
        Close(5, (squaredTime / Q.Time.FromSecond(2)).Second);

        VerifyMechanicsAndFlow();
        VerifyElectromagnetism();
        VerifyHeatTransfer();
        VerifyLightChemistryAndDose();
        VerifyQuantityConversions();

        if (typeof(Q.Length).Assembly == typeof(TedToolkit.Quantities.Length).Assembly)
        {
            throw new InvalidOperationException("Unexpected quantity type.");
        }
    }

    private static void VerifyQuantityConversions()
    {
        var pressure = Q.Pressure.FromPascal(4);
        Q.ForcePerArea forcePerArea = pressure;
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
        Close(2, ((Q.DimensionlessRatio)Q.PlaneAngle.FromRadian(2)).Value);

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
        var diameter = Q.Diameter.FromMillimetre(200);
        var circleRadius = Q.Radius.FromDiameter(diameter);
        Close(100, circleRadius.Millimetre);
        Close(200, ((Q.Radius)diameter).Millimetre);
        Close(200, Q.Diameter.FromRadius(circleRadius).Millimetre);
        Close(0.2 * Math.PI, circleRadius.ToCircumference().Metre);
        Close(0.01 * Math.PI, Q.Area.FromCircle(circleRadius).SquareMetre);

        var wavelength = Q.Wavelength.FromMillimetre(500);
        Q.LinearVelocity waveSpeed = wavelength * frequency;
        Close(2, waveSpeed.MetrePerSecond);
        Close(4, (waveSpeed / wavelength).Hertz);
        Close(500, (waveSpeed / frequency).Millimetre);

        var angularAcceleration = (Q.AngularAcceleration)3d;
        Q.Torque torque = (Q.MomentOfInertia)2d * angularAcceleration;
        Q.AngularVelocity angularVelocity = angularAcceleration * Q.Time.FromSecond(4);
        Q.Power shaftPower = torque * angularVelocity;
        Close(72, shaftPower.Watt);
        Close(torque.Value, (shaftPower / angularVelocity).Value);
        Close(12, (angularVelocity * Q.Time.FromSecond(1)).Radian);

        var translatingMass = Q.Mass.FromKilogram(2);
        var speed = Q.LinearVelocity.FromMetrePerSecond(3);
        Close(9, Q.Energy.FromTranslationalKineticEnergy(translatingMass, speed).Joule);
        Close(144, Q.Energy.FromRotationalKineticEnergy((Q.MomentOfInertia)2d, angularVelocity).Joule);
        var resistingForce = Q.Force.FromNewton(-5);
        Close(-15, (resistingForce * speed).Watt);
        Close(-10, Q.Energy.FromMechanicalWork(resistingForce, radius).Joule);
        Close(10, Q.Torque.FromPerpendicularLeverArm(-resistingForce, radius).Value);
        var impulseDuration = Q.Time.FromMillisecond(200);
        Q.Impulse impulse = Q.Impulse.FromAverageForce(resistingForce, impulseDuration);
        Close(-1, ((Q.LinearMomentum)impulse).Value);
        Close(-5, (impulse / impulseDuration).Newton);

        var density = Q.Mass.FromKilogram(2) / Q.Volume.FromLitre(4);
        var volumeFlow = Q.Volume.FromLitre(3) / Q.Time.FromSecond(2);
        Q.MassPerTime massFlow = density * volumeFlow;
        Close(0.75, massFlow.Value);
        Close(volumeFlow.Value, (massFlow / density).Value);
        Close(0.002, ((Q.KinematicViscosity)4e-6 * density).Value);

        Q.ForcePerArea forcePerArea = Q.Force.FromNewton(20) / Q.Area.FromSquareMetre(0.5);
        var pressure = Q.Pressure.FromNormalForce(Q.Force.FromNewton(20), Q.Area.FromSquareMetre(0.5));
        Q.Power hydraulicPower = pressure * volumeFlow;
        Close(40, forcePerArea.Pascal);
        Close(0.06, (forcePerArea * volumeFlow).Watt);
        Close(40, pressure.Pascal);
        Close(0.06, hydraulicPower.Watt);
        Close(40, Q.Pressure.FromHydraulicPower(hydraulicPower, volumeFlow).Pascal);
        Close(0.12, Q.Energy.FromConstantPressureWork(pressure, Q.Volume.FromLitre(3)).Joule);
        Close(10000, Q.Pressure.FromHydrostaticHead(density, (Q.LinearAcceleration)10d, radius).Pascal);
        Close(density.Value, (1d / (1d / density)).Value);
        Close(2250, Q.Pressure.FromDynamicPressure(density, speed).Pascal);
        var kinematicViscosity = (Q.KinematicViscosity)4e-6;
        var reynolds = Q.ReynoldsNumber.FromFlow(speed, radius, kinematicViscosity);
        Q.DimensionlessRatio ratio = reynolds;
        Close(1500000, ratio.Value);
        Close(reynolds.Value, Q.ReynoldsNumber.FromFlow(speed, radius, density, kinematicViscosity * density).Value);
    }

    private static void VerifyElectromagnetism()
    {
        var resistance = Q.Resistance.FromOhm(10);
        var current = Q.ElectricCurrent.FromAmpere(4);
        Q.ElectricPotential voltage = resistance * current;
        Q.Power power = voltage * current;
        Close(40, voltage.Volt);
        Close(160, power.Watt);
        Close(10, (voltage / current).Ohm);
        Close(4, (power / voltage).Ampere);
        Q.Conductance conductance = current / voltage;
        Close(100, conductance.Millisiemens);
        Close(4, (conductance * voltage).Ampere);
        Close(10, (1d / conductance).Ohm);

        Q.Resistivity resistivity = 1d / (Q.ElectricConductivity)2d;
        Close(0.5, resistivity.Value);
        Close(1, Q.Resistance.FromUniformConductor(
            resistivity, Q.Length.FromMetre(4), Q.Area.FromSquareMetre(2)).Ohm);

        var path = Q.Length.FromMetre(2);
        var field = Q.ElectricFieldStrength.FromPotentialChange(voltage, path);
        Close(-40, (field * path).Volt);
        Close(-4, Q.ElectromotiveForce.FromFluxLinkageChange(
            Q.MagneticFlux.FromWeber(2), Q.Time.FromMillisecond(500)).Volt);

        Q.MagneticFlux flux = Q.MagneticFluxDensity.FromTesla(2) * Q.Area.FromSquareMetre(0.5);
        Close(1, flux.Weber);
        Close(2, (flux / Q.Area.FromSquareMetre(0.5)).Tesla);

        var capacitance = (Q.Capacitance)0.005d;
        Q.Time rcTimeConstant = resistance * capacitance;
        Q.Time rlTimeConstant = Q.Inductance.FromHenry(2) / resistance;
        Close(0.05, rcTimeConstant.Second);
        Close(0.2, rlTimeConstant.Second);
        Close(2, (rlTimeConstant * resistance).Henry);
        var plates = Q.Capacitance.FromParallelPlates(
            (Q.Permittivity)2d, Q.Area.FromSquareMetre(0.5), Q.Length.FromMillimetre(250));
        Close(4, plates.Value);
        Close(8, Q.Energy.FromCapacitor(plates, Q.ElectricPotential.FromVolt(2)).Joule);
    }

    private static void VerifyHeatTransfer()
    {
        var room = Q.ThermodynamicTemperature.FromDegreeCelsius(20);
        var heated = Q.ThermodynamicTemperature.FromDegreeCelsius(40);
        Q.TemperatureDifference temperatureRise = heated - room;
        Q.HeatCapacity capacity = (Q.SpecificHeatCapacity)1000d * Q.Mass.FromKilogram(2);
        Q.Energy heating = capacity * temperatureRise;
        Close(40, heating.Kilojoule);
        Close(2000, (heating / temperatureRise).Value);

        var coefficient = (Q.CoefficientOfHeatTransfer)0.5d;
        Q.HeatFlowRatePerArea heatFlux = coefficient * temperatureRise;
        Q.HeatFlowRate heatFlow = heatFlux * Q.Area.FromSquareMetre(10);
        Q.ThermalResistance resistance = temperatureRise / heatFlow;
        Close(100, heatFlow.Value);
        Close(0.2, resistance.Value);
        Close(2, (resistance * Q.Area.FromSquareMetre(10)).Value);
        Close(20, (resistance * heatFlow).Kelvin);
        Q.ThermalConductance conductance = 1d / resistance;
        Close(5, conductance.Value);
        Close(100, (conductance * temperatureRise).Value);
        Close(5, (coefficient * Q.Area.FromSquareMetre(10)).Value);
        Close(2, ((Q.LinearThermalExpansion)1e-4 * temperatureRise).Millimetre);

        var thickness = Q.Length.FromMillimetre(100);
        var conductivity = (Q.ThermalConductivity)0.5d;
        Q.TemperatureGradient gradient = temperatureRise / thickness;
        Close(200, gradient.Value);
        Close(-100, Q.HeatFlowRatePerArea.FromConduction(conductivity, gradient).Value);
        Close(100, Q.HeatFlowRatePerArea.FromConduction(conductivity, -gradient).Value);
        Q.ThermalInsulance insulance = thickness / conductivity;
        Close(0.2, insulance.Value);
        Close(0.2, ((1d / conductivity) * thickness).Value);
    }

    private static void VerifyLightChemistryAndDose()
    {
        Q.LuminousFlux light = Q.LuminousIntensity.FromCandela(20) * Q.SolidAngle.FromSteradian(3);
        Q.Illuminance illuminance = light / Q.Area.FromSquareMetre(2);
        Close(60, light.Lumen);
        Close(30, illuminance.Lux);
        Close(5, Q.Illuminance.FromNormalPointSource(
            Q.LuminousIntensity.FromCandela(20), Q.Length.FromMetre(2)).Lux);

        var amount = Q.AmountOfSubstance.FromMole(3);
        Q.Mass mass = (Q.MolarMass)0.028d * amount;
        Q.AmountOfSubstancePerVolume concentration = amount / Q.Volume.FromLitre(2);
        Close(0.084, mass.Kilogram);
        Close(1500, concentration.Value);
        Close(3, (concentration * Q.Volume.FromLitre(2)).Mole);
        Q.MolarVolume molarVolume = (Q.MolarMass)0.028d / (Q.Density)1.25d;
        Close(0.0224, molarVolume.Value);

        Q.AbsorbedDose dose = (Q.AbsorbedDoseRate)0.4d * Q.Time.FromSecond(5);
        Q.Energy deposited = dose * Q.Mass.FromKilogram(3);
        Q.SpecificEnergy perMass = deposited / Q.Mass.FromKilogram(3);
        Close(2, dose.Gray);
        Close(6, deposited.Joule);
        Close(2, perMass.Value);
        Close(2, Q.AbsorbedDose.FromDepositedEnergy(deposited, Q.Mass.FromKilogram(3)).Gray);
    }

    private static void Close(double expected, double actual)
    {
        if (!(Math.Abs(expected - actual) <= 1e-9))
        {
            throw new InvalidOperationException($"Expected {expected}, got {actual}.");
        }
    }
}
