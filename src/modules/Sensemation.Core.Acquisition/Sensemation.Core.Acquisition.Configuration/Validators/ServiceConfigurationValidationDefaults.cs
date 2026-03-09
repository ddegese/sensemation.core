// <copyright file="ServiceConfigurationValidationDefaults.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Configuration.Models;

namespace Sensemation.Core.Acquisition.Configuration.Validators;

/// <summary>
/// Provides the default ordered validator composition for <see cref="ServiceConfiguration"/>.
/// </summary>
public static class ServiceConfigurationValidationDefaults
{
    /// <summary>
    /// Creates the default ordered validators for service configuration.
    /// </summary>
    /// <returns>An ordered validator sequence.</returns>
    public static IReadOnlyList<IConfigurationValidator<ServiceConfiguration>> CreateValidators()
    {
        return
        [
            new AdaptersSectionValidator(),
            new SourcesSectionValidator(),
            new TriggersSectionValidator(),
            new GroupsSectionValidator(),
            new ItemsSectionValidator(),
            new LoggingSectionValidator(),
            new CacheSectionValidator(),
            new PluginsSectionValidator(),
        ];
    }
}
