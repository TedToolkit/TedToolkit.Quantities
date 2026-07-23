using TedToolkit.Quantities;

var length = Length.FromMillimetre(1000d);
if (length.Value != 1d || length.As(LengthUnit.Millimetre) != 1000d)
{
    throw new InvalidOperationException("Double length conversion failed.");
}
