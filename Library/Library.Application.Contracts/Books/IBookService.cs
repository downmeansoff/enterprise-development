using Library.Application.Contracts.BookLoans;
using Library.Application.Contracts.EditionTypes;
using Library.Application.Contracts.Publishers;
using MongoDB.Bson;

namespace Library.Application.Contracts.Books;

/// <summary>
/// Интерфейс сервиса для работы с сущностью Book
/// </summary>
public interface IBookService : IApplicationService<BookDto, BookCreateUpdateDto, ObjectId>
{
    /// <summary>
    /// Получает вид издания связанный с книгой
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <returns>DTO вида издания</returns>
    public Task<EditionTypeDto> GetEditionType(ObjectId bookId);

    /// <summary>
    /// Получает издательство связанное с книгой
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <returns>DTO издательства</returns>
    public Task<PublisherDto> GetPublisher(ObjectId bookId);

    /// <summary>
    /// Получает все записи о выдаче, связанные с книгой
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <returns>Список DTO записей о выдаче</returns>
    public Task<IList<BookLoanDto>> GetLoans(ObjectId bookId);
}