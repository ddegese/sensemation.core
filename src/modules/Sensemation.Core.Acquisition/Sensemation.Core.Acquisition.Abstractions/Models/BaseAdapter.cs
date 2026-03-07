// <copyright file="BaseAdapter.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Microsoft.Extensions.Logging;

using Sensemation.Core.Acquisition.Abstractions.Interfaces;
using Sensemation.Core.Acquisition.Abstractions.Logging;
using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.Abstractions.Models;

/// <summary>
/// Provides a base implementation for acquisition adapters.
/// </summary>
public abstract class BaseAdapter : BasePlugin, IAdapter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseAdapter"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="id">The adapter identifier.</param>
    /// <param name="parameters">The adapter parameters.</param>
    protected BaseAdapter(ILogger logger, string id, Dictionary<string, string> parameters)
        : base(logger, id, parameters)
    {
    }

    /// <summary>
    /// Gets or sets the value accessors exposed by the adapter.
    /// </summary>
    /// <value>The collection of value accessors.</value>
    protected Collection<IValueAccessor> ValueAccessors { get; set; } = [];

    /// <inheritdoc />
    public void AddValueAccessor(IValueAccessor accessor)
    {
        ArgumentNullException.ThrowIfNull(accessor);

        if (!this.ValueAccessors.Contains(accessor))
        {
            this.ValueAccessors.Add(accessor);
        }
    }

    /// <inheritdoc />
    public async Task StartAsync()
    {
        LogMessages.PluginStarting(this.Logger, this.GetType().Name, this.Id, null);

        try
        {
            await this.StartCoreAsync().ConfigureAwait(false);
            LogMessages.PluginStarted(this.Logger, this.GetType().Name, this.Id, null);
        }
        catch (Exception ex)
        {
            LogMessages.PluginStartFailed(this.Logger, this.GetType().Name, this.Id, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task StopAsync()
    {
        LogMessages.PluginStopping(this.Logger, this.GetType().Name, this.Id, null);

        try
        {
            await this.StopCoreAsync().ConfigureAwait(false);
            LogMessages.PluginStopped(this.Logger, this.GetType().Name, this.Id, null);
        }
        catch (Exception ex)
        {
            LogMessages.PluginStopFailed(this.Logger, this.GetType().Name, this.Id, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public virtual async Task OnValueChanged(string itemId, DataPoint dataPoint)
    {
    }

    /// <inheritdoc />
    protected abstract override void InitializeCore();

    /// <summary>
    /// Starts the adapter core logic.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected virtual async Task StartCoreAsync()
    {
        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <summary>
    /// Stops the adapter core logic.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected virtual async Task StopCoreAsync()
    {
        await Task.CompletedTask.ConfigureAwait(false);
    }
}
