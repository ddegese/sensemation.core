// <copyright file="ISource.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.Abstractions.Interfaces;

/// <summary>
/// Defines a source that can read and write datapoints.
/// </summary>
public interface ISource : IPlugin
{
    /// <summary>
    /// Reads the specified item sources.
    /// </summary>
    /// <param name="items">The item sources to read.</param>
    /// <returns>A mapping of item id to datapoint.</returns>
    public Task<Dictionary<string, DataPoint>> ReadAsync(Collection<IItemSource> items);

    /// <summary>
    /// Writes a value to the specified item source.
    /// </summary>
    /// <param name="item">The item source to write to.</param>
    /// <param name="value">The value payload.</param>
    /// <returns>The resulting datapoint.</returns>
    public Task<DataPoint> WriteAsync(IItemSource item, object value);
}
