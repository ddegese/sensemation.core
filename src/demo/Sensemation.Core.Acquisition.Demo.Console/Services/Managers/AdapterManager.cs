// <copyright file="AdapterManager.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Demo.Console.Services.Plugins;

namespace Sensemation.Core.Acquisition.Demo.Console.Services.Managers;

/// <summary>
/// Manages the loading and initialization of adapter implementations.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by DI")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1852:Seal internal types", Justification = "Used by dependency injection")]
internal class AdapterManager : PluginManagerBase<IAdapter, AdapterConfiguration>
{
    /// <inheritdoc/>
    protected override Type PluginInterfaceType => typeof(IAdapter);

    /// <inheritdoc/>
    protected override string AssemblySearchPattern => "Sensemation.Core.Acquisition.Adapter.*.dll";

    /// <summary>
    /// Gets an adapter by its name.
    /// </summary>
    /// <param name="name">The name of the adapter.</param>
    /// <returns>The adapter, or null if not found.</returns>
    public IAdapter? GetAdapter(string name) => this.GetPlugin(name);

    /// <summary>
    /// Starts all adapters asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task StartAllAdapters()
    {
        var adapters = this.GetAllPlugins();
        foreach (var adapter in adapters.Values)
        {
            await adapter.StartAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Stops all adapters asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task StopAllAdapters()
    {
        var adapters = this.GetAllPlugins();
        foreach (var adapter in adapters.Values)
        {
            await adapter.StopAsync().ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    protected override string GetPluginName(IAdapter adapter)
    {
        ArgumentNullException.ThrowIfNull(adapter);
        return adapter.Id;
    }
}
