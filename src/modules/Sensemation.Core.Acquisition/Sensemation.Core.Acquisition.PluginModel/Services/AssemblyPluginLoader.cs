// <copyright file="AssemblyPluginLoader.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Reflection;

using Sensemation.Core.Acquisition.Abstractions.Attributes;
using Sensemation.Core.Acquisition.Abstractions.Interfaces;
using Sensemation.Core.Acquisition.PluginModel.Models;

namespace Sensemation.Core.Acquisition.PluginModel.Services;

/// <summary>
/// Loads plugins from assemblies on disk.
/// </summary>
public sealed class AssemblyPluginLoader : IPluginLoader
{
    /// <summary>
    /// Loads plugin descriptors from the configured locations.
    /// </summary>
    /// <param name="options">The plugin load options.</param>
    /// <returns>The discovered plugin descriptors.</returns>
    public IReadOnlyCollection<PluginDescriptor> LoadPlugins(PluginLoadOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var assemblies = new List<Assembly>();
        foreach (var directory in options.ScanDirectories)
        {
            if (!Directory.Exists(directory))
            {
                continue;
            }

            foreach (var assemblyPath in Directory.EnumerateFiles(directory, "*.dll", SearchOption.TopDirectoryOnly))
            {
                assemblies.Add(Assembly.LoadFrom(assemblyPath));
            }
        }

        foreach (var assemblyPath in options.Assemblies)
        {
            if (File.Exists(assemblyPath))
            {
                assemblies.Add(Assembly.LoadFrom(assemblyPath));
            }
        }

        return assemblies.SelectMany(this.GetPluginDescriptors).ToList();
    }

    private IEnumerable<PluginDescriptor> GetPluginDescriptors(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAbstract)
            {
                continue;
            }

            var attribute = type.GetCustomAttribute<PluginTypeAttribute>();
            if (attribute == null)
            {
                continue;
            }

            var version = type.Assembly.GetName().Version ?? new Version(1, 0, 0, 0);

            if (typeof(ISource).IsAssignableFrom(type))
            {
                yield return new PluginDescriptor
                {
                    PluginType = type,
                    Kind = "Source",
                    Id = attribute.Id,
                    Version = version,
                };
            }
            else if (typeof(IAdapter).IsAssignableFrom(type))
            {
                yield return new PluginDescriptor
                {
                    PluginType = type,
                    Kind = "Adapter",
                    Id = attribute.Id,
                    Version = version,
                };
            }
            else if (typeof(ITrigger).IsAssignableFrom(type))
            {
                yield return new PluginDescriptor
                {
                    PluginType = type,
                    Kind = "Trigger",
                    Id = attribute.Id,
                    Version = version,
                };
            }
        }
    }
}
