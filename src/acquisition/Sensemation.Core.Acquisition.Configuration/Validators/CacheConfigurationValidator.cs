// <copyright file="CacheConfigurationValidator.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Configuration.Models;

namespace Sensemation.Core.Acquisition.Configuration.Validators;

/// <summary>
/// Validates cache configuration settings.
/// </summary>
public static class CacheConfigurationValidator
{
    /// <summary>
    /// Validates the cache configuration.
    /// </summary>
    /// <param name="cache">The cache configuration.</param>
    public static void ValidateCacheConfiguration(CacheConfiguration? cache)
    {
        if (cache == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(cache.BaseFolder))
        {
            throw new ArgumentException("Cache base folder cannot be null, empty or whitespace-only");
        }
    }
}
