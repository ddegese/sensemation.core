// <copyright file="DefaultValueConverter.cs" company="InnovoMind, LLC">
//     Copyright (c) 2026 InnovoMind, LLC. All rights reserved.
// </copyright>

using System.Globalization;
using System.Text.Json;

using Sensemation.Core.Acquisition.Abstractions.Enums;
using Sensemation.Core.Acquisition.Abstractions.Interfaces;

namespace Sensemation.Core.Acquisition.Runtime.Converters;

/// <summary>
/// Converts raw values into supported acquisition <see cref="ItemType"/> formats.
/// </summary>
public sealed class DefaultValueConverter : IValueConverter
{
    /// <inheritdoc />
    public object Convert(object value, ItemType targetType)
    {
        ArgumentNullException.ThrowIfNull(value);

        value = value is JsonElement jsonElement
            ? UnwrapJsonElement(jsonElement) ?? jsonElement
            : value;

        return IsCorrectType(value, targetType)
            ? value
            : this.ConvertToTargetType(value, targetType);
    }

    private object ConvertToTargetType(object value, ItemType targetType)
    {
        return targetType switch
        {
            ItemType.Bool => ConvertToBool(value),
            ItemType.Integer8 => ConvertToInteger8(value),
            ItemType.UnsignedInteger8 => ConvertToUnsignedInteger8(value),
            ItemType.Integer16 => ConvertToInteger16(value),
            ItemType.UnsignedInteger16 => ConvertToUnsignedInteger16(value),
            ItemType.Integer32 => ConvertToInteger32(value),
            ItemType.UnsignedInteger32 => ConvertToUnsignedInteger32(value),
            ItemType.Integer64 => ConvertToInteger64(value),
            ItemType.UnsignedInteger64 => ConvertToUnsignedInteger64(value),
            ItemType.SinglePrecision => ConvertToSinglePrecision(value),
            ItemType.DoublePrecision => ConvertToDoublePrecision(value),
            ItemType.Text => ConvertToText(value),
            ItemType.BoolArray => ConvertToBoolArray(this, value),
            ItemType.Integer8Array => ConvertToInteger8Array(this, value),
            ItemType.UnsignedInteger8Array => ConvertToUnsignedInteger8Array(this, value),
            ItemType.Integer16Array => ConvertToInteger16Array(this, value),
            ItemType.UnsignedInteger16Array => ConvertToUnsignedInteger16Array(this, value),
            ItemType.Integer32Array => ConvertToInteger32Array(this, value),
            ItemType.UnsignedInteger32Array => ConvertToUnsignedInteger32Array(this, value),
            ItemType.Integer64Array => ConvertToInteger64Array(this, value),
            ItemType.UnsignedInteger64Array => ConvertToUnsignedInteger64Array(this, value),
            ItemType.SinglePrecisionArray => ConvertToSinglePrecisionArray(this, value),
            ItemType.DoublePrecisionArray => ConvertToDoublePrecisionArray(this, value),
            ItemType.TextArray => ConvertToTextArray(this, value),
            _ => throw new InvalidCastException($"Unsupported ItemType: {targetType}"),
        };
    }

    private static object? UnwrapJsonElement(JsonElement jsonElement) => jsonElement.ValueKind switch
    {
        JsonValueKind.String => jsonElement.GetString(),
        JsonValueKind.Number => TryGetBestNumber(jsonElement),
        JsonValueKind.True => true,
        JsonValueKind.False => false,
        JsonValueKind.Null => null,
        JsonValueKind.Array => jsonElement.EnumerateArray()
            .Select(UnwrapJsonElement)
            .ToArray(),
        JsonValueKind.Object => jsonElement,
        JsonValueKind.Undefined => jsonElement,
        _ => jsonElement,
    };

    private static object TryGetBestNumber(JsonElement jsonElement)
    {
        if (jsonElement.TryGetInt64(out var longValue))
        {
            return longValue;
        }

        if (jsonElement.TryGetDouble(out var doubleValue))
        {
            return doubleValue;
        }

