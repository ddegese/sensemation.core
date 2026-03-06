// <copyright file="PluginDiscoveryService.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.PluginModel.Models;
using Sensemation.Core.Acquisition.PluginModel.Services;

namespace Sensemation.Core.Acquisition.Demo.Console.Services.Plugins;

/// <summary>
/// Loads plugin descriptors from configuration and resolves assembly paths.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by DI")]
internal sealed class PluginDiscoveryService
{
    private readonly AssemblyPluginLoader loader = new();

    /// <summary>
    /// Loads plugin descriptors for the configured plugin load settings.
    /// </summary>
    /// <param name="plugins">The plugin configuration.</param>
    /// <param name="basePath">The base path used to resolve relative plugin paths.</param>
    /// <returns>The discovered plugin descriptors.</returns>
    public IReadOnlyCollection<PluginDescriptor> LoadPlugins(PluginLoadConfiguration plugins, string basePath)
    {
        ArgumentNullException.ThrowIfNull(plugins);
        ArgumentNullException.ThrowIfNull(basePath);

        var options = new PluginLoadOptions();

        foreach (var directory in plugins.ScanDirectories)
        {
            options.ScanDirectories.Add(ResolvePluginPath(basePath, directory));
        }

        foreach (var assembly in plugins.Assemblies)
        {
            options.Assemblies.Add(ResolvePluginPath(basePath, assembly));
        }

        return this.loader.LoadPlugins(options);
    }

    private static string ResolvePluginPath(string basePath, string path) => Path.IsPathRooted(path)
            ? path
            : Path.GetFullPath(Path.Combine(basePath, path));
}
