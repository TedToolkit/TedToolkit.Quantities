// -----------------------------------------------------------------------
// <copyright file="QuantityRelations.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Quantities;

#pragma warning disable CS1591 // Formula documentation is recorded beside the relations below.

public readonly partial struct Length
{
    // Formula geometry/rectangle-area
    // Equation A = ab
    // Applies Rectangular sides are perpendicular. The inverse solves for the other side.
    // Source [OpenStax area](https://openstax.org/books/contemporary-mathematics/pages/10-6-area)
    public static Area operator *(Length left, Length right)
        => (Area)(left.Value * right.Value);

    // Formula geometry/length-ratio
    // Equation Length ratio = ℓ₂ / ℓ₁
    // Applies The reference length is nonzero.
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static DimensionlessRatio operator /(Length left, Length right)
        => (DimensionlessRatio)(left.Value / right.Value);

    public static LinearVelocity operator /(Length left, Time right)
        => (LinearVelocity)(left.Value / right.Value);

    public static Time operator /(Length left, LinearVelocity right)
        => (Time)(left.Value / right.Value);

    public static InverseLength operator /(double left, Length right)
        => (InverseLength)(left / right.Value);

    // Formula geometry/inverse-length
    // Equation Inverse length = 1 / ℓ
    // Applies Use a nonzero length. The product with the corresponding inverse length is one.
    // Source [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes)
    public static DimensionlessRatio operator *(Length left, InverseLength right)
        => (DimensionlessRatio)(left.Value * right.Value);

    public static DimensionlessRatio operator *(InverseLength left, Length right)
        => (DimensionlessRatio)(left.Value * right.Value);
}

public readonly partial struct Area
{
    // Formula geometry/prism-volume
    // Equation V = Aℓ
    // Applies Prism or extrusion with uniform cross section perpendicular to its length.
    // Source [OpenStax prism volume](https://openstax.org/books/contemporary-mathematics/pages/10-7-volume-and-surface-area)
    public static Volume operator *(Area left, Length right)
        => (Volume)(left.Value * right.Value);

    public static Volume operator *(Length left, Area right)
        => (Volume)(left.Value * right.Value);

    public static Length operator /(Area left, Length right)
        => (Length)(left.Value / right.Value);
}

public readonly partial struct Volume
{
    public static Length operator /(Volume left, Area right)
        => (Length)(left.Value / right.Value);

    public static Area operator /(Volume left, Length right)
        => (Area)(left.Value / right.Value);

    public static VolumePerTime operator /(Volume left, Time right)
        => (VolumePerTime)(left.Value / right.Value);
}

public readonly partial struct LinearVelocity
{
    // Formula mechanics/linear-displacement
    // Equation Δx = v̄Δt
    // Applies Constant or average velocity component along the displacement axis.
    // Source [OpenStax kinematics](https://openstax.org/books/university-physics-volume-1/pages/3-key-equations)
    public static Length operator *(LinearVelocity left, Time right)
        => (Length)(left.Value * right.Value);

    public static Length operator *(Time left, LinearVelocity right)
        => (Length)(left.Value * right.Value);

    public static LinearAcceleration operator /(LinearVelocity left, Time right)
        => (LinearAcceleration)(left.Value / right.Value);

    public static Time operator /(LinearVelocity left, LinearAcceleration right)
        => (Time)(left.Value / right.Value);
}

public readonly partial struct LinearAcceleration
{
    // Formula mechanics/velocity-change
    // Equation Δv = āΔt
    // Applies Constant or average acceleration component over the interval.
    // Source [OpenStax kinematics](https://openstax.org/books/university-physics-volume-1/pages/3-key-equations)
    public static LinearVelocity operator *(LinearAcceleration left, Time right)
        => (LinearVelocity)(left.Value * right.Value);

    public static LinearVelocity operator *(Time left, LinearAcceleration right)
        => (LinearVelocity)(left.Value * right.Value);
}

public readonly partial struct Mass
{
    // Formula mechanics/force
    // Equation F = ma
    // Applies Net force along an axis for a constant-mass body.
    // Source [OpenStax Newton's laws](https://openstax.org/books/university-physics-volume-1/pages/5-key-equations)
    public static Force operator *(Mass left, LinearAcceleration right)
        => (Force)(left.Value * right.Value);

    public static Force operator *(LinearAcceleration left, Mass right)
        => (Force)(left.Value * right.Value);

    // Formula mechanics/linear-momentum
    // Equation p = mv
    // Applies Linear momentum component for the same mass and velocity.
    // Source [OpenStax impulse and momentum](https://openstax.org/books/university-physics-volume-1/pages/9-key-equations)
    public static LinearMomentum operator *(Mass left, LinearVelocity right)
        => (LinearMomentum)(left.Value * right.Value);

    public static LinearMomentum operator *(LinearVelocity left, Mass right)
        => (LinearMomentum)(left.Value * right.Value);

    public static Density operator /(Mass left, Volume right)
        => (Density)(left.Value / right.Value);
}

public readonly partial struct Force
{
    public static LinearAcceleration operator /(Force left, Mass right)
        => (LinearAcceleration)(left.Value / right.Value);

    public static Mass operator /(Force left, LinearAcceleration right)
        => (Mass)(left.Value / right.Value);
}

public readonly partial struct Density
{
    // Formula fluids/density-and-specific-volume
    // Equation ρ = m / V, vₛ = V / m = 1 / ρ
    // Applies Density and specific volume describe the same material at the same state.
    // Source [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations)
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static Mass operator *(Density left, Volume right)
        => (Mass)(left.Value * right.Value);

    public static Mass operator *(Volume left, Density right)
        => (Mass)(left.Value * right.Value);
}

public readonly partial struct Power
{
    // Formula mechanics/energy-from-power
    // Equation E = P̄Δt
    // Applies Constant or average power or heat-flow rate over the interval.
    // Source [OpenStax work and energy](https://openstax.org/books/university-physics-volume-1/pages/7-key-equations)
    // Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
    public static Energy operator *(Power left, Time right)
        => (Energy)(left.Value * right.Value);

    public static Energy operator *(Time left, Power right)
        => (Energy)(left.Value * right.Value);
}

public readonly partial struct Energy
{
    public static Power operator /(Energy left, Time right)
        => (Power)(left.Value / right.Value);

    public static Time operator /(Energy left, Power right)
        => (Time)(left.Value / right.Value);

    // Formula geometry/squared-energy
    // Equation Squared energy = E²
    // Applies Algebraic square of the energy value.
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static SquareEnergy operator *(Energy left, Energy right)
        => (SquareEnergy)(left.Value * right.Value);
}

public readonly partial struct ElectricCurrent
{
    // Formula electromagnetism/charge-from-current
    // Equation ΔQ = ĪΔt
    // Applies Use average electrical current through the same cross section and interval.
    // Source [OpenStax current and resistance](https://openstax.org/books/university-physics-volume-2/pages/9-key-equations)
    public static ElectricCharge operator *(ElectricCurrent left, Time right)
        => (ElectricCharge)(left.Value * right.Value);

    public static ElectricCharge operator *(Time left, ElectricCurrent right)
        => (ElectricCharge)(left.Value * right.Value);
}

public readonly partial struct ElectricCharge
{
    public static ElectricCurrent operator /(ElectricCharge left, Time right)
        => (ElectricCurrent)(left.Value / right.Value);

    public static Time operator /(ElectricCharge left, ElectricCurrent right)
        => (Time)(left.Value / right.Value);
}

public readonly partial struct ThermodynamicTemperature
{
    public static TemperatureDifference operator -(ThermodynamicTemperature left, ThermodynamicTemperature right)
        => (TemperatureDifference)(left.Value - right.Value);

    // Formula thermal/temperature-change
    // Equation ΔT = T₂ − T₁, T₂ = T₁ + ΔT
    // Applies Use the temperature difference as the increment of an absolute temperature.
    // Source [BIPM SI Brochure](https://www.bipm.org/en/publications/si-brochure)
    public static ThermodynamicTemperature operator +(ThermodynamicTemperature left, TemperatureDifference right)
        => (ThermodynamicTemperature)(left.Value + right.Value);

    public static ThermodynamicTemperature operator +(TemperatureDifference left, ThermodynamicTemperature right)
        => (ThermodynamicTemperature)(left.Value + right.Value);

    public static ThermodynamicTemperature operator -(ThermodynamicTemperature left, TemperatureDifference right)
        => (ThermodynamicTemperature)(left.Value - right.Value);
}

// Linear and rotational mechanics.

public readonly partial struct Time
{
    // Formula geometry/squared-time
    // Equation Squared time = t²
    // Applies Algebraic square of the elapsed time.
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static SquareTime operator *(Time left, Time right)
        => (SquareTime)(left.Value * right.Value);
}

public readonly partial struct SquareTime
{
    public static Time operator /(SquareTime left, Time right)
        => (Time)(left.Value / right.Value);
}

public readonly partial struct LinearMomentum
{
    public static LinearVelocity operator /(LinearMomentum left, Mass right)
        => (LinearVelocity)(left.Value / right.Value);

    public static Mass operator /(LinearMomentum left, LinearVelocity right)
        => (Mass)(left.Value / right.Value);

    public static Time operator /(LinearMomentum left, Force right)
        => (Time)(left.Value / right.Value);

    public static Force operator /(LinearMomentum left, Time right)
        => (Force)(left.Value / right.Value);
}

public readonly partial struct Force
{
    // Average force over the interval gives the change in linear momentum.
    // Formula mechanics/impulse-and-momentum-change
    // Equation J = Δp = F̄Δt
    // Applies Use the signed component of the average force over the interval.
    // Source [OpenStax impulse and momentum](https://openstax.org/books/university-physics-volume-1/pages/9-key-equations)
    public static LinearMomentum operator *(Force left, Time right)
        => (LinearMomentum)(left.Value * right.Value);

    public static LinearMomentum operator *(Time left, Force right)
        => (LinearMomentum)(left.Value * right.Value);

    public static Area operator /(Force left, ForcePerArea right)
        => (Area)(left.Value / right.Value);

    public static ForcePerArea operator /(Force left, Area right)
        => (ForcePerArea)(left.Value / right.Value);

    public static Length operator /(Force left, ForcePerLength right)
        => (Length)(left.Value / right.Value);

    public static ForcePerLength operator /(Force left, Length right)
        => (ForcePerLength)(left.Value / right.Value);
}