        var raw = jsonElement.GetRawText();
        return decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var decimalValue)
            ? decimalValue
            : raw;
    }

    private static bool IsCorrectType(object value, ItemType targetType)
    {
        return (targetType, value) switch
        {
            (ItemType.Bool, bool) => true,
            (ItemType.Integer8, sbyte) => true,
            (ItemType.UnsignedInteger8, byte) => true,
            (ItemType.Integer16, short) => true,
            (ItemType.UnsignedInteger16, ushort) => true,
            (ItemType.Integer32, int) => true,
            (ItemType.UnsignedInteger32, uint) => true,
            (ItemType.Integer64, long) => true,
            (ItemType.UnsignedInteger64, ulong) => true,
            (ItemType.SinglePrecision, float) => true,
            (ItemType.DoublePrecision, double) => true,
            (ItemType.Text, string) => true,
            (ItemType.BoolArray, bool[]) => true,
            (ItemType.Integer8Array, sbyte[]) => true,
            (ItemType.UnsignedInteger8Array, byte[]) => true,
            (ItemType.Integer16Array, short[]) => true,
            (ItemType.UnsignedInteger16Array, ushort[]) => true,
            (ItemType.Integer32Array, int[]) => true,
            (ItemType.UnsignedInteger32Array, uint[]) => true,
            (ItemType.Integer64Array, long[]) => true,
            (ItemType.UnsignedInteger64Array, ulong[]) => true,
            (ItemType.SinglePrecisionArray, float[]) => true,
            (ItemType.DoublePrecisionArray, double[]) => true,
            (ItemType.TextArray, string[]) => true,
            _ => false,
        };
    }

    private static bool ConvertToBool(object value)
    {
        if (value is bool boolVal)
        {
            return boolVal;
        }

        if (value is string str)
        {
            var trimmed = str.Trim();
            return string.Equals(trimmed, "true", StringComparison.OrdinalIgnoreCase)
                || (!string.Equals(trimmed, "false", StringComparison.OrdinalIgnoreCase)
                    && (decimal.TryParse(trimmed, NumberStyles.Any, CultureInfo.InvariantCulture, out var numeric)
                        ? numeric != 0m
                        : throw new InvalidCastException($"Cannot convert string '{str}' to Boolean")));
        }

        if (value is IConvertible convertible)
        {
            try
            {
                return value switch
                {
                    int intValue => intValue != 0,
                    long longValue => longValue != 0,
                    short shortValue => shortValue != 0,
                    byte byteValue => byteValue != 0,
                    sbyte sbyteValue => sbyteValue != 0,
                    float floatValue => Math.Abs(floatValue) > float.Epsilon,
                    double doubleValue => Math.Abs(doubleValue) > double.Epsilon,
                    decimal decimalValue => decimalValue != 0m,
                    _ => convertible.ToBoolean(CultureInfo.InvariantCulture),
                };
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Boolean");
            }
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Boolean");
    }

    private static sbyte ConvertToInteger8(object value)
    {
        if (value is sbyte sbyteVal)
        {
            return sbyteVal;
        }

        if (TryConvertIntegral(value, sbyte.MinValue, sbyte.MaxValue, out var converted))
        {
            return (sbyte)converted;
        }

        if (value is string str && sbyte.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed;
        }

        if (value is IConvertible convertible)
        {
            try
            {
                return convertible.ToSByte(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to SByte");
            }
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to SByte");
    }

    private static byte ConvertToUnsignedInteger8(object value)
    {
        if (value is byte byteVal)
        {
            return byteVal;
        }

        if (TryConvertIntegral(value, byte.MinValue, byte.MaxValue, out var converted))
        {
            return (byte)converted;
        }

        if (value is string str && byte.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed;
        }

        if (value is IConvertible convertible)
        {
            try
            {
                return convertible.ToByte(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Byte");
            }
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Byte");
    }

    private static short ConvertToInteger16(object value)
    {
        if (value is short shortVal)
        {
            return shortVal;
        }

        if (TryConvertIntegral(value, short.MinValue, short.MaxValue, out var converted))
        {
            return (short)converted;
        }

        if (value is string str && short.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed;
        }

        if (value is IConvertible convertible)
        {
            try
            {
                return convertible.ToInt16(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Int16");
            }
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Int16");
    }

    private static ushort ConvertToUnsignedInteger16(object value)
    {
        if (value is ushort ushortVal)
        {
            return ushortVal;
        }

        if (TryConvertIntegral(value, ushort.MinValue, ushort.MaxValue, out var converted))
        {
            return (ushort)converted;
        }

        if (value is string str && ushort.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed;
        }

        if (value is IConvertible convertible)
        {
            try
            {
                return convertible.ToUInt16(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to UInt16");
            }
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to UInt16");
    }

    private static int ConvertToInteger32(object value)
    {
        if (value is int intVal)
        {
            return intVal;
        }

        if (TryConvertIntegral(value, int.MinValue, int.MaxValue, out var converted))
        {
            return (int)converted;
        }

        if (value is string str)
        {
            return int.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed)
                ? parsed
                : throw new InvalidCastException($"Cannot convert string '{str}' to Int32");
        }

        if (value is IConvertible convertible)
        {
            try
            {
                return convertible.ToInt32(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Int32");
            }
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Int32");
    }

    private static uint ConvertToUnsignedInteger32(object value)
    {
        if (value is uint uintVal)
        {
            return uintVal;
        }

        if (TryConvertIntegral(value, uint.MinValue, uint.MaxValue, out var converted))
        {
            return (uint)converted;
        }

        if (value is string str)
        {
            return uint.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed)
                ? parsed
                : throw new InvalidCastException($"Cannot convert string '{str}' to UInt32");
        }

        if (value is IConvertible convertible)
        {
            try
            {
                return convertible.ToUInt32(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to UInt32");
            }
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to UInt32");
    }

    private static long ConvertToInteger64(object value)
    {
        if (value is long longVal)
        {
            return longVal;
        }

        if (TryConvertIntegral(value, long.MinValue, long.MaxValue, out var converted))
        {
            return converted;
        }

        if (value is string str)
        {
            return long.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed)
                ? parsed
                : throw new InvalidCastException($"Cannot convert string '{str}' to Int64");
        }

        if (value is IConvertible convertible)
        {
            try
            {
                return convertible.ToInt64(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Int64");
            }
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Int64");
    }

    private static ulong ConvertToUnsignedInteger64(object value)
    {
        if (value is ulong ulongVal)
        {
            return ulongVal;
        }

        if (TryConvertIntegral(value, decimal.Zero, ulong.MaxValue, out var converted))
        {
            return (ulong)converted;
        }

        if (value is string str)
        {
            return ulong.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed)
                ? parsed
                : throw new InvalidCastException($"Cannot convert string '{str}' to UInt64");
        }

        if (value is IConvertible convertible)
        {
            try
            {
                return convertible.ToUInt64(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to UInt64");
            }
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to UInt64");
    }

    private static float ConvertToSinglePrecision(object value)
    {
        if (value is float floatVal)
        {
            return floatVal;
        }

        if (value is string str && float.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed;
        }

        if (value is double dblVal)
        {
            return dblVal is >= -float.MaxValue and <= float.MaxValue
                ? (float)dblVal
                : throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Single");
        }

        if (value is decimal decVal)
        {
            return ConvertDecimalToSingle(decVal, value);
        }

        if (value is IConvertible convertible)
        {
            try
            {
                return convertible.ToSingle(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Single");
            }
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Single");
    }

    private static double ConvertToDoublePrecision(object value)
    {
        if (value is double doubleVal)
        {
            return doubleVal;
        }

        if (value is string str)
        {
            return double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed)
                ? parsed
                : throw new InvalidCastException($"Cannot convert string '{str}' to Double");
        }

        if (value is float floatVal)
        {
            return Math.Round(floatVal, 6, MidpointRounding.AwayFromZero);
        }

        if (value is decimal decimalVal)
        {
            return (double)decimalVal;
        }

        if (value is IConvertible convertible)
        {
            try
            {
                return convertible.ToDouble(CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Double");
            }
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Double");
    }

    private static string ConvertToText(object value)
    {
        return value is string str ? str : value.ToString() ?? string.Empty;
    }

    private static bool[] ConvertToBoolArray(DefaultValueConverter converter, object value)
    {
        ArgumentNullException.ThrowIfNull(converter);

        if (value is bool[] boolArray)
        {
            return boolArray;
        }

        if (value is string jsonStr)
        {
            try
            {
                var result = JsonSerializer.Deserialize<bool[]>(jsonStr);
                if (result is not null)
                {
                    return result;
                }
            }
            catch (JsonException)
            {
            }
        }

        if (value is object[] objArray)
        {
            var result = new bool[objArray.Length];
            for (var i = 0; i < objArray.Length; i++)
            {
                result[i] = (bool)converter.Convert(objArray[i], ItemType.Bool);
            }

            return result;
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Boolean[]");
    }

    private static sbyte[] ConvertToInteger8Array(DefaultValueConverter converter, object value)
    {
        ArgumentNullException.ThrowIfNull(converter);

        if (value is sbyte[] sbyteArray)
        {
            return sbyteArray;
        }

        if (value is string jsonStr)
        {
            try
            {
                var result = JsonSerializer.Deserialize<sbyte[]>(jsonStr);
                if (result is not null)
                {
                    return result;
                }
            }
            catch (JsonException)
            {
            }
        }

        if (value is object[] objArray)
        {
            var result = new sbyte[objArray.Length];
            for (var i = 0; i < objArray.Length; i++)
            {
                result[i] = (sbyte)converter.Convert(objArray[i], ItemType.Integer8);
            }

            return result;
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to SByte[]");
    }

    private static byte[] ConvertToUnsignedInteger8Array(DefaultValueConverter converter, object value)
    {
        ArgumentNullException.ThrowIfNull(converter);

        if (value is byte[] byteArray)
        {
            return byteArray;
        }

        if (value is string jsonStr)
        {
            try
            {
                var result = JsonSerializer.Deserialize<byte[]>(jsonStr);
                if (result is not null)
                {
                    return result;
                }
            }
            catch (JsonException)
            {
                var trimmed = jsonStr.Trim();
                if (trimmed.StartsWith('[') && trimmed.EndsWith(']'))
                {
                    var inner = trimmed[1..^1];
                    var parts = inner.Split(',');
                    var result = new byte[parts.Length];

                    for (var i = 0; i < parts.Length; i++)
                    {
                        var part = parts[i].Trim();
                        result[i] = byte.TryParse(part, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed)
                            ? parsed
                            : throw new InvalidCastException($"Cannot convert '{part}' to Byte");
                    }

                    return result;
                }
            }
        }

        if (value is object[] objArray)
        {
            var result = new byte[objArray.Length];
            for (var i = 0; i < objArray.Length; i++)
            {
                result[i] = (byte)converter.Convert(objArray[i], ItemType.UnsignedInteger8);
            }

            return result;
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Byte[]");
    }

    private static short[] ConvertToInteger16Array(DefaultValueConverter converter, object value)
    {
        ArgumentNullException.ThrowIfNull(converter);

        if (value is short[] shortArray)
        {
            return shortArray;
        }

        if (value is string jsonStr)
        {
            try
            {
                var result = JsonSerializer.Deserialize<short[]>(jsonStr);
                if (result is not null)
                {
                    return result;
                }
            }
            catch (JsonException)
            {
            }
        }

        if (value is object[] objArray)
        {
            var result = new short[objArray.Length];
            for (var i = 0; i < objArray.Length; i++)
            {
                result[i] = (short)converter.Convert(objArray[i], ItemType.Integer16);
            }

            return result;
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Int16[]");
    }

    private static ushort[] ConvertToUnsignedInteger16Array(DefaultValueConverter converter, object value)
    {
        ArgumentNullException.ThrowIfNull(converter);

        if (value is ushort[] ushortArray)
        {
            return ushortArray;
        }

        if (value is string jsonStr)
        {
            try
            {
                var result = JsonSerializer.Deserialize<ushort[]>(jsonStr);
                if (result is not null)
                {
                    return result;
                }
            }
            catch (JsonException)
            {
            }
        }

        if (value is object[] objArray)
        {
            var result = new ushort[objArray.Length];
            for (var i = 0; i < objArray.Length; i++)
            {
                result[i] = (ushort)converter.Convert(objArray[i], ItemType.UnsignedInteger16);
            }

            return result;
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to UInt16[]");
    }

    private static int[] ConvertToInteger32Array(DefaultValueConverter converter, object value)
    {
        ArgumentNullException.ThrowIfNull(converter);

        if (value is int[] intArray)
        {
            return intArray;
        }

        if (value is string jsonStr)
        {
            try
            {
                var result = JsonSerializer.Deserialize<int[]>(jsonStr);
                if (result is not null)
                {
                    return result;
                }
            }
            catch (JsonException)
            {
            }
        }

        if (value is object[] objArray)
        {
            var result = new int[objArray.Length];
            for (var i = 0; i < objArray.Length; i++)
            {
                result[i] = (int)converter.Convert(objArray[i], ItemType.Integer32);
            }

            return result;
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Int32[]");
    }

    private static uint[] ConvertToUnsignedInteger32Array(DefaultValueConverter converter, object value)
    {
        ArgumentNullException.ThrowIfNull(converter);

        if (value is uint[] uintArray)
        {
            return uintArray;
        }

        if (value is string jsonStr)
        {
            try
            {
                var result = JsonSerializer.Deserialize<uint[]>(jsonStr);
                if (result is not null)
                {
                    return result;
                }
            }
            catch (JsonException)
            {
            }
        }

        if (value is object[] objArray)
        {
            var result = new uint[objArray.Length];
            for (var i = 0; i < objArray.Length; i++)
            {
                result[i] = (uint)converter.Convert(objArray[i], ItemType.UnsignedInteger32);
            }

            return result;
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to UInt32[]");
    }

    private static long[] ConvertToInteger64Array(DefaultValueConverter converter, object value)
    {
        ArgumentNullException.ThrowIfNull(converter);

        if (value is long[] longArray)
        {
            return longArray;
        }

        if (value is string jsonStr)
        {
            try
            {
                var result = JsonSerializer.Deserialize<long[]>(jsonStr);
                if (result is not null)
                {
                    return result;
                }
            }
            catch (JsonException)
            {
            }
        }

        if (value is object[] objArray)
        {
            var result = new long[objArray.Length];
            for (var i = 0; i < objArray.Length; i++)
            {
                result[i] = (long)converter.Convert(objArray[i], ItemType.Integer64);
            }

            return result;
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Int64[]");
    }

    private static ulong[] ConvertToUnsignedInteger64Array(DefaultValueConverter converter, object value)
    {
        ArgumentNullException.ThrowIfNull(converter);

        if (value is ulong[] ulongArray)
        {
            return ulongArray;
        }

        if (value is string jsonStr)
        {
            try
            {
                var result = JsonSerializer.Deserialize<ulong[]>(jsonStr);
                if (result is not null)
                {
                    return result;
                }
            }
            catch (JsonException)
            {
            }
        }

        if (value is object[] objArray)
        {
            var result = new ulong[objArray.Length];
            for (var i = 0; i < objArray.Length; i++)
            {
                result[i] = (ulong)converter.Convert(objArray[i], ItemType.UnsignedInteger64);
            }

            return result;
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to UInt64[]");
    }

    private static float[] ConvertToSinglePrecisionArray(DefaultValueConverter converter, object value)
    {
        ArgumentNullException.ThrowIfNull(converter);

        if (value is float[] floatArray)
        {
            return floatArray;
        }

        if (value is string jsonStr)
        {
            try
            {
                var result = JsonSerializer.Deserialize<float[]>(jsonStr);
                if (result is not null)
                {
                    return result;
                }
            }
            catch (JsonException)
            {
            }
        }

        if (value is object[] objArray)
        {
            var result = new float[objArray.Length];
            for (var i = 0; i < objArray.Length; i++)
            {
                result[i] = (float)converter.Convert(objArray[i], ItemType.SinglePrecision);
            }

            return result;
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Single[]");
    }

    private static double[] ConvertToDoublePrecisionArray(DefaultValueConverter converter, object value)
    {
        ArgumentNullException.ThrowIfNull(converter);

        if (value is double[] doubleArray)
        {
            return doubleArray;
        }

        if (value is string jsonStr)
        {
            try
            {
                var result = JsonSerializer.Deserialize<double[]>(jsonStr);
                if (result is not null)
                {
                    return result;
                }
            }
            catch (JsonException)
            {
            }
        }

        if (value is object[] objArray)
        {
            var result = new double[objArray.Length];
            for (var i = 0; i < objArray.Length; i++)
            {
                result[i] = (double)converter.Convert(objArray[i], ItemType.DoublePrecision);
            }

            return result;
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to Double[]");
    }

    private static string[] ConvertToTextArray(DefaultValueConverter converter, object value)
    {
        ArgumentNullException.ThrowIfNull(converter);

        if (value is string[] stringArray)
        {
            return stringArray;
        }

        if (value is string jsonStr)
        {
            try
            {
                var result = JsonSerializer.Deserialize<string[]>(jsonStr);
                if (result is not null)
                {
                    return result;
                }
            }
            catch (JsonException)
            {
            }
        }

        if (value is object[] objArray)
        {
            var result = new string[objArray.Length];
            for (var i = 0; i < objArray.Length; i++)
            {
                result[i] = converter.Convert(objArray[i], ItemType.Text).ToString() ?? string.Empty;
            }

            return result;
        }

        throw new InvalidCastException($"Cannot convert '{value}' ({value.GetType().Name}) to String[]");
    }

    private static bool TryConvertIntegral(object value, decimal min, decimal max, out long result)
    {
        result = 0;
        if (value is null)
        {
            return false;
        }

        if (value is decimal decimalValue)
        {
            return TryConvertIntegralDecimal(decimalValue, min, max, out result);
        }

        if (value is double doubleValue)
        {
            return TryConvertIntegralDouble(doubleValue, min, max, out result);
        }

        if (value is float floatValue)
        {
            return TryConvertIntegralFloat(floatValue, min, max, out result);
        }

        if (value is string str && decimal.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
        {
            return TryConvertIntegralDecimal(parsed, min, max, out result);
        }

        if (value is IConvertible convertible)
        {
            try
            {
                var converted = convertible.ToDecimal(CultureInfo.InvariantCulture);
                return TryConvertIntegralDecimal(converted, min, max, out result);
            }
            catch (Exception)
            {
                return false;
            }
        }

        return false;
    }

    private static bool TryConvertIntegralDecimal(decimal value, decimal min, decimal max, out long result)
    {
        result = 0;
        if (value % 1 != 0)
        {
            return false;
        }

        if (value < min || value > max)
        {
            return false;
        }

        result = (long)value;
        return true;
    }

    private static bool TryConvertIntegralDouble(double value, decimal min, decimal max, out long result)
    {
        result = 0;
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            return false;
        }

        if (Math.Abs(value % 1) > double.Epsilon)
        {
            return false;
        }

        var minDouble = (double)min;
        var maxDouble = (double)max;
        if (value < minDouble || value > maxDouble)
        {
            return false;
        }

        result = (long)value;
        return true;
    }

    private static bool TryConvertIntegralFloat(float value, decimal min, decimal max, out long result)
    {
        result = 0;
        if (float.IsNaN(value) || float.IsInfinity(value))
        {
            return false;
        }

        if (Math.Abs(value % 1) > float.Epsilon)
        {
            return false;
        }

        var minFloat = (float)min;
        var maxFloat = (float)max;
        if (value < minFloat || value > maxFloat)
        {
            return false;
        }

        result = (long)value;
        return true;
    }

    private static float ConvertDecimalToSingle(decimal value, object originalValue)
    {
        var doubleValue = (double)value;
        return double.IsInfinity(doubleValue) || doubleValue < -float.MaxValue || doubleValue > float.MaxValue
            ? throw new InvalidCastException($"Cannot convert '{originalValue}' ({originalValue.GetType().Name}) to Single")
            : (float)doubleValue;
    }
}
