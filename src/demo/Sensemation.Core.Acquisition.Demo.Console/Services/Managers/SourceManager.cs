// <copyright file="SourceManager.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Demo.Console.Services.Plugins;

namespace Sensemation.Core.Acquisition.Demo.Console.Services.Managers;

/// <summary>
/// Manages the loading and initialization of source implementations.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by DI")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1852:Seal internal types", Justification = "Used by dependency injection")]
internal class SourceManager : PluginManagerBase<ISource, SourceConfiguration>
{
    /// <inheritdoc/>
    protected override Type PluginInterfaceType => typeof(ISource);

    /// <inheritdoc/>
    protected override string AssemblySearchPattern => "Sensemation.Core.Acquisition.Source.*.dll";

    /// <summary>
    /// Gets a source by its name.
    /// </summary>
    /// <param name="name">The name of the source.</param>
    /// <returns>The source, or null if not found.</returns>
    public ISource? GetSource(string name) => this.GetPlugin(name);

    /// <inheritdoc/>
    protected override string GetPluginName(ISource source)
    {
        ArgumentNullException.ThrowIfNull(source);
        return source.Id;
    }
}
