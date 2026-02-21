// <copyright file="Program.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Configuration.Logging;
using Sensemation.Core.Acquisition.Configuration.Validators;
using Sensemation.Core.Acquisition.Demo.Console.Services.Configuration;
using Sensemation.Core.Acquisition.Demo.Console.Services.Managers;
using Sensemation.Core.Acquisition.Demo.Console.Services.Plugins;
using Sensemation.Core.Acquisition.Runtime.Converters;
using Sensemation.Core.Acquisition.Runtime.Services;

namespace Sensemation.Core.Acquisition.Demo.Console;

/// <summary>
/// Entry point for the acquisition console demo.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    /// <returns>The exit code.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1506:Avoid excessive class coupling", Justification = "Demo wiring")]
    public static async Task<int> Main(string[] args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var configurationPath = args.Length > 0
            ? ResolveConfigurationPath(args[0])
            : Path.Combine(Directory.GetCurrentDirectory(), "./docs/config-examples/acquisition.demo.json");

        var serviceConfig = ConfigurationService.LoadConfiguration(configurationPath);
        ConfigurationValidator.ValidateConfiguration(serviceConfig);

        var configBasePath = Path.GetDirectoryName(configurationPath);
        var pluginBasePath = ResolvePluginBasePath(configBasePath);

        EnsurePluginAssemblies(serviceConfig, pluginBasePath);

        var services = new ServiceCollection();
        _ = services.AddCustomLogging(serviceConfig.Logging);
        _ = services.AddSingleton<UpdateEventDispatcher>();
        _ = services.AddSingleton<IValueConverter, DefaultValueConverter>();
        _ = services.AddSingleton<PluginDiscoveryService>();
        _ = services.AddSingleton<AdapterManager>();
        _ = services.AddSingleton<SourceManager>();
        _ = services.AddSingleton<TriggerManager>();
        _ = services.AddSingleton<GroupManager>();
        _ = services.AddSingleton<CacheService>();
        _ = services.AddSingleton<ItemManager>();

        using var provider = services.BuildServiceProvider();
        var pluginDiscovery = provider.GetRequiredService<PluginDiscoveryService>();
        var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
        var pluginDescriptors = pluginDiscovery.LoadPlugins(serviceConfig.Plugins, pluginBasePath);

        var adapterManager = provider.GetRequiredService<AdapterManager>();
        adapterManager.LoadPluginDescriptors(pluginDescriptors);
        adapterManager.InitializePlugins(serviceConfig.Adapters, provider);

        var sourceManager = provider.GetRequiredService<SourceManager>();
        sourceManager.LoadPluginDescriptors(pluginDescriptors);
        sourceManager.InitializePlugins(serviceConfig.Sources, provider);

        var triggerManager = provider.GetRequiredService<TriggerManager>();
        triggerManager.LoadTriggerDescriptors(pluginDescriptors);
        triggerManager.InitializeTriggers(serviceConfig.Triggers);

        var groupManager = provider.GetRequiredService<GroupManager>();
        groupManager.CreateGroups(serviceConfig.Groups);

        var cacheService = provider.GetRequiredService<CacheService>();
        cacheService.InitializeCacheService(serviceConfig.Cache);

        var itemManager = provider.GetRequiredService<ItemManager>();
        itemManager.CreateItems(serviceConfig.Items);

        await adapterManager.StartAllAdapters().ConfigureAwait(false);
        await triggerManager.StartAllTriggers().ConfigureAwait(false);

        using var cancellationTokenSource = new CancellationTokenSource();
        System.Console.CancelKeyPress += (sender, eventArgs) =>
        {
            eventArgs.Cancel = true;
            cancellationTokenSource.Cancel();
        };

        try
        {
            await Task.Delay(Timeout.Infinite, cancellationTokenSource.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            await triggerManager.StopAllTriggers().ConfigureAwait(false);
            await adapterManager.StopAllAdapters().ConfigureAwait(false);

            (itemManager as IDisposable)?.Dispose();
            (groupManager as IDisposable)?.Dispose();
            (triggerManager as IDisposable)?.Dispose();
            (sourceManager as IDisposable)?.Dispose();
            (adapterManager as IDisposable)?.Dispose();
        }

        _ = loggerFactory;
        return 0;
    }

    private static string ResolvePluginBasePath(string? configBasePath)
    {
        if (!string.IsNullOrWhiteSpace(configBasePath))
        {
            var expectedPluginDirectory = Path.Combine(configBasePath, "plugins");
            if (Directory.Exists(expectedPluginDirectory))
            {
                return configBasePath;
            }

            var hasPluginAssembly = Directory.EnumerateFiles(configBasePath, "Sensemation.Core.Acquisition.*.dll").Any();
            if (hasPluginAssembly)
            {
                return configBasePath;
            }
        }

        return AppContext.BaseDirectory;
    }

    private static void EnsurePluginAssemblies(ServiceConfiguration config, string pluginBasePath)
    {
        if (!Directory.Exists(pluginBasePath))
        {
            return;
        }

        var resolvedAssemblies = new List<string>();
        foreach (var assembly in config.Plugins.Assemblies)
        {
            if (string.IsNullOrWhiteSpace(assembly))
            {
                continue;
            }

            var resolved = Path.IsPathRooted(assembly)
                ? assembly
                : Path.GetFullPath(Path.Combine(pluginBasePath, assembly));
            resolvedAssemblies.Add(resolved);
        }

        var existingAssemblies = new HashSet<string>(resolvedAssemblies, StringComparer.OrdinalIgnoreCase);
        var pluginAssemblies = Directory
            .EnumerateFiles(pluginBasePath, "Sensemation.Core.Acquisition.*.dll", SearchOption.TopDirectoryOnly)
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var assembly in pluginAssemblies)
        {
            if (existingAssemblies.Add(assembly))
            {
                resolvedAssemblies.Add(assembly);
            }
        }

        config.Plugins.Assemblies.Clear();
        foreach (var assembly in resolvedAssemblies)
        {
            config.Plugins.Assemblies.Add(assembly);
        }
    }

    private static string ResolveConfigurationPath(string path)
    {
        var trimmedPath = path.Trim().Trim('"');
        if (trimmedPath.Length == 0)
        {
            return path;
        }

        if (Path.IsPathRooted(trimmedPath) && File.Exists(trimmedPath))
        {
            return trimmedPath;
        }

        if (File.Exists(trimmedPath))
        {
            return Path.GetFullPath(trimmedPath);
        }

        var searchRoots = new[]
        {
            new DirectoryInfo(Directory.GetCurrentDirectory()),
            new DirectoryInfo(AppContext.BaseDirectory),
        };

        foreach (var root in searchRoots)
        {
            var current = root;
            while (current != null)
            {
                var candidate = Path.Combine(current.FullName, trimmedPath);
                if (File.Exists(candidate))
                {
                    return candidate;
                }

                var solutionPath = Path.Combine(current.FullName, "Sensemation.Core.sln");
                if (File.Exists(solutionPath))
                {
                    var solutionCandidate = Path.Combine(current.FullName, trimmedPath);
                    if (File.Exists(solutionCandidate))
                    {
                        return solutionCandidate;
                    }
                }

                current = current.Parent;
            }
        }

        return Path.GetFullPath(trimmedPath, Directory.GetCurrentDirectory());
    }
}
