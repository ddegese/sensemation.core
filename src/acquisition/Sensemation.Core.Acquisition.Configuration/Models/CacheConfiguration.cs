// <copyright file="CacheConfiguration.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Configuration.Models;

/// <summary>
/// Represents cache configuration options.
/// </summary>
public class CacheConfiguration
{
    /// <summary>
    /// Gets or sets the base folder for cached data.
    /// </summary>
    /// <value>The base folder path.</value>
    public string BaseFolder { get; set; } = "cache";

    /// <summary>
    /// Gets or sets a value indicating whether caching is enabled.
    /// </summary>
    /// <value><c>true</c> when caching is enabled; otherwise <c>false</c>.</value>
    public bool Enabled { get; set; }

    /// <summary>
    /// Gets or sets the default cache size.
    /// </summary>
    /// <value>The default cache size.</value>
    public int DefaultCacheSize { get; set; } = 50;
}
