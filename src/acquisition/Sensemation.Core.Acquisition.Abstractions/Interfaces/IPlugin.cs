// <copyright file="IPlugin.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Abstractions.Interfaces;

/// <summary>
/// Defines a plugin with initialization semantics.
/// </summary>
public interface IPlugin
{
    /// <summary>
    /// Gets the plugin identifier.
    /// </summary>
    /// <value>The plugin id.</value>
    public string Id { get; }

    /// <summary>
    /// Initializes the plugin.
    /// </summary>
    public void Initialize();
}
