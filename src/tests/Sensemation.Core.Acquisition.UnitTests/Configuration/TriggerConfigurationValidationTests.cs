// <copyright file="TriggerConfigurationValidationTests.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Sensemation.Core.Acquisition.Configuration.Models;
using Sensemation.Core.Acquisition.Configuration.Validators;

namespace Sensemation.Core.Acquisition.UnitTests.Configuration;

/// <summary>
/// Tests for trigger configuration validation.
/// </summary>
public class TriggerConfigurationValidationTests
{
    /// <summary>
    /// Ensures validation fails when the trigger id is missing.
    /// </summary>
    [Fact]
    public void ValidateTriggerConfigurationsShouldThrowForMissingId()
    {
        var triggers = new Collection<TriggerConfiguration>
        {
            new() { Id = " ", Type = "time" },
        };

        _ = Assert.Throws<ArgumentException>(() => TriggerConfigurationValidator.ValidateTriggerConfigurations(triggers));
    }

    /// <summary>
    /// Ensures validation fails when duplicate trigger ids are present.
    /// </summary>
    [Fact]
    public void ValidateTriggerConfigurationsShouldThrowForDuplicateIds()
    {
        var triggers = new Collection<TriggerConfiguration>
        {
            new() { Id = "fast", Type = "time" },
            new() { Id = "fast", Type = "time" },
        };

        _ = Assert.Throws<ArgumentException>(() => TriggerConfigurationValidator.ValidateTriggerConfigurations(triggers));
    }

    /// <summary>
    /// Ensures validation fails when the trigger type is missing.
    /// </summary>
    [Fact]
    public void ValidateTriggerConfigurationsShouldThrowForMissingType()
    {
        var triggers = new Collection<TriggerConfiguration>
        {
            new() { Id = "fast", Type = string.Empty },
        };

        _ = Assert.Throws<ArgumentException>(() => TriggerConfigurationValidator.ValidateTriggerConfigurations(triggers));
    }
}
