// <copyright file="IAdapter.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Abstractions.Interfaces;

/// <summary>
/// Represents an acquisition adapter that can start and stop processing.
/// </summary>
public interface IAdapter : IPlugin, IValueListener
{
    /// <summary>
    /// Starts the adapter execution.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task StartAsync();

    /// <summary>
    /// Stops the adapter execution.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task StopAsync();

    /// <summary>
    /// Adds a value accessor to the item.
    /// </summary>
    /// <param name="accessor">The accessor to add.</param>
    public void AddValueAccessor(IValueAccessor accessor);
}
