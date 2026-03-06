// <copyright file="ItemConfigurationValidator.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Net;

using Sensemation.Core.Acquisition.Configuration.Models;

namespace Sensemation.Core.Acquisition.Configuration.Validators;

/// <summary>
/// Validates item configuration collections.
/// </summary>
public static class ItemConfigurationValidator
{
    /// <summary>
    /// Validates item configurations for consistency and uniqueness.
    /// </summary>
    /// <param name="items">The item configurations.</param>
    /// <param name="groups">The group configurations.</param>
    /// <param name="adapters">The adapter configurations.</param>
    public static void ValidateItemConfigurations(Collection<ItemConfiguration>? items, Collection<GroupConfiguration>? groups, Collection<AdapterConfiguration>? adapters = null)
    {
        if (items == null)
        {
            return;
        }

        var itemSourceAddressesByGroup = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        var itemIdentifiers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var item in items)
        {
            ValidateItemConfiguration(item, groups, itemSourceAddressesByGroup, itemIdentifiers, adapters);
        }
    }

    /// <summary>
    /// Determines whether the provided string is a valid IP address.
    /// </summary>
    /// <param name="ipAddress">The IP address string.</param>
    /// <returns><c>true</c> when valid; otherwise <c>false</c>.</returns>
    public static bool IsValidIpAddress(string ipAddress)
    {
        return IPAddress.TryParse(ipAddress, out _);
    }

    /// <summary>
    /// Determines whether the datatype string is supported.
    /// </summary>
    /// <param name="datatype">The datatype string.</param>
    /// <returns><c>true</c> when valid; otherwise <c>false</c>.</returns>
    public static bool IsValidDatatype(string datatype)
    {
        if (string.IsNullOrWhiteSpace(datatype))
        {
            return false;
        }

        var validTypes = new[]
        {
            "BOOL",
            "SBYTE", "INTEGER8",
            "BYTE", "UBYTE", "UNSIGNEDINTEGER8",
            "SHORT", "INTEGER16",
            "USHORT", "UNSIGNEDINTEGER16",
            "INT", "INTEGER32",
            "UINT", "UNSIGNEDINTEGER32",
            "LONG", "INTEGER64",
            "ULONG", "UNSIGNEDINTEGER64",
            "FLOAT", "SINGLE", "SINGLEPRECISION",
            "DOUBLE", "DOUBLEPRECISION",
            "STRING", "TEXT",
            "BOOL[]", "BOOLARRAY",
            "SBYTE[]", "SBYTEARRAY", "INTEGER8[]", "INTEGER8ARRAY",
            "BYTE[]", "BYTEARRAY", "UBYTE[]", "UBYTEARRAY", "UNSIGNEDINTEGER8[]", "UNSIGNEDINTEGER8ARRAY",
            "SHORT[]", "SHORTARRAY", "INTEGER16[]", "INTEGER16ARRAY",
            "USHORT[]", "USHORTARRAY", "UNSIGNEDINTEGER16[]", "UNSIGNEDINTEGER16ARRAY",
            "INT[]", "INTARRAY", "INTEGER32[]", "INTEGER32ARRAY",
            "UINT[]", "UINTARRAY", "UNSIGNEDINTEGER32[]", "UNSIGNEDINTEGER32ARRAY",
            "LONG[]", "LONGARRAY", "INTEGER64[]", "INTEGER64ARRAY",
            "ULONG[]", "ULONGARRAY", "UNSIGNEDINTEGER64[]", "UNSIGNEDINTEGER64ARRAY",
            "FLOAT[]", "FLOATARRAY", "SINGLE[]", "SINGLEARRAY",
            "DOUBLE[]", "DOUBLEARRAY",
            "STRING[]", "STRINGARRAY", "TEXT[]", "TEXTARRAY",
        };

        return validTypes.Contains(datatype.ToUpperInvariant());
    }

    /// <summary>
    /// Validates a single item configuration.
    /// </summary>
    /// <param name="item">The item configuration.</param>
    /// <param name="groups">The group configurations.</param>
    /// <param name="itemSourceAddressesByGroup">The item address lookup.</param>
    /// <param name="itemIdentifiers">The item identifier lookup.</param>
    /// <param name="adapters">The adapter configurations.</param>
    public static void ValidateItemConfiguration(ItemConfiguration item, Collection<GroupConfiguration>? groups, Dictionary<string, HashSet<string>> itemSourceAddressesByGroup, HashSet<string> itemIdentifiers, Collection<AdapterConfiguration>? adapters = null)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(itemSourceAddressesByGroup);
        ArgumentNullException.ThrowIfNull(itemIdentifiers);

        ValidateItemSourceAddress(item);
        ValidateItemGroup(item, groups);
        ValidateItemDatatype(item);
        ValidateItemCacheSize(item);
        ValidateItemUniqueness(item, itemSourceAddressesByGroup);
        ValidateItemIdentifier(item, itemIdentifiers);
        ValidateItemAdapters(item, adapters);
    }

    private static void ValidateItemSourceAddress(ItemConfiguration item)
    {
        if (string.IsNullOrWhiteSpace(item.SourceAddress))
        {
            throw new ArgumentException($"Item '{item.Id}' must have a SourceAddress.", nameof(item));
        }
    }

    private static void ValidateItemGroup(ItemConfiguration item, Collection<GroupConfiguration>? groups)
    {
        if (string.IsNullOrWhiteSpace(item.Group))
        {
            throw new ArgumentException($"Item '{item.SourceAddress}' has a null, empty or whitespace-only group", nameof(item));
        }

        if (groups != null && !groups.Any(g => g.Id.Equals(item.Group, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ArgumentException($"Item '{item.SourceAddress}' references non-existent group: {item.Group}", nameof(item));
        }
    }

    private static void ValidateItemDatatype(ItemConfiguration item)
    {
        if (string.IsNullOrWhiteSpace(item.Datatype))
        {
            throw new ArgumentException($"Item '{item.SourceAddress}' has a null, empty or whitespace-only datatype", nameof(item));
        }

        if (!IsValidDatatype(item.Datatype))
        {
            throw new ArgumentException($"Item '{item.SourceAddress}' has invalid datatype: {item.Datatype}", nameof(item));
        }
    }

    private static void ValidateItemCacheSize(ItemConfiguration item)
    {
        if (item.CacheSize.HasValue && item.CacheSize <= 0)
        {
            throw new ArgumentException($"Item '{item.SourceAddress}' has invalid cache size: {item.CacheSize}. Cache size must be greater than 0.", nameof(item));
        }
    }

    private static void ValidateItemUniqueness(ItemConfiguration item, Dictionary<string, HashSet<string>> itemSourceAddressesByGroup)
    {
        if (!itemSourceAddressesByGroup.TryGetValue(item.Group, out var value))
        {
            value = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            itemSourceAddressesByGroup[item.Group] = value;
        }

        if (!value.Add(item.SourceAddress))
        {
            throw new ArgumentException($"Duplicate item source address '{item.SourceAddress}' in group '{item.Group}'", nameof(item));
        }
    }

    private static void ValidateItemIdentifier(ItemConfiguration item, HashSet<string> itemIdentifiers)
    {
        var effectiveIdentifier = string.IsNullOrWhiteSpace(item.Id) ? item.SourceAddress : item.Id;

        if (!itemIdentifiers.Add(effectiveIdentifier))
        {
            throw new ArgumentException($"Duplicate item identifier: {effectiveIdentifier}", nameof(item));
        }
    }

    private static void ValidateItemAdapters(ItemConfiguration item, Collection<AdapterConfiguration>? adapters)
    {
        if (adapters != null && item.Adapters.Count > 0)
        {
            foreach (var adapterName in item.Adapters)
            {
                if (!adapters.Any(a => a.Id.Equals(adapterName, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException($"Item '{item.SourceAddress}' references non-existent adapter: {adapterName}", nameof(item));
                }
            }
        }
    }
}