public readonly partial struct ForcePerArea
{
    // Formula fluids/pressure-from-force
    // Equation p = Fₙ / A
    // Applies Use the normal force over the surface. The general division returns `ForcePerArea`.
    // Source [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations)
    public static Force operator *(ForcePerArea left, Area right)
        => (Force)(left.Value * right.Value);

    public static Force operator *(Area left, ForcePerArea right)
        => (Force)(left.Value * right.Value);

    // Formula fluids/hydraulic-power
    // Equation P = ΔpQᵥ
    // Applies Pressure difference and flow refer to the same path. Use pressures referenced to the same datum.
    // Source [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations)
    // Source [OpenStax pressure–volume work](https://openstax.org/books/university-physics-volume-2/pages/3-key-equations)
    public static Power operator *(ForcePerArea left, VolumePerTime right)
        => (Power)(left.Value * right.Value);

    public static Power operator *(VolumePerTime left, ForcePerArea right)
        => (Power)(left.Value * right.Value);
}

public readonly partial struct ForcePerLength
{
    // Formula mechanics/distributed-force
    // Equation F = (F / ℓ) × ℓ
    // Applies Uniform or mean force per length over the loaded span.
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    // Source [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes)
    public static Force operator *(ForcePerLength left, Length right)
        => (Force)(left.Value * right.Value);

    public static Force operator *(Length left, ForcePerLength right)
        => (Force)(left.Value * right.Value);
}

public readonly partial struct AngularVelocity
{
    // Formula mechanics/angular-displacement
    // Equation Δθ = ω̄Δt
    // Applies Fixed axis and angular velocity averaged over the interval.
    // Source [OpenStax rotational variables](https://openstax.org/books/university-physics-volume-1/pages/10-1-rotational-variables)
    public static PlaneAngle operator *(AngularVelocity left, Time right)
        => (PlaneAngle)(left.Value * right.Value);

    public static PlaneAngle operator *(Time left, AngularVelocity right)
        => (PlaneAngle)(left.Value * right.Value);

    public static Time operator /(AngularVelocity left, AngularAcceleration right)
        => (Time)(left.Value / right.Value);

    public static AngularAcceleration operator /(AngularVelocity left, Time right)
        => (AngularAcceleration)(left.Value / right.Value);
}

public readonly partial struct PlaneAngle
{
    public static Time operator /(PlaneAngle left, AngularVelocity right)
        => (Time)(left.Value / right.Value);

    public static AngularVelocity operator /(PlaneAngle left, Time right)
        => (AngularVelocity)(left.Value / right.Value);
}

public readonly partial struct AngularAcceleration
{
    // Formula mechanics/angular-velocity-change
    // Equation Δω = ᾱΔt
    // Applies Fixed axis and angular acceleration averaged over the interval.
    // Source [OpenStax fixed-axis rotation](https://openstax.org/books/university-physics-volume-1/pages/10-key-equations)
    public static AngularVelocity operator *(AngularAcceleration left, Time right)
        => (AngularVelocity)(left.Value * right.Value);

    public static AngularVelocity operator *(Time left, AngularAcceleration right)
        => (AngularVelocity)(left.Value * right.Value);
}

public readonly partial struct MomentOfInertia
{
    // Formula mechanics/torque-from-angular-acceleration
    // Equation τ = Iα
    // Applies Net torque and moment of inertia about the same fixed axis.
    // Source [OpenStax fixed-axis rotation](https://openstax.org/books/university-physics-volume-1/pages/10-key-equations)
    public static Torque operator *(MomentOfInertia left, AngularAcceleration right)
        => (Torque)(left.Value * right.Value);

    public static Torque operator *(AngularAcceleration left, MomentOfInertia right)
        => (Torque)(left.Value * right.Value);

    // Formula mechanics/angular-momentum
    // Equation L = Iω
    // Applies Rigid-body angular momentum component about the specified axis.
    // Source [OpenStax angular momentum](https://openstax.org/books/university-physics-volume-1/pages/11-summary)
    public static AngularMomentum operator *(MomentOfInertia left, AngularVelocity right)
        => (AngularMomentum)(left.Value * right.Value);

    public static AngularMomentum operator *(AngularVelocity left, MomentOfInertia right)
        => (AngularMomentum)(left.Value * right.Value);
}

public readonly partial struct Torque
{
    public static AngularAcceleration operator /(Torque left, MomentOfInertia right)
        => (AngularAcceleration)(left.Value / right.Value);

    public static MomentOfInertia operator /(Torque left, AngularAcceleration right)
        => (MomentOfInertia)(left.Value / right.Value);

    // Average torque over the interval gives the change in angular momentum about the same axis.
    // Formula mechanics/angular-momentum-change
    // Equation ΔL = τ̄Δt
    // Applies Average torque and angular momentum change about the same axis.
    // Source [OpenStax angular momentum](https://openstax.org/books/university-physics-volume-1/pages/11-summary)
    public static AngularMomentum operator *(Torque left, Time right)
        => (AngularMomentum)(left.Value * right.Value);

    public static AngularMomentum operator *(Time left, Torque right)
        => (AngularMomentum)(left.Value * right.Value);

    // Formula mechanics/shaft-power
    // Equation P = τω
    // Applies Corresponding torque and angular velocity components about the same axis.
    // Source [OpenStax fixed-axis rotation](https://openstax.org/books/university-physics-volume-1/pages/10-key-equations)
    public static Power operator *(Torque left, AngularVelocity right)
        => (Power)(left.Value * right.Value);

    public static Power operator *(AngularVelocity left, Torque right)
        => (Power)(left.Value * right.Value);

    // Formula mechanics/rotational-work
    // Equation W = τ̄Δθ
    // Applies Constant or angle-averaged torque along the rotation.
    // Source [OpenStax fixed-axis rotation](https://openstax.org/books/university-physics-volume-1/pages/10-key-equations)
    public static Energy operator *(Torque left, PlaneAngle right)
        => (Energy)(left.Value * right.Value);

    public static Energy operator *(PlaneAngle left, Torque right)
        => (Energy)(left.Value * right.Value);
}

public readonly partial struct AngularMomentum
{
    public static AngularVelocity operator /(AngularMomentum left, MomentOfInertia right)
        => (AngularVelocity)(left.Value / right.Value);

    public static MomentOfInertia operator /(AngularMomentum left, AngularVelocity right)
        => (MomentOfInertia)(left.Value / right.Value);

    public static Time operator /(AngularMomentum left, Torque right)
        => (Time)(left.Value / right.Value);

    public static Torque operator /(AngularMomentum left, Time right)
        => (Torque)(left.Value / right.Value);
}

public readonly partial struct Power
{
    public static AngularVelocity operator /(Power left, Torque right)
        => (AngularVelocity)(left.Value / right.Value);

    public static Torque operator /(Power left, AngularVelocity right)
        => (Torque)(left.Value / right.Value);

    public static VolumePerTime operator /(Power left, ForcePerArea right)
        => (VolumePerTime)(left.Value / right.Value);

    public static ForcePerArea operator /(Power left, VolumePerTime right)
        => (ForcePerArea)(left.Value / right.Value);
}

public readonly partial struct Energy
{
    public static PlaneAngle operator /(Energy left, Torque right)
        => (PlaneAngle)(left.Value / right.Value);

    public static Torque operator /(Energy left, PlaneAngle right)
        => (Torque)(left.Value / right.Value);
}

// Mass, fluid flow, and energy distributions.

public readonly partial struct Mass
{
    public static Volume operator /(Mass left, Density right)
        => (Volume)(left.Value / right.Value);

    public static Length operator /(Mass left, MassPerLength right)
        => (Length)(left.Value / right.Value);

    public static MassPerLength operator /(Mass left, Length right)
        => (MassPerLength)(left.Value / right.Value);

    public static Area operator /(Mass left, MassPerArea right)
        => (Area)(left.Value / right.Value);

    public static MassPerArea operator /(Mass left, Area right)
        => (MassPerArea)(left.Value / right.Value);

    public static Time operator /(Mass left, MassPerTime right)
        => (Time)(left.Value / right.Value);

    public static MassPerTime operator /(Mass left, Time right)
        => (MassPerTime)(left.Value / right.Value);
}

public readonly partial struct MassPerLength
{
    // Formula mechanics/linear-mass-density
    // Equation m = λₘℓ
    // Applies Uniform or mean linear mass density λₘ over the length.
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static Mass operator *(MassPerLength left, Length right)
        => (Mass)(left.Value * right.Value);

    public static Mass operator *(Length left, MassPerLength right)
        => (Mass)(left.Value * right.Value);
}

public readonly partial struct MassPerArea
{
    // Formula mechanics/areal-mass-density
    // Equation m = ρₐA
    // Applies Uniform or mean areal mass density ρₐ over the area.
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static Mass operator *(MassPerArea left, Area right)
        => (Mass)(left.Value * right.Value);

    public static Mass operator *(Area left, MassPerArea right)
        => (Mass)(left.Value * right.Value);

    public static Time operator /(MassPerArea left, MassPerAreaTime right)
        => (Time)(left.Value / right.Value);

    public static MassPerAreaTime operator /(MassPerArea left, Time right)
        => (MassPerAreaTime)(left.Value / right.Value);
}

public readonly partial struct MassPerTime
{
    // Formula fluids/mass-from-flow
    // Equation Δm = ṁ̄Δt
    // Applies Constant or time-averaged mass flow rate over the interval.
    // Source [NASA mass flow](https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/mass-flow-rate/)
    public static Mass operator *(MassPerTime left, Time right)
        => (Mass)(left.Value * right.Value);

    public static Mass operator *(Time left, MassPerTime right)
        => (Mass)(left.Value * right.Value);

    public static Area operator /(MassPerTime left, MassPerAreaTime right)
        => (Area)(left.Value / right.Value);

    public static MassPerAreaTime operator /(MassPerTime left, Area right)
        => (MassPerAreaTime)(left.Value / right.Value);

    public static VolumePerTime operator /(MassPerTime left, Density right)
        => (VolumePerTime)(left.Value / right.Value);

    public static Density operator /(MassPerTime left, VolumePerTime right)
        => (Density)(left.Value / right.Value);
}

public readonly partial struct MassPerAreaTime
{
    // Formula fluids/areal-mass-flow
    public static MassPerTime operator *(MassPerAreaTime left, Area right)
        => (MassPerTime)(left.Value * right.Value);

    public static MassPerTime operator *(Area left, MassPerAreaTime right)
        => (MassPerTime)(left.Value * right.Value);

    // Formula fluids/areal-mass-flow
    // Equation jₘ = ṁ / A = Δ(m/A) / Δt
    // Applies Use the same oriented surface and interval for the areal mass flow rate jₘ.
    // Source [NASA mass flow](https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/mass-flow-rate/)
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static MassPerArea operator *(MassPerAreaTime left, Time right)
        => (MassPerArea)(left.Value * right.Value);

    public static MassPerArea operator *(Time left, MassPerAreaTime right)
        => (MassPerArea)(left.Value * right.Value);
}

