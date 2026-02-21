// <copyright file="ItemManager.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using System.Collections.Concurrent;

using Sensemation.Core.Acquisition.Abstractions.Enums;
using Sensemation.Core.Acquisition.Abstractions.Models;
using Sensemation.Core.Acquisition.Demo.Console.Logging;
using Sensemation.Core.Acquisition.Demo.Console.Services.Cache;
using Sensemation.Core.Acquisition.Runtime.Models;
using Sensemation.Core.Acquisition.Runtime.Services;
using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.Demo.Console.Services.Managers;

/// <summary>
/// Manages items and their operations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ItemManager"/> class.
/// </remarks>
/// <param name="logger">The logger.</param>
/// <param name="groupManager">The group manager.</param>
/// <param name="adapterManager">The adapter manager.</param>
/// <param name="cacheService">The cache persistence service.</param>
/// <param name="dispatcher">The update event dispatcher.</param>
/// <param name="valueConverter">The value converter.</param>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by DI")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1852:Seal internal types", Justification = "Used by dependency injection")]
internal class ItemManager(
    ILogger<ItemManager> logger,
    GroupManager groupManager,
    AdapterManager adapterManager,
    CachePersistenceService cacheService,
    UpdateEventDispatcher dispatcher,
    IValueConverter valueConverter) : IDisposable
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA2213:Disposable fields should be disposed",
        Justification = "Owned by the DI container; ItemManager does not create or own these dependencies.")]
    private readonly ILogger<ItemManager> logger = logger;

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA2213:Disposable fields should be disposed",
        Justification = "Owned by the DI container; ItemManager does not create or own these dependencies.")]
    private readonly ConcurrentDictionary<string, IItem> itemsByIdentifier = new(StringComparer.OrdinalIgnoreCase);
    private readonly ConcurrentDictionary<string, string> itemToGroupMap = new(StringComparer.OrdinalIgnoreCase);

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA2213:Disposable fields should be disposed",
        Justification = "Owned by the DI container; ItemManager does not create or own these dependencies.")]
    private readonly GroupManager groupManager = groupManager;

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA2213:Disposable fields should be disposed",
        Justification = "Owned by the DI container; ItemManager does not create or own these dependencies.")]
    private readonly AdapterManager adapterManager = adapterManager;

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA2213:Disposable fields should be disposed",
        Justification = "Owned by the DI container; ItemManager does not create or own these dependencies.")]
    private readonly CachePersistenceService cacheService = cacheService;

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA2213:Disposable fields should be disposed",
        Justification = "Owned by the DI container; ItemManager does not create or own these dependencies.")]
    private readonly UpdateEventDispatcher dispatcher = dispatcher;

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA2213:Disposable fields should be disposed",
        Justification = "Owned by the DI container; ItemManager does not create or own these dependencies.")]
    private readonly IValueConverter valueConverter = valueConverter;

    /// <summary>
    /// Creates items from the provided configuration and assigns them to groups and adapters.
    /// </summary>
    /// <param name="itemConfigs">The item configurations.</param>
    public void CreateItems(IEnumerable<ItemConfiguration> itemConfigs)
    {
        ArgumentNullException.ThrowIfNull(itemConfigs);

        var configs = itemConfigs.ToList();

        // First pass: create all items
        foreach (var itemConfig in configs)
        {
            var hasSourceAddress = !string.IsNullOrEmpty(itemConfig.SourceAddress);

            if (!hasSourceAddress)
            {
                var msg = $"Item {itemConfig.Id ?? itemConfig.SourceAddress} must have a SourceAddress set.";
                throw new ArgumentException(msg);
            }

            var sourceItem = this.CreateItem(itemConfig);

            // Assign the item to its group
            if (!string.IsNullOrEmpty(itemConfig.Group))
            {
                var group = this.groupManager.GetGroup(itemConfig.Group);
                if (group != null)
                {
                    this.AssignItemToGroup(sourceItem, group);
                }
                else
                {
                    LogMessages.GroupNotFoundForItemLogger(this.logger, itemConfig.Group, sourceItem.Id, null);
                }
            }

            // Assign subscribers (adapters) to the item
            foreach (var adapterName in itemConfig.Adapters)
            {
                var adapter = this.adapterManager.GetAdapter(adapterName);
                if (adapter != null)
                {
                    this.AddListenerToItem(sourceItem, adapter);
                    this.AddAccessorToAdapter(sourceItem, adapter);
                }
                else
                {
                    LogMessages.AdapterNotFoundForItemLogger(this.logger, adapterName, sourceItem.Id, null);
                }
            }
        }
    }

    /// <summary>
    /// Assigns an item to a group.
    /// </summary>
    /// <param name="item">The item to assign.</param>
    /// <param name="group">The group to assign the item to.</param>
    public void AssignItemToGroup(ItemSource item, IGroup group)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(group);

        // Add the item to the group's collection
        group.Items.Add(item);

        // Set the item's group reference
        item.Source = group.Source;

        // Store the mapping
        this.itemToGroupMap[item.Id] = group.Id;

        // Log assignment
        LogMessages.ItemAssignedToGroupLogger(this.logger, item.Id, group.Id, null);
    }

    /// <summary>
    /// Adds a value listener to an item.
    /// </summary>
    /// <param name="item">The item to attach the listener to.</param>
    /// <param name="listener">The listener to add.</param>
    public void AddListenerToItem(IItem item, IValueListener listener)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(listener);

        item.AddValueListener(listener);
        LogMessages.ListenerAssignedLogger(this.logger, listener.GetType().Name, item.Id, null);
    }

    /// <summary>
    /// Adds a value accessor to an adapter.
    /// </summary>
    /// <param name="accessor">The accessor to add.</param>
    /// <param name="adapter">The adapter to attach the accessor to.</param>
    public void AddAccessorToAdapter(IValueAccessor accessor, IAdapter adapter)
    {
        ArgumentNullException.ThrowIfNull(accessor);
        ArgumentNullException.ThrowIfNull(adapter);

        if (adapter is BaseAdapter baseAdapter)
        {
            baseAdapter.AddValueAccessor(accessor);
            LogMessages.AccessorAddedToAdapterLogger(this.logger, accessor.Id, adapter.Id, null);
        }
    }

    /// <summary>
    /// Disposes of the resources used by the ItemManager.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the resources used by the ItemManager.
    /// </summary>
    /// <param name="disposing">True if disposing, false if finalizing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            // Save all caches before disposing
            if (this.cacheService != null)
            {
                foreach (var item in this.itemsByIdentifier.Values)
                {
                    this.cacheService.SaveCache<IEnumerable<DataPoint>>(item.Id, item.GetHistoryValues(), "items");
                }
            }

            // Log destruction of all items before clearing them
            foreach (var item in this.itemsByIdentifier.Values)
            {
                var address = (item as IItemSource)?.SourceAddress ?? string.Empty;
                LogMessages.ItemDisposedLogger(this.logger, item.Id, address, null);

                if (item is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            this.itemsByIdentifier.Clear();
            this.itemToGroupMap.Clear();
        }
    }

    /// <summary>
    /// Creates a new item with the specified properties.
    /// </summary>
    /// <param name="itemConfig">The item configuration.</param>
    /// <returns>The created item.</returns>
    private ItemSource CreateItem(ItemConfiguration itemConfig)
    {
        var identifier = string.IsNullOrWhiteSpace(itemConfig.Id) ? itemConfig.SourceAddress : itemConfig.Id;
        var datatype = ParseItemType(itemConfig.Datatype);

        var item = new ItemSource(this.logger, this.dispatcher, this.valueConverter)
        {
            SourceAddress = itemConfig.SourceAddress,
            Id = identifier,
            Datatype = datatype,
            CacheSize = itemConfig.CacheSize ?? 10,
        };

        this.RegisterItemAndLoadCache(item);

        return item;
    }

    /// <summary>
    /// Registers an item in the internal collection and loads its cache from disk.
    /// </summary>
    /// <param name="item">The item to register.</param>
    private void RegisterItemAndLoadCache(ItemSource item)
    {
        if (!this.itemsByIdentifier.ContainsKey(item.Id))
        {
            this.itemsByIdentifier[item.Id] = item;

            // Log item creation
            LogMessages.ItemCreatedLogger(this.logger, item.Id, item.SourceAddress, null);

            // Load cache from disk if available
            if (this.cacheService != null)
            {
                var cachedValues = this.cacheService.LoadCache<IEnumerable<DataPoint>>(item.Id, "items");
                if (cachedValues != null)
                {
                    item.RecoverInternalCache(cachedValues);
                }
            }
        }
        else
        {
            throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Duplicate identifier {0} found for item {1}", item.Id, item.SourceAddress));
        }
    }

    private static ItemType ParseItemType(string datatype)
    {
        if (Enum.TryParse<ItemType>(datatype, true, out var parsed))
        {
            return parsed;
        }

        var normalized = datatype.Replace("[]", "Array", StringComparison.OrdinalIgnoreCase)
            .Replace(" ", string.Empty, StringComparison.OrdinalIgnoreCase)
            .ToUpperInvariant();

        return normalized switch
        {
            "BOOL" => ItemType.Bool,
            "SBYTE" => ItemType.Integer8,
            "INTEGER8" => ItemType.Integer8,
            "BYTE" => ItemType.UnsignedInteger8,
            "UBYTE" => ItemType.UnsignedInteger8,
            "UNSIGNEDINTEGER8" => ItemType.UnsignedInteger8,
            "SHORT" => ItemType.Integer16,
            "INTEGER16" => ItemType.Integer16,
            "USHORT" => ItemType.UnsignedInteger16,
            "UNSIGNEDINTEGER16" => ItemType.UnsignedInteger16,
            "INT" => ItemType.Integer32,
            "INTEGER32" => ItemType.Integer32,
            "UINT" => ItemType.UnsignedInteger32,
            "UNSIGNEDINTEGER32" => ItemType.UnsignedInteger32,
            "LONG" => ItemType.Integer64,
            "INTEGER64" => ItemType.Integer64,
            "ULONG" => ItemType.UnsignedInteger64,
            "UNSIGNEDINTEGER64" => ItemType.UnsignedInteger64,
            "FLOAT" => ItemType.SinglePrecision,
            "SINGLE" => ItemType.SinglePrecision,
            "SINGLEPRECISION" => ItemType.SinglePrecision,
            "DOUBLE" => ItemType.DoublePrecision,
            "DOUBLEPRECISION" => ItemType.DoublePrecision,
            "STRING" => ItemType.Text,
            "TEXT" => ItemType.Text,
            "BOOLARRAY" => ItemType.BoolArray,
            "SBYTEARRAY" => ItemType.Integer8Array,
            "INTEGER8ARRAY" => ItemType.Integer8Array,
            "BYTEARRAY" => ItemType.UnsignedInteger8Array,
            "UBYTEARRAY" => ItemType.UnsignedInteger8Array,
            "UNSIGNEDINTEGER8ARRAY" => ItemType.UnsignedInteger8Array,
            "SHORTARRAY" => ItemType.Integer16Array,
            "INTEGER16ARRAY" => ItemType.Integer16Array,
            "USHORTARRAY" => ItemType.UnsignedInteger16Array,
            "UNSIGNEDINTEGER16ARRAY" => ItemType.UnsignedInteger16Array,
            "INTARRAY" => ItemType.Integer32Array,
            "INTEGER32ARRAY" => ItemType.Integer32Array,
            "UINTARRAY" => ItemType.UnsignedInteger32Array,
            "UNSIGNEDINTEGER32ARRAY" => ItemType.UnsignedInteger32Array,
            "LONGARRAY" => ItemType.Integer64Array,
            "INTEGER64ARRAY" => ItemType.Integer64Array,
            "ULONGARRAY" => ItemType.UnsignedInteger64Array,
            "UNSIGNEDINTEGER64ARRAY" => ItemType.UnsignedInteger64Array,
            "FLOATARRAY" => ItemType.SinglePrecisionArray,
            "SINGLEARRAY" => ItemType.SinglePrecisionArray,
            "DOUBLEARRAY" => ItemType.DoublePrecisionArray,
            "STRINGARRAY" => ItemType.TextArray,
            "TEXTARRAY" => ItemType.TextArray,
            _ => ItemType.Text,
        };
    }
}
