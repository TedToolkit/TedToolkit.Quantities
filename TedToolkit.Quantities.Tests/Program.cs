// See https://aka.ms/new-console-template for more information

using TedToolkit.Quantities;
using TedToolkit.Scopes;

Console.WriteLine("Hello, World!");

using (new Tolerance().FastPush())
{
    var length = 10.0.Metre;
}