// <copyright file="GroupConfiguration.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Configuration.Models;

/// <summary>
/// Represents configuration for an acquisition group.
/// </summary>
public class GroupConfiguration
{
    /// <summary>
    /// Gets or sets the group identifier.
    /// </summary>
    /// <value>The group id.</value>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source identifier assigned to the group.
    /// </summary>
    /// <value>The source id.</value>
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the trigger identifier assigned to the group.
    /// </summary>
    /// <value>The trigger id.</value>
    public string Trigger { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the stale threshold in seconds.
    /// </summary>
    /// <value>The stale threshold in seconds.</value>
    public int? StaleThresholdSeconds { get; set; }
}
