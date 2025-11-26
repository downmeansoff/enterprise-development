using Library.Application.Contracts.Books;
using Library.Application.Contracts.Readers;
using MongoDB.Bson;

namespace Library.Application.Contracts.BookLoans;

/// <summary>
/// Интерфейс сервиса для работы с сущностью BookLoan
/// </summary>
public interface IBookLoanService : IApplicationService<BookLoanDto, BookLoanCreateUpdateDto, ObjectId>
{
    /// <summary>
    /// Получает книгу связанную с записью о выдаче
    /// </summary>
    /// <param name="bookLoanId">Идентификатор записи о выдаче</param>
    /// <returns>DTO книги</returns>
    public Task<BookDto> GetBook(ObjectId bookLoanId);

    /// <summary>
    /// Получает читателя связанного с записью о выдаче
    /// </summary>
    /// <param name="bookLoanId">Идентификатор записи о выдаче</param>
    /// <returns>DTO читателя</returns>
    public Task<ReaderDto> GetReader(ObjectId bookLoanId);
}