// <copyright file="BaseItem.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Microsoft.Extensions.Logging;

using Sensemation.Core.Acquisition.Abstractions.Enums;
using Sensemation.Core.Acquisition.Abstractions.Interfaces;
using Sensemation.Core.Acquisition.Runtime.Logging;
using Sensemation.Core.Acquisition.Runtime.Services;
using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.Runtime.Models;

/// <summary>
/// Provides a base implementation for acquisition runtime items.
/// </summary>
public abstract class BaseItem : IItem, IValueAccessor
{
    private readonly object cacheLock = new();
    private readonly Collection<DataPoint> internalCache = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseItem"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="dispatcher">The update event dispatcher.</param>
    /// <param name="valueConverter">The value converter.</param>
    protected BaseItem(ILogger logger, UpdateEventDispatcher dispatcher, IValueConverter valueConverter)
    {
        this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        this.ValueConverter = valueConverter ?? throw new ArgumentNullException(nameof(valueConverter));
    }

    /// <inheritdoc />
    public string Id { get; set; } = string.Empty;

    /// <inheritdoc />
    public ItemType Datatype { get; set; }

    /// <inheritdoc />
    public int CacheSize { get; set; } = 10;

    /// <inheritdoc />
    public DataPoint LatestDataPoint { get; private set; } = new DataPoint(DateTime.UtcNow, null, Quality.Bad);

    /// <summary>
    /// Gets the registered value listeners.
    /// </summary>
    /// <value>The value listeners.</value>
    protected Collection<IValueListener> ValueListeners { get; private set; } = [];

    /// <summary>
    /// Gets the logger instance.
    /// </summary>
    /// <value>The logger instance.</value>
    protected ILogger Logger { get; }

    /// <summary>
    /// Gets the update event dispatcher.
    /// </summary>
    /// <value>The update event dispatcher.</value>
    protected UpdateEventDispatcher Dispatcher { get; }

    /// <summary>
    /// Gets the value converter.
    /// </summary>
    /// <value>The value converter.</value>
    protected IValueConverter ValueConverter { get; private set; }

    /// <inheritdoc />
    public void AddValueListener(IValueListener listener)
    {
        ArgumentNullException.ThrowIfNull(listener);

        if (!this.ValueListeners.Contains(listener))
        {
            this.ValueListeners.Add(listener);
        }
    }

    /// <inheritdoc />
    public ReadOnlyCollection<DataPoint> GetHistoryValues(int? count = null)
    {
        return count is null || count > this.internalCache.Count
            ? new ReadOnlyCollection<DataPoint>(this.internalCache.ToList())
            : new ReadOnlyCollection<DataPoint>(this.internalCache.Take(count.Value).ToList());
    }

    /// <inheritdoc />
    public abstract Task<DataPoint> WriteValueAsync(object value);

    /// <inheritdoc />
    public async Task OnValueRead(DataPoint dataPoint)
    {
        ArgumentNullException.ThrowIfNull(dataPoint);

        var itemUpdateEvent = new ItemUpdateEvent(this.Id, this, dataPoint);
        _ = this.Dispatcher.Enqueue(itemUpdateEvent);
    }

    /// <inheritdoc />
    public async Task OnValueChanged(string itemId, DataPoint dataPoint)
    {
        ArgumentNullException.ThrowIfNull(dataPoint);

        if (!this.UpdateInternalCache(dataPoint))
        {
            return;
        }

        var listeners = this.ValueListeners.ToList();
        var tasks = new List<Task>();
        foreach (var listener in listeners)
        {
            tasks.Add(listener.OnValueChanged(this.Id, dataPoint));
        }

        try
        {
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            LogMessages.ErrorWritingLogger(this.Logger, this.Id, ex);
            throw;
        }
    }

    /// <summary>
    /// Restores the internal cache with historical values.
    /// </summary>
    /// <param name="values">The values to restore.</param>
    public void RecoverInternalCache(IEnumerable<DataPoint> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        lock (this.cacheLock)
        {
            this.internalCache.Clear();
            foreach (var value in values)
            {
                this.internalCache.Add(value);
            }

            if (this.internalCache.Count > 0)
            {
                this.LatestDataPoint = this.internalCache.First();
            }
        }
    }

    private bool UpdateInternalCache(DataPoint dataPoint)
    {
        ArgumentNullException.ThrowIfNull(dataPoint);

        lock (this.cacheLock)
        {
            if (dataPoint == this.LatestDataPoint)
            {
                return false;
            }

            this.internalCache.Insert(0, dataPoint);
            while (this.internalCache.Count > this.CacheSize)
            {
                this.internalCache.RemoveAt(this.internalCache.Count - 1);
            }

            this.LatestDataPoint = dataPoint;

            return true;
        }
    }
}
