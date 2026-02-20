// <copyright file="IValueRead.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.Abstractions.Interfaces;

/// <summary>
/// Receives callbacks when values are read.
/// </summary>
public interface IValueRead
{
    /// <summary>
    /// Handles a read datapoint.
    /// </summary>
    /// <param name="dataPoint">The datapoint that was read.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task OnValueRead(DataPoint dataPoint);
}
