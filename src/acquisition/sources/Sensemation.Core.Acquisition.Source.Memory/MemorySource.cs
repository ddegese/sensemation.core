// <copyright file="MemorySource.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Microsoft.Extensions.Logging;

using Sensemation.Core.Acquisition.Abstractions.Attributes;
using Sensemation.Core.Acquisition.Abstractions.Interfaces;
using Sensemation.Core.Acquisition.Abstractions.Models;
using Sensemation.Core.Acquisition.Source.Memory.Converters;
using Sensemation.Core.Acquisition.Source.Memory.Logging;
using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.Source.Memory;

/// <summary>
/// In-memory source implementation used for testing and demos.
/// </summary>
[PluginType("memory")]
public class MemorySource : BaseSource
{
    private readonly Dictionary<string, object> memoryValues = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="MemorySource"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="name">The source identifier.</param>
    /// <param name="parameters">The source parameters.</param>
    public MemorySource(ILogger<MemorySource> logger, string name, Dictionary<string, string> parameters)
        : base(logger, name, parameters)
    {
    }

    /// <inheritdoc />
    public override async Task<Dictionary<string, DataPoint>> ReadAsync(Collection<IItemSource> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        var results = new Dictionary<string, DataPoint>();

        LogMessages.ReadingTagsLogger(this.Logger, items.Count, null);

        foreach (var item in items)
        {
            if (!this.memoryValues.TryGetValue(item.SourceAddress, out var value))
            {
                value = MemoryTypeConverter.GenerateDefaultValue(item.Datatype);
                this.memoryValues[item.SourceAddress] = value;
            }

            results[item.Id] = new DataPoint(DateTime.UtcNow, value, Quality.Good);
        }

        LogMessages.ReadCompleteLogger(this.Logger, items.Count, null);
        return results;
    }

    /// <inheritdoc />
    public override async Task<DataPoint> WriteAsync(IItemSource item, object value)
    {
        ArgumentNullException.ThrowIfNull(item);

        LogMessages.WritingTagLogger(this.Logger, item.SourceAddress, null);

        this.memoryValues[item.SourceAddress] = value;

        LogMessages.WriteCompleteLogger(this.Logger, item.SourceAddress, null);

        return new DataPoint(DateTime.UtcNow, value, Quality.Good);
    }

    /// <inheritdoc />
    protected override void InitializeCore()
    {
    }
}
