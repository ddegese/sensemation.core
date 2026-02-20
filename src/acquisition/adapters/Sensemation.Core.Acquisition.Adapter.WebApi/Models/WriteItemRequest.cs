// <copyright file="WriteItemRequest.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Adapter.WebApi.Models;

/// <summary>
/// Represents a write request payload for an item.
/// </summary>
public class WriteItemRequest
{
    /// <summary>
    /// Gets or sets the value to write.
    /// </summary>
    /// <value>The value payload.</value>
    public object? Value { get; set; }
}
