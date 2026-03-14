// <copyright file="LoggingExtensions.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Sensemation.Core.Acquisition.Configuration.Models;
using Sensemation.Core.Foundation.Logging;

namespace Sensemation.Core.Acquisition.Configuration.Logging;

/// <summary>
/// Provides logging configuration extensions for acquisition services.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Adds the custom logging provider to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="loggingConfig">The logging configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddCustomLogging(this IServiceCollection services, LoggingConfiguration? loggingConfig = null)
    {
        // Use default configuration if none provided
        var config = loggingConfig ?? new LoggingConfiguration();

        // Configure logging based on environment
        _ = services.AddLogging(builder =>
        {
            _ = builder.ClearProviders();

            // Set minimum log level based on configuration
            var minLevel = ParseLogLevel(config.MinimumLevel);
            _ = builder.SetMinimumLevel(minLevel);

            // Clear existing host-level filter rules (e.g. Logging:LogLevel from appsettings)
            // so this custom acquisition logging configuration is authoritative.
            _ = builder.Services.Configure<LoggerFilterOptions>(options =>
            {
                options.MinLevel = minLevel;
                options.Rules.Clear();
            });

            // Add our custom console logger provider
            _ = builder.AddProvider(new CustomConsoleLoggerProvider(minLevel, config.Enabled, config.IncludeStackTraces));
        });

        return services;
    }

    /// <summary>
    /// Parses the configured minimum log level.
    /// </summary>
    /// <param name="level">The log level string.</param>
    /// <returns>The parsed <see cref="LogLevel"/>.</returns>
    private static LogLevel ParseLogLevel(string level)
    {
        return level.ToUpperInvariant() switch
        {
            "TRACE" => LogLevel.Trace,
            "DEBUG" => LogLevel.Debug,
            "INFORMATION" or "INFO" => LogLevel.Information,
            "WARNING" or "WARN" => LogLevel.Warning,
            "ERROR" => LogLevel.Error,
            "CRITICAL" => LogLevel.Critical,
            _ => LogLevel.Debug,
        };
    }
}
