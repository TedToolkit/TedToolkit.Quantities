using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;

using Sourcy.DotNet;

using TedToolkit.ModularPipelines.Modules;

namespace Build;

/// <summary>
/// Run the console.
/// </summary>
public sealed class RunConsoleModule : PrepareModule<CommandResult>
{
    /// <inheritdoc />
    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context,
        CancellationToken cancellationToken)
    {
        return await context.DotNet()
            .Run(new DotNetRunOptions() { Project = Projects.TedToolkit_Quantities_Generator.FullName, },
                cancellationToken: cancellationToken);
    }
}