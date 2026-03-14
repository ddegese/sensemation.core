// <copyright file="ItemDatatypeBoolTests.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections;

using Microsoft.Extensions.Logging.Abstractions;

using Sensemation.Core.Acquisition.Abstractions.Enums;
using Sensemation.Core.Acquisition.Runtime.Converters;
using Sensemation.Core.Acquisition.Runtime.Models;
using Sensemation.Core.Acquisition.Runtime.Services;
using Sensemation.Core.Acquisition.Source.Memory;
using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.UnitTests.Runtime;

#pragma warning disable SA1101, SA1202, SA1600, SA1139, CA1707, CA1861, CS1591, IDE0009

public sealed class ItemDatatypeBoolTests : IDisposable
{
    private static readonly bool[] BoolArraySample = [true, false, true];
    private static readonly bool[] BoolArraySampleAlt = [true, false, true, false];
    private static readonly object[] BoolArrayObjectSample = [true, false, 1, 0];

    private readonly UpdateEventDispatcher dispatcher;
    private readonly MemorySource memorySource;

    public ItemDatatypeBoolTests()
    {
        this.dispatcher = new UpdateEventDispatcher(new NullLogger<UpdateEventDispatcher>());
        this.memorySource = new MemorySource(new NullLogger<MemorySource>(), "memory", []);
        this.memorySource.Initialize();
    }

    public void Dispose()
    {
        this.memorySource.Dispose();
        this.dispatcher.Dispose();
    }

    private ItemSource CreateItem(ItemType datatype)
    {
        return new ItemSource(
            new NullLogger<ItemSource>(),
            this.dispatcher,
            new DefaultValueConverter())
        {
            Id = "tag1",
            SourceAddress = "tag1",
            Datatype = datatype,
            Source = this.memorySource,
        };
    }

    private static async Task<DataPoint> WriteValueAsync(ItemSource item, object value)
    {
        var dataPoint = await item.WriteValueAsync(value).ConfigureAwait(false);
        await Task.Delay(30).ConfigureAwait(false);
        return dataPoint;
    }

    private static void AssertCacheValue(ItemSource item, object? expected)
    {
        var cachedValue = item.LatestDataPoint.Value;

        if (expected is Array expectedArray && cachedValue is Array cachedArray)
        {
            Assert.True(StructuralComparisons.StructuralEqualityComparer.Equals(expectedArray, cachedArray));
            return;
        }

        if (expected is null)
        {
            Assert.Null(cachedValue);
            return;
        }

        Assert.Equal(expected, cachedValue);
    }

    [Fact]
    public async Task WriteValueAsyncBoolValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Bool);

        var dataPoint = await WriteValueAsync(item, true).ConfigureAwait(true);

        Assert.Equal(Quality.Good, dataPoint.Quality);
        Assert.True((bool)dataPoint.Value!);
        AssertCacheValue(item, true);
    }

    [Fact]
    public async Task WriteValueAsyncBoolFromStringConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Bool);

        var dataPoint = await WriteValueAsync(item, "true").ConfigureAwait(true);
        Assert.True((bool)dataPoint.Value!);

        dataPoint = await WriteValueAsync(item, "false").ConfigureAwait(true);
        Assert.False((bool)dataPoint.Value!);
    }

    [Fact]
    public async Task WriteValueAsyncBoolFromNumericStringConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Bool);

        var dataPoint = await WriteValueAsync(item, "1").ConfigureAwait(true);
        Assert.True((bool)dataPoint.Value!);

        dataPoint = await WriteValueAsync(item, "0").ConfigureAwait(true);
        Assert.False((bool)dataPoint.Value!);
    }

    [Fact]
    public async Task WriteValueAsyncBoolFromOtherTypesConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Bool);

        var dataPoint = await WriteValueAsync(item, 1).ConfigureAwait(true);
        Assert.True((bool)dataPoint.Value!);

        dataPoint = await WriteValueAsync(item, 0).ConfigureAwait(true);
        Assert.False((bool)dataPoint.Value!);

        dataPoint = await WriteValueAsync(item, 1.0f).ConfigureAwait(true);
        Assert.True((bool)dataPoint.Value!);

        dataPoint = await WriteValueAsync(item, 0.0).ConfigureAwait(true);
        Assert.False((bool)dataPoint.Value!);
    }

    [Fact]
    public async Task WriteValueAsyncBoolArrayValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.BoolArray);

        var dataPoint = await WriteValueAsync(item, BoolArraySample).ConfigureAwait(true);
        AssertCacheValue(item, BoolArraySample);
        Assert.Equal(BoolArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncBoolArrayFromStringConvertsCorrectly()
    {
        var item = CreateItem(ItemType.BoolArray);

        var dataPoint = await WriteValueAsync(item, "[true, false, true]").ConfigureAwait(true);
        Assert.Equal(BoolArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncBoolArrayFromOtherTypesConvertsCorrectly()
    {
        var item = CreateItem(ItemType.BoolArray);

        var dataPoint = await WriteValueAsync(item, BoolArrayObjectSample).ConfigureAwait(true);
        Assert.Equal(BoolArraySampleAlt, dataPoint.Value);
    }

    [Fact]
    public void DataPointEqualityOperatorWithBoolReturnsTrueForEqualValues()
    {
        var left = new DataPoint(DateTime.UtcNow, true, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), true, Quality.Good);

        Assert.True(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithBoolReturnsFalseForDifferentValues()
    {
        var left = new DataPoint(DateTime.UtcNow, true, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), false, Quality.Good);

        Assert.False(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithBoolArrayReturnsTrueForEqualValues()
    {
        var left = new DataPoint(DateTime.UtcNow, new bool[] { true, false, true }, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), new bool[] { true, false, true }, Quality.Good);

        Assert.True(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithBoolArrayReturnsFalseForDifferentValues()
    {
        var left = new DataPoint(DateTime.UtcNow, new bool[] { true, false, true }, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), new bool[] { true, false }, Quality.Good);

        Assert.False(left == right);
    }

    [Fact]
    public void ConvertBoolFromInvalidStringThrows()
    {
        var converter = new DefaultValueConverter();

        var exception = Assert.Throws<InvalidCastException>(() => converter.Convert("notabool", ItemType.Bool));
        Assert.Contains("Cannot convert string", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ConvertBoolFromWhitespaceStringThrows()
    {
        var converter = new DefaultValueConverter();

        var exception = Assert.Throws<InvalidCastException>(() => converter.Convert("   ", ItemType.Bool));
        Assert.Contains("Cannot convert string", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ConvertArrayFromInvalidJsonThrows()
    {
        var converter = new DefaultValueConverter();

        _ = Assert.Throws<InvalidCastException>(() => converter.Convert("[invalid json", ItemType.BoolArray));
    }

    [Fact]
    public void ConvertArrayFromNonArrayObjectThrows()
    {
        var converter = new DefaultValueConverter();

        _ = Assert.Throws<InvalidCastException>(() => converter.Convert("not an array", ItemType.BoolArray));
    }
}

#pragma warning restore SA1101, SA1202, SA1600, SA1139, CA1707, CA1861, CS1591, IDE0009
