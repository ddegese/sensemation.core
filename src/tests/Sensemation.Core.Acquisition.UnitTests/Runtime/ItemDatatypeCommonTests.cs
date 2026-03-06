// <copyright file="ItemDatatypeCommonTests.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging.Abstractions;

using Sensemation.Core.Acquisition.Abstractions.Enums;
using Sensemation.Core.Acquisition.Runtime.Converters;
using Sensemation.Core.Acquisition.Runtime.Models;
using Sensemation.Core.Acquisition.Runtime.Services;

namespace Sensemation.Core.Acquisition.UnitTests.Runtime;

#pragma warning disable SA1101, SA1202, SA1600, SA1139, CA1707, CA1861, CS1591, IDE0009

public sealed class ItemDatatypeCommonTests
{
    [Fact]
    public async Task WriteValueAsyncNullValueThrows()
    {
        using var dispatcher = new UpdateEventDispatcher(new NullLogger<UpdateEventDispatcher>());
        var item = new ItemSource(new NullLogger<ItemSource>(), dispatcher, new DefaultValueConverter())
        {
            Id = "tag1",
            SourceAddress = "tag1",
            Datatype = ItemType.Integer32,
        };

        _ = await Assert.ThrowsAsync<ArgumentNullException>(() => item.WriteValueAsync(null!)).ConfigureAwait(true);
    }
}

#pragma warning restore SA1101, SA1202, SA1600, SA1139, CA1707, CA1861, CS1591, IDE0009
