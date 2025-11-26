using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Domain.Model;

/// <summary>
/// Класс вида издания представляет справочник типов изданий
/// </summary>
public class EditionType
{
    /// <summary>
    /// Идентификатор вида издания
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    /// <summary>
    /// Название вида издания
    /// </summary>
    [BsonElement("name")]
    public required string Name { get; set; }
}