public readonly partial struct SpecificVolume
{
    // Formula fluids/density-and-specific-volume
    public static Volume operator *(SpecificVolume left, Mass right)
        => (Volume)(left.Value * right.Value);

    public static Volume operator *(Mass left, SpecificVolume right)
        => (Volume)(left.Value * right.Value);
}

public readonly partial struct Volume
{
    public static Mass operator /(Volume left, SpecificVolume right)
        => (Mass)(left.Value / right.Value);

    public static SpecificVolume operator /(Volume left, Mass right)
        => (SpecificVolume)(left.Value / right.Value);

    public static Time operator /(Volume left, VolumePerTime right)
        => (Time)(left.Value / right.Value);
}

public readonly partial struct Area
{
    // Formula fluids/volume-flow-from-velocity
    // Equation Qᵥ = Avₙ
    // Applies Area-averaged normal speed component through the section.
    // Source [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations)
    // Source [NASA mass flow](https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/mass-flow-rate/)
    public static VolumePerTime operator *(Area left, LinearVelocity right)
        => (VolumePerTime)(left.Value * right.Value);

    public static VolumePerTime operator *(LinearVelocity left, Area right)
        => (VolumePerTime)(left.Value * right.Value);
}

public readonly partial struct VolumePerTime
{
    public static LinearVelocity operator /(VolumePerTime left, Area right)
        => (LinearVelocity)(left.Value / right.Value);

    public static Area operator /(VolumePerTime left, LinearVelocity right)
        => (Area)(left.Value / right.Value);

    // Formula fluids/volume-from-flow
    // Equation ΔV = Q̄ᵥΔt
    // Applies Constant or time-averaged volumetric flow rate.
    // Source [OpenStax fluid mechanics](https://openstax.org/books/university-physics-volume-1/pages/14-key-equations)
    public static Volume operator *(VolumePerTime left, Time right)
        => (Volume)(left.Value * right.Value);

    public static Volume operator *(Time left, VolumePerTime right)
        => (Volume)(left.Value * right.Value);
}

public readonly partial struct Density
{
    // Formula fluids/mass-flow-from-volume-flow
    // Equation ṁ = ρQᵥ
    // Applies Use consistent density and volumetric flow at the same section.
    // Source [NASA mass flow](https://www1.grc.nasa.gov/beginners-guide-to-aeronautics/mass-flow-rate/)
    public static MassPerTime operator *(Density left, VolumePerTime right)
        => (MassPerTime)(left.Value * right.Value);

    public static MassPerTime operator *(VolumePerTime left, Density right)
        => (MassPerTime)(left.Value * right.Value);
}

public readonly partial struct KinematicViscosity
{
    // Formula fluids/viscosity
    // Equation μ = ρν
    // Applies Density, dynamic viscosity, and kinematic viscosity at the same fluid state.
    // Source [NASA viscosity and flow](https://ntrs.nasa.gov/api/citations/20250001477/downloads/Paul%20Gradl_Dissertation_NASA_Title_2-11-2025.pdf)
    public static DynamicViscosity operator *(KinematicViscosity left, Density right)
        => (DynamicViscosity)(left.Value * right.Value);

    public static DynamicViscosity operator *(Density left, KinematicViscosity right)
        => (DynamicViscosity)(left.Value * right.Value);
}

public readonly partial struct DynamicViscosity
{
    public static Density operator /(DynamicViscosity left, KinematicViscosity right)
        => (Density)(left.Value / right.Value);

    public static KinematicViscosity operator /(DynamicViscosity left, Density right)
        => (KinematicViscosity)(left.Value / right.Value);
}

public readonly partial struct SpecificEnergy
{
    // Formula mechanics/specific-energy
    // Equation e = E / m
    // Applies Energy and mass refer to the same body or sample.
    // Source [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes)
    public static Energy operator *(SpecificEnergy left, Mass right)
        => (Energy)(left.Value * right.Value);

    public static Energy operator *(Mass left, SpecificEnergy right)
        => (Energy)(left.Value * right.Value);
}

public readonly partial struct Energy
{
    public static Mass operator /(Energy left, SpecificEnergy right)
        => (Mass)(left.Value / right.Value);

    public static SpecificEnergy operator /(Energy left, Mass right)
        => (SpecificEnergy)(left.Value / right.Value);

    public static Volume operator /(Energy left, EnergyDensity right)
        => (Volume)(left.Value / right.Value);

    public static EnergyDensity operator /(Energy left, Volume right)
        => (EnergyDensity)(left.Value / right.Value);

    public static Area operator /(Energy left, EnergyPerArea right)
        => (Area)(left.Value / right.Value);

    public static EnergyPerArea operator /(Energy left, Area right)
        => (EnergyPerArea)(left.Value / right.Value);
}

public readonly partial struct EnergyDensity
{
    // Formula mechanics/energy-density
    // Equation u = E / V
    // Applies Average energy per volume of the same region.
    // Source [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes)
    public static Energy operator *(EnergyDensity left, Volume right)
        => (Energy)(left.Value * right.Value);

    public static Energy operator *(Volume left, EnergyDensity right)
        => (Energy)(left.Value * right.Value);
}

public readonly partial struct EnergyPerArea
{
    // Formula mechanics/energy-per-area
    // Equation Eₐ = E / A
    // Applies Average energy per area over the relevant surface.
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static Energy operator *(EnergyPerArea left, Area right)
        => (Energy)(left.Value * right.Value);

    public static Energy operator *(Area left, EnergyPerArea right)
        => (Energy)(left.Value * right.Value);
}

public readonly partial struct PowerPerArea
{
    // Formula mechanics/power-per-area
    // Equation Pₐ = P / A
    // Applies Average power per area over the same surface.
    // Source [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes)
    public static Power operator *(PowerPerArea left, Area right)
        => (Power)(left.Value * right.Value);

    public static Power operator *(Area left, PowerPerArea right)
        => (Power)(left.Value * right.Value);
}

public readonly partial struct Power
{
    public static Area operator /(Power left, PowerPerArea right)
        => (Area)(left.Value / right.Value);

    public static PowerPerArea operator /(Power left, Area right)
        => (PowerPerArea)(left.Value / right.Value);
}

// Electricity and magnetism.

public readonly partial struct Resistance
{
    // Formula electromagnetism/ohms-law
    // Equation V = IR, I = GV, G = 1 / R
    // Applies Voltage and current refer to the same ohmic element or operating point.
    // Source [OpenStax current and resistance](https://openstax.org/books/university-physics-volume-2/pages/9-key-equations)
    // Source [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes)
    public static ElectricPotential operator *(Resistance left, ElectricCurrent right)
        => (ElectricPotential)(left.Value * right.Value);

    public static ElectricPotential operator *(ElectricCurrent left, Resistance right)
        => (ElectricPotential)(left.Value * right.Value);

    // Formula electromagnetism/rc-time-constant
    // Equation τ = RC
    // Applies Time constant of the equivalent first-order RC circuit.
    // Source [OpenStax RC circuits](https://openstax.org/books/university-physics-volume-2/pages/10-key-equations)
    public static Time operator *(Resistance left, Capacitance right)
        => (Time)(left.Value * right.Value);

    public static Time operator *(Capacitance left, Resistance right)
        => (Time)(left.Value * right.Value);

    // Formula electromagnetism/rl-time-constant
    // Equation τ = L / R
    // Applies Time constant of the equivalent first-order RL circuit.
    // Source [OpenStax RL circuits](https://openstax.org/books/university-physics-volume-2/pages/14-key-equations)
    public static Inductance operator *(Resistance left, Time right)
        => (Inductance)(left.Value * right.Value);

    public static Inductance operator *(Time left, Resistance right)
        => (Inductance)(left.Value * right.Value);
}

public readonly partial struct Time
{
    public static Capacitance operator /(Time left, Resistance right)
        => (Capacitance)(left.Value / right.Value);

    public static Resistance operator /(Time left, Capacitance right)
        => (Resistance)(left.Value / right.Value);
}

public readonly partial struct Inductance
{
    public static Time operator /(Inductance left, Resistance right)
        => (Time)(left.Value / right.Value);

    public static Resistance operator /(Inductance left, Time right)
        => (Resistance)(left.Value / right.Value);
}

public readonly partial struct ElectricPotential
{
    public static ElectricCurrent operator /(ElectricPotential left, Resistance right)
        => (ElectricCurrent)(left.Value / right.Value);

    public static Resistance operator /(ElectricPotential left, ElectricCurrent right)
        => (Resistance)(left.Value / right.Value);

    // Use corresponding instantaneous voltage and current, with the passive sign convention for absorbed power.
    // Formula electromagnetism/electrical-power
    // Equation P = VI
    // Applies Use corresponding instantaneous voltage and current. Use the passive sign convention for absorbed power.
    // Source [OpenStax current and resistance](https://openstax.org/books/university-physics-volume-2/pages/9-key-equations)
    // Source [OpenStax AC power](https://openstax.org/books/university-physics-volume-2/pages/15-key-equations)
    public static Power operator *(ElectricPotential left, ElectricCurrent right)
        => (Power)(left.Value * right.Value);

    public static Power operator *(ElectricCurrent left, ElectricPotential right)
        => (Power)(left.Value * right.Value);

    // Formula electromagnetism/potential-energy-change
    // Equation ΔU = QΔV
    // Applies Change in electrical potential energy for charge moved through a fixed potential difference.
    // Source [OpenStax electric potential](https://openstax.org/books/university-physics-volume-2/pages/7-key-equations)
    public static Energy operator *(ElectricPotential left, ElectricCharge right)
        => (Energy)(left.Value * right.Value);

    public static Energy operator *(ElectricCharge left, ElectricPotential right)
        => (Energy)(left.Value * right.Value);

    public static Length operator /(ElectricPotential left, ElectricFieldStrength right)
        => (Length)(left.Value / right.Value);

    public static ElectricFieldStrength operator /(ElectricPotential left, Length right)
        => (ElectricFieldStrength)(left.Value / right.Value);
}

public readonly partial struct Power
{
    public static ElectricCurrent operator /(Power left, ElectricPotential right)
        => (ElectricCurrent)(left.Value / right.Value);

    public static ElectricPotential operator /(Power left, ElectricCurrent right)
        => (ElectricPotential)(left.Value / right.Value);
}

