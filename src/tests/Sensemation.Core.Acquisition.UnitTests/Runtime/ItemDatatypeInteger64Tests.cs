// <copyright file="ItemDatatypeInteger64Tests.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Numerics;

using Microsoft.Extensions.Logging.Abstractions;

using Sensemation.Core.Acquisition.Abstractions.Enums;
using Sensemation.Core.Acquisition.Runtime.Converters;
using Sensemation.Core.Acquisition.Runtime.Models;
using Sensemation.Core.Acquisition.Runtime.Services;
using Sensemation.Core.Acquisition.Source.Memory;
using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.UnitTests.Runtime;

#pragma warning disable SA1101, SA1202, SA1600, SA1139, CA1707, CA1861, CS1591, IDE0009

public sealed class ItemDatatypeInteger64Tests : IDisposable
{
    private static readonly long[] LongArraySample = [-1, 0, 1, 9223372036854775807, -9223372036854775808];
    private static readonly ulong[] ULongArraySample = [0ul, 1ul, 18446744073709551615ul];

    private readonly UpdateEventDispatcher dispatcher;
    private readonly MemorySource memorySource;

    public ItemDatatypeInteger64Tests()
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
    public async Task WriteValueAsyncLongValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer64);

        var dataPoint = await WriteValueAsync(item, 123456789L).ConfigureAwait(true);
        Assert.Equal(123456789L, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncLongBoundaryValuesConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer64);

        var dataPoint = await WriteValueAsync(item, long.MinValue).ConfigureAwait(true);
        Assert.Equal(long.MinValue, dataPoint.Value);

        dataPoint = await WriteValueAsync(item, long.MaxValue).ConfigureAwait(true);
        Assert.Equal(long.MaxValue, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncULongValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.UnsignedInteger64);

        var dataPoint = await WriteValueAsync(item, 123456789UL).ConfigureAwait(true);
        Assert.Equal(123456789UL, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncULongBoundaryValuesConvertsCorrectly()
    {
        var item = CreateItem(ItemType.UnsignedInteger64);

        var dataPoint = await WriteValueAsync(item, ulong.MinValue).ConfigureAwait(true);
        Assert.Equal(ulong.MinValue, dataPoint.Value);

        dataPoint = await WriteValueAsync(item, ulong.MaxValue).ConfigureAwait(true);
        Assert.Equal(ulong.MaxValue, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncLongArrayValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer64Array);

        var dataPoint = await WriteValueAsync(item, LongArraySample).ConfigureAwait(true);
        Assert.Equal(LongArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncLongArrayFromStringConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer64Array);

        var dataPoint = await WriteValueAsync(item, "[-1, 0, 1, 9223372036854775807, -9223372036854775808]").ConfigureAwait(true);
        Assert.Equal(LongArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncULongArrayValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.UnsignedInteger64Array);

        var dataPoint = await WriteValueAsync(item, ULongArraySample).ConfigureAwait(true);
        Assert.Equal(ULongArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncULongArrayFromStringConvertsCorrectly()
    {
        var item = CreateItem(ItemType.UnsignedInteger64Array);

        var dataPoint = await WriteValueAsync(item, "[0, 1, 18446744073709551615]").ConfigureAwait(true);
        Assert.Equal(ULongArraySample, dataPoint.Value);
    }

    [Fact]
    public void DataPointEqualityOperatorWithInteger64ReturnsTrueForEqualValues()
    {
        var left = new DataPoint(DateTime.UtcNow, 123456789L, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), 123456789L, Quality.Good);

        Assert.True(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithInteger64ReturnsFalseForDifferentValues()
    {
        var left = new DataPoint(DateTime.UtcNow, 123456789L, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), 123456790L, Quality.Good);

        Assert.False(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithUnsignedInteger64ReturnsTrueForEqualValues()
    {
        var left = new DataPoint(DateTime.UtcNow, 123456789UL, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), 123456789UL, Quality.Good);

        Assert.True(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithUnsignedInteger64ReturnsFalseForDifferentValues()
    {
        var left = new DataPoint(DateTime.UtcNow, 123456789UL, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), 123456790UL, Quality.Good);

        Assert.False(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithInteger64ArrayReturnsTrueForEqualValues()
    {
        var left = new DataPoint(DateTime.UtcNow, new long[] { -1, 0, 1 }, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), new long[] { -1, 0, 1 }, Quality.Good);

        Assert.True(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithInteger64ArrayReturnsFalseForDifferentValues()
    {
        var left = new DataPoint(DateTime.UtcNow, new long[] { -1, 0, 1 }, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), new long[] { -1, 0, 2 }, Quality.Good);

        Assert.False(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithUnsignedInteger64ArrayReturnsTrueForEqualValues()
    {
        var left = new DataPoint(DateTime.UtcNow, new ulong[] { 0ul, 1ul, 2ul }, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), new ulong[] { 0ul, 1ul, 2ul }, Quality.Good);

        Assert.True(left == right);
    }

    [Fact]
    public void DataPointEqualityOperatorWithUnsignedInteger64ArrayReturnsFalseForDifferentValues()
    {
        var left = new DataPoint(DateTime.UtcNow, new ulong[] { 0ul, 1ul, 2ul }, Quality.Good);
        var right = new DataPoint(DateTime.UtcNow.AddSeconds(1), new ulong[] { 0ul, 1ul }, Quality.Good);

        Assert.False(left == right);
    }

    [Fact]
    public void ConvertLongOverflowThrows()
    {
        var converter = new DefaultValueConverter();

        _ = Assert.Throws<InvalidCastException>(() => converter.Convert(BigInteger.Parse("9223372036854775808", System.Globalization.CultureInfo.InvariantCulture), ItemType.Integer64));
    }

    [Fact]
    public void ConvertULongNegativeValueThrows()
    {
        var converter = new DefaultValueConverter();

        _ = Assert.Throws<InvalidCastException>(() => converter.Convert(-1, ItemType.UnsignedInteger64));
    }

    [Fact]
    public void ConvertULongOverflowThrows()
    {
        var converter = new DefaultValueConverter();

        _ = Assert.Throws<InvalidCastException>(() => converter.Convert(BigInteger.Parse("18446744073709551616", System.Globalization.CultureInfo.InvariantCulture), ItemType.UnsignedInteger64));
    }
}

#pragma warning restore SA1101, SA1202, SA1600, SA1139, CA1707, CA1861, CS1591, IDE0009
