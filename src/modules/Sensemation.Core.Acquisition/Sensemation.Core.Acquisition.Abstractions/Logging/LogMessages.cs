// <copyright file="LogMessages.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;

namespace Sensemation.Core.Acquisition.Abstractions.Logging;

/// <summary>
/// Provides strongly typed log message delegates for acquisition components.
/// </summary>
public static class LogMessages
{
    /// <summary>
    /// Logs when a plugin is created.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> PluginCreatedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(2000, "PluginCreated"), "Plugin {PluginType} '{PluginName}' created");

    /// <summary>
    /// Logs when a plugin begins initialization.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> PluginInitializingLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(2001, "PluginInitializing"), "Plugin {PluginType} '{PluginName}' initializing");

    /// <summary>
    /// Logs when a plugin completes initialization.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> PluginInitializedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(2002, "PluginInitialized"), "Plugin {PluginType} '{PluginName}' initialized");

    /// <summary>
    /// Logs when a plugin is disposed.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> PluginDisposedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(2003, "PluginDisposed"), "Plugin {PluginType} '{PluginName}' disposed");

    /// <summary>
    /// Logs when plugin initialization fails.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> PluginInitializationFailedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(2004, "PluginInitializationFailed"), "Plugin {PluginType} '{PluginName}' initialization failed");

    /// <summary>
    /// Logs when a plugin begins starting.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> PluginStartingLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(2005, "PluginStarting"), "Plugin {PluginType} '{PluginName}' starting");

    /// <summary>
    /// Logs when a plugin is started.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> PluginStartedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(2006, "PluginStarted"), "Plugin {PluginType} '{PluginName}' started");

    /// <summary>
    /// Logs when plugin start fails.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> PluginStartFailedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(2007, "PluginStartFailed"), "Plugin {PluginType} '{PluginName}' start failed");

    /// <summary>
    /// Logs when a plugin begins stopping.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> PluginStoppingLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(2008, "PluginStopping"), "Plugin {PluginType} '{PluginName}' stopping");

    /// <summary>
    /// Logs when a plugin is stopped.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> PluginStoppedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(2009, "PluginStopped"), "Plugin {PluginType} '{PluginName}' stopped");

    /// <summary>
    /// Logs when plugin stop fails.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> PluginStopFailedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(2010, "PluginStopFailed"), "Plugin {PluginType} '{PluginName}' stop failed");

    /// <summary>
    /// Logs when an adapter is started.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> AdapterStartedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(2011, "AdapterStarted"), "Adapter {AdapterType} '{AdapterName}' started");

    /// <summary>
    /// Logs when an adapter is stopped.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> AdapterStoppedLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(2012, "AdapterStopped"), "Adapter {AdapterType} '{AdapterName}' stopped");

    /// <summary>
    /// Logs when an adapter cannot be found for an item.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> AdapterNotFoundLogger =
        LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(2013, "AdapterNotFound"), "Adapter {AdapterName} not found for item {ItemId}");

    /// <summary>
    /// Logs when a source cannot be found for a group.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> SourceNotFoundLogger =
        LoggerMessage.Define<string, string>(LogLevel.Error, new EventId(2014, "SourceNotFound"), "Source {SourceName} not found for group {GroupName}");

    /// <summary>
    /// Logs when a source is assigned to a group.
    /// </summary>
    public static readonly Action<ILogger, string, string, Exception?> SourceAssignedToGroupLogger =
        LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(2015, "SourceAssignedToGroup"), "Source {SourceName} assigned to group {GroupName}");
}
