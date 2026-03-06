// <copyright file="LogMessages.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Demo.Console.Logging;

/// <summary>
/// Provides strongly typed log message delegates for demo console components.
/// </summary>
internal static class LogMessages
{
    /// <summary>
    /// Logs when a source cannot be found for a group.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> SourceNotFoundLogger =
        LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(5000, "SourceNotFound"), "Source {SourceName} not found for group {GroupName}");

    /// <summary>
    /// Logs when a trigger cannot be found for a group.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> TriggerNotFoundLogger =
        LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(5001, "TriggerNotFound"), "Trigger {TriggerName} not found for group {GroupName}");

    /// <summary>
    /// Logs when a source is assigned to a group.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> SourceAssignedToGroupLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(5002, "SourceAssignedToGroup"), "Source {SourceName} assigned to group {GroupName}");

    /// <summary>
    /// Logs when a trigger is assigned to a group.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> TriggerAssignedToGroupLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(5003, "TriggerAssignedToGroup"), "Trigger {TriggerName} assigned to group {GroupName}");

    /// <summary>
    /// Logs when a group is missing during assignment.
    /// </summary>
    public static readonly Action<ILogger, string, Exception?> GroupNotFoundLogger =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(5004, "GroupNotFound"), "Group {GroupName} not found for assignment");

    /// <summary>
    /// Logs when a group is created.
    /// </summary>
    public static readonly Action<ILogger, string, Exception?> GroupCreatedLogger =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(5005, "GroupCreated"), "Group {GroupName} created");

    /// <summary>
    /// Logs when a group is disposed.
    /// </summary>
    public static readonly Action<ILogger, string, Exception?> GroupDisposedLogger =
        LoggerMessage.Define<string>(LogLevel.Information, new EventId(5006, "GroupDisposed"), "Group {GroupName} disposed");

    /// <summary>
    /// Logs when an item is assigned to a group.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> ItemAssignedToGroupLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(5007, "ItemAssignedToGroup"), "Item {ItemId} assigned to group {GroupName}");

    /// <summary>
    /// Logs when a listener is assigned to an item.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> ListenerAssignedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(5008, "ListenerAssigned"), "Listener {ListenerType} assigned to item {ItemId}");

    /// <summary>
    /// Logs when an accessor is added to an adapter.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> AccessorAddedToAdapterLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(5009, "AccessorAddedToAdapter"), "Accessor {AccessorId} added to adapter {AdapterId}");

    /// <summary>
    /// Logs when a group cannot be found for an item.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> GroupNotFoundForItemLogger =
        LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(5010, "GroupNotFoundForItem"), "Group {GroupName} not found for item {ItemId}");

    /// <summary>
    /// Logs when an adapter cannot be found for an item.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> AdapterNotFoundForItemLogger =
        LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(5011, "AdapterNotFoundForItem"), "Adapter {AdapterName} not found for item {ItemId}");

    /// <summary>
    /// Logs when an item is created.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> ItemCreatedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(5012, "ItemCreated"), "Item {ItemId} created ({SourceAddress})");

    /// <summary>
    /// Logs when an item is disposed.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> ItemDisposedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(5013, "ItemDisposed"), "Item {ItemId} disposed ({SourceAddress})");

    /// <summary>
    /// Logs when an unexpected error occurs during item write.
    /// </summary>
    public static readonly Action<ILogger, string, Exception?> ItemWriteErrorLogger =
        LoggerMessage.Define<string>(LogLevel.Error, new EventId(5014, "ItemWriteError"), "Unexpected error during write for item {ItemId}");
}
