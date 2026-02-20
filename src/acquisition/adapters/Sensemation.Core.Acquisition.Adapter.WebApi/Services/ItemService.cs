// <copyright file="ItemService.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Sensemation.Core.Acquisition.Abstractions.Interfaces;
using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.Adapter.WebApi.Services;

/// <summary>
/// Provides in-memory access to value accessors for the web API.
/// </summary>
public class ItemService : IItemService
{
    private readonly Dictionary<string, IValueAccessor> valueAccessorsById = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemService"/> class.
    /// </summary>
    /// <param name="valueAccessors">The value accessors.</param>
    public ItemService(Collection<IValueAccessor> valueAccessors)
    {
        ArgumentNullException.ThrowIfNull(valueAccessors);

        foreach (var valueAccessor in valueAccessors)
        {
            this.valueAccessorsById[valueAccessor.Id] = valueAccessor;
        }
    }

    /// <inheritdoc />
    public ReadOnlyCollection<string> GetAllItemIds()
    {
        return this.valueAccessorsById.Keys.ToList().AsReadOnly();
    }

    /// <inheritdoc />
    public bool TryGetLatestDataPoint(string itemId, out DataPoint? dataPoint)
    {
        if (!this.valueAccessorsById.TryGetValue(itemId, out var valueAccessor))
        {
            dataPoint = null;
            return false;
        }

        dataPoint = valueAccessor.LatestDataPoint;
        return true;
    }

    /// <inheritdoc />
    public bool TryGetHistoryValues(string itemId, int? count, out ReadOnlyCollection<DataPoint> dataPoints)
    {
        if (!this.valueAccessorsById.TryGetValue(itemId, out var valueAccessor))
        {
            dataPoints = Array.Empty<DataPoint>().ToList().AsReadOnly();
            return false;
        }

        dataPoints = valueAccessor.GetHistoryValues(count);
        return true;
    }

    /// <inheritdoc />
    public async Task<(bool Success, DataPoint? DataPoint)> TryWriteValueAsync(string itemId, object? value)
    {
        if (!this.valueAccessorsById.TryGetValue(itemId, out var valueAccessor) ||
            valueAccessor is null ||
            value is null)
        {
            return (false, null);
        }

        try
        {
            var dataPoint = await valueAccessor.WriteValueAsync(value).ConfigureAwait(false);
            return (true, dataPoint);
        }
        catch
        {
            return (false, null);
        }
    }
}
