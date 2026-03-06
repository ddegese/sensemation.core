// <copyright file="GroupConfigurationValidator.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Sensemation.Core.Acquisition.Configuration.Models;

namespace Sensemation.Core.Acquisition.Configuration.Validators;

/// <summary>
/// Validates group configuration collections.
/// </summary>
public static class GroupConfigurationValidator
{
    /// <summary>
    /// Validates group configurations for uniqueness and references.
    /// </summary>
    /// <param name="groups">The group configurations.</param>
    /// <param name="sources">The source configurations.</param>
    /// <param name="triggers">The trigger configurations.</param>
    public static void ValidateGroupConfigurations(Collection<GroupConfiguration>? groups, Collection<SourceConfiguration>? sources, Collection<TriggerConfiguration>? triggers)
    {
        if (groups == null)
        {
            return;
        }

        var groupNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var group in groups)
        {
            if (string.IsNullOrWhiteSpace(group.Id))
            {
                throw new ArgumentException("Group name cannot be null, empty or whitespace-only");
            }

            if (!groupNames.Add(group.Id))
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Duplicate group name: {0}", group.Id));
            }

            if (string.IsNullOrWhiteSpace(group.Source))
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Group '{0}' has a null, empty or whitespace-only source", group.Id));
            }

            if (string.IsNullOrWhiteSpace(group.Trigger))
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Group '{0}' has a null, empty or whitespace-only trigger", group.Id));
            }

            if (group.StaleThresholdSeconds.HasValue && group.StaleThresholdSeconds <= 0)
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Group '{0}' has invalid stale threshold seconds: {1}", group.Id, group.StaleThresholdSeconds));
            }

            if (sources != null && !sources.Any(s => s.Id.Equals(group.Source, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Group '{0}' references non-existent source: {1}", group.Id, group.Source));
            }

            if (triggers != null && !triggers.Any(t => t.Id.Equals(group.Trigger, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Group '{0}' references non-existent trigger: {1}", group.Id, group.Trigger));
            }
        }
    }
}
