using System.Text.Json;
using System.Text.Json.Serialization;
using MongoDB.Bson;

namespace Library.Api.Host;

/// <inheritdoc/>
public class ObjectIdJsonConverter : JsonConverter<ObjectId>
{
    /// <inheritdoc/>
    public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String || !ObjectId.TryParse(reader.GetString(), out var objectId))
        {
            throw new JsonException($"Невозможно преобразовать значение в ObjectId.");
        }
        return objectId;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}