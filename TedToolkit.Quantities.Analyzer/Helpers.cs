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
using TedToolkit.RoslynHelper;
using TedToolkit.RoslynHelper.Syntaxes;

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
        {
            sb.Append(_superscripts.TryGetValue(c, out var i) ? i : c);
        }

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
            new DescriptionText(string.Join("\t", links.Select(l => l.Remarks)))));
    }

    /// <summary>
    /// Get the data.
    /// </summary>
    /// <param name="fileName">file name.</param>
    /// <param name="jsons">json values.</param>
    /// <param name="quantities">quantities.</param>
    /// <param name="catalog">The supplied catalog when the project provides one.</param>
    /// <returns>results.</returns>
    public static DataCollection GetData(string? fileName, IEnumerable<string> jsons, string[] quantities,
        string? catalog)
    {
        var jObject = JObject.Parse(catalog ?? ReadEmbeddedCatalog(fileName));

        foreach (var json in jsons)
        {
            AppendObject(JObject.Parse(json));
        }

        var result = jObject.ToObject<DataCollection>();

        return result with
        {
            Quantities = result.Quantities
                .Where(QuantityPredict)
                .ToDictionary(i => i.Key, i => i.Value),
        };

        static string ReadEmbeddedCatalog(string? name)
        {
            var assembly = typeof(QuantitiesGenerator).Assembly;
            var resources = assembly.GetManifestResourceNames();
            var resource = name is null
                ? null
                : resources.FirstOrDefault(item => item.EndsWith(name + ".json", StringComparison.OrdinalIgnoreCase));
            resource ??= resources.FirstOrDefault(item => item.EndsWith("ISQ.json", StringComparison.OrdinalIgnoreCase));
            using var reader = new StreamReader(assembly.GetManifestResourceStream(resource)!);
            return reader.ReadToEnd();
        }

        bool QuantityPredict(KeyValuePair<string, Quantity> pair)
        {
            if (fileName is null)
            {
                return pair.Value.IsBasic;
            }

            if (quantities.Length is 0)
            {
                return true;
            }

            return pair.Value.IsBasic || quantities.Contains(pair.Key);
        }

        void AppendObject(JObject obj)
        {
            jObject.Merge(obj,
                new JsonMergeSettings()
                {
                    MergeNullValueHandling = MergeNullValueHandling.Merge,
                    MergeArrayHandling = MergeArrayHandling.Union,
                });
        }
    }

    /// <summary>
    /// Get the system to unit.
    /// </summary>
    /// <param name="unit">unit.</param>
    /// <param name="system">system.</param>
    /// <param name="dimension">dimension.</param>
    /// <param name="dataType">data type.</param>
    /// <returns>result.</returns>
    public static IExpression? GetSystemToUnit(this in Unit unit, in UnitSystem system, in Dimension dimension,
        ITypeSymbol dataType)
    {
        var conversion = ToSystemConversion(system, dimension)?.TransformTo(unit.Conversion);
        return ToExpression(conversion, dataType, "Value".ToSimpleName());
    }

    /// <summary>
    /// get the unit to system.
    /// </summary>
    /// <param name="unit">unit.</param>
    /// <param name="system">system.</param>
    /// <param name="dimension">dimension.</param>
    /// <param name="dataType">data type.</param>
    /// <returns>result.</returns>
    public static IExpression? GetUnitToSystem(this in Unit unit, in UnitSystem system, in Dimension dimension,
        ITypeSymbol dataType)
    {
        var conversion = unit.Conversion.TransformTo(ToSystemConversion(system, dimension));
        return ToExpression(conversion, dataType, "value".ToSimpleName());
    }

    private static IExpression? ToExpression(Data.Conversion? conversion, ITypeSymbol dataType,
        SimpleNameExpression argument)
    {
        if (conversion?.IsValid != true)
        {
            return null;
        }

        IExpression multiple = conversion.Value.Multiplier.ToEDecimal().Equals(EDecimal.One)
            ? argument
            : CreateNumber(conversion.Value.Multiplier, dataType).Operator("*", argument);

        if (conversion.Value.Offset.IsZero)
        {
            return multiple;
        }

        return multiple.Operator("+", CreateNumber(conversion.Value.Offset, dataType));
    }

    /// <summary>
    /// To system conversion.
    /// </summary>
    /// <param name="system">system.</param>
    /// <param name="dimension">dimension.</param>
    /// <returns>result.</returns>
    public static Data.Conversion? ToSystemConversion(scoped in UnitSystem system, scoped in Dimension dimension)
    {
        Data.Conversion? result = Data.Conversion.Unit;
        foreach (var systemKey in system.Keys)
        {
            var unitConversion = system.GetUnit(systemKey).Conversion;
            var conversion = unitConversion.Pow(dimension.GetExponent(systemKey));
            result = result?.Merge(conversion);
        }

        return result;
    }

    /// <summary>
    /// To decimal.
    /// </summary>
    /// <param name="data">data.</param>
    /// <returns>result.</returns>
    public static decimal ToDecimal(ERational data)
    {
        try
        {
            var dec = data.ToEDecimal();
            if (!dec.IsNaN())
            {
                return dec.ToDecimal();
            }
        }
        catch (OverflowException)
        {
            // Ignore
        }

        return (decimal)data.Numerator.ToInt64Unchecked() / data.Denominator.ToInt64Unchecked();
    }

    private static IExpression CreateNumber(ERational data, ITypeSymbol dataType)
    {
        var dec = data.ToEDecimal();
        if (!dec.IsNaN())
        {
            return CreateNumber(dec, dataType);
        }

        return CreateNumber(data.Numerator, dataType)
            .Operator("/", CreateNumber(data.Denominator, dataType));
    }

    /// <summary>
    /// Creates a numeric source expression compatible with the target data type.
    /// </summary>
    /// <param name="data">The source numeric value.</param>
    /// <param name="dataType">The target data type.</param>
    /// <returns>A numeric source expression.</returns>
    internal static IExpression CreateNumber(EDecimal data, ITypeSymbol dataType)
    {
        var num = data.ToString();

        if (!IsFloatingPoint(dataType))
        {
            if (int.TryParse(num, out var i))
            {
                return i.ToLiteral();
            }

            if (uint.TryParse(num, out var u))
            {
                return u.ToLiteral();
            }

            if (long.TryParse(num, out var l))
            {
                return l.ToLiteral();
            }

            if (ulong.TryParse(num, out var ul))
            {
                return ul.ToLiteral();
            }
        }

        if (dataType.SpecialType is SpecialType.System_Decimal)
        {
            return ZString.Concat("global::System.Decimal.Parse(\"", num,
                "\", global::System.Globalization.CultureInfo.InvariantCulture)").ToSimpleName();
        }

        if (dataType.SpecialType is SpecialType.System_Single)
        {
            return ZString.Concat("global::System.Single.Parse(\"", num,
                "\", global::System.Globalization.CultureInfo.InvariantCulture)").ToSimpleName();
        }

        if (double.TryParse(num, out _))
        {
            return ZString.Concat(num, 'd').ToSimpleName();
        }

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

    /// <summary>
    /// Gets whether the type supports IEEE special floating-point values.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>Whether the type supports NaN and infinity.</returns>
    public static bool IsIeeeFloatingPoint(this ITypeSymbol type)
    {
        return type.SpecialType is SpecialType.System_Single
            or SpecialType.System_Double;
    }
}