// <copyright file="ItemConfigurationValidationTests.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Sensemation.Core.Acquisition.Configuration.Models;
using Sensemation.Core.Acquisition.Configuration.Validators;

namespace Sensemation.Core.Acquisition.UnitTests.Configuration;

/// <summary>
/// Tests for item configuration validation.
/// </summary>
public class ItemConfigurationValidationTests
{
    /// <summary>
    /// Ensures validation fails when the source address is missing.
    /// </summary>
    [Fact]
    public void ValidateItemConfigurationsShouldThrowForMissingSourceAddress()
    {
        var items = new Collection<ItemConfiguration>
        {
            new() { SourceAddress = " ", Datatype = "Integer32", Group = "group-1" },
        };

        _ = Assert.Throws<ArgumentException>(() => ItemConfigurationValidator.ValidateItemConfigurations(items, null));
    }

    /// <summary>
    /// Ensures validation fails when the group is missing.
    /// </summary>
    [Fact]
    public void ValidateItemConfigurationsShouldThrowForMissingGroup()
    {
        var items = new Collection<ItemConfiguration>
        {
            new() { SourceAddress = "tag1", Datatype = "Integer32", Group = " " },
        };

        _ = Assert.Throws<ArgumentException>(() => ItemConfigurationValidator.ValidateItemConfigurations(items, null));
    }

    /// <summary>
    /// Ensures validation fails when the datatype is missing.
    /// </summary>
    [Fact]
    public void ValidateItemConfigurationsShouldThrowForMissingDatatype()
    {
        var items = new Collection<ItemConfiguration>
        {
            new() { SourceAddress = "tag1", Datatype = " ", Group = "group-1" },
        };

        _ = Assert.Throws<ArgumentException>(() => ItemConfigurationValidator.ValidateItemConfigurations(items, null));
    }

    /// <summary>
    /// Ensures validation fails when the datatype is invalid.
    /// </summary>
    [Fact]
    public void ValidateItemConfigurationsShouldThrowForInvalidDatatype()
    {
        var items = new Collection<ItemConfiguration>
        {
            new() { SourceAddress = "tag1", Datatype = "Bogus", Group = "group-1" },
        };

        _ = Assert.Throws<ArgumentException>(() => ItemConfigurationValidator.ValidateItemConfigurations(items, null));
    }

    /// <summary>
    /// Ensures validation fails when the cache size is invalid.
    /// </summary>
    [Fact]
    public void ValidateItemConfigurationsShouldThrowForInvalidCacheSize()
    {
        var items = new Collection<ItemConfiguration>
        {
            new() { SourceAddress = "tag1", Datatype = "Integer32", Group = "group-1", CacheSize = 0 },
        };

        _ = Assert.Throws<ArgumentException>(() => ItemConfigurationValidator.ValidateItemConfigurations(items, null));
    }

    /// <summary>
    /// Ensures validation fails when duplicate identifiers are used.
    /// </summary>
    [Fact]
    public void ValidateItemConfigurationsShouldThrowForDuplicateIdentifiers()
    {
        var items = new Collection<ItemConfiguration>
        {
            new() { SourceAddress = "tag1", Id = "tag1", Datatype = "Integer32", Group = "group-1" },
            new() { SourceAddress = "tag2", Id = "tag1", Datatype = "Integer32", Group = "group-1" },
        };

        _ = Assert.Throws<ArgumentException>(() => ItemConfigurationValidator.ValidateItemConfigurations(items, null));
    }

    /// <summary>
    /// Ensures validation fails for duplicate source addresses in the same group.
    /// </summary>
    [Fact]
    public void ValidateItemConfigurationsShouldThrowForDuplicateSourceAddressInGroup()
    {
        var items = new Collection<ItemConfiguration>
        {
            new() { SourceAddress = "tag1", Datatype = "Integer32", Group = "group-1" },
            new() { SourceAddress = "tag1", Datatype = "Integer32", Group = "group-1" },
        };

        _ = Assert.Throws<ArgumentException>(() => ItemConfigurationValidator.ValidateItemConfigurations(items, null));
    }

    /// <summary>
    /// Ensures validation fails when the group reference is missing.
    /// </summary>
    [Fact]
    public void ValidateItemConfigurationsShouldThrowForMissingGroupReference()
    {
        var items = new Collection<ItemConfiguration>
        {
            new() { SourceAddress = "tag1", Datatype = "Integer32", Group = "missing" },
        };
        var groups = new Collection<GroupConfiguration>
        {
            new() { Id = "group-1", Source = "memory", Trigger = "fast" },
        };

        _ = Assert.Throws<ArgumentException>(() => ItemConfigurationValidator.ValidateItemConfigurations(items, groups));
    }

    /// <summary>
    /// Ensures validation fails when the adapter reference is missing.
    /// </summary>
    [Fact]
    public void ValidateItemConfigurationsShouldThrowForMissingAdapterReference()
    {
        var items = new Collection<ItemConfiguration>
        {
            new() { SourceAddress = "tag1", Datatype = "Integer32", Group = "group-1", Adapters = { "adapter-1" } },
        };
        var groups = new Collection<GroupConfiguration>
        {
            new() { Id = "group-1", Source = "memory", Trigger = "fast" },
        };
        var adapters = new Collection<AdapterConfiguration>
        {
            new() { Id = "adapter-2", Type = "webapi" },
        };

        _ = Assert.Throws<ArgumentException>(() => ItemConfigurationValidator.ValidateItemConfigurations(items, groups, adapters));
    }
}