public readonly partial struct Energy
{
    public static ElectricCharge operator /(Energy left, ElectricPotential right)
        => (ElectricCharge)(left.Value / right.Value);

    public static ElectricPotential operator /(Energy left, ElectricCharge right)
        => (ElectricPotential)(left.Value / right.Value);
}

public readonly partial struct Capacitance
{
    // Formula electromagnetism/capacitor-charge
    // Equation Q = CV
    // Applies Linear capacitor with the voltage across its terminals.
    // Source [OpenStax capacitance](https://openstax.org/books/university-physics-volume-2/pages/8-key-equations)
    public static ElectricCharge operator *(Capacitance left, ElectricPotential right)
        => (ElectricCharge)(left.Value * right.Value);

    public static ElectricCharge operator *(ElectricPotential left, Capacitance right)
        => (ElectricCharge)(left.Value * right.Value);
}

public readonly partial struct ElectricCharge
{
    public static ElectricPotential operator /(ElectricCharge left, Capacitance right)
        => (ElectricPotential)(left.Value / right.Value);

    public static Capacitance operator /(ElectricCharge left, ElectricPotential right)
        => (Capacitance)(left.Value / right.Value);

    public static Length operator /(ElectricCharge left, ElectricChargeLineDensity right)
        => (Length)(left.Value / right.Value);

    public static ElectricChargeLineDensity operator /(ElectricCharge left, Length right)
        => (ElectricChargeLineDensity)(left.Value / right.Value);

    public static Area operator /(ElectricCharge left, ElectricChargePerArea right)
        => (Area)(left.Value / right.Value);

    public static ElectricChargePerArea operator /(ElectricCharge left, Area right)
        => (ElectricChargePerArea)(left.Value / right.Value);

    public static Volume operator /(ElectricCharge left, ElectricChargeVolumeDensity right)
        => (Volume)(left.Value / right.Value);

    public static ElectricChargeVolumeDensity operator /(ElectricCharge left, Volume right)
        => (ElectricChargeVolumeDensity)(left.Value / right.Value);

    public static Mass operator /(ElectricCharge left, ElectricChargePerMass right)
        => (Mass)(left.Value / right.Value);

    public static ElectricChargePerMass operator /(ElectricCharge left, Mass right)
        => (ElectricChargePerMass)(left.Value / right.Value);

    public static AmountOfSubstance operator /(ElectricCharge left, ElectricChargePerAmountOfSubstance right)
        => (AmountOfSubstance)(left.Value / right.Value);

    public static ElectricChargePerAmountOfSubstance operator /(ElectricCharge left, AmountOfSubstance right)
        => (ElectricChargePerAmountOfSubstance)(left.Value / right.Value);

    // For equal and opposite charges, use the separation directed from the negative to the positive charge.
    // Formula electromagnetism/dipole-moment
    // Equation p = Qd
    // Applies Equal and opposite charges. Separation is directed from negative charge to positive charge.
    // Source [OpenStax electric fields](https://openstax.org/books/university-physics-volume-2/pages/5-key-equations)
    public static ElectricDipoleMoment operator *(ElectricCharge left, Length right)
        => (ElectricDipoleMoment)(left.Value * right.Value);

    public static ElectricDipoleMoment operator *(Length left, ElectricCharge right)
        => (ElectricDipoleMoment)(left.Value * right.Value);
}

public readonly partial struct ElectricFieldStrength
{
    // The field component times path length gives the voltage drop, V_start - V_end.
    // Formula electromagnetism/electric-field-and-voltage
    // Equation Vdrop = E∥ℓ, ΔV = −E∥ℓ
    // Applies Use the path-averaged field component and final-minus-initial voltage for ΔV.
    // Source [OpenStax electric potential](https://openstax.org/books/university-physics-volume-2/pages/7-key-equations)
    public static ElectricPotential operator *(ElectricFieldStrength left, Length right)
        => (ElectricPotential)(left.Value * right.Value);

    public static ElectricPotential operator *(Length left, ElectricFieldStrength right)
        => (ElectricPotential)(left.Value * right.Value);

    // Formula electromagnetism/electric-force
    // Equation F = QE
    // Applies Use the electric field component at the charge.
    // Source [OpenStax electric fields](https://openstax.org/books/university-physics-volume-2/pages/5-key-equations)
    public static Force operator *(ElectricFieldStrength left, ElectricCharge right)
        => (Force)(left.Value * right.Value);

    public static Force operator *(ElectricCharge left, ElectricFieldStrength right)
        => (Force)(left.Value * right.Value);

    // Formula electromagnetism/electric-flux
    // Equation Φₑ = EₙA
    // Applies Uniform or area-averaged normal electric field on the surface.
    // Source [OpenStax electric fields](https://openstax.org/books/university-physics-volume-2/pages/5-key-equations)
    // Source [MIT linear electromagnetic media](https://ocw.mit.edu/courses/18-325-topics-in-applied-mathematics-waves-and-imaging-fall-2015/c5db24b1a6d0d3301b1a3f21f6ceba3a_MIT18_325F15_CompleteLect.pdf)
    public static ElectricFlux operator *(ElectricFieldStrength left, Area right)
        => (ElectricFlux)(left.Value * right.Value);

    public static ElectricFlux operator *(Area left, ElectricFieldStrength right)
        => (ElectricFlux)(left.Value * right.Value);
}

public readonly partial struct Force
{
    public static ElectricCharge operator /(Force left, ElectricFieldStrength right)
        => (ElectricCharge)(left.Value / right.Value);

    public static ElectricFieldStrength operator /(Force left, ElectricCharge right)
        => (ElectricFieldStrength)(left.Value / right.Value);
}

public readonly partial struct ElectricFlux
{
    public static Area operator /(ElectricFlux left, ElectricFieldStrength right)
        => (Area)(left.Value / right.Value);

    public static ElectricFieldStrength operator /(ElectricFlux left, Area right)
        => (ElectricFieldStrength)(left.Value / right.Value);
}

public readonly partial struct ElectricConductivity
{
    // Formula electromagnetism/ohmic-current-density
    // Equation J = σE
    // Applies Scalar conductivity in the local ohmic regime. Use corresponding components.
    // Source [OpenStax resistivity](https://openstax.org/books/university-physics-volume-2/pages/9-3-resistivity-and-resistance)
    public static ElectricCurrentDensity operator *(ElectricConductivity left, ElectricFieldStrength right)
        => (ElectricCurrentDensity)(left.Value * right.Value);

    public static ElectricCurrentDensity operator *(ElectricFieldStrength left, ElectricConductivity right)
        => (ElectricCurrentDensity)(left.Value * right.Value);
}

public readonly partial struct ElectricCurrentDensity
{
    public static ElectricFieldStrength operator /(ElectricCurrentDensity left, ElectricConductivity right)
        => (ElectricFieldStrength)(left.Value / right.Value);

    public static ElectricConductivity operator /(ElectricCurrentDensity left, ElectricFieldStrength right)
        => (ElectricConductivity)(left.Value / right.Value);

    // Formula electromagnetism/current-through-surface
    // Equation I = JₙA
    // Applies Uniform or area-averaged normal current density through the section.
    // Source [OpenStax current and resistance](https://openstax.org/books/university-physics-volume-2/pages/9-key-equations)
    public static ElectricCurrent operator *(ElectricCurrentDensity left, Area right)
        => (ElectricCurrent)(left.Value * right.Value);

    public static ElectricCurrent operator *(Area left, ElectricCurrentDensity right)
        => (ElectricCurrent)(left.Value * right.Value);
}

public readonly partial struct ElectricCurrent
{
    public static Area operator /(ElectricCurrent left, ElectricCurrentDensity right)
        => (Area)(left.Value / right.Value);

    public static ElectricCurrentDensity operator /(ElectricCurrent left, Area right)
        => (ElectricCurrentDensity)(left.Value / right.Value);
}

public readonly partial struct ElectricChargeLineDensity
{
    // Formula electromagnetism/line-charge-density
    // Equation Q = λqℓ
    // Applies Uniform or mean line charge density λq over the length.
    // Source [OpenStax electric fields](https://openstax.org/books/university-physics-volume-2/pages/5-key-equations)
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static ElectricCharge operator *(ElectricChargeLineDensity left, Length right)
        => (ElectricCharge)(left.Value * right.Value);

    public static ElectricCharge operator *(Length left, ElectricChargeLineDensity right)
        => (ElectricCharge)(left.Value * right.Value);
}

public readonly partial struct ElectricChargePerArea
{
    // Formula electromagnetism/surface-charge-density
    // Equation Q = σqA
    // Applies Uniform or mean surface charge density σq over the area.
    // Source [OpenStax electric fields](https://openstax.org/books/university-physics-volume-2/pages/5-key-equations)
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static ElectricCharge operator *(ElectricChargePerArea left, Area right)
        => (ElectricCharge)(left.Value * right.Value);

    public static ElectricCharge operator *(Area left, ElectricChargePerArea right)
        => (ElectricCharge)(left.Value * right.Value);
}

public readonly partial struct ElectricChargeVolumeDensity
{
    // Formula electromagnetism/volume-charge-density
    // Equation Q = ρqV
    // Applies Uniform or mean volume charge density ρq over the region.
    // Source [MIT linear electromagnetic media](https://ocw.mit.edu/courses/18-325-topics-in-applied-mathematics-waves-and-imaging-fall-2015/c5db24b1a6d0d3301b1a3f21f6ceba3a_MIT18_325F15_CompleteLect.pdf)
    public static ElectricCharge operator *(ElectricChargeVolumeDensity left, Volume right)
        => (ElectricCharge)(left.Value * right.Value);

    public static ElectricCharge operator *(Volume left, ElectricChargeVolumeDensity right)
        => (ElectricCharge)(left.Value * right.Value);
}

public readonly partial struct ElectricChargePerMass
{
    // Formula electromagnetism/charge-per-mass
    // Equation Q = (Q / m)m
    // Applies Charge and mass refer to the same particles or sample.
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static ElectricCharge operator *(ElectricChargePerMass left, Mass right)
        => (ElectricCharge)(left.Value * right.Value);

    public static ElectricCharge operator *(Mass left, ElectricChargePerMass right)
        => (ElectricCharge)(left.Value * right.Value);
}

public readonly partial struct ElectricChargePerAmountOfSubstance
{
    // Formula electromagnetism/charge-per-mole
    // Equation Q = (Q / n)n
    // Applies Use charge per mole for the specified entities and process.
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    // Source [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971)
    public static ElectricCharge operator *(ElectricChargePerAmountOfSubstance left, AmountOfSubstance right)
        => (ElectricCharge)(left.Value * right.Value);

    public static ElectricCharge operator *(AmountOfSubstance left, ElectricChargePerAmountOfSubstance right)
        => (ElectricCharge)(left.Value * right.Value);
}

