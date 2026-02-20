// <copyright file="PluginLoadConfigurationValidationTests.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Configuration.Models;
using Sensemation.Core.Acquisition.Configuration.Validators;

namespace Sensemation.Core.Acquisition.UnitTests.Configuration;

/// <summary>
/// Tests for plugin load configuration validation.
/// </summary>
public class PluginLoadConfigurationValidationTests
{
    /// <summary>
    /// Ensures validation fails when a scan directory entry is missing.
    /// </summary>
    [Fact]
    public void ValidatePluginLoadConfigurationShouldThrowForMissingScanDirectoriesEntry()
    {
        var plugins = new PluginLoadConfiguration();
        plugins.ScanDirectories.Add(" ");

        _ = Assert.Throws<ArgumentException>(() => PluginLoadConfigurationValidator.ValidatePluginLoadConfiguration(plugins));
    }

    /// <summary>
    /// Ensures validation fails when an assembly entry is missing.
    /// </summary>
    [Fact]
    public void ValidatePluginLoadConfigurationShouldThrowForMissingAssemblyEntry()
    {
        var plugins = new PluginLoadConfiguration();
        plugins.Assemblies.Add(" ");

        _ = Assert.Throws<ArgumentException>(() => PluginLoadConfigurationValidator.ValidatePluginLoadConfiguration(plugins));
    }
}
