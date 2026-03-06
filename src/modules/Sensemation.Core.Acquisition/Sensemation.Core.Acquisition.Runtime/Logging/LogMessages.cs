// <copyright file="LogMessages.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;

namespace Sensemation.Core.Acquisition.Runtime.Logging;

/// <summary>
/// Provides strongly typed log message delegates for runtime components.
/// </summary>
public static class LogMessages
{
    /// <summary>
    /// Logs when an update event fails to enqueue.
    /// </summary>
    public static readonly Action<ILogger, string, Exception?> LogEnqueueFailed =
        LoggerMessage.Define<string>(LogLevel.Warning, new EventId(2100, "EnqueueFailed"), "Failed to enqueue item update for {ItemId}.");

    /// <summary>
    /// Logs when update processing fails.
    /// </summary>
    public static readonly Action<ILogger, Exception> LogUpdateProcessingError =
        LoggerMessage.Define(LogLevel.Error, new EventId(2101, "UpdateProcessingError"), "Error processing item update event.");

    /// <summary>
    /// Logs when writing to an item source fails.
    /// </summary>
    public static readonly Action<ILogger, string, Exception?> ItemSourceWriteFailedLogger =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(2102, "ItemSourceWriteFailed"), "Error writing value for item source {ItemId}.");

    /// <summary>
    /// Logs when writing to an item fails.
    /// </summary>
    public static readonly Action<ILogger, string, Exception?> ErrorWritingLogger =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(2103, "ErrorWriting"), "Error writing to item {Id}");

    /// <summary>
    /// Logs when a group becomes overloaded.
    /// </summary>
    public static readonly Action<ILogger, string, Exception?> GroupBusyLogger =
        LoggerMessage.Define<string>(LogLevel.Warning, new EventId(2104, "GroupOverloaded"), "Group {GroupName} is overloaded");

    /// <summary>
    /// Logs the duration of group refresh cycles.
    /// </summary>
    public static readonly Action<ILogger, string, double, Exception?> GroupSourceReadTimeLogger =
        LoggerMessage.Define<string, double>(LogLevel.Debug, new EventId(2105, "GroupRefreshTime"), "Group {GroupName} update cycle took {ElapsedMilliseconds}ms");

    /// <summary>
    /// Logs errors that occur during group refresh cycles.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> GroupRefreshErrorLogger =
        LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(2106, "GroupRefreshError"), "Error during group {GroupName} update cycle: {ErrorMessage}");
}