public readonly partial struct ElectricDipoleMoment
{
    public static Length operator /(ElectricDipoleMoment left, ElectricCharge right)
        => (Length)(left.Value / right.Value);

    public static ElectricCharge operator /(ElectricDipoleMoment left, Length right)
        => (ElectricCharge)(left.Value / right.Value);
}

public readonly partial struct Permittivity
{
    // Formula electromagnetism/electric-displacement
    // Equation D = εE
    // Applies Linear isotropic dielectric, using corresponding field components.
    // Source [MIT linear electromagnetic media](https://ocw.mit.edu/courses/18-325-topics-in-applied-mathematics-waves-and-imaging-fall-2015/c5db24b1a6d0d3301b1a3f21f6ceba3a_MIT18_325F15_CompleteLect.pdf)
    public static ElectricDisplacement operator *(Permittivity left, ElectricFieldStrength right)
        => (ElectricDisplacement)(left.Value * right.Value);

    public static ElectricDisplacement operator *(ElectricFieldStrength left, Permittivity right)
        => (ElectricDisplacement)(left.Value * right.Value);
}

public readonly partial struct ElectricDisplacement
{
    public static ElectricFieldStrength operator /(ElectricDisplacement left, Permittivity right)
        => (ElectricFieldStrength)(left.Value / right.Value);

    public static Permittivity operator /(ElectricDisplacement left, ElectricFieldStrength right)
        => (Permittivity)(left.Value / right.Value);
}

public readonly partial struct MagneticFluxDensity
{
    // Use the area-averaged field component normal to the surface.
    // Formula electromagnetism/magnetic-flux
    // Equation Φ = BₙA
    // Applies Use the uniform or area-averaged normal flux density.
    // Source [OpenStax Faraday's law](https://openstax.org/books/university-physics-volume-2/pages/13-key-equations)
    public static MagneticFlux operator *(MagneticFluxDensity left, Area right)
        => (MagneticFlux)(left.Value * right.Value);

    public static MagneticFlux operator *(Area left, MagneticFluxDensity right)
        => (MagneticFlux)(left.Value * right.Value);

    public static AuxillaryMagneticField operator /(MagneticFluxDensity left, Permeability right)
        => (AuxillaryMagneticField)(left.Value / right.Value);

    public static Permeability operator /(MagneticFluxDensity left, AuxillaryMagneticField right)
        => (Permeability)(left.Value / right.Value);
}

public readonly partial struct MagneticFlux
{
    public static Area operator /(MagneticFlux left, MagneticFluxDensity right)
        => (Area)(left.Value / right.Value);

    public static MagneticFluxDensity operator /(MagneticFlux left, Area right)
        => (MagneticFluxDensity)(left.Value / right.Value);

    public static Length operator /(MagneticFlux left, MagneticFluxPerLength right)
        => (Length)(left.Value / right.Value);

    public static MagneticFluxPerLength operator /(MagneticFlux left, Length right)
        => (MagneticFluxPerLength)(left.Value / right.Value);
}

public readonly partial struct MagneticFluxPerLength
{
    // Formula electromagnetism/magnetic-flux-per-length
    // Equation Φ = (Φ / ℓ)ℓ
    // Applies Uniform or mean magnetic flux per length over the interval.
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static MagneticFlux operator *(MagneticFluxPerLength left, Length right)
        => (MagneticFlux)(left.Value * right.Value);

    public static MagneticFlux operator *(Length left, MagneticFluxPerLength right)
        => (MagneticFlux)(left.Value * right.Value);
}

public readonly partial struct Permeability
{
    // Formula electromagnetism/magnetic-field-in-material
    // Equation B = μH
    // Applies Linear isotropic magnetic medium. The QUDT `AuxillaryMagneticField` type represents H.
    // Source [MIT linear electromagnetic media](https://ocw.mit.edu/courses/18-325-topics-in-applied-mathematics-waves-and-imaging-fall-2015/c5db24b1a6d0d3301b1a3f21f6ceba3a_MIT18_325F15_CompleteLect.pdf)
    public static MagneticFluxDensity operator *(Permeability left, AuxillaryMagneticField right)
        => (MagneticFluxDensity)(left.Value * right.Value);

    public static MagneticFluxDensity operator *(AuxillaryMagneticField left, Permeability right)
        => (MagneticFluxDensity)(left.Value * right.Value);
}

// Heat and temperature.

public readonly partial struct HeatCapacity
{
    // Use a constant heat capacity or its average over the temperature interval.
    // Formula thermal/sensible-heat
    // Equation Q = CΔT
    // Applies Sensible heat with constant or interval-averaged heat capacity.
    // Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
    public static Energy operator *(HeatCapacity left, TemperatureDifference right)
        => (Energy)(left.Value * right.Value);

    public static Energy operator *(TemperatureDifference left, HeatCapacity right)
        => (Energy)(left.Value * right.Value);

    public static Mass operator /(HeatCapacity left, SpecificHeatCapacity right)
        => (Mass)(left.Value / right.Value);

    public static SpecificHeatCapacity operator /(HeatCapacity left, Mass right)
        => (SpecificHeatCapacity)(left.Value / right.Value);

    public static Volume operator /(HeatCapacity left, VolumetricHeatCapacity right)
        => (Volume)(left.Value / right.Value);

    public static VolumetricHeatCapacity operator /(HeatCapacity left, Volume right)
        => (VolumetricHeatCapacity)(left.Value / right.Value);
}

public readonly partial struct Energy
{
    public static TemperatureDifference operator /(Energy left, HeatCapacity right)
        => (TemperatureDifference)(left.Value / right.Value);

    public static HeatCapacity operator /(Energy left, TemperatureDifference right)
        => (HeatCapacity)(left.Value / right.Value);

    public static Time operator /(Energy left, HeatFlowRate right)
        => (Time)(left.Value / right.Value);
}

public readonly partial struct SpecificHeatCapacity
{
    // Formula thermal/specific-heat-capacity
    // Equation C = mc
    // Applies Use mass-specific heat capacity for the specified thermodynamic process.
    // Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
    public static HeatCapacity operator *(SpecificHeatCapacity left, Mass right)
        => (HeatCapacity)(left.Value * right.Value);

    public static HeatCapacity operator *(Mass left, SpecificHeatCapacity right)
        => (HeatCapacity)(left.Value * right.Value);

    // Formula thermal/volumetric-capacity-from-density
    // Equation cᵥ = ρc
    // Applies Density and mass-specific heat capacity describe the same state.
    // Source [NIST TN 622](https://nvlpubs.nist.gov/nistpubs/Legacy/TN/nbstechnicalnote622.pdf)
    public static VolumetricHeatCapacity operator *(SpecificHeatCapacity left, Density right)
        => (VolumetricHeatCapacity)(left.Value * right.Value);

    public static VolumetricHeatCapacity operator *(Density left, SpecificHeatCapacity right)
        => (VolumetricHeatCapacity)(left.Value * right.Value);
}

public readonly partial struct VolumetricHeatCapacity
{
    // Formula thermal/volumetric-heat-capacity
    // Equation C = Vcᵥ
    // Applies Uniform or volume-averaged volumetric heat capacity cᵥ.
    // Source [NIST SP 811](https://www.nist.gov/pml/special-publication-811/nist-guide-si-chapter-4-two-classes-si-units-and-si-prefixes)
    public static HeatCapacity operator *(VolumetricHeatCapacity left, Volume right)
        => (HeatCapacity)(left.Value * right.Value);

    public static HeatCapacity operator *(Volume left, VolumetricHeatCapacity right)
        => (HeatCapacity)(left.Value * right.Value);

    public static Density operator /(VolumetricHeatCapacity left, SpecificHeatCapacity right)
        => (Density)(left.Value / right.Value);

    public static SpecificHeatCapacity operator /(VolumetricHeatCapacity left, Density right)
        => (SpecificHeatCapacity)(left.Value / right.Value);
}

public readonly partial struct ThermalDiffusivity
{
    // Formula thermal/thermal-diffusivity
    // Equation α = k / cᵥ
    // Applies Thermal diffusivity for scalar conductivity and volumetric heat capacity.
    // Source [NIST TN 622](https://nvlpubs.nist.gov/nistpubs/Legacy/TN/nbstechnicalnote622.pdf)
    public static ThermalConductivity operator *(ThermalDiffusivity left, VolumetricHeatCapacity right)
        => (ThermalConductivity)(left.Value * right.Value);

    public static ThermalConductivity operator *(VolumetricHeatCapacity left, ThermalDiffusivity right)
        => (ThermalConductivity)(left.Value * right.Value);
}

public readonly partial struct ThermalConductivity
{
    public static VolumetricHeatCapacity operator /(ThermalConductivity left, ThermalDiffusivity right)
        => (VolumetricHeatCapacity)(left.Value / right.Value);

    public static ThermalDiffusivity operator /(ThermalConductivity left, VolumetricHeatCapacity right)
        => (ThermalDiffusivity)(left.Value / right.Value);
}

public readonly partial struct HeatFlowRate
{
    // Formula mechanics/energy-from-power
    public static Energy operator *(HeatFlowRate left, Time right)
        => (Energy)(left.Value * right.Value);

    public static Energy operator *(Time left, HeatFlowRate right)
        => (Energy)(left.Value * right.Value);

    public static Area operator /(HeatFlowRate left, HeatFlowRatePerArea right)
        => (Area)(left.Value / right.Value);

    public static HeatFlowRatePerArea operator /(HeatFlowRate left, Area right)
        => (HeatFlowRatePerArea)(left.Value / right.Value);
}

public readonly partial struct HeatFlowRatePerArea
{
    // Formula thermal/heat-flow-from-flux
    // Equation Pheat = q″A
    // Applies Use average normal heat flux over the surface.
    // Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
    // Source [QUDT heat-transfer coefficient](https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer)
    public static HeatFlowRate operator *(HeatFlowRatePerArea left, Area right)
        => (HeatFlowRate)(left.Value * right.Value);

    public static HeatFlowRate operator *(Area left, HeatFlowRatePerArea right)
        => (HeatFlowRate)(left.Value * right.Value);

    public static TemperatureDifference operator /(HeatFlowRatePerArea left, CoefficientOfHeatTransfer right)
        => (TemperatureDifference)(left.Value / right.Value);

    public static CoefficientOfHeatTransfer operator /(HeatFlowRatePerArea left, TemperatureDifference right)
        => (CoefficientOfHeatTransfer)(left.Value / right.Value);
}

