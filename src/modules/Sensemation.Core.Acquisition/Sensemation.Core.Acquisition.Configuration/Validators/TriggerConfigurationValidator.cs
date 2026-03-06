// <copyright file="TriggerConfigurationValidator.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Sensemation.Core.Acquisition.Configuration.Models;

namespace Sensemation.Core.Acquisition.Configuration.Validators;

/// <summary>
/// Validates trigger configuration collections.
/// </summary>
public static class TriggerConfigurationValidator
{
    /// <summary>
    /// Validates trigger configurations for uniqueness and completeness.
    /// </summary>
    /// <param name="triggers">The trigger configurations.</param>
    public static void ValidateTriggerConfigurations(Collection<TriggerConfiguration>? triggers)
    {
        if (triggers == null)
        {
            return;
        }

        var triggerNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var trigger in triggers)
        {
            if (string.IsNullOrWhiteSpace(trigger.Id))
            {
                throw new ArgumentException("Trigger name cannot be null, empty or whitespace-only");
            }

            if (!triggerNames.Add(trigger.Id))
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Duplicate trigger name: {0}", trigger.Id));
            }

            if (string.IsNullOrWhiteSpace(trigger.Type))
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Trigger '{0}' has a null, empty or whitespace-only type", trigger.Id));
            }
        }
    }
}
