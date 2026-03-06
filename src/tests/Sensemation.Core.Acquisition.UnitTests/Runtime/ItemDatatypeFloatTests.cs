// <copyright file="ItemDatatypeFloatTests.cs" company="InnovoMind, LLC">
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

public sealed class ItemDatatypeFloatTests : IDisposable
{
    private static readonly float[] FloatArraySample = [-1.0f, 0.0f, 1.0f, float.MaxValue, float.MinValue, float.PositiveInfinity, float.NegativeInfinity];
    private static readonly double[] DoubleArraySample = [-1.0, 0.0, 1.0, double.MaxValue, double.MinValue, double.PositiveInfinity, double.NegativeInfinity];

    private readonly UpdateEventDispatcher dispatcher;
    private readonly MemorySource memorySource;

    public ItemDatatypeFloatTests()
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
    public async Task WriteValueAsyncFloatValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.SinglePrecision);

        var dataPoint = await WriteValueAsync(item, 3.14f).ConfigureAwait(true);
        Assert.Equal(3.14f, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncFloatFromDoubleConvertsCorrectly()
    {
        var item = CreateItem(ItemType.SinglePrecision);

        var dataPoint = await WriteValueAsync(item, 3.14).ConfigureAwait(true);
        Assert.Equal(3.14f, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncFloatSpecialValuesConvertsCorrectly()
    {
        var item = CreateItem(ItemType.SinglePrecision);

        var dataPoint = await WriteValueAsync(item, float.PositiveInfinity).ConfigureAwait(true);
        Assert.Equal(float.PositiveInfinity, dataPoint.Value);

        dataPoint = await WriteValueAsync(item, float.NegativeInfinity).ConfigureAwait(true);
        Assert.Equal(float.NegativeInfinity, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncFloatNaNValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.SinglePrecision);

        var dataPoint = await WriteValueAsync(item, float.NaN).ConfigureAwait(true);
        Assert.NotNull(dataPoint.Value);
        Assert.True(float.IsNaN((float)dataPoint.Value));
    }

    [Fact]
    public async Task WriteValueAsyncDoubleValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.DoublePrecision);

        var dataPoint = await WriteValueAsync(item, 3.14159).ConfigureAwait(true);
        Assert.Equal(3.14159, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncDoubleFromFloatConvertsCorrectly()
    {
        var item = CreateItem(ItemType.DoublePrecision);

        var dataPoint = await WriteValueAsync(item, 3.14f).ConfigureAwait(true);
        Assert.NotNull(dataPoint.Value);
        Assert.Equal(3.14, (double)dataPoint.Value, precision: 6);
    }

    [Fact]
    public async Task WriteValueAsyncDoubleSpecialValuesConvertsCorrectly()
    {
        var item = CreateItem(ItemType.DoublePrecision);

        var dataPoint = await WriteValueAsync(item, double.PositiveInfinity).ConfigureAwait(true);
        Assert.Equal(double.PositiveInfinity, dataPoint.Value);

        dataPoint = await WriteValueAsync(item, double.NegativeInfinity).ConfigureAwait(true);
        Assert.Equal(double.NegativeInfinity, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncDoubleNaNValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.DoublePrecision);

        var dataPoint = await WriteValueAsync(item, double.NaN).ConfigureAwait(true);
        Assert.NotNull(dataPoint.Value);
        Assert.True(double.IsNaN((double)dataPoint.Value));
    }

    [Fact]
    public async Task WriteValueAsyncFloatArrayValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.SinglePrecisionArray);

        var dataPoint = await WriteValueAsync(item, FloatArraySample).ConfigureAwait(true);
        Assert.Equal(FloatArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncFloatArrayFromStringConvertsCorrectly()
    {
        var item = CreateItem(ItemType.SinglePrecisionArray);

        var expected = new float[] { -1.0f, 0.0f, 1.0f, 3.402823E+38f, -3.402823E+38f };
        var dataPoint = await WriteValueAsync(item, "[-1.0, 0.0, 1.0, 3.402823E+38, -3.402823E+38]").ConfigureAwait(true);
        Assert.Equal(expected, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncDoubleArrayValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.DoublePrecisionArray);

        var dataPoint = await WriteValueAsync(item, DoubleArraySample).ConfigureAwait(true);
        Assert.Equal(DoubleArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncDoubleArrayFromStringConvertsCorrectly()
    {
        var item = CreateItem(ItemType.DoublePrecisionArray);

        var expected = new double[] { -1.0, 0.0, 1.0, 1.7976931348623157E+308, -1.7976931348623157E+308 };
        var dataPoint = await WriteValueAsync(item, "[-1.0, 0.0, 1.0, 1.7976931348623157E+308, -1.7976931348623157E+308]").ConfigureAwait(true);
        Assert.Equal(expected, dataPoint.Value);
    }

    [Fact]
    public void ConvertFloatOverflowThrows()
    {
        var converter = new DefaultValueConverter();

        _ = Assert.Throws<InvalidCastException>(() => converter.Convert(double.MaxValue, ItemType.SinglePrecision));
    }
}

#pragma warning restore SA1101, SA1202, SA1600, SA1139, CA1707, CA1861, CS1591, IDE0009
