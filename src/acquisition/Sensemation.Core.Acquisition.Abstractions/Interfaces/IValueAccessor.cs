// <copyright file="IValueAccessor.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;

using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.Abstractions.Interfaces;

/// <summary>
/// Provides access to historical and latest values for an item.
/// </summary>
public interface IValueAccessor
{
    /// <summary>
    /// Gets the accessor identifier.
    /// </summary>
    /// <value>The accessor id.</value>
    public string Id { get; }

    /// <summary>
    /// Gets the latest datapoint.
    /// </summary>
    /// <value>The latest datapoint.</value>
    public DataPoint LatestDataPoint { get; }

    /// <summary>
    /// Gets historical datapoints.
    /// </summary>
    /// <param name="count">The maximum number of points to return.</param>
    /// <returns>The historical datapoints.</returns>
    public ReadOnlyCollection<DataPoint> GetHistoryValues(int? count = null);

    /// <summary>
    /// Writes a value for the accessor.
    /// </summary>
    /// <param name="value">The value payload.</param>
    /// <returns>The resulting datapoint.</returns>
    public Task<DataPoint> WriteValueAsync(object value);
}
