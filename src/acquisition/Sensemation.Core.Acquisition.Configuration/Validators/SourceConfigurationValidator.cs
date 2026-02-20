// <copyright file="SourceConfigurationValidator.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Sensemation.Core.Acquisition.Configuration.Models;

namespace Sensemation.Core.Acquisition.Configuration.Validators;

/// <summary>
/// Validates source configuration collections.
/// </summary>
public static class SourceConfigurationValidator
{
    /// <summary>
    /// Validates source configurations for uniqueness and completeness.
    /// </summary>
    /// <param name="sources">The source configurations.</param>
    public static void ValidateSourceConfigurations(Collection<SourceConfiguration>? sources)
    {
        if (sources == null)
        {
            return;
        }

        var sourceNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var source in sources)
        {
            if (string.IsNullOrWhiteSpace(source.Id))
            {
                throw new ArgumentException("Source name cannot be null, empty or whitespace-only");
            }

            if (!sourceNames.Add(source.Id))
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Duplicate source name: {0}", source.Id));
            }

            if (string.IsNullOrWhiteSpace(source.Type))
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Source '{0}' has a null, empty or whitespace-only type", source.Id));
            }
        }
    }
}
