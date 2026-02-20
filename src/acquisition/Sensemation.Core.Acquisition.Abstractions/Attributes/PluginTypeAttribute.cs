// <copyright file="PluginTypeAttribute.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Abstractions.Attributes;

/// <summary>
/// Identifies the plugin type metadata for discovery.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PluginTypeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PluginTypeAttribute"/> class.
    /// </summary>
    /// <param name="id">The plugin type identifier.</param>
    public PluginTypeAttribute(string id) => this.Id = id;

    /// <summary>
    /// Gets the plugin type identifier.
    /// </summary>
    /// <value>The plugin type id.</value>
    public string Id { get; }
}
