// <copyright file="ServiceConfiguration.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace Sensemation.Core.Acquisition.Configuration.Models;

/// <summary>
/// Represents configuration for the acquisition service.
/// </summary>
public class ServiceConfiguration
{
    /// <summary>
    /// Gets the adapter configurations.
    /// </summary>
    /// <value>The adapter configurations.</value>
    public Collection<AdapterConfiguration> Adapters { get; } = [];

    /// <summary>
    /// Gets the source configurations.
    /// </summary>
    /// <value>The source configurations.</value>
    public Collection<SourceConfiguration> Sources { get; } = [];

    /// <summary>
    /// Gets the trigger configurations.
    /// </summary>
    /// <value>The trigger configurations.</value>
    public Collection<TriggerConfiguration> Triggers { get; } = [];

    /// <summary>
    /// Gets the group configurations.
    /// </summary>
    /// <value>The group configurations.</value>
    public Collection<GroupConfiguration> Groups { get; } = [];

    /// <summary>
    /// Gets the item configurations.
    /// </summary>
    /// <value>The item configurations.</value>
    public Collection<ItemConfiguration> Items { get; } = [];

    /// <summary>
    /// Gets or sets the logging configuration.
    /// </summary>
    /// <value>The logging configuration.</value>
    public LoggingConfiguration Logging { get; set; } = new LoggingConfiguration();

    /// <summary>
    /// Gets or sets the cache configuration.
    /// </summary>
    /// <value>The cache configuration.</value>
    public CacheConfiguration Cache { get; set; } = new CacheConfiguration();

    /// <summary>
    /// Gets or sets the plugin loading configuration.
    /// </summary>
    /// <value>The plugin loading configuration.</value>
    public PluginLoadConfiguration Plugins { get; set; } = new PluginLoadConfiguration();
}
