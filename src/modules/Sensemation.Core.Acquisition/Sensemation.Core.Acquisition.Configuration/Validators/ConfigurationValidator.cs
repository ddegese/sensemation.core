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
        new(ServiceConfigurationValidationDefaults.CreateValidators());

    /// <summary>
    /// Validates the provided service configuration.
    /// </summary>
    /// <param name="configuration">The service configuration.</param>
    public static void ValidateConfiguration(ServiceConfiguration configuration)
    {
        Pipeline.Validate(configuration);
    }
}
