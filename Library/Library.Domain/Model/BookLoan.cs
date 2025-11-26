using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Domain.Model;

/// <summary>
/// Класс записи выдачи книги содержит дату выдачи и срок возврата
/// </summary>
public class BookLoan
{
    /// <summary>
    /// Идентификатор записи выдачи
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    /// <summary>
    /// Идентификатор книги
    /// </summary>
    [BsonElement("bookId")]
    public required ObjectId BookId { get; set; }

    /// <summary>
    /// Книга выданная читателю
    /// </summary>
    public Book? Book { get; set; }

    /// <summary>
    /// Идентификатор читателя
    /// </summary>
    [BsonElement("readerId")]
    public required ObjectId ReaderId { get; set; }

    /// <summary>
    /// Читатель которому выдана книга
    /// </summary>
    public Reader? Reader { get; set; }

    /// <summary>
    /// Дата выдачи книги
    /// </summary>
    [BsonElement("loanDate")]
    public required DateTime LoanDate { get; set; }

    /// <summary>
    /// Количество дней на которое выдана книга
    /// </summary>
    [BsonElement("loanDays")]
    public int? LoanDays { get; set; }

    /// <summary>
    /// Дата возврата книги
    /// </summary>
    [BsonElement("returnDate")]
    public DateTime? ReturnDate { get; set; }
}