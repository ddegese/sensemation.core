// <copyright file="ItemDatatypeInteger32Tests.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging.Abstractions;

using Sensemation.Core.Acquisition.Abstractions.Enums;
using Sensemation.Core.Acquisition.Runtime.Converters;
using Sensemation.Core.Acquisition.Runtime.Models;
using Sensemation.Core.Acquisition.Runtime.Services;
using Sensemation.Core.Acquisition.Source.Memory;
using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.UnitTests.Runtime;

#pragma warning disable SA1101, SA1202, SA1600, SA1139, CA1707, CA1861, CS1591, IDE0009

public sealed class ItemDatatypeInteger32Tests : IDisposable
{
    private static readonly int[] IntArraySample = [-1, 0, 1, 2147483647, -2147483648];
    private static readonly uint[] UIntArraySample = [0u, 1u, 4294967295u];

    private readonly UpdateEventDispatcher dispatcher;
    private readonly MemorySource memorySource;

    public ItemDatatypeInteger32Tests()
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

    [Fact]
    public async Task WriteValueAsyncIntValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer32);

        var dataPoint = await WriteValueAsync(item, 789).ConfigureAwait(true);
        Assert.Equal(789, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncIntBoundaryValuesConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer32);

        var dataPoint = await WriteValueAsync(item, int.MinValue).ConfigureAwait(true);
        Assert.Equal(int.MinValue, dataPoint.Value);

        dataPoint = await WriteValueAsync(item, int.MaxValue).ConfigureAwait(true);
        Assert.Equal(int.MaxValue, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncUIntValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.UnsignedInteger32);

        var dataPoint = await WriteValueAsync(item, (uint)789).ConfigureAwait(true);
        Assert.Equal((uint)789, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncUIntBoundaryValuesConvertsCorrectly()
    {
        var item = CreateItem(ItemType.UnsignedInteger32);

        var dataPoint = await WriteValueAsync(item, uint.MinValue).ConfigureAwait(true);
        Assert.Equal(uint.MinValue, dataPoint.Value);

        dataPoint = await WriteValueAsync(item, uint.MaxValue).ConfigureAwait(true);
        Assert.Equal(uint.MaxValue, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncIntArrayValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer32Array);

        var dataPoint = await WriteValueAsync(item, IntArraySample).ConfigureAwait(true);
        Assert.Equal(IntArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncIntArrayFromStringConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer32Array);

        var dataPoint = await WriteValueAsync(item, "[-1, 0, 1, 2147483647, -2147483648]").ConfigureAwait(true);
        Assert.Equal(IntArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncUIntArrayValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.UnsignedInteger32Array);

        var dataPoint = await WriteValueAsync(item, UIntArraySample).ConfigureAwait(true);
        Assert.Equal(UIntArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncUIntArrayFromStringConvertsCorrectly()
    {
        var item = CreateItem(ItemType.UnsignedInteger32Array);

        var dataPoint = await WriteValueAsync(item, "[0, 1, 4294967295]").ConfigureAwait(true);
        Assert.Equal(UIntArraySample, dataPoint.Value);
    }

    [Fact]
    public void DataPointEqualityOperatorWithInteger32ReturnsTrueForEqualValues()
    {
        var left = new DataPoint(DateTime.UtcNow, 789, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), 789, Quality.Good);

        Assert.True(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithInteger32ReturnsFalseForDifferentValues()
    {
        var left = new DataPoint(DateTime.UtcNow, 789, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), 790, Quality.Good);

        Assert.False(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithUnsignedInteger32ReturnsTrueForEqualValues()
    {
        var left = new DataPoint(DateTime.UtcNow, (uint)789, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), (uint)789, Quality.Good);

        Assert.True(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithUnsignedInteger32ReturnsFalseForDifferentValues()
    {
        var left = new DataPoint(DateTime.UtcNow, (uint)789, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), (uint)790, Quality.Good);

        Assert.False(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithInteger32ArrayReturnsTrueForEqualValues()
    {
        var left = new DataPoint(DateTime.UtcNow, new int[] { -1, 0, 1 }, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), new int[] { -1, 0, 1 }, Quality.Good);

        Assert.True(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithInteger32ArrayReturnsFalseForDifferentValues()
    {
        var left = new DataPoint(DateTime.UtcNow, new int[] { -1, 0, 1 }, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), new int[] { -1, 0, 2 }, Quality.Good);

        Assert.False(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithUnsignedInteger32ArrayReturnsTrueForEqualValues()
    {
        var left = new DataPoint(DateTime.UtcNow, new uint[] { 0u, 1u, 2u }, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), new uint[] { 0u, 1u, 2u }, Quality.Good);

        Assert.True(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithUnsignedInteger32ArrayReturnsFalseForDifferentValues()
    {
        var left = new DataPoint(DateTime.UtcNow, new uint[] { 0u, 1u, 2u }, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), new uint[] { 0u, 1u }, Quality.Good);

        Assert.False(left == right);
    }

    [Fact]
    public void ConvertIntOverflowThrows()
    {
        var converter = new DefaultValueConverter();

        _ = Assert.Throws<InvalidCastException>(() => converter.Convert(long.MaxValue, ItemType.Integer32));
        _ = Assert.Throws<InvalidCastException>(() => converter.Convert(long.MinValue, ItemType.Integer32));
    }

    [Fact]
    public void ConvertUIntNegativeValueThrows()
    {
        var converter = new DefaultValueConverter();

        _ = Assert.Throws<InvalidCastException>(() => converter.Convert(-1, ItemType.UnsignedInteger32));
    }

    [Fact]
    public void ConvertUIntOverflowThrows()
    {
        var converter = new DefaultValueConverter();

        _ = Assert.Throws<InvalidCastException>(() => converter.Convert(long.MaxValue, ItemType.UnsignedInteger32));
    }
}

#pragma warning restore SA1101, SA1202, SA1600, SA1139, CA1707, CA1861, CS1591, IDE0009
