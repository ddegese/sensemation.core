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
    /// <summary>
    /// Validates the provided service configuration.
    /// </summary>
    /// <param name="configuration">The service configuration.</param>
    public static void ValidateConfiguration(ServiceConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        AdapterConfigurationValidator.ValidateAdapterConfigurations(configuration.Adapters);
        SourceConfigurationValidator.ValidateSourceConfigurations(configuration.Sources);
        TriggerConfigurationValidator.ValidateTriggerConfigurations(configuration.Triggers);
        GroupConfigurationValidator.ValidateGroupConfigurations(configuration.Groups, configuration.Sources, configuration.Triggers);
        ItemConfigurationValidator.ValidateItemConfigurations(configuration.Items, configuration.Groups, configuration.Adapters);
        LoggingConfigurationValidator.ValidateLoggingConfiguration(configuration.Logging);
        CacheConfigurationValidator.ValidateCacheConfiguration(configuration.Cache);
        PluginLoadConfigurationValidator.ValidatePluginLoadConfiguration(configuration.Plugins);
    }
}
