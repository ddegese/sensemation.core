// <copyright file="PluginLoadOptions.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace Sensemation.Core.Acquisition.PluginModel.Models;

/// <summary>
/// Represents options for plugin discovery and loading.
/// </summary>
public sealed class PluginLoadOptions
{
    /// <summary>
    /// Gets the directories to scan for plugin assemblies.
    /// </summary>
    /// <value>The scan directories.</value>
    public Collection<string> ScanDirectories { get; } = [];

    /// <summary>
    /// Gets the plugin assemblies to load.
    /// </summary>
    /// <value>The assemblies to load.</value>
    public Collection<string> Assemblies { get; } = [];
}
