// <copyright file="PluginLoadConfigurationValidator.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Configuration.Models;

namespace Sensemation.Core.Acquisition.Configuration.Validators;

/// <summary>
/// Validates plugin loading configuration settings.
/// </summary>
public static class PluginLoadConfigurationValidator
{
    /// <summary>
    /// Validates the plugin loading configuration.
    /// </summary>
    /// <param name="plugins">The plugin loading configuration.</param>
    public static void ValidatePluginLoadConfiguration(PluginLoadConfiguration? plugins)
    {
        if (plugins == null)
        {
            return;
        }

        if (plugins.ScanDirectories.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException("Plugin scan directories cannot contain null or whitespace-only values.");
        }

        if (plugins.Assemblies.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException("Plugin assemblies cannot contain null or whitespace-only values.");
        }
    }
}
