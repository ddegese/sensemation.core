// <copyright file="IConfigurationValidator.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Configuration.Validators;

/// <summary>
/// Defines a composable validator contract.
/// </summary>
/// <typeparam name="TConfiguration">The configuration type to validate.</typeparam>
public interface IConfigurationValidator<in TConfiguration>
{
    /// <summary>
    /// Validates the provided configuration.
    /// </summary>
    /// <param name="configuration">The configuration instance.</param>
    public void Validate(TConfiguration configuration);
}
