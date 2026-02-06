// -----------------------------------------------------------------------
// <copyright file="Helpers.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using System.Text;

using Cysharp.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Newtonsoft.Json.Linq;

using PeterO.Numbers;

using TedToolkit.Quantities.Data;
using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

using Conversion = TedToolkit.Quantities.Data.Conversion;

namespace TedToolkit.Quantities.Analyzer;

/// <summary>
/// Some helpers.
/// </summary>
internal static class Helpers
{
    private static readonly Dictionary<char, char> _superscripts = new()
    {
        ['0'] = '⁰',
        ['1'] = '¹',
        ['2'] = '²',
        ['3'] = '³',
        ['4'] = '⁴',
        ['5'] = '⁵',
        ['6'] = '⁶',
        ['7'] = '⁷',
        ['8'] = '⁸',
        ['9'] = '⁹',
        ['-'] = '⁻',
    };

    /// <summary>
    /// To super script.
    /// </summary>
    /// <param name="value">value.</param>
    /// <returns>script.</returns>
    public static string ToSuperscript(this int value)
    {
        var s = value.ToString(CultureInfo.InvariantCulture);
        var sb = new StringBuilder(s.Length);
        foreach (var c in s)
            sb.Append(_superscripts.TryGetValue(c, out var i) ? i : c);

        return sb.ToString();
    }

    /// <summary>
    /// Add the summary things.
    /// </summary>
    /// <param name="owner">the owner.</param>
    /// <param name="description">description.</param>
    /// <param name="links">links.</param>
    /// <param name="dimension">dimension.</param>
    public static void AddSummary(IRootDescription owner, string description, IEnumerable<Link> links,
        string dimension)
    {
        dimension = string.IsNullOrEmpty(dimension) ? dimension : $"<b>{dimension}</b>";

        owner.AddRootDescription(
            new DescriptionSummary(description.Split('\n').Select(t => new DescriptionText(t)).ToArray()));
        owner.AddRootDescription(new DescriptionRemarks(
            new DescriptionText(dimension),
            new DescriptionText(string.Join("\t", links.Select(l => l.Remarks)))
        ));
    }

    public static DataCollection GetData(string? fileName, IEnumerable<string> jsons, string[] quantities)
    {
        var asm = typeof(QuantitiesGenerator).Assembly;

        string? resourceName = null;
        if (fileName is not null)
        {
            resourceName = asm.GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith(fileName + ".json", StringComparison.OrdinalIgnoreCase));
        }

        resourceName ??= asm.GetManifestResourceNames()
            .FirstOrDefault(n => n.EndsWith("ISQ.json", StringComparison.OrdinalIgnoreCase));

        JObject jObject;
        {
            using var stream = asm.GetManifestResourceStream(resourceName)!;
            using var reader = new StreamReader(stream);
            jObject = JObject.Parse(reader.ReadToEnd());
        }

        foreach (var json in jsons)
            AppendObject(JObject.Parse(json));

        var result = jObject.ToObject<DataCollection>();

        return result with
        {
            Quantities = result.Quantities
                .Where(QuantityPredict)
                .ToDictionary(i => i.Key, i => i.Value),
        };

        bool QuantityPredict(KeyValuePair<string, Quantity> pair)
        {
            if (fileName is null)
                return pair.Value.IsBasic;

            if (quantities.Length is not 0)
                return pair.Value.IsBasic || quantities.Contains(pair.Key);

            return true;
        }

        void AppendObject(JObject obj)
        {
            jObject.Merge(obj,
                new JsonMergeSettings
                {
                    MergeNullValueHandling = MergeNullValueHandling.Merge,
                    MergeArrayHandling = MergeArrayHandling.Union,
                });
        }
    }

    public static IExpression? GetSystemToUnit(this in Unit unit, in UnitSystem system, in Dimension dimension,
        ITypeSymbol dataType)
    {
        var conversion = ToSystemConversion(system, dimension)?.TransformTo(unit.Conversion);
        return ToExpression(conversion, dataType, "Value".ToSimpleName());
    }

    public static IExpression? GetUnitToSystem(this in Unit unit, in UnitSystem system, in Dimension dimension,
        ITypeSymbol dataType)
    {
        var conversion = unit.Conversion.TransformTo(ToSystemConversion(system, dimension));
        return ToExpression(conversion, dataType, "value".ToSimpleName());
    }

    private static IExpression? ToExpression(Conversion? conversion, ITypeSymbol dataType, SimpleNameExpression argument)
    {
        if (conversion?.IsValid != true)
            return null;

        IExpression multiple = conversion.Value.Multiplier.ToEDecimal().Equals(EDecimal.One)
            ? argument
            : CreateNumber(conversion.Value.Multiplier, dataType).Operator("*", argument);

        if (conversion.Value.Offset.IsZero)
            return multiple;

        return multiple.Operator("+", CreateNumber(conversion.Value.Offset, dataType));
    }

    public static Conversion? ToSystemConversion(scoped in UnitSystem system, scoped in Dimension dimension)
    {
        Conversion? result = Conversion.Unit;
        foreach (var systemKey in system.Keys)
        {
            var unitConversion = system.GetUnit(systemKey).Conversion;
            var conversion = unitConversion.Pow(dimension.GetExponent(systemKey));
            result = result?.Merge(conversion);
        }

        return result;
    }

    public static decimal ToDecimal(ERational data)
    {
        try
        {
            var dec = data.ToEDecimal();
            if (!dec.IsNaN())
                return dec.ToDecimal();
        }
        catch (OverflowException)
        {
        }

        return (decimal)data.Numerator.ToInt64Unchecked() / data.Denominator.ToInt64Unchecked();
    }

    private static IExpression CreateNumber(ERational data, ITypeSymbol dataType)
    {
        var dec = data.ToEDecimal();
        if (!dec.IsNaN())
            return CreateNumber(dec, dataType);

        return CreateNumber(data.Numerator, dataType)
            .Operator("/", CreateNumber(data.Denominator, dataType));
    }

    private static IExpression CreateNumber(EDecimal data, ITypeSymbol dataType)
    {
        var num = data.ToString();

        if (!IsFloatingPoint(dataType))
        {
            if (int.TryParse(num, out var i))
                return i.ToLiteral();

            if (uint.TryParse(num, out var u))
                return u.ToLiteral();

            if (long.TryParse(num, out var l))
                return l.ToLiteral();

            if (ulong.TryParse(num, out var ul))
                return ul.ToLiteral();
        }

        if (dataType.SpecialType is SpecialType.System_Decimal && decimal.TryParse(num, out var m))
            return ZString.Concat(num, 'm').ToSimpleName();

        if (double.TryParse(num, out var d))
            return ZString.Concat(num, 'd').ToSimpleName();

        return num.ToSimpleName();
    }

    /// <summary>
    /// Is floating point type.
    /// </summary>
    /// <param name="type">the type.</param>
    /// <returns>result.</returns>
    public static bool IsFloatingPoint(this ITypeSymbol type)
    {
        return type.SpecialType is SpecialType.System_Single
            or SpecialType.System_Double
            or SpecialType.System_Decimal;
    }
}