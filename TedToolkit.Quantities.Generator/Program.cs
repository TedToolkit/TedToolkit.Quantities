// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Newtonsoft.Json;

using PeterO.Numbers;

using Sourcy.DotNet;

using TedToolkit.Quantities.Generator;

using VDS.RDF;
using VDS.RDF.Parsing;

var solutionFolder = Solutions.TedToolkit_Quantities.Directory;

if (solutionFolder is null)
    return;

var quantityFolder = solutionFolder.CreateSubdirectory("TedToolkit.Quantities");
var unitFolder = solutionFolder.CreateSubdirectory("TedToolkit.Quantities.Analyzer").CreateSubdirectory("QUDT");
var qudtFolder = solutionFolder.CreateSubdirectory("externals").CreateSubdirectory("QUDT");
var path = Path.Combine(qudtFolder.FullName, "QUDT-all-in-one-OWL.ttl");

using var g = new Graph();
var parser = new TurtleParser();
parser.Load(g, path);

var names = new List<(string Name, string Description)>() { ("ALL", "All quantities.") };
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
    await File.WriteAllTextAsync(Path.Combine(unitFolder.FullName, name + ".json"),
        JsonConvert.SerializeObject(data)).ConfigureAwait(false);
}

var allData = new QudtAnalyzer(g, null).Analyze();
await File.WriteAllTextAsync(Path.Combine(unitFolder.FullName, "ALL.json"), JsonConvert.SerializeObject(allData))
    .ConfigureAwait(false);

QuantitySystemGenerator.GenerateQuantitySystem(quantityFolder.FullName, names);