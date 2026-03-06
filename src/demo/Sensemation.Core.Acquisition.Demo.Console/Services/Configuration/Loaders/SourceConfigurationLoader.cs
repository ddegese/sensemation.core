// <copyright file="SourceConfigurationLoader.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace Sensemation.Core.Acquisition.Demo.Console.Services.Configuration.Loaders;

/// <summary>
/// Service to load source configurations from the configuration.
/// </summary>
internal static class SourceConfigurationLoader
{
    /// <summary>
    /// Loads source configurations from the configuration.
    /// </summary>
    /// <param name="config">The configuration.</param>
    /// <returns>A collection of source configurations.</returns>
    public static Collection<SourceConfiguration> LoadSources(IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        var sources = new Collection<SourceConfiguration>();

        var sourcesSection = config.GetSection("sources");
        foreach (var sourceSection in sourcesSection.GetChildren())
        {
            var sourceConfig = new SourceConfiguration
            {
                Id = sourceSection["id"] ?? sourceSection["name"] ?? string.Empty,
                Type = sourceSection["type"] ?? string.Empty,
            };

            var parametersSection = sourceSection.GetSection("parameters");
            var arrayParameters = parametersSection.GetChildren().ToList();

            if (arrayParameters.Count > 0 && arrayParameters.First().GetChildren().Any())
            {
                foreach (var parameterSection in arrayParameters)
                {
                    sourceConfig.Parameters.Add(new ParameterConfiguration
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
                    sourceConfig.Parameters.Add(new ParameterConfiguration
                    {
                        Name = parameterSection.Key ?? string.Empty,
                        Value = parameterSection.Value ?? string.Empty,
                    });
                }
            }

            sources.Add(sourceConfig);
        }

        return sources;
    }
}
