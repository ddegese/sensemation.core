// <copyright file="ITrigger.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Abstractions.Interfaces;

/// <summary>
/// Defines a trigger that signals when acquisition should occur.
/// </summary>
public interface ITrigger : IPlugin
{
    /// <summary>
    /// Occurs when the trigger fires.
    /// </summary>
    public event EventHandler? Triggered;

    /// <summary>
    /// Starts the trigger.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task StartAsync();

    /// <summary>
    /// Stops the trigger.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task StopAsync();
}
