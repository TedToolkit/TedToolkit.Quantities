// -----------------------------------------------------------------------
// <copyright file="QuantityWidenings.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// -----------------------------------------------------------------------

namespace TedToolkit.Quantities.SI.Double;

// The target must be a semantic generalization. Sharing the same SI dimensions is only a prerequisite.
[QuantityImplicitlyConvertsTo<Length>]
public readonly partial struct Thickness;

[QuantityImplicitlyConvertsTo<Length>]
public readonly partial struct Width;

[QuantityImplicitlyConvertsTo<Length>]
public readonly partial struct Height;

[QuantityImplicitlyConvertsTo<Length>]
public readonly partial struct Depth;

[QuantityImplicitlyConvertsTo<Length>]
public readonly partial struct Radius;

[QuantityImplicitlyConvertsTo<Length>]
public readonly partial struct Diameter;

[QuantityImplicitlyConvertsTo<Length>]
public readonly partial struct Distance;

[QuantityImplicitlyConvertsTo<Length>]
public readonly partial struct Wavelength;

[QuantityImplicitlyConvertsTo<Length>]
public readonly partial struct PathLength;

[QuantityImplicitlyConvertsTo<Force>]
public readonly partial struct GravitationalAttraction;

[QuantityImplicitlyConvertsTo<Power>]
public readonly partial struct HeatFlowRate;

[QuantityImplicitlyConvertsTo<PowerPerArea>]
public readonly partial struct HeatFlowRatePerArea;

[QuantityImplicitlyConvertsTo<ForcePerArea>]
public readonly partial struct Pressure;

// The scalar Stress type represents a component of force per area.
[QuantityImplicitlyConvertsTo<ForcePerArea>]
public readonly partial struct Stress;

[QuantityImplicitlyConvertsTo<LuminousFluxPerArea>]
public readonly partial struct Illuminance;

public readonly partial struct LuminousFluxPerArea;

[QuantityImplicitlyConvertsTo<DimensionlessRatio>]
public readonly partial struct ReynoldsNumber;