public readonly partial struct CoefficientOfHeatTransfer
{
    // Formula thermal/surface-heat-transfer
    // Equation q″ = hΔT
    // Applies Transfer coefficient h is per unit area. Use the driving temperature difference in the indicated direction.
    // Source [QUDT heat-transfer coefficient](https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer)
    public static HeatFlowRatePerArea operator *(CoefficientOfHeatTransfer left, TemperatureDifference right)
        => (HeatFlowRatePerArea)(left.Value * right.Value);

    public static HeatFlowRatePerArea operator *(TemperatureDifference left, CoefficientOfHeatTransfer right)
        => (HeatFlowRatePerArea)(left.Value * right.Value);
}

public readonly partial struct ThermalResistance
{
    // Formula thermal/thermal-path
    // Equation Pheat = ΔT / Rₜ = GₜΔT, Gₜ = 1 / Rₜ
    // Applies Use total resistance or conductance for the same thermal path. A signed temperature drop gives the signed heat-flow rate.
    // Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
    // Source [QUDT thermal conductance](https://qudt.org/vocab/quantitykind/ThermalConductance)
    public static TemperatureDifference operator *(ThermalResistance left, HeatFlowRate right)
        => (TemperatureDifference)(left.Value * right.Value);

    public static TemperatureDifference operator *(HeatFlowRate left, ThermalResistance right)
        => (TemperatureDifference)(left.Value * right.Value);

    // Formula thermal/areal-thermal-resistance
    // Equation R″ = ARₜ
    // Applies Relate total thermal resistance to area-normalized resistance for the same uniform surface.
    // Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
    // Source [QUDT heat-transfer coefficient](https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer)
    public static ThermalInsulance operator *(ThermalResistance left, Area right)
        => (ThermalInsulance)(left.Value * right.Value);

    public static ThermalInsulance operator *(Area left, ThermalResistance right)
        => (ThermalInsulance)(left.Value * right.Value);
}

public readonly partial struct TemperatureDifference
{
    public static HeatFlowRate operator /(TemperatureDifference left, ThermalResistance right)
        => (HeatFlowRate)(left.Value / right.Value);

    public static ThermalResistance operator /(TemperatureDifference left, HeatFlowRate right)
        => (ThermalResistance)(left.Value / right.Value);

    public static HeatFlowRatePerArea operator /(TemperatureDifference left, ThermalInsulance right)
        => (HeatFlowRatePerArea)(left.Value / right.Value);

    public static ThermalInsulance operator /(TemperatureDifference left, HeatFlowRatePerArea right)
        => (ThermalInsulance)(left.Value / right.Value);

    public static Time operator /(TemperatureDifference left, TemperaturePerTime right)
        => (Time)(left.Value / right.Value);

    public static TemperaturePerTime operator /(TemperatureDifference left, Time right)
        => (TemperaturePerTime)(left.Value / right.Value);
}

public readonly partial struct ThermalInsulance
{
    public static Area operator /(ThermalInsulance left, ThermalResistance right)
        => (Area)(left.Value / right.Value);

    public static ThermalResistance operator /(ThermalInsulance left, Area right)
        => (ThermalResistance)(left.Value / right.Value);

    // Formula thermal/temperature-drop-from-heat-flux
    // Equation ΔT = q″R″
    // Applies Use areal thermal resistance and the corresponding heat-flux component.
    // Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
    // Source [QUDT heat-transfer coefficient](https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer)
    public static TemperatureDifference operator *(ThermalInsulance left, HeatFlowRatePerArea right)
        => (TemperatureDifference)(left.Value * right.Value);

    public static TemperatureDifference operator *(HeatFlowRatePerArea left, ThermalInsulance right)
        => (TemperatureDifference)(left.Value * right.Value);
}

public readonly partial struct TemperaturePerTime
{
    // Formula thermal/temperature-change-rate
    // Equation ΔT = (dT/dt)̄Δt
    // Applies Temperature-change rate is constant or averaged over the interval.
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static TemperatureDifference operator *(TemperaturePerTime left, Time right)
        => (TemperatureDifference)(left.Value * right.Value);

    public static TemperatureDifference operator *(Time left, TemperaturePerTime right)
        => (TemperatureDifference)(left.Value * right.Value);
}

public readonly partial struct LinearThermalExpansion
{
    // Formula thermal/thermal-expansion
    // Equation ΔL = L′ΔT, ΔA = A′ΔT, ΔV = V′ΔT
    // Applies Use each whole object's dimensional expansion per kelvin, incorporating its initial size. The return is a change in size.
    // Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static Length operator *(LinearThermalExpansion left, TemperatureDifference right)
        => (Length)(left.Value * right.Value);

    public static Length operator *(TemperatureDifference left, LinearThermalExpansion right)
        => (Length)(left.Value * right.Value);
}

public readonly partial struct Length
{
    public static TemperatureDifference operator /(Length left, LinearThermalExpansion right)
        => (TemperatureDifference)(left.Value / right.Value);
}

public readonly partial struct AreaThermalExpansion
{
    // Formula thermal/thermal-expansion
    public static Area operator *(AreaThermalExpansion left, TemperatureDifference right)
        => (Area)(left.Value * right.Value);

    public static Area operator *(TemperatureDifference left, AreaThermalExpansion right)
        => (Area)(left.Value * right.Value);
}

public readonly partial struct Area
{
    public static TemperatureDifference operator /(Area left, AreaThermalExpansion right)
        => (TemperatureDifference)(left.Value / right.Value);
}

public readonly partial struct VolumeThermalExpansion
{
    // Formula thermal/thermal-expansion
    public static Volume operator *(VolumeThermalExpansion left, TemperatureDifference right)
        => (Volume)(left.Value * right.Value);

    public static Volume operator *(TemperatureDifference left, VolumeThermalExpansion right)
        => (Volume)(left.Value * right.Value);
}

public readonly partial struct Volume
{
    public static TemperatureDifference operator /(Volume left, VolumeThermalExpansion right)
        => (TemperatureDifference)(left.Value / right.Value);
}

// Photometry and radiometry.

public readonly partial struct LuminousIntensity
{
    // Use the average intensity over the solid angle. For luminance, use the projected emitting area.
    // Formula optics/luminous-flux
    // Equation Φᵥ = IᵥΩ
    // Applies Use mean luminous intensity over the solid angle.
    // Source [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf)
    // Source [NIST photometric equations 2–6](https://www.nist.gov/system/files/documents/calibrations/sp250-15.pdf)
    public static LuminousFlux operator *(LuminousIntensity left, SolidAngle right)
        => (LuminousFlux)(left.Value * right.Value);

    public static LuminousFlux operator *(SolidAngle left, LuminousIntensity right)
        => (LuminousFlux)(left.Value * right.Value);

    public static Area operator /(LuminousIntensity left, Luminance right)
        => (Area)(left.Value / right.Value);

    public static Luminance operator /(LuminousIntensity left, Area right)
        => (Luminance)(left.Value / right.Value);
}

public readonly partial struct LuminousFlux
{
    public static SolidAngle operator /(LuminousFlux left, LuminousIntensity right)
        => (SolidAngle)(left.Value / right.Value);

    public static LuminousIntensity operator /(LuminousFlux left, SolidAngle right)
        => (LuminousIntensity)(left.Value / right.Value);

    public static Area operator /(LuminousFlux left, Illuminance right)
        => (Area)(left.Value / right.Value);

    // Illuminance is incident luminous flux per illuminated area.
    public static Illuminance operator /(LuminousFlux left, Area right)
        => (Illuminance)(left.Value / right.Value);

    // Formula optics/luminous-energy
    // Equation Qᵥ = Φ̄ᵥΔt
    // Applies Constant or time-averaged luminous flux over the interval.
    // Source [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf)
    public static LuminousEnergy operator *(LuminousFlux left, Time right)
        => (LuminousEnergy)(left.Value * right.Value);

    public static LuminousEnergy operator *(Time left, LuminousFlux right)
        => (LuminousEnergy)(left.Value * right.Value);

    public static Power operator /(LuminousFlux left, LuminousEfficacy right)
        => (Power)(left.Value / right.Value);

    public static LuminousEfficacy operator /(LuminousFlux left, Power right)
        => (LuminousEfficacy)(left.Value / right.Value);
}

public readonly partial struct Illuminance
{
    // Formula optics/illuminance
    // Equation Eᵥ = Φᵥ,incident / A
    // Applies Incident luminous flux averaged over the receiver area.
    // Source [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf)
    // Source [NIST photometric equations 2–6](https://www.nist.gov/system/files/documents/calibrations/sp250-15.pdf)
    public static LuminousFlux operator *(Illuminance left, Area right)
        => (LuminousFlux)(left.Value * right.Value);

    public static LuminousFlux operator *(Area left, Illuminance right)
        => (LuminousFlux)(left.Value * right.Value);
}

public readonly partial struct Luminance
{
    // Formula optics/luminance
    // Equation Iᵥ = LᵥAproj
    // Applies Use projected emitting area as viewed from the specified direction.
    // Source [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf)
    public static LuminousIntensity operator *(Luminance left, Area right)
        => (LuminousIntensity)(left.Value * right.Value);

    public static LuminousIntensity operator *(Area left, Luminance right)
        => (LuminousIntensity)(left.Value * right.Value);
}

public readonly partial struct LuminousEnergy
{
    public static Time operator /(LuminousEnergy left, LuminousFlux right)
        => (Time)(left.Value / right.Value);

    public static LuminousFlux operator /(LuminousEnergy left, Time right)
        => (LuminousFlux)(left.Value / right.Value);
}

public readonly partial struct LuminousEfficacy
{
    // Formula optics/luminous-efficacy
    // Equation Φᵥ = ηᵥP
    // Applies Use the power basis for which the luminous efficacy was specified.
    // Source [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf)
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static LuminousFlux operator *(LuminousEfficacy left, Power right)
        => (LuminousFlux)(left.Value * right.Value);

    public static LuminousFlux operator *(Power left, LuminousEfficacy right)
        => (LuminousFlux)(left.Value * right.Value);
}

public readonly partial struct RadiantIntensity
{
    // Formula optics/radiant-flux
    // Equation Φₑ = IₑΩ
    // Applies Use mean radiant intensity over the solid angle.
    // Source [NIST photometric quantities](https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication250-37.pdf)
    public static Power operator *(RadiantIntensity left, SolidAngle right)
        => (Power)(left.Value * right.Value);

    public static Power operator *(SolidAngle left, RadiantIntensity right)
        => (Power)(left.Value * right.Value);
}

