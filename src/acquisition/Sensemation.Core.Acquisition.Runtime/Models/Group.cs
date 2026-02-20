// <copyright file="Group.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Diagnostics;

using Microsoft.Extensions.Logging;

using Sensemation.Core.Acquisition.Abstractions.Enums;
using Sensemation.Core.Acquisition.Abstractions.Interfaces;
using Sensemation.Core.Acquisition.Runtime.Logging;
using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.Runtime.Models;

/// <summary>
/// Represents a runtime acquisition group.
/// </summary>
public class Group : IGroup, IDisposable
{
    private readonly SemaphoreSlim semaphore = new(1, 1);
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="Group"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="id">The group identifier.</param>
    public Group(ILogger logger, string id)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.Id = id;
    }

    /// <inheritdoc />
    public string Id { get; set; } = string.Empty;

    /// <inheritdoc />
    public ISource? Source { get; set; }

    /// <inheritdoc />
    public ITrigger? Trigger
    {
        get;
        set
        {
            field?.Triggered -= this.OnTriggered;
            field = value;
            field?.Triggered += this.OnTriggered;
        }
    }

    /// <summary>
    /// Gets or sets the stale threshold in seconds.
    /// </summary>
    /// <value>The stale threshold in seconds.</value>
    public int? StaleThresholdSeconds { get; set; }

    /// <inheritdoc />
    public GroupState State
    {
        get
        {
            // Return Stale state if time since last update exceeds the stale threshold
            var isStale = this.StaleThresholdSeconds.HasValue &&
                          this.LastRefreshUtc.HasValue &&
                          (DateTime.UtcNow - this.LastRefreshUtc.Value).TotalSeconds > this.StaleThresholdSeconds.Value;

            return isStale ? GroupState.Stale : field;
        }

        set;
    } = GroupState.Idle;

    /// <inheritdoc />
    public Collection<IItemSource> Items { get; } = [];

    /// <inheritdoc />
    public DateTime? LastRefreshUtc { get; private set; }

    /// <inheritdoc />
    public DateTime? LastSuccessUtc { get; private set; }

    /// <inheritdoc />
    public DateTime? LastErrorUtc { get; private set; }

    /// <inheritdoc />
    public int ConsecutiveFailures { get; private set; }

    /// <inheritdoc />
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes managed resources.
    /// </summary>
    /// <param name="disposing">Whether the method is disposing managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.semaphore.Dispose();
        }
    }

    private async void OnTriggered(object? sender, EventArgs e)
    {
        if (this.Source == null)
        {
            return;
        }

        if (!await this.semaphore.WaitAsync(0).ConfigureAwait(false))
        {
            LogMessages.GroupBusyLogger(this.logger, this.Id, null);
            return;
        }

        try
        {
            this.State = GroupState.Reading;
            this.LastRefreshUtc = DateTime.UtcNow;

            var stopwatch = Stopwatch.StartNew();
            var results = await this.Source.ReadAsync(this.Items).ConfigureAwait(false);
            stopwatch.Stop();

            foreach (var item in this.Items)
            {
                if (!results.TryGetValue(item.Id, out var dataPoint))
                {
                    dataPoint = new DataPoint(DateTime.UtcNow, null, Quality.Bad);
                    LogMessages.GroupRefreshErrorLogger(this.logger, this.Id, $"Item '{item.Id}' not found in source results", null);
                }

                await item.OnValueRead(dataPoint).ConfigureAwait(false);
            }

            this.State = GroupState.Idle;
            this.LastSuccessUtc = this.LastRefreshUtc;
            this.ConsecutiveFailures = 0;

            LogMessages.GroupSourceReadTimeLogger(this.logger, this.Id, stopwatch.Elapsed.TotalMilliseconds, null);
        }
        catch (Exception ex)
        {
            this.State = GroupState.Idle;
            this.LastErrorUtc = this.LastRefreshUtc;
            this.ConsecutiveFailures++;

            var dataPoint = new DataPoint(DateTime.UtcNow, null, Quality.Bad);
            foreach (var item in this.Items)
            {
                await item.OnValueRead(dataPoint).ConfigureAwait(false);
            }

            LogMessages.GroupRefreshErrorLogger(this.logger, this.Id, ex.Message, ex);
            throw;
        }
        finally
        {
            _ = this.semaphore.Release();
        }
    }
}
