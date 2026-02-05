// -----------------------------------------------------------------------
// <copyright file="ToleranceGenerator.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using TedToolkit.Quantities.Data;
using TedToolkit.RoslynHelper.Extensions;
using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;
using TedToolkit.RoslynHelper.Names;

using static TedToolkit.RoslynHelper.Generators.SourceComposer;
using static TedToolkit.RoslynHelper.Generators.SourceComposer<
    TedToolkit.Quantities.Analyzer.ToleranceGenerator>;

namespace TedToolkit.Quantities.Analyzer;

internal sealed class ToleranceGenerator(
    UnitSystem unitSystem,
    IReadOnlyList<Quantity> quantities,
    bool isPublic,
    TypeName typeName)
{
    public void Generate(SourceProductionContext context)
    {
        var declaration = Struct("Tolerance").Partial
            .AddBaseType(new DataType("TedToolkit.Scopes.IScope"));
        declaration = isPublic ? declaration.Public : declaration.Internal;

        foreach (var quantity in quantities)
        {
            var quantityType = new DataType(quantity.Name);
            declaration
                .AddBaseType(new DataType("System.Collections.Generic.IEqualityComparer".ToSimpleName()
                    .Generic(quantityType)))
                .AddBaseType(new DataType("System.Collections.Generic.IComparer".ToSimpleName()
                    .Generic(quantityType)))
                .AddMember(CreateToleranceProperty(quantity.Name));
        }

        File().AddNameSpace(NameSpace("TedToolkit.Quantities")
                .AddMember(declaration))
            .Generate(context, "_Tolerance");

        return;

        var nameSpace = NamespaceDeclaration("TedToolkit.Quantities")
            .WithMembers([
                ClassDeclaration("Tolerance")
                    .WithModifiers(TokenList(Token(isPublic ? SyntaxKind.PublicKeyword : SyntaxKind.InternalKeyword),
                        Token(SyntaxKind.PartialKeyword)))
                    .WithXmlComment()
                    .WithMembers([
                        ConstructorDeclaration(Identifier("Tolerance"))
                            .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                            .WithAttributeLists([GeneratedCodeAttribute(typeof(ToleranceGenerator))])
                            .WithBody(Block()),
                        ..quantities.Select(q => CreateToleranceProperty(q.Name)),
                        ..quantities.Select(q =>
                            MethodDeclaration(PredefinedType(Token(SyntaxKind.BoolKeyword)), Identifier("Equals"))
                                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                .WithAttributeLists([GeneratedCodeAttribute(typeof(ToleranceGenerator))])
                                .WithParameterList(ParameterList(
                                [
                                    Parameter(Identifier("x")).WithType(IdentifierName(q.Name)),
                                    Parameter(Identifier("y")).WithType(IdentifierName(q.Name))
                                ]))
                                .WithBody(Block(
                                    ReturnStatement(BinaryExpression(SyntaxKind.LessThanExpression,
                                        CastExpression(
                                            IdentifierName(typeName.FullName), InvocationExpression(
                                                    MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        IdentifierName("global::System.Math"), IdentifierName("Abs")))
                                                .WithArgumentList(ArgumentList(
                                                [
                                                    Argument(BinaryExpression(SyntaxKind.SubtractExpression,
                                                        MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                                            IdentifierName("x"), IdentifierName("Value")),
                                                        MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                                            IdentifierName("y"), IdentifierName("Value"))))
                                                ]))),
                                        MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName(q.Name), IdentifierName("Value"))))))),
                        ..quantities.Select(q => MethodDeclaration(PredefinedType(Token(SyntaxKind.IntKeyword)),
                                Identifier("GetHashCode"))
                            .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                            .WithAttributeLists([GeneratedCodeAttribute(typeof(ToleranceGenerator))])
                            .WithParameterList(ParameterList(
                            [
                                Parameter(Identifier("obj")).WithType(IdentifierName(q.Name))
                            ]))
                            .WithExpressionBody(ArrowExpressionClause(InvocationExpression(
                                MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                    MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName("obj"), IdentifierName("Value")),
                                    IdentifierName("GetHashCode")))))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))),
                        ..quantities.Select(q =>
                            MethodDeclaration(PredefinedType(Token(SyntaxKind.IntKeyword)), Identifier("Compare"))
                                .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                                .WithAttributeLists([GeneratedCodeAttribute(typeof(ToleranceGenerator))])
                                .WithParameterList(ParameterList(
                                [
                                    Parameter(Identifier("x")).WithType(IdentifierName(q.Name)),
                                    Parameter(Identifier("y")).WithType(IdentifierName(q.Name))
                                ]))
                                .WithBody(Block(
                                    ReturnStatement(ConditionalExpression(InvocationExpression(IdentifierName("Equals"))
                                            .WithArgumentList(ArgumentList(
                                            [
                                                Argument(IdentifierName("x")),
                                                Argument(IdentifierName("y"))
                                            ])),
                                        LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(0)),
                                        InvocationExpression(MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                                    IdentifierName("x"), IdentifierName("Value")),
                                                IdentifierName("CompareTo")))
                                            .WithArgumentList(
                                                ArgumentList(
                                                [
                                                    Argument(MemberAccessExpression(
                                                        SyntaxKind.SimpleMemberAccessExpression,
                                                        IdentifierName("y"),
                                                        IdentifierName("Value")))
                                                ]))))))),
                        ..quantities.Select(q => ConversionOperatorDeclaration(
                                Token(SyntaxKind.ImplicitKeyword),
                                IdentifierName(q.Name))
                            .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword)))
                            .WithAttributeLists([GeneratedCodeAttribute(typeof(ToleranceGenerator))])
                            .WithParameterList(ParameterList(
                            [
                                Parameter(Identifier("tolerance"))
                                    .WithType(IdentifierName("Tolerance"))
                            ]))
                            .WithExpressionBody(ArrowExpressionClause(MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                IdentifierName("tolerance"), IdentifierName(q.Name))))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))),
                    ])
            ]);

        context.AddSource("_Tolerance.g.cs", nameSpace.NodeToString());
    }

    private Property CreateToleranceProperty(string name)
    {
        return Property(new DataType(name), name).Public
            .AddAccessor(new Accessor(AccessorType.GET))
            .AddAccessor(new Accessor(AccessorType.INIT))
            .AddDefault((typeName.Symbol.IsFloatingPoint() ? "1E-6" : "1").ToSimpleName());
    }
}