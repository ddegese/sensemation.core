// <copyright file="CacheConfigurationValidationTests.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Configuration.Models;
using Sensemation.Core.Acquisition.Configuration.Validators;

namespace Sensemation.Core.Acquisition.UnitTests.Configuration;

/// <summary>
/// Tests for cache configuration validation.
/// </summary>
public class CacheConfigurationValidationTests
{
    /// <summary>
    /// Ensures validation fails when the cache base folder is missing.
    /// </summary>
    [Fact]
    public void ValidateCacheConfigurationShouldThrowForMissingBaseFolder()
    {
        var cache = new CacheConfiguration { BaseFolder = " " };

        _ = Assert.Throws<ArgumentException>(() => CacheConfigurationValidator.ValidateCacheConfiguration(cache));
    }
}
