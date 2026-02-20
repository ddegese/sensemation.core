// <copyright file="IValueConverter.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Abstractions.Enums;

namespace Sensemation.Core.Acquisition.Abstractions.Interfaces;

/// <summary>
/// Converts values between acquisition item types.
/// </summary>
public interface IValueConverter
{
    /// <summary>
    /// Converts a value into the target item type.
    /// </summary>
    /// <param name="value">The input value.</param>
    /// <param name="targetType">The target item type.</param>
    /// <returns>The converted value.</returns>
    public object Convert(object value, ItemType targetType);
}
