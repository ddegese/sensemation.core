// <copyright file="RuntimeUpdateTests.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging.Abstractions;

using Sensemation.Core.Acquisition.Abstractions.Enums;
using Sensemation.Core.Acquisition.Runtime.Converters;
using Sensemation.Core.Acquisition.Runtime.Models;
using Sensemation.Core.Acquisition.Runtime.Services;
using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.UnitTests.Runtime;

/// <summary>
/// Tests for runtime update behavior.
/// </summary>
public class RuntimeUpdateTests
{
    /// <summary>
    /// Ensures updates refresh cache and last updated timestamp.
    /// </summary>
    /// <returns>A task that represents the asynchronous test.</returns>
    [Fact]
    public async Task UpdateEventDispatcherShouldUpdateCacheAndLastUpdated()
    {
        using var dispatcher = new UpdateEventDispatcher(new NullLogger<UpdateEventDispatcher>());
        var item = new ItemSource(new NullLogger<ItemSource>(), dispatcher, new DefaultValueConverter())
        {
            Id = "tag1",
            Datatype = ItemType.Integer32,
            CacheSize = 5,
            SourceAddress = "tag1",
        };

        var dataPoint = new DataPoint(DateTime.UtcNow, 10, Quality.Good);
        var itemUpdateEvent = new ItemUpdateEvent(item.Id, item, dataPoint);
        _ = dispatcher.Enqueue(itemUpdateEvent);

        await Task.Delay(50);

        Assert.Equal(10, item.LatestDataPoint?.Value);
        _ = Assert.NotNull(item.LatestDataPoint?.TimestampUtc);
    }

    /// <summary>
    /// Ensures duplicate datapoints are ignored.
    /// </summary>
    /// <returns>A task that represents the asynchronous test.</returns>
    [Fact]
    public async Task UpdateEventDispatcherShouldIgnoreDuplicateDataPoint()
    {
        using var dispatcher = new UpdateEventDispatcher(new NullLogger<UpdateEventDispatcher>());
        var item = new ItemSource(new NullLogger<ItemSource>(), dispatcher, new DefaultValueConverter())
        {
            Id = "tag1",
            Datatype = ItemType.Integer32,
            CacheSize = 5,
            SourceAddress = "tag1",
        };

        var dataPoint = new DataPoint(DateTime.UtcNow, 10, Quality.Good);
        var itemUpdateEvent = new ItemUpdateEvent(item.Id, item, dataPoint);
        _ = dispatcher.Enqueue(itemUpdateEvent);
        _ = dispatcher.Enqueue(itemUpdateEvent);

        await Task.Delay(50);

        var history = item.GetHistoryValues();
        _ = Assert.Single(history);
        Assert.Equal(10, item.LatestDataPoint.Value);
    }

    /// <summary>
    /// Ensures cache is trimmed to configured size.
    /// </summary>
    /// <returns>A task that represents the asynchronous test.</returns>
    [Fact]
    public async Task UpdateEventDispatcherShouldTrimCacheToCacheSize()
    {
        using var dispatcher = new UpdateEventDispatcher(new NullLogger<UpdateEventDispatcher>());
        var item = new ItemSource(new NullLogger<ItemSource>(), dispatcher, new DefaultValueConverter())
        {
            Id = "tag1",
            Datatype = ItemType.Integer32,
            CacheSize = 2,
            SourceAddress = "tag1",
        };

        _ = dispatcher.Enqueue(new ItemUpdateEvent(item.Id, item, new DataPoint(DateTime.UtcNow, 10, Quality.Good)));
        _ = dispatcher.Enqueue(new ItemUpdateEvent(item.Id, item, new DataPoint(DateTime.UtcNow.AddSeconds(1), 20, Quality.Good)));
        _ = dispatcher.Enqueue(new ItemUpdateEvent(item.Id, item, new DataPoint(DateTime.UtcNow.AddSeconds(2), 30, Quality.Good)));

        await Task.Delay(50);

        var history = item.GetHistoryValues();
        Assert.Equal(2, history.Count);
        Assert.Equal(30, item.LatestDataPoint.Value);
    }
}
