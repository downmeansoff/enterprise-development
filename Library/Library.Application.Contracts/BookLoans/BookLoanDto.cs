using MongoDB.Bson;

namespace Library.Application.Contracts.BookLoans;

/// <summary>
/// DTO для представления записи о выдаче книги
/// </summary>
/// <param name="Id">Идентификатор записи выдачи</param>
/// <param name="BookId">Идентификатор книги</param>
/// <param name="ReaderId">Идентификатор читателя</param>
/// <param name="LoanDate">Дата выдачи книги</param>
/// <param name="LoanDays">Количество дней на которое выдана книга</param>
/// <param name="ReturnDate">Дата возврата книги</param>
public record BookLoanDto(
    ObjectId Id,
    ObjectId BookId,
    ObjectId ReaderId,
    DateTime LoanDate,
    int? LoanDays,
    DateTime? ReturnDate
);