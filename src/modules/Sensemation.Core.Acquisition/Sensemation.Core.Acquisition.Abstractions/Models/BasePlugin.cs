// <copyright file="BasePlugin.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;

using Sensemation.Core.Acquisition.Abstractions.Interfaces;
using Sensemation.Core.Acquisition.Abstractions.Logging;

namespace Sensemation.Core.Acquisition.Abstractions.Models;

/// <summary>
/// Provides a base implementation for acquisition plugins.
/// </summary>
public abstract class BasePlugin : IPlugin, IDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BasePlugin"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="id">The plugin identifier.</param>
    /// <param name="parameters">The plugin parameters.</param>
    protected BasePlugin(ILogger logger, string id, Dictionary<string, string> parameters)
    {
        this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.Id = id;
        this.Parameters = parameters ?? [];

        LogMessages.PluginCreated(this.Logger, this.GetType().Name, this.Id, null);
    }

    /// <inheritdoc />
    public string Id { get; } = string.Empty;

    /// <summary>
    /// Gets the plugin parameters.
    /// </summary>
    /// <value>The plugin parameters.</value>
    protected Dictionary<string, string> Parameters { get; }

    /// <summary>
    /// Gets the logger instance.
    /// </summary>
    /// <value>The logger instance.</value>
    protected ILogger Logger { get; private set; }

    /// <inheritdoc />
    public void Initialize()
    {
        LogMessages.PluginInitializing(this.Logger, this.GetType().Name, this.Id, null);

        try
        {
            this.InitializeCore();
            LogMessages.PluginInitialized(this.Logger, this.GetType().Name, this.Id, null);
        }
        catch (Exception ex)
        {
            LogMessages.PluginInitializationFailed(this.Logger, this.GetType().Name, this.Id, ex);
            throw;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Performs plugin initialization logic.
    /// </summary>
    protected abstract void InitializeCore();

    /// <summary>
    /// Disposes managed resources.
    /// </summary>
    /// <param name="disposing">Whether the method is disposing managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            LogMessages.PluginDisposed(this.Logger, this.GetType().Name, this.Id, null);
        }
    }
}
