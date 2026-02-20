// <copyright file="SourceConfigurationValidationTests.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Sensemation.Core.Acquisition.Configuration.Models;
using Sensemation.Core.Acquisition.Configuration.Validators;

namespace Sensemation.Core.Acquisition.UnitTests.Configuration;

/// <summary>
/// Tests for source configuration validation.
/// </summary>
public class SourceConfigurationValidationTests
{
    /// <summary>
    /// Ensures validation fails when the source id is missing.
    /// </summary>
    [Fact]
    public void ValidateSourceConfigurationsShouldThrowForMissingId()
    {
        var sources = new Collection<SourceConfiguration>
        {
            new() { Id = " ", Type = "memory" },
        };

        _ = Assert.Throws<ArgumentException>(() => SourceConfigurationValidator.ValidateSourceConfigurations(sources));
    }

    /// <summary>
    /// Ensures validation fails when duplicate source ids are present.
    /// </summary>
    [Fact]
    public void ValidateSourceConfigurationsShouldThrowForDuplicateIds()
    {
        var sources = new Collection<SourceConfiguration>
        {
            new() { Id = "memory", Type = "memory" },
            new() { Id = "memory", Type = "memory" },
        };

        _ = Assert.Throws<ArgumentException>(() => SourceConfigurationValidator.ValidateSourceConfigurations(sources));
    }

    /// <summary>
    /// Ensures validation fails when the source type is missing.
    /// </summary>
    [Fact]
    public void ValidateSourceConfigurationsShouldThrowForMissingType()
    {
        var sources = new Collection<SourceConfiguration>
        {
            new() { Id = "memory", Type = string.Empty },
        };

        _ = Assert.Throws<ArgumentException>(() => SourceConfigurationValidator.ValidateSourceConfigurations(sources));
    }
}
