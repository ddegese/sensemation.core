// <copyright file="SignalValue.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Adapter.WebApi.Models;

/// <summary>
/// Represents a signal value returned by the web API.
/// </summary>
public class SignalValue
{
    /// <summary>
    /// Gets or sets the item identifier.
    /// </summary>
    /// <value>The item id.</value>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>The value as text.</value>
    public object? Value { get; set; }

    /// <summary>
    /// Gets or sets the timestamp in UTC.
    /// </summary>
    /// <value>The UTC timestamp.</value>
    public DateTime TimestampUtc { get; set; }

    /// <summary>
    /// Gets or sets the quality indicator.
    /// </summary>
    /// <value>The quality.</value>
    public string? Quality { get; set; }
}
