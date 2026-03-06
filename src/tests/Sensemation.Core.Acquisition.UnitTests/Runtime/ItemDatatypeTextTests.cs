// <copyright file="ItemDatatypeTextTests.cs" company="InnovoMind, LLC">
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

public sealed class ItemDatatypeTextTests : IDisposable
{
    private static readonly string[] StringArraySample = ["Hello", "World", string.Empty, "Test"];

    private readonly UpdateEventDispatcher dispatcher;
    private readonly MemorySource memorySource;

    public ItemDatatypeTextTests()
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
    public async Task WriteValueAsyncStringValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.Text);

        var dataPoint = await WriteValueAsync(item, "Hello World").ConfigureAwait(true);
        Assert.Equal("Hello World", dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncTextArrayValueConvertsCorrectly()
    {
        var item = CreateItem(ItemType.TextArray);

        var dataPoint = await WriteValueAsync(item, StringArraySample).ConfigureAwait(true);
        Assert.Equal(StringArraySample, dataPoint.Value);
    }

    [Fact]
    public async Task WriteValueAsyncTextArrayFromStringConvertsCorrectly()
    {
        var item = CreateItem(ItemType.TextArray);

        var dataPoint = await WriteValueAsync(item, "[\"Hello\", \"World\", \"\", \"Test\"]").ConfigureAwait(true);
        Assert.Equal(StringArraySample, dataPoint.Value);
    }
}

#pragma warning restore SA1101, SA1202, SA1600, SA1139, CA1707, CA1861, CS1591, IDE0009
