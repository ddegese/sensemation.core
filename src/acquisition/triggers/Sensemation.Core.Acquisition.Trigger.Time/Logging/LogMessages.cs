// <copyright file="LogMessages.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;

namespace Sensemation.Core.Acquisition.Trigger.Time.Logging;

/// <summary>
/// Provides strongly typed log messages for time-based triggers.
/// </summary>
public static class LogMessages
{
    /// <summary>
    /// Logs when a trigger is created.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> TriggerCreatedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(3000, "TriggerCreated"), "Trigger {TriggerType} '{TriggerName}' created");

    /// <summary>
    /// Logs when a trigger is started.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> TriggerStartedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(3001, "TriggerStarted"), "Trigger {TriggerType} '{TriggerName}' started");

    /// <summary>
    /// Logs when a trigger is stopped.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> TriggerStoppedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(3002, "TriggerStopped"), "Trigger {TriggerType} '{TriggerName}' stopped");

    /// <summary>
    /// Logs when a trigger is disposed.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> TriggerDisposedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(3003, "TriggerDisposed"), "Trigger {TriggerType} '{TriggerName}' disposed");

    /// <summary>
    /// Logs when a trigger cannot be found for a group.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> TriggerNotFoundLogger =
        LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(3004, "TriggerNotFound"), "Trigger {TriggerName} not found for group {GroupName}");

    /// <summary>
    /// Logs when a trigger fails to create.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> FailedToCreateTriggerLogger =
        LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(3005, "FailedToCreateTrigger"), "Failed to create trigger '{TriggerName}' of type '{TriggerType}'");

    /// <summary>
    /// Logs when a trigger has a missing or invalid update rate.
    /// </summary>
    public static readonly Action<ILogger, string, Exception?> TriggerMissingUpdateRateLogger =
        LoggerMessage.Define<string>(LogLevel.Warning, new EventId(3006, "TriggerMissingUpdateRate"), "Trigger '{TriggerName}' is missing or has invalid update rate parameter. Assuming Default Value.");
}
