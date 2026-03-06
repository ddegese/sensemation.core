// <copyright file="ItemType.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

namespace Sensemation.Core.Acquisition.Abstractions.Enums;

/// <summary>
/// Defines supported data types for acquisition items.
/// </summary>
public enum ItemType
{
    /// <summary>
    /// Boolean scalar value.
    /// </summary>
    Bool,

    /// <summary>
    /// Signed 8-bit integer scalar value.
    /// </summary>
    Integer8,

    /// <summary>
    /// Unsigned 8-bit integer scalar value.
    /// </summary>
    UnsignedInteger8,

    /// <summary>
    /// Signed 16-bit integer scalar value.
    /// </summary>
    Integer16,

    /// <summary>
    /// Unsigned 16-bit integer scalar value.
    /// </summary>
    UnsignedInteger16,

    /// <summary>
    /// Signed 32-bit integer scalar value.
    /// </summary>
    Integer32,

    /// <summary>
    /// Unsigned 32-bit integer scalar value.
    /// </summary>
    UnsignedInteger32,

    /// <summary>
    /// Signed 64-bit integer scalar value.
    /// </summary>
    Integer64,

    /// <summary>
    /// Unsigned 64-bit integer scalar value.
    /// </summary>
    UnsignedInteger64,

    /// <summary>
    /// Single-precision floating point scalar value.
    /// </summary>
    SinglePrecision,

    /// <summary>
    /// Double-precision floating point scalar value.
    /// </summary>
    DoublePrecision,

    /// <summary>
    /// Text string scalar value.
    /// </summary>
    Text,

    /// <summary>
    /// Boolean array value.
    /// </summary>
    BoolArray,

    /// <summary>
    /// Signed 8-bit integer array value.
    /// </summary>
    Integer8Array,

    /// <summary>
    /// Unsigned 8-bit integer array value.
    /// </summary>
    UnsignedInteger8Array,

    /// <summary>
    /// Signed 16-bit integer array value.
    /// </summary>
    Integer16Array,

    /// <summary>
    /// Unsigned 16-bit integer array value.
    /// </summary>
    UnsignedInteger16Array,

    /// <summary>
    /// Signed 32-bit integer array value.
    /// </summary>
    Integer32Array,

    /// <summary>
    /// Unsigned 32-bit integer array value.
    /// </summary>
    UnsignedInteger32Array,

    /// <summary>
    /// Signed 64-bit integer array value.
    /// </summary>
    Integer64Array,

    /// <summary>
    /// Unsigned 64-bit integer array value.
    /// </summary>
    UnsignedInteger64Array,

    /// <summary>
    /// Single-precision floating point array value.
    /// </summary>
    SinglePrecisionArray,

    /// <summary>
    /// Double-precision floating point array value.
    /// </summary>
    DoublePrecisionArray,

    /// <summary>
    /// Text string array value.
    /// </summary>
    TextArray,
}
