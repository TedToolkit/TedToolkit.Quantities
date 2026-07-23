// See https://aka.ms/new-console-template for more information

using TedToolkit.Quantities;
using TedToolkit.Scopes;

Console.WriteLine("Hello, World!");

_ = Angle.FromDegree(1);

try
{
    _ = Angle.FromDegree(-1);
    throw new InvalidOperationException("Expected the value assertion to reject a negative angle.");
}
catch (ArgumentOutOfRangeException)
{
}

using (new Tolerance().FastPush())
{
    var length = 10.0.Metre;
}