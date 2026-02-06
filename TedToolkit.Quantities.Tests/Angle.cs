namespace TedToolkit.Quantities;

[QuantityDisplayUnit<AngleUnit>(AngleUnit.Degree)]
[QuantityOperator<Angle, Angle, DimensionlessRatio>(Operator.DIVIDE)]
[QuantityOperator<Angle, Angle, Angle>(Operator.SUBTRACT)]
[QuantityOperator<Angle, Length, Length>(Operator.MULTIPLY)]
[QuantityOperator<Length, Angle, Length>(Operator.MULTIPLY)]
public partial struct Angle;