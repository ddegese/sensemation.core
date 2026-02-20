// <copyright file="IItem.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Abstractions.Enums;
using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.Abstractions.Interfaces;

/// <summary>
/// Represents an acquisition item with cached values.
/// </summary>
public interface IItem : IValueRead, IValueListener
{
    /// <summary>
    /// Gets the item identifier.
    /// </summary>
    /// <value>The item id.</value>
    public string Id { get; }

    /// <summary>
    /// Gets the item data type.
    /// </summary>
    /// <value>The item data type.</value>
    public ItemType Datatype { get; }

    /// <summary>
    /// Gets the configured cache size.
    /// </summary>
    /// <value>The cache size.</value>
    public int CacheSize { get; }

    /// <summary>
    /// Gets the latest datapoint.
    /// </summary>
    /// <value>The latest datapoint.</value>
    public DataPoint LatestDataPoint { get; }

    /// <summary>
    /// Adds a value listener to the item.
    /// </summary>
    /// <param name="listener">The listener to add.</param>
    public void AddValueListener(IValueListener listener);
}
