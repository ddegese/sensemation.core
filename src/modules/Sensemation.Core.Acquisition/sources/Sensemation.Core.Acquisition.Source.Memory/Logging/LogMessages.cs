// <copyright file="LogMessages.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;

namespace Sensemation.Core.Acquisition.Source.Memory.Logging;

/// <summary>
/// Provides strongly typed log message delegates for the memory source.
/// </summary>
public static class LogMessages
{
    /// <summary>
    /// Logs when memory tags are being read.
    /// </summary>
    public static readonly Action<ILogger, int, Exception?> ReadingTagsLogger =
        LoggerMessage.Define<int>(LogLevel.Debug, new EventId(4001, "ReadingTags"), "Reading {Count} tags from Memory source");

    /// <summary>
    /// Logs when memory tag reads complete.
    /// </summary>
    public static readonly Action<ILogger, int, Exception?> ReadCompleteLogger =
        LoggerMessage.Define<int>(LogLevel.Debug, new EventId(4002, "ReadComplete"), "Finished reading {Count} tags from Memory source");

    /// <summary>
    /// Logs when a memory tag is being written.
    /// </summary>
    public static readonly Action<ILogger, string, Exception?> WritingTagLogger =
        LoggerMessage.Define<string>(LogLevel.Debug, new EventId(4003, "WritingTag"), "Writing to tag '{Id}' in Memory");

    /// <summary>
    /// Logs when a memory tag write completes.
    /// </summary>
    public static readonly Action<ILogger, string, Exception?> WriteCompleteLogger =
        LoggerMessage.Define<string>(LogLevel.Debug, new EventId(4004, "WriteComplete"), "Successfully wrote to tag '{Id}' in Memory");
}
