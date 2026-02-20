// <copyright file="CustomConsoleLogger.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;

namespace Sensemation.Core.Foundation.Logging;

/// <summary>
/// Provides a console logger that uses <see cref="CustomLogFormatter"/> output.
/// </summary>
public sealed class CustomConsoleLogger : ILogger
{
    private readonly CustomLogFormatter formatter;
    private readonly LogLevel logLevel;
    private readonly bool loggingEnabled;
    private readonly bool includeStackTraces;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomConsoleLogger"/> class.
    /// </summary>
    /// <param name="categoryName">The logger category name.</param>
    /// <param name="logLevel">Level to be checked.</param>
    /// <param name="loggingEnabled">Whether logging is enabled.</param>
    /// <param name="includeStackTraces">Whether to include stack traces in output.</param>
    public CustomConsoleLogger(string categoryName, LogLevel logLevel, bool loggingEnabled = true, bool includeStackTraces = true)
    {
        this.formatter = new CustomLogFormatter(categoryName, includeStackTraces);
        this.logLevel = logLevel;
        this.loggingEnabled = loggingEnabled;
        this.includeStackTraces = includeStackTraces;
    }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel) => this.loggingEnabled && logLevel >= this.logLevel;

    /// <inheritdoc />
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!this.IsEnabled(logLevel))
        {
            return;
        }

        var message = this.formatter.Format(state, this.includeStackTraces ? exception : null, logLevel, eventId, formatter);
        Console.WriteLine(message);
    }

    /// <inheritdoc />
    IDisposable ILogger.BeginScope<TState>(TState state) => NullScope.Instance;

    /// <summary>
    /// Represents a no-op logging scope.
    /// </summary>
    private sealed class NullScope : IDisposable
    {
        private NullScope()
        {
        }

        /// <summary>
        /// Gets the singleton instance of the null scope.
        /// </summary>
        /// <value>The shared <see cref="NullScope"/> instance.</value>
        public static NullScope Instance { get; } = new NullScope();

        /// <inheritdoc />
        public void Dispose()
        {
        }
    }
}
