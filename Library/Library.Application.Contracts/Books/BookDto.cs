using MongoDB.Bson;

namespace Library.Application.Contracts.Books;

/// <summary>
/// DTO для представления книги
/// </summary>
/// <param name="Id">Идентификатор книги</param>
/// <param name="InventoryNumber">Инвентарный номер книги</param>
/// <param name="CatalogCode">Шифр книги в каталоге</param>
/// <param name="Authors">Строка авторов книги</param>
/// <param name="Title">Название книги</param>
/// <param name="Year">Год издания книги</param>
public record BookDto(
    ObjectId Id,
    int InventoryNumber,
    string CatalogCode,
    string Authors,
    string Title,
    int Year
);