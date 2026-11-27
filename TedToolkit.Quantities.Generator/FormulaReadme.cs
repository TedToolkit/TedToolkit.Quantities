// -----------------------------------------------------------------------
// <copyright file="FormulaReadme.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.FileSystem;

namespace TedToolkit.Quantities.Generator;

/// <summary>
/// Synchronizes the generated portion of the package README.
/// </summary>
internal static class FormulaReadme
{
    private const string StartMarker = "<!-- BEGIN GENERATED FORMULA COVERAGE -->";

    private const string EndMarker = "<!-- END GENERATED FORMULA COVERAGE -->";

    /// <summary>
    /// Updates or verifies the formula inventory between the README markers.
    /// </summary>
    /// <param name="generatedDirectory">The compiler's generated source directory.</param>
    /// <param name="readmePath">The package README path.</param>
    /// <param name="check">Whether to report stale coverage while leaving the README intact.</param>
    /// <exception cref="InvalidDataException">The generated content, README markers, or checked-in coverage is invalid.</exception>
    /// <exception cref="IOException">The README was modified during regeneration.</exception>
    internal static void Update(string generatedDirectory, string readmePath, bool check)
    {
        var source = new DirectoryPath(generatedDirectory).EnumerateFiles("FormulaCoverage.g.cs", SearchOption.AllDirectories)
            .Single();
        var markdown = source.ReadAllLines().Select(line => line.StartsWith("// ", StringComparison.Ordinal)
            ? line[3..]
            : throw new InvalidDataException("FormulaCoverage.g.cs must contain only Markdown comments."));
        var file = new FilePath(readmePath);
        var readme = file.ReadAllText();
        var start = readme.IndexOf(StartMarker, StringComparison.Ordinal);
        var end = readme.IndexOf(EndMarker, StringComparison.Ordinal);
        if (start < 0 || end < start + StartMarker.Length
            || start != readme.LastIndexOf(StartMarker, StringComparison.Ordinal)
            || end != readme.LastIndexOf(EndMarker, StringComparison.Ordinal))
        {
            throw new InvalidDataException("The README must contain one ordered pair of formula coverage markers.");
        }

        var newline = readme.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
        var updated = readme[..(start + StartMarker.Length)] + newline
            + string.Join(newline, markdown) + newline + readme[end..];
        if (updated == readme)
        {
            return;
        }

        if (check)
        {
            throw new InvalidDataException("Formula coverage is stale. Build the SI.Double project locally to regenerate it.");
        }

        var temporary = new FilePath(readmePath + "." + Guid.NewGuid().ToString("N") + ".tmp");
        try
        {
            temporary.WriteAllText(updated);
            if (file.ReadAllText() != readme)
            {
                throw new IOException("The README changed while regenerating formula coverage. Run the build again.");
            }

            temporary.MoveTo(file, overwrite: true);
        }
        finally
        {
            temporary.Delete();
        }
    }
}