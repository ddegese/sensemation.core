// <copyright file="AdapterConfigurationLoader.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace Sensemation.Core.Acquisition.Demo.Console.Services.Configuration.Loaders;

/// <summary>
/// Service to load adapter configurations from the configuration.
/// </summary>
internal static class AdapterConfigurationLoader
{
    /// <summary>
    /// Loads adapter configurations from the configuration.
    /// </summary>
    /// <param name="config">The configuration.</param>
    /// <returns>A collection of adapter configurations.</returns>
    public static Collection<AdapterConfiguration> LoadAdapters(IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        var adapters = new Collection<AdapterConfiguration>();

        var adaptersSection = config.GetSection("adapters");
        foreach (var adapterSection in adaptersSection.GetChildren())
        {
            var adapterConfig = new AdapterConfiguration
            {
                Id = adapterSection["id"] ?? adapterSection["name"] ?? string.Empty,
                Type = adapterSection["type"] ?? string.Empty,
            };

            var parametersSection = adapterSection.GetSection("parameters");
            var arrayParameters = parametersSection.GetChildren().ToList();

            if (arrayParameters.Count > 0 && arrayParameters.First().GetChildren().Any())
            {
                foreach (var parameterSection in arrayParameters)
                {
                    adapterConfig.Parameters.Add(new ParameterConfiguration
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
                    adapterConfig.Parameters.Add(new ParameterConfiguration
                    {
                        Name = parameterSection.Key ?? string.Empty,
                        Value = parameterSection.Value ?? string.Empty,
                    });
                }
            }

            adapters.Add(adapterConfig);
        }

        return adapters;
    }
}
