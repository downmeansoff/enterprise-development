using MongoDB.Bson;

namespace Library.Application.Contracts.Publishers;

/// <summary>
/// DTO для представления издательства
/// </summary>
/// <param name="Id">Идентификатор издательства</param>
/// <param name="Name">Название издательства</param>
public record PublisherDto(
    ObjectId Id,
    string Name
);