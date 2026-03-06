// <copyright file="PluginDescriptor.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.PluginModel.Models;

/// <summary>
/// Describes a discovered plugin and its metadata.
/// </summary>
public sealed class PluginDescriptor
{
    /// <summary>
    /// Gets or sets the plugin type.
    /// </summary>
    /// <value>The plugin type.</value>
    public required Type PluginType { get; set; }

    /// <summary>
    /// Gets or sets the plugin kind (Adapter, Source, Trigger).
    /// </summary>
    /// <value>The plugin kind.</value>
    public required string Kind { get; set; }

    /// <summary>
    /// Gets or sets the plugin identifier.
    /// </summary>
    /// <value>The plugin identifier.</value>
    public required string Id { get; set; }

    /// <summary>
    /// Gets or sets the plugin version.
    /// </summary>
    /// <value>The plugin version.</value>
    public Version Version { get; set; } = new(1, 0, 0, 0);
}
