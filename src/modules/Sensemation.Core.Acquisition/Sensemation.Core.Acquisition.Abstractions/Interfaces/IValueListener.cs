// <copyright file="IValueListener.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.Abstractions.Interfaces;

/// <summary>
/// Receives callbacks when values change.
/// </summary>
public interface IValueListener
{
    /// <summary>
    /// Handles an updated datapoint for an item.
    /// </summary>
    /// <param name="itemId">The item identifier.</param>
    /// <param name="dataPoint">The updated datapoint.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task OnValueChanged(string itemId, DataPoint dataPoint);
}
