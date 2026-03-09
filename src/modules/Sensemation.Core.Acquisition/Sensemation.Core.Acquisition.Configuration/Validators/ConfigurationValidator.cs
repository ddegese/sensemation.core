// <copyright file="ConfigurationValidator.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Configuration.Models;

namespace Sensemation.Core.Acquisition.Configuration.Validators;

/// <summary>
/// Validates the full service configuration.
/// </summary>
public static class ConfigurationValidator
{
    private static readonly ConfigurationValidationPipeline<ServiceConfiguration> Pipeline =
        new(CreateValidators());

    /// <summary>
    /// Validates the provided service configuration.
    /// </summary>
    /// <param name="configuration">The service configuration.</param>
    public static void ValidateConfiguration(ServiceConfiguration configuration)
    {
        Pipeline.Validate(configuration);
    }

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
