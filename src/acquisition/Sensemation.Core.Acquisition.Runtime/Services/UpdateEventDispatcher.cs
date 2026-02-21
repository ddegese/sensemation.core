// <copyright file="UpdateEventDispatcher.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using System.Threading.Channels;

using Microsoft.Extensions.Logging;

using Sensemation.Core.Acquisition.Runtime.Logging;
using Sensemation.Core.Acquisition.Runtime.Models;

namespace Sensemation.Core.Acquisition.Runtime.Services;

/// <summary>
/// Dispatches item update events to listeners on a background task.
/// </summary>
public sealed class UpdateEventDispatcher : IDisposable
{
    private readonly Channel<ItemUpdateEvent> channel;
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly Task processingTask;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEventDispatcher"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public UpdateEventDispatcher(ILogger<UpdateEventDispatcher> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        this.channel = Channel.CreateUnbounded<ItemUpdateEvent>(new UnboundedChannelOptions
        {
            SingleReader = true,
            SingleWriter = false,
        });

        this.processingTask = Task.Run(() => this.ProcessUpdatesAsync(this.cancellationTokenSource.Token));
    }

    /// <summary>
    /// Enqueues an update event for processing.
    /// </summary>
    /// <param name="updateEvent">The update event.</param>
    /// <returns><c>true</c> if queued; otherwise <c>false</c>.</returns>
    public bool Enqueue(ItemUpdateEvent updateEvent)
    {
        ArgumentNullException.ThrowIfNull(updateEvent);

        if (!this.channel.Writer.TryWrite(updateEvent))
        {
            LogMessages.LogEnqueueFailed(this.logger, updateEvent.ItemId, null);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Stops processing update events.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task StopAsync()
    {
        _ = this.channel.Writer.TryComplete();
        await this.cancellationTokenSource.CancelAsync().ConfigureAwait(false);
        await this.processingTask.ConfigureAwait(false);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private async Task ProcessUpdatesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await foreach (var updateEvent in this.channel.Reader.ReadAllAsync(cancellationToken).ConfigureAwait(false))
            {
                try
                {
                    var itemId = updateEvent.ItemId;
                    var listener = updateEvent.ValueListener;
                    var dataPoint = updateEvent.DataPoint;

                    await listener.OnValueChanged(itemId, dataPoint).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    LogMessages.LogUpdateProcessingError(this.logger, ex);
                }
            }
        }
        catch (OperationCanceledException)
        {
            // expected on shutdown
        }
    }

    /// <summary>
    /// Disposes managed resources.
    /// </summary>
    /// <param name="disposing">Whether the method is disposing managed resources.</param>
    private void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        try
        {
            if (!this.cancellationTokenSource.IsCancellationRequested)
            {
                this.cancellationTokenSource.Cancel();
            }
        }
        catch (ObjectDisposedException)
        {
        }

        try
        {
            this.cancellationTokenSource.Dispose();
        }
        catch (ObjectDisposedException)
        {
        }
    }
}
