// <copyright file="ConfigurationValidationPipeline.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Configuration.Validators;

/// <summary>
/// Runs an ordered set of validators for a configuration instance.
/// </summary>
/// <typeparam name="TConfiguration">The configuration type to validate.</typeparam>
public sealed class ConfigurationValidationPipeline<TConfiguration> : IConfigurationValidator<TConfiguration>
{
    private readonly IReadOnlyList<IConfigurationValidator<TConfiguration>> validators;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationValidationPipeline{TConfiguration}"/> class.
    /// </summary>
    /// <param name="validators">The validators to run in order.</param>
    public ConfigurationValidationPipeline(IEnumerable<IConfigurationValidator<TConfiguration>> validators)
    {
        ArgumentNullException.ThrowIfNull(validators);
        this.validators = validators.ToList();
    }

    /// <inheritdoc/>
    public void Validate(TConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        foreach (var validator in this.validators)
        {
            validator.Validate(configuration);
        }
    }
}