public readonly partial struct Power
{
    public static SolidAngle operator /(Power left, RadiantIntensity right)
        => (SolidAngle)(left.Value / right.Value);

    public static RadiantIntensity operator /(Power left, SolidAngle right)
        => (RadiantIntensity)(left.Value / right.Value);
}

// Amount of substance and radiation dose.

public readonly partial struct MolarMass
{
    // Formula chemistry/molar-mass
    // Equation M = m / n
    // Applies Mass and amount describe the same substance or specified entities.
    // Source [IUPAC molar mass](https://goldbook.iupac.org/terms/view/12214)
    public static Mass operator *(MolarMass left, AmountOfSubstance right)
        => (Mass)(left.Value * right.Value);

    public static Mass operator *(AmountOfSubstance left, MolarMass right)
        => (Mass)(left.Value * right.Value);

    public static MolarVolume operator /(MolarMass left, Density right)
        => (MolarVolume)(left.Value / right.Value);

    public static Density operator /(MolarMass left, MolarVolume right)
        => (Density)(left.Value / right.Value);
}

public readonly partial struct Mass
{
    public static AmountOfSubstance operator /(Mass left, MolarMass right)
        => (AmountOfSubstance)(left.Value / right.Value);

    public static MolarMass operator /(Mass left, AmountOfSubstance right)
        => (MolarMass)(left.Value / right.Value);
}

public readonly partial struct MolarVolume
{
    // Formula chemistry/molar-volume
    // Equation Vₘ = V / n
    // Applies Volume and amount refer to the same sample at the same state.
    // Source [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971)
    public static Volume operator *(MolarVolume left, AmountOfSubstance right)
        => (Volume)(left.Value * right.Value);

    public static Volume operator *(AmountOfSubstance left, MolarVolume right)
        => (Volume)(left.Value * right.Value);

    // Formula chemistry/molar-mass-from-density
    // Equation M = ρVₘ
    // Applies Density and molar volume describe the same material at the same state.
    // Source [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971)
    // Source [QUDT molar volume](https://qudt.org/vocab/quantitykind/MolarVolume)
    public static MolarMass operator *(MolarVolume left, Density right)
        => (MolarMass)(left.Value * right.Value);

    public static MolarMass operator *(Density left, MolarVolume right)
        => (MolarMass)(left.Value * right.Value);
}

public readonly partial struct Volume
{
    public static AmountOfSubstance operator /(Volume left, MolarVolume right)
        => (AmountOfSubstance)(left.Value / right.Value);

    public static MolarVolume operator /(Volume left, AmountOfSubstance right)
        => (MolarVolume)(left.Value / right.Value);
}

public readonly partial struct MolarEnergy
{
    // Formula chemistry/molar-energy
    // Equation Eₘ = E / n
    // Applies Use energy for the same specified amount of substance.
    // Source [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971)
    public static Energy operator *(MolarEnergy left, AmountOfSubstance right)
        => (Energy)(left.Value * right.Value);

    public static Energy operator *(AmountOfSubstance left, MolarEnergy right)
        => (Energy)(left.Value * right.Value);
}

public readonly partial struct Energy
{
    public static AmountOfSubstance operator /(Energy left, MolarEnergy right)
        => (AmountOfSubstance)(left.Value / right.Value);

    public static MolarEnergy operator /(Energy left, AmountOfSubstance right)
        => (MolarEnergy)(left.Value / right.Value);

    public static Mass operator /(Energy left, AbsorbedDose right)
        => (Mass)(left.Value / right.Value);
}

public readonly partial struct MolarHeatCapacity
{
    // Formula chemistry/molar-heat-capacity
    // Equation Cₘ = C / n
    // Applies Use heat capacity for the same amount and thermodynamic process.
    // Source [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971)
    // Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
    public static HeatCapacity operator *(MolarHeatCapacity left, AmountOfSubstance right)
        => (HeatCapacity)(left.Value * right.Value);

    public static HeatCapacity operator *(AmountOfSubstance left, MolarHeatCapacity right)
        => (HeatCapacity)(left.Value * right.Value);
}

public readonly partial struct HeatCapacity
{
    public static AmountOfSubstance operator /(HeatCapacity left, MolarHeatCapacity right)
        => (AmountOfSubstance)(left.Value / right.Value);

    public static MolarHeatCapacity operator /(HeatCapacity left, AmountOfSubstance right)
        => (MolarHeatCapacity)(left.Value / right.Value);
}

public readonly partial struct AmountOfSubstancePerVolume
{
    // Formula chemistry/amount-concentration
    // Equation c = n / V
    // Applies Amount of the constituent divided by the volume of the mixture.
    // Source [IUPAC amount concentration](https://goldbook.iupac.org/terms/view/A00295)
    public static AmountOfSubstance operator *(AmountOfSubstancePerVolume left, Volume right)
        => (AmountOfSubstance)(left.Value * right.Value);

    public static AmountOfSubstance operator *(Volume left, AmountOfSubstancePerVolume right)
        => (AmountOfSubstance)(left.Value * right.Value);
}

public readonly partial struct AmountOfSubstance
{
    public static Volume operator /(AmountOfSubstance left, AmountOfSubstancePerVolume right)
        => (Volume)(left.Value / right.Value);

    public static AmountOfSubstancePerVolume operator /(AmountOfSubstance left, Volume right)
        => (AmountOfSubstancePerVolume)(left.Value / right.Value);

    public static Mass operator /(AmountOfSubstance left, AmountOfSubstancePerMass right)
        => (Mass)(left.Value / right.Value);

    public static AmountOfSubstancePerMass operator /(AmountOfSubstance left, Mass right)
        => (AmountOfSubstancePerMass)(left.Value / right.Value);

    public static Time operator /(AmountOfSubstance left, CatalyticActivity right)
        => (Time)(left.Value / right.Value);
}

public readonly partial struct AmountOfSubstancePerMass
{
    // Formula chemistry/amount-per-mass
    // Equation b = n / m
    // Applies Use the reference mass for the specified amount-per-mass quantity, such as solvent mass when calculating molality.
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    // Source [IUPAC molar quantities](https://goldbook.iupac.org/terms/view/M03971)
    public static AmountOfSubstance operator *(AmountOfSubstancePerMass left, Mass right)
        => (AmountOfSubstance)(left.Value * right.Value);

    public static AmountOfSubstance operator *(Mass left, AmountOfSubstancePerMass right)
        => (AmountOfSubstance)(left.Value * right.Value);
}

public readonly partial struct AbsorbedDoseRate
{
    // Formula chemistry/absorbed-dose-rate
    // Equation ΔD = Ḋ̄Δt
    // Applies Constant or interval-averaged absorbed dose rate for the same matter.
    // Source [IAEA dosimetry](https://www-pub.iaea.org/MTCD/Publications/PDF/te_1540_web.pdf)
    public static AbsorbedDose operator *(AbsorbedDoseRate left, Time right)
        => (AbsorbedDose)(left.Value * right.Value);

    public static AbsorbedDose operator *(Time left, AbsorbedDoseRate right)
        => (AbsorbedDose)(left.Value * right.Value);
}

public readonly partial struct AbsorbedDose
{
    public static Time operator /(AbsorbedDose left, AbsorbedDoseRate right)
        => (Time)(left.Value / right.Value);

    public static AbsorbedDoseRate operator /(AbsorbedDose left, Time right)
        => (AbsorbedDoseRate)(left.Value / right.Value);

    // The dose is the mean over the same mass to which the deposited energy refers.
    // Formula chemistry/absorbed-dose
    // Equation D = Edeposited / m
    // Applies Mean energy deposited by ionizing radiation in the specified matter.
    // Source [IAEA dosimetry](https://www-pub.iaea.org/MTCD/Publications/PDF/te_1540_web.pdf)
    public static Energy operator *(AbsorbedDose left, Mass right)
        => (Energy)(left.Value * right.Value);

    public static Energy operator *(Mass left, AbsorbedDose right)
        => (Energy)(left.Value * right.Value);
}

public readonly partial struct CatalyticActivity
{
    // Formula chemistry/catalytic-activity
    // Equation Δn = ācΔt
    // Applies Use the constant or time-averaged catalytic activity under the specified assay conditions.
    // Source [BIPM SI Brochure](https://www.bipm.org/en/publications/si-brochure)
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static AmountOfSubstance operator *(CatalyticActivity left, Time right)
        => (AmountOfSubstance)(left.Value * right.Value);

    public static AmountOfSubstance operator *(Time left, CatalyticActivity right)
        => (AmountOfSubstance)(left.Value * right.Value);
}

// Pressure, power, and reciprocal density.

public readonly partial struct Force
{
    // Supply the force component along the direction represented by the velocity.
    // Formula mechanics/mechanical-power
    // Equation P = F∥v
    // Applies Use the signed force component along the velocity axis.
    // Source [OpenStax work and energy](https://openstax.org/books/university-physics-volume-1/pages/7-key-equations)
    public static Power operator *(Force left, LinearVelocity right)
        => (Power)(left.Value * right.Value);

    public static Power operator *(LinearVelocity left, Force right)
        => (Power)(left.Value * right.Value);

    public static Area operator /(Force left, Pressure right)
        => (Area)(left.Value / right.Value);
}

public readonly partial struct Power
{
    public static LinearVelocity operator /(Power left, Force right)
        => (LinearVelocity)(left.Value / right.Value);

    public static Force operator /(Power left, LinearVelocity right)
        => (Force)(left.Value / right.Value);

    public static VolumePerTime operator /(Power left, Pressure right)
        => (VolumePerTime)(left.Value / right.Value);
}

public readonly partial struct Pressure
{
    // Formula fluids/pressure-from-force
    public static Force operator *(Pressure left, Area right)
        => (Force)(left.Value * right.Value);

    public static Force operator *(Area left, Pressure right)
        => (Force)(left.Value * right.Value);

    // For hydraulic power, the pressure operand is the signed difference across the flow path.
    // Formula fluids/hydraulic-power
    public static Power operator *(Pressure left, VolumePerTime right)
        => (Power)(left.Value * right.Value);

    public static Power operator *(VolumePerTime left, Pressure right)
        => (Power)(left.Value * right.Value);
}

public readonly partial struct Density
{
    public static SpecificVolume operator /(double left, Density right)
        => (SpecificVolume)(left / right.Value);

    // Formula fluids/density-and-specific-volume
    public static DimensionlessRatio operator *(Density left, SpecificVolume right)
        => (DimensionlessRatio)(left.Value * right.Value);

    public static DimensionlessRatio operator *(SpecificVolume left, Density right)
        => (DimensionlessRatio)(left.Value * right.Value);
}

