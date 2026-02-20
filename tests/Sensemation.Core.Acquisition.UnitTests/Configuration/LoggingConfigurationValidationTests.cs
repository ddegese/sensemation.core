// <copyright file="LoggingConfigurationValidationTests.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Configuration.Models;
using Sensemation.Core.Acquisition.Configuration.Validators;

namespace Sensemation.Core.Acquisition.UnitTests.Configuration;

/// <summary>
/// Tests for logging configuration validation.
/// </summary>
public class LoggingConfigurationValidationTests
{
    /// <summary>
    /// Ensures validation fails when the minimum log level is missing.
    /// </summary>
    [Fact]
    public void ValidateLoggingConfigurationShouldThrowForMissingMinimumLevel()
    {
        var logging = new LoggingConfiguration { MinimumLevel = " " };

        _ = Assert.Throws<ArgumentException>(() => LoggingConfigurationValidator.ValidateLoggingConfiguration(logging));
    }
}
