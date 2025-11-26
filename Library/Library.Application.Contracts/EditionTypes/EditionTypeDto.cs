using MongoDB.Bson;

namespace Library.Application.Contracts.EditionTypes;

/// <summary>
/// DTO для представления вида издания
/// </summary>
/// <param name="Id">Идентификатор вида издания</param>
/// <param name="Name">Название вида издания</param>
public record EditionTypeDto(
    ObjectId Id,
    string Name
);