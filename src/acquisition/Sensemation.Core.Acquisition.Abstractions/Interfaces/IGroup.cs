// <copyright file="IGroup.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Sensemation.Core.Acquisition.Abstractions.Enums;

namespace Sensemation.Core.Acquisition.Abstractions.Interfaces;

/// <summary>
/// Represents a collection of items and their acquisition state.
/// </summary>
public interface IGroup
{
    /// <summary>
    /// Gets the group identifier.
    /// </summary>
    /// <value>The group id.</value>
    public string Id { get; }

    /// <summary>
    /// Gets the source assigned to the group.
    /// </summary>
    /// <value>The source instance or <c>null</c>.</value>
    public ISource? Source { get; }

    /// <summary>
    /// Gets the trigger assigned to the group.
    /// </summary>
    /// <value>The trigger instance or <c>null</c>.</value>
    public ITrigger? Trigger { get; }

    /// <summary>
    /// Gets the current group state.
    /// </summary>
    /// <value>The group state.</value>
    public GroupState State { get; }

    /// <summary>
    /// Gets the item sources in the group.
    /// </summary>
    /// <value>The collection of item sources.</value>
    public Collection<IItemSource> Items { get; }

    /// <summary>
    /// Gets the last refresh timestamp in UTC.
    /// </summary>
    /// <value>The last refresh timestamp.</value>
    public DateTime? LastRefreshUtc { get; }

    /// <summary>
    /// Gets the last successful refresh timestamp in UTC.
    /// </summary>
    /// <value>The last successful refresh timestamp.</value>
    public DateTime? LastSuccessUtc { get; }

    /// <summary>
    /// Gets the last error timestamp in UTC.
    /// </summary>
    /// <value>The last error timestamp.</value>
    public DateTime? LastErrorUtc { get; }

    /// <summary>
    /// Gets the number of consecutive failures.
    /// </summary>
    /// <value>The count of consecutive failures.</value>
    public int ConsecutiveFailures { get; }
}
