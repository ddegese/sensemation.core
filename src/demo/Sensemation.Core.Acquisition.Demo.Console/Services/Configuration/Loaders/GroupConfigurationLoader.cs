// <copyright file="GroupConfigurationLoader.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace Sensemation.Core.Acquisition.Demo.Console.Services.Configuration.Loaders;

/// <summary>
/// Service to load group configurations from the configuration.
/// </summary>
internal static class GroupConfigurationLoader
{
    /// <summary>
    /// Loads group configurations from the configuration.
    /// </summary>
    /// <param name="config">The configuration.</param>
    /// <returns>A collection of group configurations.</returns>
    public static Collection<GroupConfiguration> LoadGroups(IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        var groups = new Collection<GroupConfiguration>();

        var groupsSection = config.GetSection("groups");
        foreach (var groupSection in groupsSection.GetChildren())
        {
            groups.Add(new GroupConfiguration
            {
                Id = groupSection["id"] ?? groupSection["name"] ?? string.Empty,
                Source = groupSection["source"] ?? string.Empty,
                Trigger = groupSection["trigger"] ?? string.Empty,
                StaleThresholdSeconds = int.TryParse(groupSection["staleThresholdSeconds"], out var staleSeconds)
                    ? staleSeconds
                    : null,
            });
        }

        return groups;
    }
}
