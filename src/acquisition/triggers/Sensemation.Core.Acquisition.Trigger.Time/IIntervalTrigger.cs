// <copyright file="IIntervalTrigger.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Abstractions.Interfaces;

namespace Sensemation.Core.Acquisition.Trigger.Time;

/// <summary>
/// Defines a trigger that fires on a fixed interval.
/// </summary>
public interface IIntervalTrigger : ITrigger
{
    /// <summary>
    /// Gets the update rate in milliseconds.
    /// </summary>
    /// <value>The update rate in milliseconds.</value>
    public int UpdateRate { get; }
}
