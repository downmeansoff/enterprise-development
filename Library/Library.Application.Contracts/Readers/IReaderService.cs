using Library.Application.Contracts.BookLoans;
using MongoDB.Bson;

namespace Library.Application.Contracts.Readers;

/// <summary>
/// Интерфейс сервиса для работы с сущностью Book
/// </summary>
public interface IReaderService : IApplicationService<ReaderDto, ReaderCreateUpdateDto, ObjectId>
{
    /// <summary>
    /// Получает все записи о выдаче, связанные с данным читателем
    /// </summary>
    /// <param name="readerId">Идентификатор читателя</param>
    /// <returns>Список DTO записей о выдаче</returns>
    public Task<IList<BookLoanDto>> GetLoans(ObjectId readerId);
}