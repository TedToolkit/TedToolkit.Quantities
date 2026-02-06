// -----------------------------------------------------------------------
// <copyright file="Helpers.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.Quantities.Data;

using VDS.RDF;

namespace TedToolkit.Quantities.Generator;

/// <summary>
/// The helpers.
/// </summary>
internal static class Helpers
{
    /// <summary>
    /// Find the url name.
    /// </summary>
    /// <param name="uriNode">uri node.</param>
    /// <returns>result.</returns>
#pragma warning disable CA1055
    public static string GetUrlName(this IUriNode uriNode)
#pragma warning restore CA1055
    {
        ArgumentNullException.ThrowIfNull(uriNode);
        return uriNode.Uri.AbsolutePath.Split('/')[^1].Replace("_", "", StringComparison.InvariantCulture);
    }

    /// <summary>
    /// Get the description.
    /// </summary>
    /// <param name="quantity">quantity.</param>
    /// <param name="g">graph.</param>
    /// <returns>result.</returns>
    public static string GetDescription(this IUriNode quantity, Graph g)
    {
        return GetString("qudt:plainTextDescription")
               ?? GetString("dcterms:description")
               ?? "Nothing";

        string? GetString(string predicate)
        {
            return g.GetTriplesWithSubjectPredicate(quantity,
                    g.CreateUriNode(predicate))
                .Select(t => t.Object)
                .OfType<ILiteralNode>()
                .FirstOrDefault()?.Value;
        }
    }

    /// <summary>
    /// Get the links.
    /// </summary>
    /// <param name="quantity">quantity.</param>
    /// <param name="g">graph.</param>
    /// <returns>links.</returns>
    public static IReadOnlyList<Link> GetLinks(this IUriNode quantity, Graph g)
    {
        var links = new List<Link>();
        AddLinks("qudt:dbpediaMatch", "DBpedia");
        AddLinks("qudt:informativeReference", "Link");
        AddLinks("qudt:isoNormativeReference", "ISO");
        AddLinks("qudt:wikidataMatch", "WikiData");

        return links;

        void AddLinks(string predicate, string name)
        {
            links.AddRange(g.GetTriplesWithSubjectPredicate(quantity, g.CreateUriNode(predicate))
                .Select(n => n.Object)
                .OfType<ILiteralNode>()
                .Select(literalNode => new Link(name, literalNode.Value)));
        }
    }

    /// <summary>
    /// Change the label to name.
    /// </summary>
    /// <param name="label">label.</param>
    /// <returns>result.</returns>
    public static string LabelToName(this string label)
    {
        ArgumentNullException.ThrowIfNull(label);
        return string.Join(null, label.Split(' ')
            .Select(i => char.ToUpperInvariant(i[0]) + i[1..])
            .Select(s => s
                .Replace("\'", "", StringComparison.InvariantCulture)
                .Replace("?", "", StringComparison.InvariantCulture)));
    }

    /// <summary>
    /// Get the labels.
    /// </summary>
    /// <param name="node">node.</param>
    /// <param name="g">graph.</param>
    /// <returns>result.</returns>
    public static IEnumerable<ILiteralNode> GetLabels(this IUriNode node, Graph g)
    {
        return g.GetTriplesWithSubjectPredicate(node, g.CreateUriNode("rdfs:label"))
            .Select(t => t.Object)
            .OfType<ILiteralNode>();
    }

    /// <summary>
    /// Get the property.
    /// </summary>
    /// <param name="node">node.</param>
    /// <param name="g">graph.</param>
    /// <param name="predicateName">predicate name.</param>
    /// <typeparam name="TNode">node type.</typeparam>
    /// <returns>result.</returns>
    public static IEnumerable<TNode> GetProperty<TNode>(this INode node, Graph g, string predicateName)
        where TNode : INode
    {
        return g.GetTriplesWithSubjectPredicate(node,
                g.CreateUriNode(predicateName))
            .Select(n => n.Object)
            .OfType<TNode>();
    }
}