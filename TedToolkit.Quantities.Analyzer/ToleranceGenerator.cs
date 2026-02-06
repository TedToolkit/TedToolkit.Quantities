// -----------------------------------------------------------------------
// <copyright file="ToleranceGenerator.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

using Microsoft.CodeAnalysis;

using TedToolkit.Quantities.Data;
using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

using static TedToolkit.RoslynHelper.Generators.SourceComposer;
using static TedToolkit.RoslynHelper.Generators.SourceComposer<
    TedToolkit.Quantities.Analyzer.ToleranceGenerator>;

namespace TedToolkit.Quantities.Analyzer;

/// <summary>
/// Create the tolerance generator.
/// </summary>
/// <param name="quantities">quantities.</param>
/// <param name="isPublic">is public.</param>
/// <param name="typeName">type symbol.</param>
internal sealed class ToleranceGenerator(
    IReadOnlyList<Quantity> quantities,
    bool isPublic,
    ITypeSymbol typeName)
{
    /// <summary>
    /// Generate the code.
    /// </summary>
    /// <param name="context">the context.</param>
    /// <param name="compilations">compilations.</param>
    public void Generate(in SourceProductionContext context, Compilation compilations)
    {
        var declaration = Struct("Tolerance").Partial
            .AddBaseType(new DataType("TedToolkit.Scopes.IScope"))
            .AddMember(Constructor().Public)
            .AddMember(Method("TedToolkit.Scopes.IScope.OnEntry"))
            .AddMember(Method("TedToolkit.Scopes.IScope.OnExit"));
        declaration = isPublic ? declaration.Public : declaration.Internal;

        declaration.AddMember(Property(new DataType("Tolerance".ToSimpleName()).RefReadonly, "CurrentOrDefault").Public
            .Static
            .AddAccessor(Accessor(AccessorType.GET)
                .AddStatement("global::TedToolkit.Scopes.ScopeValues.Struct<Tolerance>.HasCurrent".ToSimpleName().If
                    .AddStatement("global::TedToolkit.Scopes.ScopeValues.Struct<Tolerance>.Current".ToSimpleName().Ref
                        .Return))
                .AddStatement("_default".ToSimpleName().Ref.Return)));

        if (compilations.GetTypeByMetadataName("TedToolkit.Quantities.Tolerance") is not { } tolerance
            || !tolerance.GetMembers("_default").Any())
        {
            declaration.AddMember(Field(new DataType("Tolerance".ToSimpleName()), "_default").Private.Static.Readonly
                .AddDefault(new ObjectCreationExpression()));
        }

        foreach (var quantityName in quantities.Select(i => i.Name))
        {
            var quantityType = new DataType(quantityName);

            declaration
#pragma warning disable CS8620
                .AddBaseType(new DataType("System.Collections.Generic.IEqualityComparer".ToSimpleName()
                    .Generic(quantityType)))
                .AddBaseType(new DataType("System.Collections.Generic.IComparer".ToSimpleName()
                    .Generic(quantityType)))
#pragma warning restore CS8620
                .AddMember(CreateToleranceProperty(quantityName))
                .AddMember(Method("Equals", new(DataType.Bool)).Public
                    .AddParameter(Parameter(quantityType, "left"))
                    .AddParameter(Parameter(quantityType, "right"))
                    .AddStatement("global::System.Math.Abs".ToSimpleName().Invoke()
                        .AddArgument(Argument("left".ToSimpleName().Sub("Value")
                            .Operator("-", "right".ToSimpleName().Sub("Value"))))
                        .Operator("<", quantityName.ToSimpleName().Sub("Value")).Return))
                .AddMember(Method("GetHashCode", new(DataType.Int)).Public
                    .AddParameter(Parameter(quantityType, "obj"))
                    .AddStatement(0.ToLiteral().Return))
                .AddMember(Method("Compare", new(DataType.Int)).Public
                    .AddParameter(Parameter(quantityType, "left"))
                    .AddParameter(Parameter(quantityType, "right"))
                    .AddStatement("Equals".ToSimpleName().Invoke()
                        .AddArgument(Argument("left".ToSimpleName()))
                        .AddArgument(Argument("right".ToSimpleName())).If
                        .AddStatement(0.ToLiteral().Return))
                    .AddStatement("left.Value.CompareTo(right.Value)".ToSimpleName().Return))
                .AddMember(ImplicitConversionTo(quantityType).ScopedIn
                    .AddStatement(ZString.Concat("value.", quantityName).ToSimpleName().Return));
        }

        File().AddNameSpace(NameSpace("TedToolkit.Quantities")
                .AddMember(declaration))
            .Generate(context, "_Tolerance");
    }

    private Property CreateToleranceProperty(string name)
    {
        return Property(new DataType(name), name).Public
            .AddAccessor(new Accessor(AccessorType.GET))
            .AddAccessor(new Accessor(AccessorType.INIT))
            .AddDefault((typeName.IsFloatingPoint() ? "1E-6" : "1").ToSimpleName().Cast(new DataType(name)));
    }
}