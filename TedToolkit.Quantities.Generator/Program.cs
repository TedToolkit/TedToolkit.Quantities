// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Newtonsoft.Json;

using PeterO.Numbers;

using TedToolkit.FileSystem;

using TedToolkit.Quantities.Generator;

using VDS.RDF;
using VDS.RDF.Parsing;

var repository = DirectoryPath.CurrentDirectory;
var quantityFolder = repository / "TedToolkit.Quantities";
var unitFolder = (repository / "TedToolkit.Quantities.Analyzer" / "QUDT").Create();
var source = repository / "externals" / "QUDT" / "QUDT-all-in-one-OWL".AsFileName("ttl");

using var g = new Graph();
var parser = new TurtleParser();
parser.Load(g, source.FullName);

var names = new List<(string Name, string Description)>() { ("ALL", "All quantities."), };
foreach (var uriNode in g.GetTriplesWithPredicateObject(
                 g.CreateUriNode("rdf:type"),
                 g.CreateUriNode("qudt:SystemOfQuantityKinds"))
             .Select(t => t.Subject)
             .OfType<IUriNode>())
{
    var name = uriNode.GetUrlName();
    var desc = uriNode.GetLabels(g)
        .OrderBy(l => l.Language.Length)
        .ThenBy(l => l.Language != "en")
        .FirstOrDefault()?.Value ?? "";
    names.Add((name, desc));
    var data = new QudtAnalyzer(g, uriNode).Analyze();
    await (unitFolder / name.AsFileName("json")).WriteAllTextAsync(
        JsonConvert.SerializeObject(data)).ConfigureAwait(false);
}

var allData = new QudtAnalyzer(g, null).Analyze();
await (unitFolder / "ALL".AsFileName("json")).WriteAllTextAsync(JsonConvert.SerializeObject(allData))
    .ConfigureAwait(false);

QuantitySystemGenerator.GenerateQuantitySystem(quantityFolder, names);