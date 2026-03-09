// <copyright file="TriggersSectionValidator.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Configuration.Models;

namespace Sensemation.Core.Acquisition.Configuration.Validators;

/// <summary>
/// Validates the triggers section.
/// </summary>
public sealed class TriggersSectionValidator : IConfigurationValidator<ServiceConfiguration>
{
    /// <inheritdoc/>
    public void Validate(ServiceConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        TriggerConfigurationValidator.ValidateTriggerConfigurations(configuration.Triggers);
    }
}
