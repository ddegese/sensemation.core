// <copyright file="IItemService.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.Adapter.WebApi.Services;

/// <summary>
/// Provides access to item values for the web API.
/// </summary>
public interface IItemService
{
    /// <summary>
    /// Gets all available item identifiers.
    /// </summary>
    /// <returns>The item identifiers.</returns>
    public ReadOnlyCollection<string> GetAllItemIds();

    /// <summary>
    /// Attempts to get the latest datapoint for an item.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="dataPoint">The latest datapoint.</param>
    /// <returns><c>true</c> if the item exists; otherwise, <c>false</c>.</returns>
    public bool TryGetLatestDataPoint(string itemId, out DataPoint? dataPoint);

    /// <summary>
    /// Attempts to get historical datapoints for an item.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="count">The maximum number of values to return.</param>
    /// <param name="dataPoints">The datapoint history.</param>
    /// <returns><c>true</c> if the item exists; otherwise, <c>false</c>.</returns>
    public bool TryGetHistoryValues(string itemId, int? count, out ReadOnlyCollection<DataPoint> dataPoints);

    /// <summary>
    /// Attempts to write a value to an item.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="value">The value to write.</param>
    /// <returns>
    /// A tuple indicating whether the write succeeded along with the resulting datapoint when available.
    /// </returns>
    public Task<(bool Success, DataPoint? DataPoint)> TryWriteValueAsync(string itemId, object? value);
}
