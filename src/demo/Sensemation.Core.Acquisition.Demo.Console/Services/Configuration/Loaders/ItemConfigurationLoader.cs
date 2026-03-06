// <copyright file="ItemConfigurationLoader.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

namespace Sensemation.Core.Acquisition.Demo.Console.Services.Configuration.Loaders;

/// <summary>
/// Service to load item configurations from the configuration.
/// </summary>
internal static class ItemConfigurationLoader
{
    /// <summary>
    /// Loads item configurations from the configuration.
    /// </summary>
    /// <param name="config">The configuration.</param>
    /// <returns>A collection of item configurations.</returns>
    public static Collection<ItemConfiguration> LoadItems(IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        var items = new Collection<ItemConfiguration>();

        var itemsSection = config.GetSection("items");
        foreach (var itemSection in itemsSection.GetChildren())
        {
            var itemConfig = new ItemConfiguration
            {
                SourceAddress = itemSection["sourceAddress"] ?? itemSection["sourceaddress"] ?? string.Empty,
                Group = itemSection["group"] ?? string.Empty,
                Datatype = itemSection["datatype"] ?? string.Empty,
                Id = itemSection["id"] ?? itemSection["identifier"],
                CacheSize = int.TryParse(itemSection["cacheSize"] ?? itemSection["cachesize"], out var cacheSize)
                    ? cacheSize
                    : null,
            };

            var adaptersArray = itemSection.GetSection("adapters").Get<string[]>();
            if (adaptersArray != null)
            {
                foreach (var adapter in adaptersArray)
                {
                    itemConfig.Adapters.Add(adapter);
                }
            }

            items.Add(itemConfig);
        }

        return items;
    }
}
