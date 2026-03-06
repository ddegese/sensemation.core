// <copyright file="PluginLoadConfiguration.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace Sensemation.Core.Acquisition.Configuration.Models;

/// <summary>
/// Represents configuration for plugin discovery and loading.
/// </summary>
public class PluginLoadConfiguration
{
    /// <summary>
    /// Gets the directories to scan for plugins.
    /// </summary>
    /// <value>The scan directories.</value>
    public Collection<string> ScanDirectories { get; } = [];

    /// <summary>
    /// Gets the plugin assemblies to load.
    /// </summary>
    /// <value>The plugin assemblies.</value>
    public Collection<string> Assemblies { get; } = [];
}
