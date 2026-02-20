// <copyright file="ItemUpdateEvent.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Abstractions.Interfaces;
using Sensemation.Core.Contracts;

namespace Sensemation.Core.Acquisition.Runtime.Models;

/// <summary>
/// Represents an update event dispatched to listeners.
/// </summary>
/// <param name="ItemId">The item identifier.</param>
/// <param name="ValueListener">The listener to notify.</param>
/// <param name="DataPoint">The datapoint payload.</param>
public sealed record ItemUpdateEvent(string ItemId, IValueListener ValueListener, DataPoint DataPoint);
