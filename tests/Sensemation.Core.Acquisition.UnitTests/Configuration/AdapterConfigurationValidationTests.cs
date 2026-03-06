// <copyright file="AdapterConfigurationValidationTests.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Sensemation.Core.Acquisition.Configuration.Models;
using Sensemation.Core.Acquisition.Configuration.Validators;

namespace Sensemation.Core.Acquisition.UnitTests.Configuration;

/// <summary>
/// Tests for adapter configuration validation.
/// </summary>
public class AdapterConfigurationValidationTests
{
    /// <summary>
    /// Ensures validation fails when the adapter id is missing.
    /// </summary>
    [Fact]
    public void ValidateAdapterConfigurationsShouldThrowForMissingId()
    {
        var adapters = new Collection<AdapterConfiguration>
        {
            new() { Id = " ", Type = "webapi" },
        };

        _ = Assert.Throws<ArgumentException>(() => AdapterConfigurationValidator.ValidateAdapterConfigurations(adapters));
    }

    /// <summary>
    /// Ensures validation fails when duplicate adapter ids are present.
    /// </summary>
    [Fact]
    public void ValidateAdapterConfigurationsShouldThrowForDuplicateIds()
    {
        var adapters = new Collection<AdapterConfiguration>
        {
            new() { Id = "adapter-1", Type = "webapi" },
            new() { Id = "adapter-1", Type = "webapi" },
        };

        _ = Assert.Throws<ArgumentException>(() => AdapterConfigurationValidator.ValidateAdapterConfigurations(adapters));
    }

    /// <summary>
    /// Ensures validation fails when the adapter type is missing.
    /// </summary>
    [Fact]
    public void ValidateAdapterConfigurationsShouldThrowForMissingType()
    {
        var adapters = new Collection<AdapterConfiguration>
        {
            new() { Id = "adapter-1", Type = string.Empty },
        };

        _ = Assert.Throws<ArgumentException>(() => AdapterConfigurationValidator.ValidateAdapterConfigurations(adapters));
    }
}
