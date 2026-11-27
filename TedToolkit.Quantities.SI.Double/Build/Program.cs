// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.Quantities.SI.Double.Build;

switch (args)
{
    case ["--quantity-catalog", var basePath, var catalogPath, var outputPath, .. var selections]:
        QuantityCatalogBuilder.Generate(basePath, catalogPath, outputPath, selections);
        break;

    case ["--formula-readme", var directory, var catalogPath, var sourceListPath, var mode]
        when mode is "check" or "update":
        FormulaReadme.Update(directory, catalogPath, sourceListPath, check: mode == "check");
        break;

    default:
        throw new ArgumentException("Expected --quantity-catalog or --formula-readme with its build arguments.");
}