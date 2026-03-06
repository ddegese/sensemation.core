// <copyright file="MemoryTypeConverter.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using Sensemation.Core.Acquisition.Abstractions.Enums;

namespace Sensemation.Core.Acquisition.Source.Memory.Converters;

/// <summary>
/// Provides default values for memory source item types.
/// </summary>
public static class MemoryTypeConverter
{
    /// <summary>
    /// Generates a default value for the given item type.
    /// </summary>
    /// <param name="itemType">The item type.</param>
    /// <returns>The default value.</returns>
    public static object GenerateDefaultValue(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Bool => false,
            ItemType.Integer8 => (sbyte)0,
            ItemType.UnsignedInteger8 => (byte)0,
            ItemType.Integer16 => (short)0,
            ItemType.UnsignedInteger16 => (ushort)0,
            ItemType.Integer32 => 0,
            ItemType.UnsignedInteger32 => 0u,
            ItemType.Integer64 => 0L,
            ItemType.UnsignedInteger64 => 0UL,
            ItemType.SinglePrecision => 0.0f,
            ItemType.DoublePrecision => 0.0,
            ItemType.Text => string.Empty,
            ItemType.BoolArray => Array.Empty<bool>(),
            ItemType.Integer8Array => Array.Empty<sbyte>(),
            ItemType.UnsignedInteger8Array => Array.Empty<byte>(),
            ItemType.Integer16Array => Array.Empty<short>(),
            ItemType.UnsignedInteger16Array => Array.Empty<ushort>(),
            ItemType.Integer32Array => Array.Empty<int>(),
            ItemType.UnsignedInteger32Array => Array.Empty<uint>(),
            ItemType.Integer64Array => Array.Empty<long>(),
            ItemType.UnsignedInteger64Array => Array.Empty<ulong>(),
            ItemType.SinglePrecisionArray => Array.Empty<float>(),
            ItemType.DoublePrecisionArray => Array.Empty<double>(),
            ItemType.TextArray => Array.Empty<string>(),
            _ => throw new ArgumentException($"Unsupported item type: {itemType}"),
        };
    }
}
