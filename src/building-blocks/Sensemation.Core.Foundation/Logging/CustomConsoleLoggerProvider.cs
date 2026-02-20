// <copyright file="CustomConsoleLoggerProvider.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;

namespace Sensemation.Core.Foundation.Logging;

/// <summary>
/// Provides <see cref="CustomConsoleLogger"/> instances for dependency injection.
/// </summary>
public class CustomConsoleLoggerProvider : ILoggerProvider
{
    private readonly LogLevel logLevel;
    private readonly bool loggingEnabled;
    private readonly bool includeStackTraces;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomConsoleLoggerProvider"/> class.
    /// </summary>
    /// <param name="logLevel">Level to be checked.</param>
    /// <param name="loggingEnabled">Whether logging is enabled.</param>
    /// <param name="includeStackTraces">Whether to include stack traces in output.</param>
    public CustomConsoleLoggerProvider(LogLevel logLevel, bool loggingEnabled = true, bool includeStackTraces = true)
    {
        this.logLevel = logLevel;
        this.loggingEnabled = loggingEnabled;
        this.includeStackTraces = includeStackTraces;
    }

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName)
    {
        return new CustomConsoleLogger(categoryName, this.logLevel, this.loggingEnabled, this.includeStackTraces);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the provider resources.
    /// </summary>
    /// <param name="disposing">Whether the method was called from <see cref="Dispose()"/>.</param>
    protected virtual void Dispose(bool disposing)
    {
        // Nothing to dispose
    }
}
