// <copyright file="MemorySourceTests.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging.Abstractions;

using Sensemation.Core.Acquisition.Abstractions.Enums;
using Sensemation.Core.Acquisition.Runtime.Converters;
using Sensemation.Core.Acquisition.Runtime.Models;
using Sensemation.Core.Acquisition.Runtime.Services;
using Sensemation.Core.Acquisition.Source.Memory;

namespace Sensemation.Core.Acquisition.UnitTests.Sources;

/// <summary>
/// Tests for the in-memory source implementation.
/// </summary>
public class MemorySourceTests
{
    /// <summary>
    /// Ensures writes are reflected on subsequent reads.
    /// </summary>
    /// <returns>A task that represents the asynchronous test.</returns>
    [Fact]
    public async Task WriteThenReadShouldReturnUpdatedValue()
    {
        using var dispatcher = new UpdateEventDispatcher(new NullLogger<UpdateEventDispatcher>());
        var source = new MemorySource(new NullLogger<MemorySource>(), "memory", []);
        source.Initialize();

        var item = new ItemSource(new NullLogger<ItemSource>(), dispatcher, new DefaultValueConverter())
        {
            Id = "tag1",
            Datatype = ItemType.Integer32,
            CacheSize = 5,
            SourceAddress = "tag1",
            Source = source,
        };

        _ = await item.WriteValueAsync(123);
        System.Collections.ObjectModel.Collection<Abstractions.Interfaces.IItemSource> items = [item];

        var results = await source.ReadAsync(items);

        Assert.True(results.ContainsKey("tag1"));
        Assert.Equal(123, results["tag1"].Value);
    }
}
