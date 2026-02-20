// <copyright file="GroupState.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Abstractions.Enums;

/// <summary>
/// Defines the runtime state of an acquisition group.
/// </summary>
public enum GroupState
{
    /// <summary>
    /// The group is idle.
    /// </summary>
    Idle,

    /// <summary>
    /// The group is currently reading values.
    /// </summary>
    Reading,

    /// <summary>
    /// The group is stale and has not refreshed recently.
    /// </summary>
    Stale,
}
