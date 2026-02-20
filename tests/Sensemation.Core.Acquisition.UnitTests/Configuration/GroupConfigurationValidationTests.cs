// <copyright file="GroupConfigurationValidationTests.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Sensemation.Core.Acquisition.Configuration.Models;
using Sensemation.Core.Acquisition.Configuration.Validators;

namespace Sensemation.Core.Acquisition.UnitTests.Configuration;

/// <summary>
/// Tests for group configuration validation.
/// </summary>
public class GroupConfigurationValidationTests
{
    /// <summary>
    /// Ensures validation fails when the group id is missing.
    /// </summary>
    [Fact]
    public void ValidateGroupConfigurationsShouldThrowForMissingId()
    {
        var groups = new Collection<GroupConfiguration>
        {
            new() { Id = " ", Source = "memory", Trigger = "fast" },
        };

        _ = Assert.Throws<ArgumentException>(() => GroupConfigurationValidator.ValidateGroupConfigurations(groups, null, null));
    }

    /// <summary>
    /// Ensures validation fails when duplicate group ids are present.
    /// </summary>
    [Fact]
    public void ValidateGroupConfigurationsShouldThrowForDuplicateIds()
    {
        var groups = new Collection<GroupConfiguration>
        {
            new() { Id = "group-1", Source = "memory", Trigger = "fast" },
            new() { Id = "group-1", Source = "memory", Trigger = "fast" },
        };

        _ = Assert.Throws<ArgumentException>(() => GroupConfigurationValidator.ValidateGroupConfigurations(groups, null, null));
    }

    /// <summary>
    /// Ensures validation fails when the group source is missing.
    /// </summary>
    [Fact]
    public void ValidateGroupConfigurationsShouldThrowForMissingSource()
    {
        var groups = new Collection<GroupConfiguration>
        {
            new() { Id = "group-1", Source = " ", Trigger = "fast" },
        };

        _ = Assert.Throws<ArgumentException>(() => GroupConfigurationValidator.ValidateGroupConfigurations(groups, null, null));
    }

    /// <summary>
    /// Ensures validation fails when the group trigger is missing.
    /// </summary>
    [Fact]
    public void ValidateGroupConfigurationsShouldThrowForMissingTrigger()
    {
        var groups = new Collection<GroupConfiguration>
        {
            new() { Id = "group-1", Source = "memory", Trigger = string.Empty },
        };

        _ = Assert.Throws<ArgumentException>(() => GroupConfigurationValidator.ValidateGroupConfigurations(groups, null, null));
    }

    /// <summary>
    /// Ensures validation fails when the stale threshold is invalid.
    /// </summary>
    [Fact]
    public void ValidateGroupConfigurationsShouldThrowForInvalidStaleThreshold()
    {
        var groups = new Collection<GroupConfiguration>
        {
            new() { Id = "group-1", Source = "memory", Trigger = "fast", StaleThresholdSeconds = 0 },
        };

        _ = Assert.Throws<ArgumentException>(() => GroupConfigurationValidator.ValidateGroupConfigurations(groups, null, null));
    }

    /// <summary>
    /// Ensures validation fails when the source reference is missing.
    /// </summary>
    [Fact]
    public void ValidateGroupConfigurationsShouldThrowForMissingSourceReference()
    {
        var groups = new Collection<GroupConfiguration>
        {
            new() { Id = "group-1", Source = "missing", Trigger = "fast" },
        };
        var sources = new Collection<SourceConfiguration>
        {
            new() { Id = "memory", Type = "memory" },
        };
        var triggers = new Collection<TriggerConfiguration>
        {
            new() { Id = "fast", Type = "time" },
        };

        _ = Assert.Throws<ArgumentException>(() => GroupConfigurationValidator.ValidateGroupConfigurations(groups, sources, triggers));
    }

    /// <summary>
    /// Ensures validation fails when the trigger reference is missing.
    /// </summary>
    [Fact]
    public void ValidateGroupConfigurationsShouldThrowForMissingTriggerReference()
    {
        var groups = new Collection<GroupConfiguration>
        {
            new() { Id = "group-1", Source = "memory", Trigger = "missing" },
        };
        var sources = new Collection<SourceConfiguration>
        {
            new() { Id = "memory", Type = "memory" },
        };
        var triggers = new Collection<TriggerConfiguration>
        {
            new() { Id = "fast", Type = "time" },
        };

        _ = Assert.Throws<ArgumentException>(() => GroupConfigurationValidator.ValidateGroupConfigurations(groups, sources, triggers));
    }
}
