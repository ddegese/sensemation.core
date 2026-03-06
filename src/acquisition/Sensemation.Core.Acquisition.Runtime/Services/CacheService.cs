// <copyright file="CacheService.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Text.Json;

using Microsoft.Extensions.Logging;

using Sensemation.Core.Acquisition.Configuration.Models;
using Sensemation.Core.Contracts.Serialization;

namespace Sensemation.Core.Acquisition.Runtime.Services;

/// <summary>
/// Service for persisting caches to disk.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CacheService"/> class.
/// </remarks>
/// <param name="logger">The logger.</param>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by DI")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1852:Seal internal types", Justification = "Used by dependency injection")]
public class CacheService(ILogger<CacheService> logger) : IDisposable
{
#pragma warning disable CA1848
    private readonly ILogger<CacheService> logger = logger;
    private readonly JsonSerializerOptions serializerOptions = CreateSerializerOptions();
    private CacheConfiguration? config;
    private string? cacheDirectory;
    private bool disposed;

    /// <summary>
    /// Initializes the service with the provided configuration.
    /// </summary>
    /// <param name="config">The cache configuration.</param>
    public void InitializeCacheService(CacheConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config);
        this.config = config;
        this.cacheDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, config.BaseFolder);

        if (!Directory.Exists(this.cacheDirectory))
        {
            _ = Directory.CreateDirectory(this.cacheDirectory);
        }
    }

    /// <summary>
    /// Saves the cache for a collection of items.
    /// </summary>
    /// <typeparam name="T">The type of data being saved.</typeparam>
    /// <param name="identifier">The identifier for the cache file.</param>
    /// <param name="data">The data to save.</param>
    /// <param name="category">The category subfolder (e.g., "items").</param>
    public void SaveCache<T>(string identifier, T data, string category = "")
    {
        if (this.config == null || this.cacheDirectory == null)
        {
            throw new ArgumentException("CacheService must be initialized before use.");
        }

        if (!this.config.Enabled)
        {
            return;
        }

        try
        {
            var targetDirectory = Path.Combine(this.cacheDirectory, category);
            if (!Directory.Exists(targetDirectory))
            {
                _ = Directory.CreateDirectory(targetDirectory);
            }

            var filePath = Path.Combine(targetDirectory, $"{identifier}.json");
            var json = JsonSerializer.Serialize(data, this.serializerOptions);
            File.WriteAllText(filePath, json);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to save cache for {Identifier}", identifier);
        }
    }

    /// <summary>
    /// Loads the cache for an identifier from disk.
    /// </summary>
    /// <typeparam name="T">The type of data being loaded.</typeparam>
    /// <param name="identifier">The identifier for the cache file.</param>
    /// <param name="category">The category subfolder (e.g., "items").</param>
    /// <returns>The loaded data, or default if not found or error.</returns>
    public T? LoadCache<T>(string identifier, string category = "")
    {
        if (this.config == null || this.cacheDirectory == null)
        {
            throw new ArgumentException("CacheService must be initialized before use.");
        }

        if (!this.config.Enabled)
        {
            return default;
        }

        try
        {
            var filePath = Path.Combine(this.cacheDirectory, category, $"{identifier}.json");
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<T>(json, this.serializerOptions);
            }
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Failed to load cache for {Identifier}", identifier);
        }

        return default;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the resources.
    /// </summary>
    /// <param name="disposing">True if disposing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            this.disposed = true;
        }
    }

    private static JsonSerializerOptions CreateSerializerOptions()
    {
        var options = new JsonSerializerOptions(JsonSerializerDefaults.General);
        options.Converters.Add(new DataPointJsonConverter());
        return options;
    }
}