public readonly partial struct SpecificVolume
{
    public static Density operator /(double left, SpecificVolume right)
        => (Density)(left / right.Value);
}

// Conductance, resistivity, and the mass-to-charge ratio.

public readonly partial struct Conductance
{
    // Formula electromagnetism/ohms-law
    public static ElectricCurrent operator *(Conductance left, ElectricPotential right)
        => (ElectricCurrent)(left.Value * right.Value);

    public static ElectricCurrent operator *(ElectricPotential left, Conductance right)
        => (ElectricCurrent)(left.Value * right.Value);

    public static Resistance operator /(double left, Conductance right)
        => (Resistance)(left / right.Value);
}

public readonly partial struct ElectricCurrent
{
    public static ElectricPotential operator /(ElectricCurrent left, Conductance right)
        => (ElectricPotential)(left.Value / right.Value);

    public static Conductance operator /(ElectricCurrent left, ElectricPotential right)
        => (Conductance)(left.Value / right.Value);
}

public readonly partial struct Resistance
{
    public static Conductance operator /(double left, Resistance right)
        => (Conductance)(left / right.Value);

    // Formula electromagnetism/ohms-law
    public static DimensionlessRatio operator *(Resistance left, Conductance right)
        => (DimensionlessRatio)(left.Value * right.Value);

    public static DimensionlessRatio operator *(Conductance left, Resistance right)
        => (DimensionlessRatio)(left.Value * right.Value);
}

public readonly partial struct ElectricConductivity
{
    public static Resistivity operator /(double left, ElectricConductivity right)
        => (Resistivity)(left / right.Value);

    // Formula electromagnetism/conductivity-and-resistivity
    // Equation ρₑ = 1 / σ
    // Applies Resistivity and conductivity refer to the same ohmic material and state.
    // Source [OpenStax resistivity](https://openstax.org/books/university-physics-volume-2/pages/9-3-resistivity-and-resistance)
    public static DimensionlessRatio operator *(ElectricConductivity left, Resistivity right)
        => (DimensionlessRatio)(left.Value * right.Value);

    public static DimensionlessRatio operator *(Resistivity left, ElectricConductivity right)
        => (DimensionlessRatio)(left.Value * right.Value);
}

public readonly partial struct Resistivity
{
    public static ElectricConductivity operator /(double left, Resistivity right)
        => (ElectricConductivity)(left / right.Value);
}

public readonly partial struct MassPerElectricCharge
{
    // Formula electromagnetism/mass-per-charge
    // Equation m = (m / Q)Q
    // Applies Mass and signed charge refer to the same charged entities.
    // Source [QUDT quantity definitions](https://github.com/qudt/qudt-public-repo/releases/tag/v3.5.4)
    public static Mass operator *(MassPerElectricCharge left, ElectricCharge right)
        => (Mass)(left.Value * right.Value);

    public static Mass operator *(ElectricCharge left, MassPerElectricCharge right)
        => (Mass)(left.Value * right.Value);
}

public readonly partial struct Mass
{
    public static ElectricCharge operator /(Mass left, MassPerElectricCharge right)
        => (ElectricCharge)(left.Value / right.Value);

    public static MassPerElectricCharge operator /(Mass left, ElectricCharge right)
        => (MassPerElectricCharge)(left.Value / right.Value);
}

// Temperature gradients and thermal material properties.

public readonly partial struct TemperatureGradient
{
    // Formula thermal/temperature-gradient
    // Equation ∂T/∂x ≈ ΔT / Δx
    // Applies Signed temperature difference over a distance along the axis. The quotient gives the interval average.
    // Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
    public static TemperatureDifference operator *(TemperatureGradient left, Length right)
        => (TemperatureDifference)(left.Value * right.Value);

    public static TemperatureDifference operator *(Length left, TemperatureGradient right)
        => (TemperatureDifference)(left.Value * right.Value);
}

public readonly partial struct TemperatureDifference
{
    public static Length operator /(TemperatureDifference left, TemperatureGradient right)
        => (Length)(left.Value / right.Value);

    public static TemperatureGradient operator /(TemperatureDifference left, Length right)
        => (TemperatureGradient)(left.Value / right.Value);
}

public readonly partial struct ThermalConductivity
{
    public static ThermalResistivity operator /(double left, ThermalConductivity right)
        => (ThermalResistivity)(left / right.Value);

    // Formula thermal/thermal-resistivity
    // Equation ρₜ = 1 / k
    // Applies Thermal resistivity and conductivity describe the same material and direction.
    // Source [QUDT thermal resistivity](https://qudt.org/vocab/quantitykind/ThermalResistivity)
    public static DimensionlessRatio operator *(ThermalConductivity left, ThermalResistivity right)
        => (DimensionlessRatio)(left.Value * right.Value);

    public static DimensionlessRatio operator *(ThermalResistivity left, ThermalConductivity right)
        => (DimensionlessRatio)(left.Value * right.Value);
}

public readonly partial struct ThermalResistivity
{
    public static ThermalConductivity operator /(double left, ThermalResistivity right)
        => (ThermalConductivity)(left / right.Value);

    // A uniform plane layer's depth times thermal resistivity gives its areal resistance.
    // Formula thermal/plane-layer-insulance
    // Equation R″ = ℓ/k = ℓρₜ
    // Applies Uniform plane layer. Use thickness along the heat-flow direction.
    // Source [OpenStax heat and conduction](https://openstax.org/books/university-physics-volume-2/pages/1-key-equations)
    // Source [QUDT thermal resistivity](https://qudt.org/vocab/quantitykind/ThermalResistivity)
    public static ThermalInsulance operator *(ThermalResistivity left, Length right)
        => (ThermalInsulance)(left.Value * right.Value);

    public static ThermalInsulance operator *(Length left, ThermalResistivity right)
        => (ThermalInsulance)(left.Value * right.Value);
}

public readonly partial struct ThermalInsulance
{
    // Formula thermal/plane-layer-insulance
    public static Length operator *(ThermalInsulance left, ThermalConductivity right)
        => (Length)(left.Value * right.Value);

    public static Length operator *(ThermalConductivity left, ThermalInsulance right)
        => (Length)(left.Value * right.Value);

    public static Length operator /(ThermalInsulance left, ThermalResistivity right)
        => (Length)(left.Value / right.Value);

    public static ThermalResistivity operator /(ThermalInsulance left, Length right)
        => (ThermalResistivity)(left.Value / right.Value);
}

public readonly partial struct Length
{
    public static ThermalConductivity operator /(Length left, ThermalInsulance right)
        => (ThermalConductivity)(left.Value / right.Value);

    public static ThermalInsulance operator /(Length left, ThermalConductivity right)
        => (ThermalInsulance)(left.Value / right.Value);
}

public readonly partial struct Wavelength
{
    // Wave propagation uses wavelength and frequency for the same wave.
    // Formula mechanics/wave-speed
    // Equation v = λf
    // Applies Wavelength and frequency belong to the same propagating wave.
    // Source [OpenStax waves](https://openstax.org/books/university-physics-volume-1/pages/16-1-traveling-waves)
    public static LinearVelocity operator *(Wavelength left, Frequency right)
        => (LinearVelocity)(left.Value * right.Value);

    public static LinearVelocity operator *(Frequency left, Wavelength right)
        => (LinearVelocity)(left.Value * right.Value);
}

public readonly partial struct LinearVelocity
{
    public static Frequency operator /(LinearVelocity left, Wavelength right)
        => (Frequency)(left.Value / right.Value);

    public static Wavelength operator /(LinearVelocity left, Frequency right)
        => (Wavelength)(left.Value / right.Value);
}

public readonly partial struct Impulse
{
    // Formula mechanics/impulse-and-momentum-change
    public static Force operator /(Impulse left, Time right)
        => (Force)(left.Value / right.Value);

    public static Time operator /(Impulse left, Force right)
        => (Time)(left.Value / right.Value);
}

public readonly partial struct ThermalConductance
{
    // Conductance is for the whole thermal path. The heat-transfer coefficient is per area.
    // Formula thermal/thermal-path
    public static HeatFlowRate operator *(ThermalConductance left, TemperatureDifference right)
        => (HeatFlowRate)(left.Value * right.Value);

    public static HeatFlowRate operator *(TemperatureDifference left, ThermalConductance right)
        => (HeatFlowRate)(left.Value * right.Value);

    public static ThermalResistance operator /(double left, ThermalConductance right)
        => (ThermalResistance)(left / right.Value);
}

public readonly partial struct HeatFlowRate
{
    public static ThermalConductance operator /(HeatFlowRate left, TemperatureDifference right)
        => (ThermalConductance)(left.Value / right.Value);

    public static TemperatureDifference operator /(HeatFlowRate left, ThermalConductance right)
        => (TemperatureDifference)(left.Value / right.Value);
}

public readonly partial struct CoefficientOfHeatTransfer
{
    // Formula thermal/heat-transfer-conductance
    // Equation Gₜ = hA
    // Applies Uniform or area-averaged heat-transfer coefficient for the same thermal path and driving temperature difference.
    // Source [QUDT heat-transfer coefficient](https://qudt.org/vocab/quantitykind/CoefficientOfHeatTransfer)
    // Source [QUDT thermal conductance](https://qudt.org/vocab/quantitykind/ThermalConductance)
    public static ThermalConductance operator *(CoefficientOfHeatTransfer left, Area right)
        => (ThermalConductance)(left.Value * right.Value);

    public static ThermalConductance operator *(Area left, CoefficientOfHeatTransfer right)
        => (ThermalConductance)(left.Value * right.Value);
}

public readonly partial struct ThermalConductance
{
    public static CoefficientOfHeatTransfer operator /(ThermalConductance left, Area right)
        => (CoefficientOfHeatTransfer)(left.Value / right.Value);

    public static Area operator /(ThermalConductance left, CoefficientOfHeatTransfer right)
        => (Area)(left.Value / right.Value);
}

public readonly partial struct ThermalResistance
{
    public static ThermalConductance operator /(double left, ThermalResistance right)
        => (ThermalConductance)(left / right.Value);

    // Formula thermal/thermal-path
    public static DimensionlessRatio operator *(ThermalResistance left, ThermalConductance right)
        => (DimensionlessRatio)(left.Value * right.Value);

    public static DimensionlessRatio operator *(ThermalConductance left, ThermalResistance right)
        => (DimensionlessRatio)(left.Value * right.Value);
}
