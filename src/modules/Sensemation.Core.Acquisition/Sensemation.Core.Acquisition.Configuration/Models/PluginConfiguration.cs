// <copyright file="PluginConfiguration.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace Sensemation.Core.Acquisition.Configuration.Models;

/// <summary>
/// Represents configuration for a generic plugin.
/// </summary>
public class PluginConfiguration
{
    /// <summary>
    /// Gets or sets the plugin identifier.
    /// </summary>
    /// <value>The plugin id.</value>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the plugin type name.
    /// </summary>
    /// <value>The plugin type name.</value>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets the plugin parameters.
    /// </summary>
    /// <value>The parameter list.</value>
    public Collection<ParameterConfiguration> Parameters { get; } = [];
}
