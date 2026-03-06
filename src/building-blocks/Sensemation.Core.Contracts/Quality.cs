// <copyright file="Quality.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

namespace Sensemation.Core.Contracts;

/// <summary>
/// Defines the quality state of a datapoint.
/// </summary>
public enum Quality
{
    /// <summary>
    /// Indicates a valid, reliable datapoint.
    /// </summary>
    Good = 0,

    /// <summary>
    /// Indicates an invalid or unreliable datapoint.
    /// </summary>
    Bad = 1
}
