// <copyright file="PluginManagerBase.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Sensemation.Core.Acquisition.PluginModel.Models;

namespace Sensemation.Core.Acquisition.Demo.Console.Services.Plugins;

/// <summary>
/// Base class for managing plugin implementations (adapters and sources).
/// </summary>
/// <typeparam name="TPlugin">The plugin interface type (IAdapter or ISource).</typeparam>
/// <typeparam name="TConfig">The configuration type (AdapterConfiguration or SourceConfiguration).</typeparam>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by DI")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1852:Seal internal types", Justification = "Used by dependency injection")]
internal abstract class PluginManagerBase<TPlugin, TConfig> : IDisposable
    where TPlugin : class, IPlugin
    where TConfig : PluginConfiguration
{
#pragma warning disable IDE0028
    private readonly Dictionary<string, TPlugin> plugins = [];
    private readonly Dictionary<string, Type> pluginTypes = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginManagerBase{TPlugin, TConfig}"/> class.
    /// </summary>
    protected PluginManagerBase()
    {
    }

    /// <summary>
    /// Gets the plugin interface type for filtering assemblies.
    /// </summary>
    /// <value>The plugin interface type.</value>
    protected abstract Type PluginInterfaceType { get; }

    /// <summary>
    /// Gets the assembly search pattern for loading plugin assemblies.
    /// </summary>
    /// <value>The assembly search pattern.</value>
    protected abstract string AssemblySearchPattern { get; }

    /// <summary>
    /// Gets a plugin by its name.
    /// </summary>
    /// <param name="name">The name of the plugin.</param>
    /// <returns>The plugin, or null if not found.</returns>
    public TPlugin? GetPlugin(string name) => this.plugins.TryGetValue(name, out var plugin) ? plugin : null;

    /// <summary>
    /// Gets all plugins.
    /// </summary>
    /// <returns>A dictionary of all plugins.</returns>
    public Dictionary<string, TPlugin> GetAllPlugins() => new(this.plugins);

    /// <summary>
    /// Loads plugin descriptors from the plugin model loader.
    /// </summary>
    /// <param name="pluginDescriptors">The plugin descriptors.</param>
    public void LoadPluginDescriptors(IEnumerable<PluginDescriptor> pluginDescriptors)
    {
        ArgumentNullException.ThrowIfNull(pluginDescriptors);

        foreach (var descriptor in pluginDescriptors)
        {
            var pluginType = descriptor.Id?.Trim();
            if (string.IsNullOrWhiteSpace(pluginType))
            {
                continue;
            }

            if (descriptor.PluginType == null)
            {
                continue;
            }

            if (!this.PluginInterfaceType.IsAssignableFrom(descriptor.PluginType))
            {
                continue;
            }

            this.pluginTypes[pluginType.ToUpperInvariant()] = descriptor.PluginType;
        }
    }

    /// <summary>
    /// Initializes plugins based on the configuration.
    /// </summary>
    /// <param name="pluginConfigs">The plugin configurations.</param>
    /// <param name="serviceProvider">The service provider used for dependency injection when creating plugin instances.</param>
    public void InitializePlugins(Collection<TConfig> pluginConfigs, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(pluginConfigs);

        foreach (var pluginConfig in pluginConfigs)
        {
            var plugin = this.CreatePlugin(pluginConfig, serviceProvider);

            plugin.Initialize();

            this.plugins[this.GetPluginName(plugin)] = plugin;
        }
    }

    /// <summary>
    /// Disposes of all plugins when the manager is disposed.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gets the name of a plugin instance.
    /// </summary>
    /// <param name="plugin">The plugin instance.</param>
    /// <returns>The name of the plugin.</returns>
    protected abstract string GetPluginName(TPlugin plugin);

    /// <summary>
    /// Disposes of all plugins when the manager is disposed.
    /// </summary>
    /// <param name="disposing">True if disposing, false if finalizing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var plugin in this.plugins.Values)
            {
                if (plugin is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            this.plugins.Clear();
        }
    }

    /// <summary>
    /// Creates a trigger based on the configuration.
    /// </summary>
    /// <param name="pluginConfig">The plugin configuration.</param>
    /// <param name="serviceProvider">The service provider used for dependency injection when creating plugin instances.</param>
    /// <returns>The created trigger, or null if it could not be created.</returns>
    private TPlugin CreatePlugin(TConfig pluginConfig, IServiceProvider serviceProvider)
    {
        var type = pluginConfig.Type.ToUpperInvariant();
        if (this.pluginTypes.TryGetValue(type, out var pluginType))
        {
            // Create instance
            var parameters = pluginConfig.Parameters.ToDictionary(p => p.Name, p => p.Value);
            var plugin = (TPlugin)ActivatorUtilities.CreateInstance(serviceProvider, pluginType, pluginConfig.Id, parameters);

            return plugin;
        }
        else
        {
            throw new ArgumentException($"Unsupported plugin type: {pluginConfig.Type}");
        }
    }
}
