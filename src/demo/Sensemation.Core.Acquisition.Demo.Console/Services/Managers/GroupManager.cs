// <copyright file="GroupManager.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Demo.Console.Logging;
using Sensemation.Core.Acquisition.Runtime.Models;

namespace Sensemation.Core.Acquisition.Demo.Console.Services.Managers;

/// <summary>
/// Manages groups and their triggers.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GroupManager"/> class.
/// </remarks>
/// <param name="logger">The logger.</param>
/// <param name="sourceManager">The source manager.</param>
/// <param name="triggerManager">The trigger manager.</param>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by DI")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1852:Seal internal types", Justification = "Used by dependency injection")]
internal class GroupManager(
    ILogger<GroupManager> logger,
    SourceManager sourceManager,
    TriggerManager triggerManager) : IDisposable
{
    /// <summary>
    /// The logger for the group manager.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA2213:Disposable fields should be disposed",
        Justification = "Owned by the DI container; GroupManager does not create or own these dependencies.")]
    private readonly ILogger<GroupManager> logger = logger;

    /// <summary>
    /// The dictionary of groups by name.
    /// </summary>
    private readonly Dictionary<string, Group> groups = [];

    /// <summary>
    /// The source manager.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA2213:Disposable fields should be disposed",
        Justification = "Owned by the DI container; GroupManager does not create or own these dependencies.")]
    private readonly SourceManager sourceManager = sourceManager;

    /// <summary>
    /// The trigger manager.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design",
        "CA2213:Disposable fields should be disposed",
        Justification = "Owned by the DI container; GroupManager does not create or own these dependencies.")]
    private readonly TriggerManager triggerManager = triggerManager;

    /// <summary>
    /// Whether the group manager has been disposed.
    /// </summary>
    private bool disposed;

    /// <summary>
    /// Creates groups from the provided configuration and assigns sources and triggers to them.
    /// </summary>
    /// <param name="groupConfigs">The group configurations.</param>
    public void CreateGroups(IEnumerable<GroupConfiguration> groupConfigs)
    {
        ArgumentNullException.ThrowIfNull(groupConfigs);

        foreach (var groupConfig in groupConfigs)
        {
            _ = this.CreateGroup(groupConfig.Id);

            // Assign the source to the group
            var source = this.sourceManager.GetSource(groupConfig.Source);
            if (source != null)
            {
                this.AssignSourceToGroup(groupConfig.Id, source);
            }
            else
            {
                LogMessages.SourceNotFoundLogger(this.logger, groupConfig.Source, groupConfig.Id, null);
            }

            // Assign the trigger to the group
            var trigger = this.triggerManager.GetTrigger(groupConfig.Trigger);
            if (trigger != null)
            {
                this.AssignTriggerToGroup(groupConfig.Id, trigger);
            }
            else
            {
                LogMessages.TriggerNotFoundLogger(this.logger, groupConfig.Trigger, groupConfig.Id, null);
            }
        }
    }

    /// <summary>
    /// Assigns a source to the specified group.
    /// </summary>
    /// <param name="groupName">The name of the group.</param>
    /// <param name="source">The source to assign.</param>
    public void AssignSourceToGroup(string groupName, ISource source)
    {
        ArgumentNullException.ThrowIfNull(source);

        if (this.groups.TryGetValue(groupName, out var group))
        {
            group.Source = source;
            LogMessages.SourceAssignedToGroupLogger(this.logger, source.Id, groupName, null);
        }
        else
        {
            LogMessages.GroupNotFoundLogger(this.logger, groupName, null);
        }
    }

    /// <summary>
    /// Assigns a trigger to the specified group.
    /// </summary>
    /// <param name="groupName">The name of the group.</param>
    /// <param name="trigger">The trigger to assign.</param>
    public void AssignTriggerToGroup(string groupName, ITrigger trigger)
    {
        ArgumentNullException.ThrowIfNull(trigger);

        if (this.groups.TryGetValue(groupName, out var group))
        {
            group.Trigger = trigger;
            LogMessages.TriggerAssignedToGroupLogger(this.logger, trigger.Id, groupName, null);
        }
        else
        {
            LogMessages.GroupNotFoundLogger(this.logger, groupName, null);
        }
    }

    /// <summary>
    /// Gets a group by its name.
    /// </summary>
    /// <param name="groupName">The name of the group.</param>
    /// <returns>The group with the specified name, or null if not found.</returns>
    public IGroup? GetGroup(string groupName) => this.groups.TryGetValue(groupName, out var group) ? group : null;

    /// <summary>
    /// Gets all groups.
    /// </summary>
    /// <returns>An enumerable collection of all groups.</returns>
    public IEnumerable<IGroup> GetAllGroups() => this.groups.Values;

    /// <summary>
    /// Disposes of the resources used by the GroupManager.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the resources used by the GroupManager.
    /// </summary>
    /// <param name="disposing">True if disposing, false if finalizing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                // Dispose of groups
                foreach (var group in this.groups.Values)
                {
                    if (group is IDisposable disposableGroup)
                    {
                        // Log group destruction before disposing
                        LogMessages.GroupDisposedLogger(this.logger, group.Id, null);

                        disposableGroup.Dispose();
                    }
                }

                this.groups.Clear();
            }

            this.disposed = true;
        }
    }

    /// <summary>
    /// Creates a new group with the specified name.
    /// </summary>
    /// <param name="name">The name of the group.</param>
    /// <returns>The created group.</returns>
    private Group CreateGroup(string name)
    {
        var group = new Group(this.logger, name);

        this.groups[name] = group;

        // Log group creation
        LogMessages.GroupCreatedLogger(this.logger, name, null);

        return group;
    }
}
