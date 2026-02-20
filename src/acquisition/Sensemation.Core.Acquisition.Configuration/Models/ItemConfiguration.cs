// <copyright file="ItemConfiguration.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace Sensemation.Core.Acquisition.Configuration.Models;

/// <summary>
/// Represents configuration for an acquisition item.
/// </summary>
public class ItemConfiguration
{
    /// <summary>
    /// Gets or sets the source address for the item.
    /// </summary>
    /// <value>The source address.</value>
    public string SourceAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional item identifier.
    /// </summary>
    /// <value>The item id.</value>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the group identifier for the item.
    /// </summary>
    /// <value>The group id.</value>
    public string Group { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the datatype name.
    /// </summary>
    /// <value>The datatype name.</value>
    public string Datatype { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the cache size override for the item.
    /// </summary>
    /// <value>The cache size.</value>
    public int? CacheSize { get; set; }

    /// <summary>
    /// Gets the adapter names associated with the item.
    /// </summary>
    /// <value>The adapter list.</value>
    public Collection<string> Adapters { get; } = [];
}
