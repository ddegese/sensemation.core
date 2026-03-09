// <copyright file="GroupsSectionValidator.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Configuration.Models;

namespace Sensemation.Core.Acquisition.Configuration.Validators;

/// <summary>
/// Validates the groups section.
/// </summary>
public sealed class GroupsSectionValidator : IConfigurationValidator<ServiceConfiguration>
{
    /// <inheritdoc/>
    public void Validate(ServiceConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        GroupConfigurationValidator.ValidateGroupConfigurations(configuration.Groups, configuration.Sources, configuration.Triggers);
    }
}
