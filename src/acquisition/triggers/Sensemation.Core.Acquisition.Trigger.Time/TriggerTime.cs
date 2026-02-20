// <copyright file="TriggerTime.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;

using Sensemation.Core.Acquisition.Abstractions.Attributes;
using Sensemation.Core.Acquisition.Abstractions.Models;
using Sensemation.Core.Acquisition.Trigger.Time.Logging;

namespace Sensemation.Core.Acquisition.Trigger.Time;

/// <summary>
/// Time-based trigger that fires on a fixed interval.
/// </summary>
[PluginType("time")]
public class TriggerTime : BaseTrigger, IIntervalTrigger
{
    private Timer? timer;

    /// <summary>
    /// Initializes a new instance of the <see cref="TriggerTime"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="name">The trigger identifier.</param>
    /// <param name="parameters">The trigger parameters.</param>
    public TriggerTime(ILogger<TriggerTime> logger, string name, Dictionary<string, string> parameters)
        : base(logger, name, parameters)
    {
    }

    /// <inheritdoc />
    public int UpdateRate { get; private set; } = 1000;

    /// <inheritdoc />
    protected override void InitializeCore()
    {
        if (int.TryParse(this.Parameters.GetValueOrDefault("updaterate", "invalid"), out var updateRate))
        {
            this.UpdateRate = updateRate;
        }
        else
        {
            LogMessages.TriggerMissingUpdateRateLogger(this.Logger, this.Id, null);
        }
    }

    /// <inheritdoc />
    protected override async Task StartCoreAsync()
    {
        await base.StartCoreAsync().ConfigureAwait(false);
        this.timer = new Timer(
            callback: _ => this.OnTriggered(),
            state: null,
            dueTime: TimeSpan.FromMilliseconds(this.UpdateRate),
            period: TimeSpan.FromMilliseconds(this.UpdateRate));
    }

    /// <inheritdoc />
    protected override async Task StopCoreAsync()
    {
        await base.StopCoreAsync().ConfigureAwait(false);
        _ = this.timer?.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.timer?.Dispose();
            this.timer = null;
        }

        base.Dispose(disposing);
    }
}
