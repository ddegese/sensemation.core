// <copyright file="DataPointJsonConverter.cs" company="Sensemation">
//     Copyright (c) 2026 Sensemation. All rights reserved.
// </copyright>

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sensemation.Core.Contracts.Serialization;

/// <summary>
/// JSON converter that preserves the original value type for <see cref="DataPoint"/>.
/// </summary>
public sealed class DataPointJsonConverter : JsonConverter<DataPoint>
{
    /// <inheritdoc />
    public override DataPoint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var root = document.RootElement;

        var timestamp = root.TryGetProperty("timestampUtc", out var timestampElement)
            ? timestampElement.GetDateTime()
            : root.GetProperty("TimestampUtc").GetDateTime();

        var quality = root.TryGetProperty("quality", out var qualityElement)
            ? (Quality)qualityElement.GetInt32()
            : (Quality)root.GetProperty("Quality").GetInt32();

        var valueType = root.TryGetProperty("valueType", out var valueTypeElement)
            ? valueTypeElement.GetString()
            : root.GetProperty("ValueType").GetString();

        var valueElement = root.TryGetProperty("value", out var valueProperty)
            ? valueProperty
            : root.GetProperty("Value");

        var value = DeserializeValue(valueElement, valueType, options);

        return new DataPoint(timestamp, value, quality);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DataPoint value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        writer.WriteStartObject();
        writer.WriteString("timestampUtc", value.TimestampUtc);
        writer.WriteNumber("quality", (int)value.Quality);

        if (value.Value is null)
        {
            writer.WriteString("valueType", "null");
            writer.WriteNull("value");
        }
        else
        {
            var type = value.Value.GetType();
            writer.WriteString("valueType", type.AssemblyQualifiedName);
            writer.WritePropertyName("value");
            JsonSerializer.Serialize(writer, value.Value, type, options);
        }

        writer.WriteEndObject();
    }

    private static object? DeserializeValue(JsonElement element, string? valueType, JsonSerializerOptions options)
    {
        if (string.IsNullOrWhiteSpace(valueType) || valueType == "null")
        {
            return null;
        }

        var resolvedType = Type.GetType(valueType, throwOnError: false);
        return resolvedType is null ? element.GetRawText() : JsonSerializer.Deserialize(element.GetRawText(), resolvedType, options);
    }
}
