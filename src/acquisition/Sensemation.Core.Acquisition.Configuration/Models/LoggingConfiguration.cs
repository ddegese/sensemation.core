// <copyright file="LoggingConfiguration.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Configuration.Models;

/// <summary>
/// Represents logging configuration options.
/// </summary>
public class LoggingConfiguration
{
    /// <summary>
    /// Gets or sets a value indicating whether logging is enabled.
    /// </summary>
    /// <value><c>true</c> when logging is enabled; otherwise <c>false</c>.</value>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the minimum log level.
    /// </summary>
    /// <value>The minimum log level.</value>
    public string MinimumLevel { get; set; } = "Information";

    /// <summary>
    /// Gets or sets a value indicating whether to include stack traces.
    /// </summary>
    /// <value><c>true</c> to include stack traces; otherwise <c>false</c>.</value>
    public bool IncludeStackTraces { get; set; } = true;
}
