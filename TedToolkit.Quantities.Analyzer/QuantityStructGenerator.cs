// -----------------------------------------------------------------------
// <copyright file="QuantityStructGenerator.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using TedToolkit.Quantities.Data;
using TedToolkit.RoslynHelper;
using TedToolkit.RoslynHelper.Syntaxes;

using static TedToolkit.RoslynHelper.SourceComposer;
using static TedToolkit.RoslynHelper.SourceComposer<
    TedToolkit.Quantities.Analyzer.QuantityStructGenerator>;

namespace TedToolkit.Quantities.Analyzer;

/// <summary>
/// The quantity struct generator.
/// </summary>
/// <param name="data">data.</param>
/// <param name="quantity">quantity.</param>
/// <param name="typeName">type name.</param>
/// <param name="unitSystem">unit system.</param>
/// <param name="isPublic">is public.</param>
/// <param name="quantitySymbol">quantity symbol.</param>
/// <param name="generatedNamespace">The namespace of the generated quantity.</param>
/// <param name="precompiled">Whether the quantity is part of the coherent SI library.</param>
internal sealed class QuantityStructGenerator(
    DataCollection data,
    Quantity quantity,
    ITypeSymbol typeName,
    UnitSystem unitSystem,
    bool isPublic,
    INamedTypeSymbol? quantitySymbol,
    string generatedNamespace,
    bool precompiled)
{
    private bool IsAbsoluteTemperature
    {
        get
        {
            return precompiled && quantity.Name == "ThermodynamicTemperature";
        }
    }

    private IEnumerable<Operator> CreateCustomOperators()
    {
        foreach (var (_, leftType, rightType, resultType, operatorName) in
                 QuantityOperatorDeclaration.Read(quantitySymbol))
        {
            if (precompiled && !MatchesDimension(operatorName, leftType, rightType, resultType))
            {
                throw new InvalidOperationException(
                    $"The declared result of {leftType.Name} {operatorName} {rightType.Name} has incompatible dimensions: {resultType.Name}.");
            }

            var leftValue = leftType.SpecialType is SpecialType.None ? "left.Value" : "left";
            var rightValue = rightType.SpecialType is SpecialType.None ? "right.Value" : "right";

            var returnType = DataType.FromSymbol(resultType);
            yield return Operator(new(returnType), operatorName)
                .AddParameter(Parameter(DataType.FromSymbol(leftType), "left"))
                .AddParameter(Parameter(DataType.FromSymbol(rightType), "right"))
                .AddStatement(leftValue.ToSimpleName().Operator(operatorName, rightValue.ToSimpleName()).Parenthesized
                    .Cast(returnType).Return);
        }
    }

    private bool MatchesDimension(string operation, ITypeSymbol left, ITypeSymbol right, ITypeSymbol result)
    {
        var leftDimension = GetDimension(left);
        var rightDimension = GetDimension(right);
        var resultDimension = GetDimension(result);
        return operation switch
        {
            "+" or "-" => leftDimension == rightDimension && leftDimension == resultDimension,
            "*" => CombinesDimensions(leftDimension, rightDimension, resultDimension),
            "/" => CombinesDimensions(rightDimension, resultDimension, leftDimension),
            _ => false,
        };

        Dimension GetDimension(ITypeSymbol type)
        {
            // QUDT's dimensionless flag is metadata. Only the seven SI exponents compose.
            return type.SpecialType == SpecialType.System_Double
                ? default
                : GetSiDimension(data.Quantities[type.Name]);
        }
    }

    private Dimension GetSiDimension(in Quantity value)
    {
        return data.Dimensions[value.Dimension] with { Dimensionless = 0, };
    }

    private IEnumerable<TedToolkit.RoslynHelper.Syntaxes.Conversion> CreateQuantityConversions()
    {
        if (!precompiled)
        {
            foreach (var name in quantity.ExactMatch.Where(data.Quantities.ContainsKey))
            {
                yield return CreateConversion(name, true);
            }

            yield break;
        }

        var dimension = GetSiDimension(quantity);
        var wideningTargets = GetWideningTargets(dimension);
        foreach (var target in data.Quantities.Values)
        {
            if (target.Name != quantity.Name && GetSiDimension(target) == dimension)
            {
                yield return CreateConversion(target.Name, wideningTargets.Contains(target.Name));
            }
        }

        static TedToolkit.RoslynHelper.Syntaxes.Conversion CreateConversion(string name, bool isImplicit)
        {
            var target = new DataType(name);
            return (isImplicit ? ImplicitConversionTo(target) : ExplicitConversionTo(target))
                .AddStatement("@value.Value".ToSimpleName().Cast(target).Return);
        }
    }

    private HashSet<string> GetWideningTargets(in Dimension dimension)
    {
        var result = new HashSet<string>(StringComparer.Ordinal);
        if (quantitySymbol is null)
        {
            return result;
        }

        foreach (var attribute in quantitySymbol.GetAttributes())
        {
            if (attribute.AttributeClass is not { IsGenericType: true, } attributeClass
                || attributeClass.ConstructUnboundGenericType().FullName
                    is not "TedToolkit.Quantities.QuantityImplicitlyConvertsToAttribute<>")
            {
                continue;
            }

            var target = attributeClass.TypeArguments[0];
            if (target.ContainingNamespace.ToDisplayString() != generatedNamespace
                || target.Name == quantity.Name
                || !data.Quantities.TryGetValue(target.Name, out var targetQuantity)
                || GetSiDimension(targetQuantity) != dimension)
            {
                throw new InvalidOperationException(
                    $"Implicit conversion from {quantity.Name} to {target} must target a distinct quantity in the same SI dimension.");
            }

            result.Add(target.Name);
        }

        return result;
    }

    private static bool CombinesDimensions(in Dimension left, in Dimension right, in Dimension result)
    {
        return left.AmountOfSubstance + right.AmountOfSubstance == result.AmountOfSubstance
               && left.ElectricCurrent + right.ElectricCurrent == result.ElectricCurrent
               && left.Length + right.Length == result.Length
               && left.LuminousIntensity + right.LuminousIntensity == result.LuminousIntensity
               && left.Mass + right.Mass == result.Mass
               && left.ThermodynamicTemperature + right.ThermodynamicTemperature == result.ThermodynamicTemperature
               && left.Time + right.Time == result.Time;
    }

    /// <summary>
    /// Generate the code.
    /// </summary>
    /// <param name="context">context.</param>
    public void GenerateCode(scoped in SourceProductionContext context)
    {
        var quantityType = new DataType(quantity.Name.ToSimpleName());
        var typeType = DataType.FromSymbol(typeName);

        var structDeclaration = Struct(quantity.Name).Readonly.Partial
            .AddBaseType(new DataType("global::TedToolkit.Quantities.IQuantity".ToSimpleName())
                .Generic(quantityType, typeType,
                    new DataType(quantity.UnitName.ToSimpleName())))
            .AddRootDescription(new DescriptionInheritDoc(quantity.UnitName.ToSimpleName()));

        structDeclaration = isPublic ? structDeclaration.Public : structDeclaration.Internal;

        structDeclaration
            .AddMember(Property(quantityType, "Zero").Static.Public
                .AddAccessor(Accessor(AccessorType.GET)
                    .AddStatement(0.ToLiteral().Cast(quantityType).Return)))
            .AddMember(Property(quantityType, "One").Static.Public
                .AddAccessor(Accessor(AccessorType.GET)
                    .AddStatement(1.ToLiteral().Cast(quantityType).Return)))
            .AddMember(Property(quantityType, "MinValue").Static.Public
                .AddAccessor(Accessor(AccessorType.GET)
                    .AddStatement(typeType.Type.Sub("MinValue").Cast(quantityType).Return)))
            .AddMember(Property(quantityType, "MaxValue").Static.Public
                .AddAccessor(Accessor(AccessorType.GET)
                    .AddStatement(typeType.Type.Sub("MaxValue").Cast(quantityType).Return)))
            .AddMember(Property(typeType, "Value").Public
                .AddAccessor(Accessor(AccessorType.GET)))
            .AddMember(Method("AssertValue", new(DataType.Void)).Static.Partial
                .AddParameter(Parameter(typeType, "value")));

        var valueConstructor = Constructor()
            .AddParameter(Parameter(typeType, "value"))
            .AddStatement("Value = @value".ToSimpleName())
            .AddStatement("AssertValue".ToSimpleName().Invoke()
                .AddArgument(Argument("Value".ToSimpleName())));

        valueConstructor = quantity.IsNoDimensions ? valueConstructor.Public : valueConstructor.Private;
        structDeclaration
            .AddMember(valueConstructor)
            .AddMember(Constructor().Public
                .AddParameter(Parameter(typeType, "value"))
                .AddParameter(Parameter(new DataType(quantity.UnitName), "unit"))
                .AddStatement(CreateSwitchStatement((info, section) =>
                {
                    var exp = info.GetUnitToSystem(unitSystem,
                        data.Dimensions[quantity.Dimension],
                        typeName);

                    section.AddStatement(exp is null
                        ? new ObjectCreationExpression(DataType.FromType<NotImplementedException>()).Throw
                        : "Value".ToSimpleName().Operator("=", exp));
                    section.AddStatement("AssertValue".ToSimpleName().Invoke()
                        .AddArgument(Argument("Value".ToSimpleName())));
                    section.AddStatement(new ReturnStatement());
                }, section => section
                    .AddStatement("Value = @value".ToSimpleName())
                    .AddStatement("AssertValue(Value)".ToSimpleName())
                    .AddStatement(new ReturnStatement()))))
            .AddMember(Method("As", new(typeType)).Public
                .AddParameter(Parameter(new DataType(quantity.UnitName), "unit"))
                .AddStatement(CreateSwitchStatement((info, section) =>
                {
                    var exp = info.GetSystemToUnit(unitSystem, data.Dimensions[quantity.Dimension],
                        typeName);

                    if (exp is null)
                    {
                        section.AddStatement(new ObjectCreationExpression(DataType.FromType<NotImplementedException>())
                            .Throw);
                    }
                    else
                    {
                        section.AddStatement(exp.Cast(typeType).Return);
                    }
                }, section => section.AddStatement("Value".ToSimpleName().Return))))
            .AddMember(Method("ToString", new(DataType.String)).Override.Public
                .AddStatement("ToString".ToSimpleName().Invoke()
                    .AddArgument(Argument(SimpleNameExpression.Null))
                    .AddArgument(Argument("global::System.Globalization.CultureInfo.CurrentCulture".ToSimpleName()))
                    .Return))
            .AddMember(Method("ToString", new(DataType.String)).Public
                .AddParameter(Parameter<string?>("format").AddNull())
                .AddParameter(Parameter<IFormatProvider?>("formatProvider").AddNull())
                .AddStatement("ToString".ToSimpleName().Invoke()
                    .AddArgument(Argument(GetSystemUnitName().ToSimpleName()))
                    .AddArguments(precompiled ? ["format".ToSimpleName(), "formatProvider".ToSimpleName(),] : [])
                    .Return))
            .AddMember(Method("ToString", new(DataType.String)).Public
                .AddParameter(Parameter(new DataType(quantity.UnitName), "unit"))
                .AddParameter(Parameter<string?>("format").AddNull())
                .AddParameter(Parameter<IFormatProvider?>("formatProvider").AddNull())
                .AddStatement("format = global::TedToolkit.Quantities.Internals.ParseFormat(format, out var index)"
                    .ToSimpleName())
                .AddStatement("As(unit).ToString(format, formatProvider) + \" \" + unit.ToString(index, formatProvider)"
                    .ToSimpleName().Return))
            .AddMember(Method("Equals", new(DataType.Bool)).Public
                .AddParameter(Parameter(quantityType, "other"))
                .AddStatement((precompiled ? "Value.Equals(other.Value)" : "Tolerance.CurrentOrDefault.Equals(this, other)")
                    .ToSimpleName().Return))
            .AddMember(Method("Equals", new(DataType.Bool)).Public.Override
                .AddParameter(Parameter(DataType.Object.Null, "obj"))
                .AddStatement(ZString.Concat("obj is ", quantity.Name, " other && Equals(other)").ToSimpleName()
                    .Return))
            .AddMember(Method("GetHashCode", new(DataType.Int)).Public.Override
                .AddStatement("Value.GetHashCode()".ToSimpleName().Return))
            .AddMember(Method("CompareTo", new(DataType.Int)).Public
                .AddParameter(Parameter(quantityType, "other"))
                .AddStatement((precompiled ? "Value.CompareTo(other.Value)" : "Tolerance.CurrentOrDefault.Compare(this, other)")
                    .ToSimpleName().Return))
            .AddMember(Method("CompareTo", new(DataType.Int)).Public
                .AddParameter(Parameter(DataType.Object.Null, "other"))
                .AddStatement("global::TedToolkit.Quantities.Internals.CompareTo(this, other)".ToSimpleName().Return))
            .AddMember(Method("From", new(quantityType)).Public.Static
                .AddParameter(Parameter(typeType, "value"))
                .AddParameter(Parameter(new DataType(quantity.UnitName), "unit"))
                .AddStatement(new ObjectCreationExpression()
                    .AddArgument(Argument("@value".ToSimpleName()))
                    .AddArgument(Argument("unit".ToSimpleName())).Return));

        structDeclaration
            .AddMember(Operator(new(DataType.Bool), "==")
                .AddParameter(Parameter(quantityType, "left"))
                .AddParameter(Parameter(quantityType, "right"))
                .AddStatement("left.Equals(right)".ToSimpleName().Return))
            .AddMember(Operator(new(DataType.Bool), "!=")
                .AddParameter(Parameter(quantityType, "left"))
                .AddParameter(Parameter(quantityType, "right"))
                .AddStatement("!left.Equals(right)".ToSimpleName().Return))
            .AddMember(Operator(new(DataType.Bool), ">")
                .AddParameter(Parameter(quantityType, "left"))
                .AddParameter(Parameter(quantityType, "right"))
                .AddStatement("left.CompareTo(right) > 0".ToSimpleName().Return))
            .AddMember(Operator(new(DataType.Bool), ">=")
                .AddParameter(Parameter(quantityType, "left"))
                .AddParameter(Parameter(quantityType, "right"))
                .AddStatement("left.CompareTo(right) >= 0".ToSimpleName().Return))
            .AddMember(Operator(new(DataType.Bool), "<")
                .AddParameter(Parameter(quantityType, "left"))
                .AddParameter(Parameter(quantityType, "right"))
                .AddStatement("left.CompareTo(right) < 0".ToSimpleName().Return))
            .AddMember(Operator(new(DataType.Bool), "<=")
                .AddParameter(Parameter(quantityType, "left"))
                .AddParameter(Parameter(quantityType, "right"))
                .AddStatement("left.CompareTo(right) <= 0".ToSimpleName().Return));

        foreach (var quantityUnit in quantity.Units)
        {
            var info = data.Units[quantityUnit];
            var unitName = info.GetUnitName(data.Units.Values);
            var enumName = ZString.Concat(quantity.UnitName, '.', unitName).ToSimpleName();

            structDeclaration
                .AddMember(Method(ZString.Concat("From", unitName),
                        new(quantityType)).Public.Static
                    .AddRootDescription(new DescriptionSummary(new DescriptionText("From "),
                        new DescriptionSee(enumName)))
                    .AddParameter(Parameter(typeType, "value"))
                    .AddStatement(new ObjectCreationExpression()
                        .AddArgument(Argument("@value".ToSimpleName()))
                        .AddArgument(Argument(enumName))
                        .Return))
                .AddMember(Property(typeType, unitName == quantity.Name ? unitName + "_" : unitName).Public
                    .AddRootDescription(new DescriptionInheritDoc(enumName))
                    .AddAccessor(Accessor(AccessorType.GET)
                        .AddStatement("As".ToSimpleName().Invoke()
                            .AddArgument(Argument(enumName)).Return)));
        }

        if (!IsAbsoluteTemperature)
        {
            structDeclaration
                .AddMember(Operator(new(quantityType), "-")
                    .AddParameter(Parameter(quantityType, "value"))
                    .AddStatement("(-@value.Value)".ToSimpleName().Cast(quantityType).Return))
                .AddMember(Operator(new(quantityType), "+")
                    .AddParameter(Parameter(quantityType, "left"))
                    .AddParameter(Parameter(quantityType, "right"))
                    .AddStatement("(left.Value + right.Value)".ToSimpleName().Cast(quantityType).Return))
                .AddMember(Operator(new(quantityType), "%")
                    .AddParameter(Parameter(quantityType, "left"))
                    .AddParameter(Parameter(quantityType, "right"))
                    .AddStatement("(left.Value % right.Value)".ToSimpleName().Cast(quantityType).Return))
                .AddMember(Operator(new(quantityType), "/")
                    .AddParameter(Parameter(quantityType, "left"))
                    .AddParameter(Parameter(typeType, "right"))
                    .AddStatement("(left.Value / right)".ToSimpleName().Cast(quantityType).Return))
                .AddMember(Operator(new(quantityType), "*")
                    .AddParameter(Parameter(quantityType, "left"))
                    .AddParameter(Parameter(typeType, "right"))
                    .AddStatement("(left.Value * right)".ToSimpleName().Cast(quantityType).Return))
                .AddMember(Operator(new(quantityType), "*")
                    .AddParameter(Parameter(typeType, "left"))
                    .AddParameter(Parameter(quantityType, "right"))
                    .AddStatement("(left * right.Value)".ToSimpleName().Cast(quantityType).Return));

            if (precompiled)
            {
                structDeclaration.AddMember(Operator(new(quantityType), "-")
                    .AddParameter(Parameter(quantityType, "left"))
                    .AddParameter(Parameter(quantityType, "right"))
                    .AddStatement("(left.Value - right.Value)".ToSimpleName().Cast(quantityType).Return));
            }
        }

        foreach (var customOperator in CreateCustomOperators())
        {
            structDeclaration.AddMember(customOperator);
        }

        var math = DataType.FromType(typeof(Math)).Type;

        structDeclaration
            .AddMember(CreateMathMethod(nameof(Math.Abs)))
            .AddMember(Method("Min", new(quantityType)).Public
                .AddRootDescription(new DescriptionInheritDoc(
                    ZString.Concat("global::System.Math.Min(", typeType, ", ", typeType, ')').ToSimpleName()))
                .AddParameter(Parameter(quantityType, "val2"))
                .AddStatement(math.Sub("Min").Invoke()
                    .AddArgument(Argument("Value".ToSimpleName()))
                    .AddArgument(Argument("val2.Value".ToSimpleName())).Cast(quantityType).Return))
            .AddMember(Method("Max", new(quantityType)).Public
                .AddRootDescription(new DescriptionInheritDoc(
                    ZString.Concat("global::System.Math.Max(", typeType, ", ", typeType, ')').ToSimpleName()))
                .AddParameter(Parameter(quantityType, "val2"))
                .AddStatement(math.Sub("Max").Invoke()
                    .AddArgument(Argument("Value".ToSimpleName()))
                    .AddArgument(Argument("val2.Value".ToSimpleName())).Cast(quantityType).Return))
            .AddMember(Method("Clamp", new(quantityType)).Public
                .AddRootDescription(new DescriptionInheritDoc(
                    ZString.Concat("global::System.Math.Clamp(", typeType, ", ", typeType, ", ", typeType, ')')
                        .ToSimpleName()))
                .AddParameter(Parameter(quantityType, "min"))
                .AddParameter(Parameter(quantityType, "max"))
                .AddStatement("Max(min).Min(max)".ToSimpleName().Return))
            .AddMember(Property(DataType.Int, "Sign").Public
                .AddRootDescription(new DescriptionInheritDoc(
                    ZString.Concat("global::System.Math.Sign(", typeType, ')').ToSimpleName()))
                .AddAccessor(Accessor(AccessorType.GET)
                    .AddStatement("CompareTo".ToSimpleName().Invoke()
                        .AddArgument(Argument(0.ToLiteral().Cast(quantityType)))
                        .Return)));

        if (typeName.IsFloatingPoint())
        {
            structDeclaration
                .AddMember(CreateMathMethod(nameof(Math.Floor)))
                .AddMember(CreateMathMethod(nameof(Math.Ceiling)))
                .AddMember(CreateMathMethod(nameof(Math.Round)));
        }

        if (typeName.IsIeeeFloatingPoint())
        {
            structDeclaration
                .AddMember(Property(quantityType, "NaN").Static.Public
                    .AddAccessor(Accessor(AccessorType.GET)
                        .AddStatement(typeType.Type.Sub("NaN").Cast(quantityType).Return)))
                .AddMember(Property(quantityType, "PositiveInfinity").Static.Public
                    .AddAccessor(Accessor(AccessorType.GET)
                        .AddStatement(typeType.Type.Sub("PositiveInfinity").Cast(quantityType).Return)))
                .AddMember(Property(quantityType, "NegativeInfinity").Static.Public
                    .AddAccessor(Accessor(AccessorType.GET)
                        .AddStatement(typeType.Type.Sub("NegativeInfinity").Cast(quantityType).Return)))
                .AddMember(Property(DataType.Bool, "IsNaN").Public
                    .AddAccessor(Accessor(AccessorType.GET)
                        .AddStatement(typeType.Type.Sub("IsNaN").Invoke()
                            .AddArgument(Argument("Value".ToSimpleName())).Return)))
                .AddMember(Property(DataType.Bool, "IsInfinity").Public
                    .AddAccessor(Accessor(AccessorType.GET)
                        .AddStatement(typeType.Type.Sub("IsInfinity").Invoke()
                            .AddArgument(Argument("Value".ToSimpleName())).Return)))
                .AddMember(Property(DataType.Bool, "IsPositiveInfinity").Public
                    .AddAccessor(Accessor(AccessorType.GET)
                        .AddStatement(typeType.Type.Sub("IsPositiveInfinity").Invoke()
                            .AddArgument(Argument("Value".ToSimpleName())).Return)))
                .AddMember(Property(DataType.Bool, "IsNegativeInfinity").Public
                    .AddAccessor(Accessor(AccessorType.GET)
                        .AddStatement(typeType.Type.Sub("IsNegativeInfinity").Invoke()
                            .AddArgument(Argument("Value".ToSimpleName())).Return)));
        }

        var convertTo = quantitySymbol?.GetAttributes().Any(a => a.AttributeClass?.FullName
            is "TedToolkit.Quantities.QuantityImplicitToValueTypeAttribute") ?? false
            ? ImplicitConversionTo(typeType)
            : ExplicitConversionTo(typeType);

        var convertFrom = quantitySymbol?.GetAttributes().Any(a => a.AttributeClass?.FullName
            is "TedToolkit.Quantities.QuantityImplicitFromValueTypeAttribute") ?? false
            ? ImplicitConversionFrom(typeType)
            : ExplicitConversionFrom(typeType);

        structDeclaration
            .AddMember(convertTo.AddStatement("@value.Value".ToSimpleName().Return))
            .AddMember(convertFrom.AddStatement("new(@value)".ToSimpleName().Return));

        foreach (var conversion in CreateQuantityConversions())
        {
            structDeclaration.AddMember(conversion);
        }

        var space = NameSpace(generatedNamespace)
            .AddMember(structDeclaration);

        if (typeName.IsFloatingPoint())
        {
            var seeThing = new DescriptionSee(quantity.Name.ToSimpleName());
            space = space.AddMember(
                Class(ZString.Concat(quantity.Name, "Extensions")).Public.Static
                    .AddRootDescription(new DescriptionSummary(
                        new DescriptionText("Some extension about "),
                        seeThing))
                    .AddMember(CreateEnumerableMethod(nameof(Enumerable.Sum),
                        new DescriptionSummary(new DescriptionText("Computes the sum of a sequence of "),
                            seeThing,
                            new DescriptionText(" values."))))
                    .AddMember(CreateEnumerableMethod(nameof(Enumerable.Average),
                        new DescriptionSummary(new DescriptionText("Computes the average of a sequence of "),
                            seeThing,
                            new DescriptionText(
                                " values that are obtained by invoking a transform function on each element of the input sequence.")))));
        }

        File()
            .AddNameSpace(space)
            .Generate(context, quantity.Name);

        Method CreateMathMethod(string methodName)
        {
            return Method(methodName, new(new DataType(quantity.Name))).Public
                .AddRootDescription(new DescriptionInheritDoc(
                    ZString.Concat("global::System.Math.", methodName, '(', typeName.FullName, ')').ToSimpleName()))
                .AddStatement(ZString.Concat("global::System.Math.", methodName).ToSimpleName().Invoke()
                    .AddArgument(Argument("Value".ToSimpleName()))
                    .Cast(new DataType(quantity.Name)).Return);
        }

        Method CreateEnumerableMethod(string methodName, IRootDescriptionItem xml)
        {
            return Method(methodName, new(quantityType)).Public.Static
                .AddRootDescription(xml)
                .AddParameter(Parameter(DataType.FromType(typeof(IEnumerable<>))
                    .Generic(quantityType), "values").This)
                .AddStatement("global::System.Linq.Enumerable".ToSimpleName().Sub(methodName).Invoke()
                    .AddArgument(Argument("global::System.Linq.Enumerable.Select(values, i => i.Value)".ToSimpleName()))
                    .Cast(quantityType).Return);
        }
    }

    private string? UnitString
    {
        get
        {
            if (quantitySymbol?.GetAttributes().FirstOrDefault(a =>
                    a.AttributeClass is { IsGenericType: true, } attributeClass
                    && attributeClass.ConstructUnboundGenericType().FullName is
                        "global::TedToolkit.Quantities.QuantityDisplayUnitAttribute<>") is not
                        { } displayUnitAttribute)
            {
                return null;
            }

            var syntax = displayUnitAttribute.ApplicationSyntaxReference?.GetSyntax()
                as AttributeSyntax;
            var argSyntax = syntax?.ArgumentList?.Arguments[0].Expression;
            return argSyntax?.ToString();
        }
    }

    private string GetSystemUnitName()
    {
        var allUnits = quantity.Units
            .Select(u => data.Units[u])
            .ToArray();
        var none = ZString.Concat(quantity.UnitName, ".None");

        if (allUnits.Length == 0)
        {
            return none;
        }

        if (UnitString is { } argText)
        {
            return argText;
        }

        if (precompiled)
        {
            var coherentUnit = allUnits
                .Where(u => u.Conversion.Multiplier.CompareTo(Data.Conversion.Unit.Multiplier) == 0
                            && u.Conversion.Offset.IsZero)
                .OrderByDescending(u => u.ApplicableSystem)
                .FirstOrDefault();
            return string.IsNullOrEmpty(coherentUnit.Key)
                ? none
                : ZString.Concat(quantity.UnitName, '.', coherentUnit.GetUnitName(data.Units.Values));
        }

        if (quantity.IsNoDimensions)
        {
            var memberName = allUnits
                .OrderBy(u => u.DistanceToDefault)
                .ThenByDescending(u => u.ApplicableSystem)
                .First().GetUnitName(data.Units.Values);
            return ZString.Concat(quantity.UnitName, '.', memberName);
        }

        var dimension = data.Dimensions[quantity.Dimension];
        var systemConversion = Helpers.ToSystemConversion(unitSystem, dimension);
        if (systemConversion is not null)
        {
            var conversion = systemConversion.Value;
            var multiplier = Helpers.ToDecimal(conversion.Multiplier);
            var offset = Helpers.ToDecimal(conversion.Offset);
            var choiceUnit = allUnits
                .Where(u =>
                {
                    if (Math.Abs(Helpers.ToDecimal(u.Conversion.Multiplier) - multiplier) > 1e-9m)
                    {
                        return false;
                    }

                    if (Math.Abs(Helpers.ToDecimal(u.Conversion.Offset) - offset) > 1e-9m)
                    {
                        return false;
                    }

                    return true;
                })
                .OrderBy(u => u.DistanceToDefault)
                .ThenByDescending(u => u.ApplicableSystem)
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(choiceUnit.Name))
            {
                return ZString.Concat(quantity.UnitName, '.', choiceUnit.GetUnitName(data.Units.Values));
            }
        }

        return none;
    }

    private SwitchStatement CreateSwitchStatement(
        Action<Unit, SwitchSection> getStatements,
        Action<SwitchSection> addCoherentCase)
    {
        var statement = new SwitchStatement("unit".ToSimpleName());

        if (precompiled)
        {
            var coherentCase = new SwitchSection()
                .AddLabel(new SwitchLabel(ZString.Concat(quantity.UnitName, ".None").ToSimpleName()));
            addCoherentCase(coherentCase);
            statement.AddSection(coherentCase);
        }

        foreach (var quantityUnit in quantity.Units)
        {
            var unit = data.Units[quantityUnit];

            var section = new SwitchSection()
                .AddLabel(new SwitchLabel(ZString
                    .Concat(quantity.UnitName, '.',
                        unit.GetUnitName(data.Units.Values))
                    .ToSimpleName()));

            getStatements(unit, section);
            statement.AddSection(section);
        }

        statement.AddSection(new SwitchSection()
            .AddLabel(new SwitchLabel())
            .AddStatement(new ObjectCreationExpression(DataType.FromType<ArgumentOutOfRangeException>())
                .AddArgument(Argument("unit".ToLiteral()))
                .AddArgument(Argument("unit".ToSimpleName()))
                .AddArgument(Argument(SimpleNameExpression.Null))
                .Throw));

        return statement;
    }
}