// <copyright file="CacheConfigurationLoader.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Demo.Console.Services.Configuration.Loaders;

/// <summary>
/// Service to load cache configurations from the configuration.
/// </summary>
internal static class CacheConfigurationLoader
{
    /// <summary>
    /// Loads cache configuration from the configuration.
    /// </summary>
    /// <param name="config">The configuration.</param>
    /// <returns>The cache configuration.</returns>
    public static CacheConfiguration LoadCache(IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);

        var cacheSection = config.GetSection("cache");
        var cacheConfig = new CacheConfiguration
        {
            BaseFolder = cacheSection["baseFolder"] ?? "cache",
            Enabled = cacheSection.GetValue("enabled", false),
            DefaultCacheSize = cacheSection.GetValue("defaultCacheSize", 50),
        };

        return cacheConfig;
    }
}
