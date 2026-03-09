// <copyright file="SourcesSectionValidator.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Configuration.Models;

namespace Sensemation.Core.Acquisition.Configuration.Validators;

/// <summary>
/// Validates the sources section.
/// </summary>
public sealed class SourcesSectionValidator : IConfigurationValidator<ServiceConfiguration>
{
    /// <inheritdoc/>
    public void Validate(ServiceConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        SourceConfigurationValidator.ValidateSourceConfigurations(configuration.Sources);
    }
}
