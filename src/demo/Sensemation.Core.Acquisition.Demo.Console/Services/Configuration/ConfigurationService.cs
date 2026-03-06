// <copyright file="ConfigurationService.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Demo.Console.Services.Configuration.Loaders;

namespace Sensemation.Core.Acquisition.Demo.Console.Services.Configuration;

/// <summary>
/// Service to load and manage configuration from the sensemation.acquisition.json file.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by DI")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1852:Seal internal types", Justification = "Used by dependency injection")]
internal static class ConfigurationService
{
    /// <summary>
    /// Loads the configuration for the specified file path.
    /// </summary>
    /// <param name="configPath">The path to the configuration file.</param>
    /// <returns>The loaded configuration.</returns>
    public static ServiceConfiguration LoadConfiguration(string configPath)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile(configPath, false, true);

        var config = configurationBuilder.Build();

        var appConfig = new ServiceConfiguration
        {
            Logging = LoggingConfigurationLoader.LoadLogging(config),
            Cache = CacheConfigurationLoader.LoadCache(config),
        };

        var pluginsSection = config.GetSection("plugins");
        var scanDirectories = pluginsSection.GetSection("scanDirectories").Get<string[]>();
        if (scanDirectories != null)
        {
            foreach (var directory in scanDirectories)
            {
                appConfig.Plugins.ScanDirectories.Add(directory);
            }
        }

        var assemblies = pluginsSection.GetSection("assemblies").Get<string[]>();
        if (assemblies != null)
        {
            foreach (var assembly in assemblies)
            {
                appConfig.Plugins.Assemblies.Add(assembly);
            }
        }

        foreach (var adapter in AdapterConfigurationLoader.LoadAdapters(config))
        {
            appConfig.Adapters.Add(adapter);
        }

        foreach (var source in SourceConfigurationLoader.LoadSources(config))
        {
            appConfig.Sources.Add(source);
        }

        foreach (var trigger in TriggerConfigurationLoader.LoadTriggers(config))
        {
            appConfig.Triggers.Add(trigger);
        }

        foreach (var group in GroupConfigurationLoader.LoadGroups(config))
        {
            appConfig.Groups.Add(group);
        }

        foreach (var item in ItemConfigurationLoader.LoadItems(config))
        {
            appConfig.Items.Add(item);
        }

        return appConfig;
    }
}
