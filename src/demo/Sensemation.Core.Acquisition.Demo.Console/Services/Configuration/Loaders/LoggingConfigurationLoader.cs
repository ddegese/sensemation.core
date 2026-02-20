// <copyright file="LoggingConfigurationLoader.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Demo.Console.Services.Configuration.Loaders;

/// <summary>
/// Service to load logging configurations from the configuration.
/// </summary>
internal static class LoggingConfigurationLoader
{
    /// <summary>
    /// Loads logging configuration from the configuration.
    /// </summary>
    /// <param name="config">The configuration.</param>
    /// <returns>The logging configuration.</returns>
    public static LoggingConfiguration LoadLogging(IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        var loggingSection = config.GetSection("logging");
        var loggingConfig = new LoggingConfiguration
        {
            Enabled = loggingSection.GetValue("enabled", true),
            MinimumLevel = loggingSection["minimumLevel"] ?? "Information",
            IncludeStackTraces = loggingSection.GetValue("includeStackTraces", true),
        };

        return loggingConfig;
    }
}
