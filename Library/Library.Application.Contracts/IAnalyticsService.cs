using Library.Application.Contracts.Books;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.Readers;

namespace Library.Application.Contracts;

/// <summary>
/// Интерфейс сервиса для получения аналитической информации по данным библиотеки
/// </summary>
public interface IAnalyticsService
{
    /// <summary>
    /// Получает информацию о всех выданных книгах, упорядоченных по названию
    /// </summary>
    /// <returns>Список BookDto выданных книг</returns>
    public Task<IList<BookDto>> GetIssuedBookTitlesOrderedByTitle();

    /// <summary>
    /// Получает топ-5 читателей, прочитавших наибольшее количество книг за заданный период
    /// </summary>
    /// <param name="startDate">Дата начала периода</param>
    /// <param name="endDate">Дата окончания периода</param>
    /// <returns>Список ReaderDto топ-5 читателей</returns>
    public Task<IList<ReaderDto>> GetTopReadersByBooksRead(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Получает информацию о читателях, бравших книги на наибольший максимальный срок, упорядоченных по ФИО
    /// </summary>
    /// <returns>Список ReaderDto читателей</returns>
    public Task<IList<ReaderDto>> GetReadersByLongestMaxLoanPeriod();

    /// <summary>
    /// Получает топ-5 наиболее популярных издательств, по количеству выданных книг, за заданный год
    /// </summary>
    /// <param name="year">Год для анализа</param>
    /// <returns>Список PublisherDto топ-5 издательств</returns>
    public Task<IList<PublisherDto>> GetTop5PopularPublishersByYear(int year);

    /// <summary>
    /// Получает топ-5 наименее популярных книг по количеству выдач за заданный год
    /// </summary>
    /// <param name="year">Год для анализа</param>
    /// <returns>Список BookDto топ-5 наименее популярных книг</returns>
    public Task<IList<BookDto>> GetTop5LeastPopularBooksByYear(int year);
}