using TedToolkit.Quantities;

var length = Length.FromMillimetre(1000m);
if (length.Value != 1m || length.As(LengthUnit.Millimetre) != 1000m)
{
    throw new InvalidOperationException("Decimal length conversion failed.");
}
