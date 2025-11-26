using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Domain.Model;

/// <summary>
/// Класс издательства представляет справочник издательств
/// </summary>
public class Publisher
{
    /// <summary>
    /// Идентификатор издательства
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    /// <summary>
    /// Название издательства
    /// </summary>
    [BsonElement("name")]
    public required string Name { get; set; }
}