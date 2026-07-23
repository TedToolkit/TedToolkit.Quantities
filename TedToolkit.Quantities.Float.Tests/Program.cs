using TedToolkit.Quantities;

var length = Length.FromMillimetre(1000f);
if (length.Value != 1f || length.As(LengthUnit.Millimetre) != 1000f)
{
    throw new InvalidOperationException("Single-precision length conversion failed.");
}
