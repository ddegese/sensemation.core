// <copyright file="AdapterConfigurationValidator.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Sensemation.Core.Acquisition.Configuration.Models;

namespace Sensemation.Core.Acquisition.Configuration.Validators;

/// <summary>
/// Validates adapter configuration collections.
/// </summary>
public static class AdapterConfigurationValidator
{
    /// <summary>
    /// Validates adapter configurations for uniqueness and completeness.
    /// </summary>
    /// <param name="adapters">The adapter configurations.</param>
    public static void ValidateAdapterConfigurations(Collection<AdapterConfiguration>? adapters)
    {
        if (adapters == null)
        {
            return;
        }

        var adapterNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var adapter in adapters)
        {
            if (string.IsNullOrWhiteSpace(adapter.Id))
            {
                throw new ArgumentException("Adapter name cannot be null, empty or whitespace-only");
            }

            if (!adapterNames.Add(adapter.Id))
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Duplicate adapter name: {0}", adapter.Id));
            }

            if (string.IsNullOrWhiteSpace(adapter.Type))
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Adapter '{0}' has a null, empty or whitespace-only type", adapter.Id));
            }
        }
    }
}
