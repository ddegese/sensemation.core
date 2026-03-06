// <copyright file="ItemDatatypeInteger8Tests.cs" company="InnovoMind, LLC">
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

public sealed class ItemDatatypeInteger8Tests : IDisposable
{
    private static readonly sbyte[] SByteArraySample = [-1, 0, 1, 127, -128];
    private static readonly byte[] ByteArraySample = [1, 2, 3, 4, 5];

    private readonly UpdateEventDispatcher dispatcher;
    private readonly MemorySource memorySource;

    public ItemDatatypeInteger8Tests()
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
    public async Task WriteValueAsyncSByteValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer8);

        var dataPoint = await WriteValueAsync(item, (sbyte)-42).ConfigureAwait(true);
        Assert.Equal((sbyte)-42, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncSByteFromStringConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer8);

        var dataPoint = await WriteValueAsync(item, "42").ConfigureAwait(true);
        Assert.Equal((sbyte)42, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncSByteFromOtherTypesConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer8);

        var dataPoint = await WriteValueAsync(item, 100).ConfigureAwait(true);
        Assert.Equal((sbyte)100, dataPoint.Value);

        dataPoint = await WriteValueAsync(item, 25.0f).ConfigureAwait(true);
        Assert.Equal((sbyte)25, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncByteValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.UnsignedInteger8);

        var dataPoint = await WriteValueAsync(item, (byte)42).ConfigureAwait(true);
        Assert.Equal((byte)42, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncByteFromStringConvertsCorrectly()
    {
        var item = CreateItem(ItemType.UnsignedInteger8);

        var dataPoint = await WriteValueAsync(item, "42").ConfigureAwait(true);
        Assert.Equal((byte)42, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncSByteArrayValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer8Array);

        var dataPoint = await WriteValueAsync(item, SByteArraySample).ConfigureAwait(true);
        Assert.Equal(SByteArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncSByteArrayFromStringConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Integer8Array);

        var dataPoint = await WriteValueAsync(item, "[-1, 0, 1, 127, -128]").ConfigureAwait(true);
        Assert.Equal(SByteArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncByteArrayFromStringConvertsCorrectly()
    {
        var item = CreateItem(ItemType.UnsignedInteger8Array);

        var dataPoint = await WriteValueAsync(item, "[1, 2, 3, 4, 5]").ConfigureAwait(true);
        Assert.Equal(ByteArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncByteArrayValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.UnsignedInteger8Array);

        var dataPoint = await WriteValueAsync(item, ByteArraySample).ConfigureAwait(true);
        Assert.Equal(ByteArraySample, dataPoint.Value);
    }

    [Fact]
    public void ConvertSByteOverflowThrows()
    {
        var converter = new DefaultValueConverter();

        _ = Assert.Throws<InvalidCastException>(() => converter.Convert(128, ItemType.Integer8));
        _ = Assert.Throws<InvalidCastException>(() => converter.Convert(-129, ItemType.Integer8));
    }

    [Fact]
    public void ConvertByteNegativeValueThrows()
    {
        var converter = new DefaultValueConverter();

        _ = Assert.Throws<InvalidCastException>(() => converter.Convert(-1, ItemType.UnsignedInteger8));
    }

    [Fact]
    public void ConvertByteOverflowThrows()
    {
        var converter = new DefaultValueConverter();

        _ = Assert.Throws<InvalidCastException>(() => converter.Convert(256, ItemType.UnsignedInteger8));
    }
}

#pragma warning restore SA1101, SA1202, SA1600, SA1139, CA1707, CA1861, CS1591, IDE0009
