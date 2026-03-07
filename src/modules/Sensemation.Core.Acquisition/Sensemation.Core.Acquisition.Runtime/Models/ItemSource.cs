// <copyright file="ItemSource.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;

using Sensemation.Core.Acquisition.Abstractions.Interfaces;
using Sensemation.Core.Acquisition.Runtime.Logging;
using Sensemation.Core.Acquisition.Runtime.Services;
using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.Runtime.Models;

/// <summary>
/// Represents a runtime item tied to a source address.
/// </summary>
public class ItemSource : BaseItem, IItemSource
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ItemSource"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="dispatcher">The update event dispatcher.</param>
    /// <param name="valueConverter">The value converter.</param>
    public ItemSource(ILogger logger, UpdateEventDispatcher dispatcher, IValueConverter valueConverter)
        : base(logger, dispatcher, valueConverter)
    {
    }

    /// <inheritdoc />
    public string SourceAddress { get; set; } = string.Empty;

    /// <inheritdoc />
    public ISource? Source { get; set; }

    /// <inheritdoc />
    public override async Task<DataPoint> WriteValueAsync(object value)
    {
        ArgumentNullException.ThrowIfNull(value);

        DataPoint dataPoint;

        try
        {
            ArgumentNullException.ThrowIfNull(this.Source);

            var convertedValue = this.ValueConverter.Convert(value, this.Datatype);
            dataPoint = await this.Source.WriteAsync(this, convertedValue).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            LogMessages.ItemSourceWriteFailed(this.Logger, this.Id, ex);
            dataPoint = new DataPoint(DateTime.UtcNow, null, Quality.Bad);
        }

        await this.OnValueChanged(this.Id, dataPoint).ConfigureAwait(false);

        return dataPoint;
    }
}
