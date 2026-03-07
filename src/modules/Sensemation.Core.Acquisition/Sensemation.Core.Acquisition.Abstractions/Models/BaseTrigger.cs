// <copyright file="BaseTrigger.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;

using Sensemation.Core.Acquisition.Abstractions.Interfaces;
using Sensemation.Core.Acquisition.Abstractions.Logging;

namespace Sensemation.Core.Acquisition.Abstractions.Models;

/// <summary>
/// Provides a base implementation for acquisition triggers.
/// </summary>
public abstract class BaseTrigger : BasePlugin, ITrigger
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseTrigger"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="id">The trigger identifier.</param>
    /// <param name="parameters">The trigger parameters.</param>
    protected BaseTrigger(ILogger logger, string id, Dictionary<string, string> parameters)
        : base(logger, id, parameters)
    {
    }

    /// <inheritdoc />
    public event EventHandler? Triggered;

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

    /// <summary>
    /// Starts the trigger core logic.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected virtual async Task StartCoreAsync()
    {
        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <summary>
    /// Stops the trigger core logic.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected virtual async Task StopCoreAsync()
    {
        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <summary>
    /// Raises the <see cref="Triggered"/> event.
    /// </summary>
    protected virtual void OnTriggered()
    {
        this.Triggered?.Invoke(this, EventArgs.Empty);
    }
}
