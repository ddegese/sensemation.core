// <copyright file="ItemDatatypeInteger16Tests.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
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

public sealed class ItemDatatypeInteger16Tests : IDisposable
{
    private static readonly short[] ShortArraySample = [-1, 0, 1, 32767, -32768];
    private static readonly ushort[] UShortArraySample = [0, 1, 65535];

    private readonly UpdateEventDispatcher dispatcher;
    private readonly MemorySource memorySource;

    public ItemDatatypeInteger16Tests()
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
    public async Task WriteValueAsyncShortValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer16);

        var dataPoint = await WriteValueAsync(item, (short)123).ConfigureAwait(true);
        Assert.Equal((short)123, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncShortBoundaryValuesConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer16);

        var dataPoint = await WriteValueAsync(item, short.MinValue).ConfigureAwait(true);
        Assert.Equal(short.MinValue, dataPoint.Value);

        dataPoint = await WriteValueAsync(item, short.MaxValue).ConfigureAwait(true);
        Assert.Equal(short.MaxValue, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncUShortValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.UnsignedInteger16);

        var dataPoint = await WriteValueAsync(item, (ushort)456).ConfigureAwait(true);
        Assert.Equal((ushort)456, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncUShortBoundaryValuesConvertsCorrectly()
    {
        var item = CreateItem(ItemType.UnsignedInteger16);

        var dataPoint = await WriteValueAsync(item, ushort.MinValue).ConfigureAwait(true);
        Assert.Equal(ushort.MinValue, dataPoint.Value);

        dataPoint = await WriteValueAsync(item, ushort.MaxValue).ConfigureAwait(true);
        Assert.Equal(ushort.MaxValue, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncShortArrayValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer16Array);

        var dataPoint = await WriteValueAsync(item, ShortArraySample).ConfigureAwait(true);
        Assert.Equal(ShortArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncShortArrayFromStringConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer16Array);

        var dataPoint = await WriteValueAsync(item, "[-1, 0, 1, 32767, -32768]").ConfigureAwait(true);
        Assert.Equal(ShortArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncUShortArrayValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.UnsignedInteger16Array);

        var dataPoint = await WriteValueAsync(item, UShortArraySample).ConfigureAwait(true);
        Assert.Equal(UShortArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncUShortArrayFromStringConvertsCorrectly()
    {
        var item = CreateItem(ItemType.UnsignedInteger16Array);

        var dataPoint = await WriteValueAsync(item, "[0, 1, 65535]").ConfigureAwait(true);
        Assert.Equal(UShortArraySample, dataPoint.Value);
    }

    [Fact]
    public void ConvertShortOverflowThrows()
    {
        var converter = new DefaultValueConverter();

        _ = Assert.Throws<InvalidCastException>(() => converter.Convert(32768, ItemType.Integer16));
        _ = Assert.Throws<InvalidCastException>(() => converter.Convert(-32769, ItemType.Integer16));
    }

    [Fact]
    public void ConvertUShortNegativeValueThrows()
    {
        var converter = new DefaultValueConverter();

        _ = Assert.Throws<InvalidCastException>(() => converter.Convert(-1, ItemType.UnsignedInteger16));
    }

    [Fact]
    public void ConvertUShortOverflowThrows()
    {
        var converter = new DefaultValueConverter();

        _ = Assert.Throws<InvalidCastException>(() => converter.Convert(65536, ItemType.UnsignedInteger16));
    }
}

#pragma warning restore SA1101, SA1202, SA1600, SA1139, CA1707, CA1861, CS1591, IDE0009
