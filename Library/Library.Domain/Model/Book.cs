using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Domain.Model;

/// <summary>
/// Класс книги содержит сведения о карточке книги в библиотеке
/// </summary>
public class Book
{
    /// <summary>
    /// Идентификатор книги
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    /// <summary>
    /// Инвентарный номер книги
    /// </summary>
    public required int InventoryNumber { get; set; }

    /// <summary>
    /// Шифр книги в алфавитном каталоге
    /// </summary>
    public required string CatalogCode { get; set; }

    /// <summary>
    /// Строка авторов книги
    /// </summary>
    public required string Authors { get; set; }

    /// <summary>
    /// Название книги
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Идентификатор вида издания
    /// </summary>
    public required ObjectId EditionTypeId { get; set; }

    /// <summary>
    /// Вид издания
    /// </summary>
    public EditionType? EditionType { get; set; }

    /// <summary>
    /// Идентификатор издательства
    /// </summary>
    public required ObjectId PublisherId { get; set; }

    /// <summary>
    /// Издательство
    /// </summary>
    public Publisher? Publisher { get; set; }

    /// <summary>
    /// Год издания книги
    /// </summary>
    public required int Year { get; set; }
}