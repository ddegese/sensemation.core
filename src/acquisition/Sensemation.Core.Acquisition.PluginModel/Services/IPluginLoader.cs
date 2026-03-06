// <copyright file="IPluginLoader.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.PluginModel.Models;

namespace Sensemation.Core.Acquisition.PluginModel.Services;

/// <summary>
/// Defines plugin loading functionality.
/// </summary>
public interface IPluginLoader
{
    /// <summary>
    /// Loads plugin descriptors using the specified options.
    /// </summary>
    /// <param name="options">The plugin load options.</param>
    /// <returns>The discovered plugin descriptors.</returns>
    public IReadOnlyCollection<PluginDescriptor> LoadPlugins(PluginLoadOptions options);
}
