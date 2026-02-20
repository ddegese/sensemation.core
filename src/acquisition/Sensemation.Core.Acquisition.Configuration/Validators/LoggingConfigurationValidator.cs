// <copyright file="LoggingConfigurationValidator.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Configuration.Models;

namespace Sensemation.Core.Acquisition.Configuration.Validators;

/// <summary>
/// Validates logging configuration settings.
/// </summary>
public static class LoggingConfigurationValidator
{
    /// <summary>
    /// Validates the logging configuration.
    /// </summary>
    /// <param name="logging">The logging configuration.</param>
    public static void ValidateLoggingConfiguration(LoggingConfiguration? logging)
    {
        if (logging == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(logging.MinimumLevel))
        {
            throw new ArgumentException("Logging minimum level cannot be null, empty or whitespace-only");
        }
    }
}
