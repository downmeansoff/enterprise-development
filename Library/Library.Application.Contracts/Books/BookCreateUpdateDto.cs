using MongoDB.Bson;

namespace Library.Application.Contracts.Books;

/// <summary>
/// DTO для создания или обновления книги
/// </summary>
/// <param name="InventoryNumber">Инвентарный номер книги</param>
/// <param name="CatalogCode">Шифр книги в каталоге</param>
/// <param name="Authors">Строка авторов книги</param>
/// <param name="Title">Название книги</param>
/// <param name="EditionTypeId">Идентификатор вида издания</param>
/// <param name="PublisherId">Идентификатор издательства</param>
/// <param name="Year">Год издания книги</param>
public record BookCreateUpdateDto(
    int InventoryNumber,
    string CatalogCode,
    string Authors,
    string Title,
    ObjectId EditionTypeId,
    ObjectId PublisherId,
    int Year
);