// <copyright file="TriggerConfiguration.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace Sensemation.Core.Acquisition.Configuration.Models;

/// <summary>
/// Represents configuration for a trigger plugin.
/// </summary>
public class TriggerConfiguration
{
    /// <summary>
    /// Gets or sets the trigger identifier.
    /// </summary>
    /// <value>The trigger id.</value>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the trigger type name.
    /// </summary>
    /// <value>The trigger type name.</value>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets the trigger parameters.
    /// </summary>
    /// <value>The parameter list.</value>
    public Collection<ParameterConfiguration> Parameters { get; } = [];
}
