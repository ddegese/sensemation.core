// <copyright file="IItemSource.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Abstractions.Interfaces;

/// <summary>
/// Represents an item that is backed by an external source address.
/// </summary>
public interface IItemSource : IItem
{
    /// <summary>
    /// Gets the source-specific address for the item.
    /// </summary>
    /// <value>The source address.</value>
    public string SourceAddress { get; }

    /// <summary>
    /// Gets the source that owns the item.
    /// </summary>
    /// <value>The owning source or <c>null</c>.</value>
    public ISource? Source { get; }
}
