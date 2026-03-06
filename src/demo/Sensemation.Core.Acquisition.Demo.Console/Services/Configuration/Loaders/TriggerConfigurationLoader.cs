// <copyright file="TriggerConfigurationLoader.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace Sensemation.Core.Acquisition.Demo.Console.Services.Configuration.Loaders;

/// <summary>
/// Service to load trigger configurations from the configuration.
/// </summary>
internal static class TriggerConfigurationLoader
{
    /// <summary>
    /// Loads trigger configurations from the configuration.
    /// </summary>
    /// <param name="config">The configuration.</param>
    /// <returns>A collection of trigger configurations.</returns>
    public static Collection<TriggerConfiguration> LoadTriggers(IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        var triggers = new Collection<TriggerConfiguration>();

        var triggersSection = config.GetSection("triggers");
        foreach (var triggerSection in triggersSection.GetChildren())
        {
            var triggerConfig = new TriggerConfiguration
            {
                Id = triggerSection["id"] ?? triggerSection["name"] ?? string.Empty,
                Type = triggerSection["type"] ?? string.Empty,
            };

            var parametersSection = triggerSection.GetSection("parameters");
            var arrayParameters = parametersSection.GetChildren().ToList();

            if (arrayParameters.Count > 0 && arrayParameters.First().GetChildren().Any())
            {
                foreach (var parameterSection in arrayParameters)
                {
                    triggerConfig.Parameters.Add(new ParameterConfiguration
                    {
                        Name = parameterSection["name"] ?? string.Empty,
                        Value = parameterSection["value"] ?? string.Empty,
                    });
                }
            }
            else
            {
                foreach (var parameterSection in parametersSection.GetChildren())
                {
                    triggerConfig.Parameters.Add(new ParameterConfiguration
                    {
                        Name = parameterSection.Key ?? string.Empty,
                        Value = parameterSection.Value ?? string.Empty,
                    });
                }
            }

            triggers.Add(triggerConfig);
        }

        return triggers;
    }
}
