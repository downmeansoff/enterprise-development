using AutoMapper;
using Library.Application.Contracts;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.Readers;
using Library.Domain;
using Library.Domain.Model;
using MongoDB.Bson;
using System.Globalization;

namespace Library.Application.Services;

/// <summary>
/// Сервис для выполнения аналитических запросов и получения сводной информации
/// </summary>
/// <param name="loanRepository">Репозиторий для доступа к данным BookLoan</param>
/// <param name="bookRepository">Репозиторий для доступа к данным Book</param>
/// <param name="readerRepository">Репозиторий для доступа к данным Reader</param>
/// <param name="publisherRepository">Репозиторий для доступа к данным Publisher</param>
/// <param name="mapper">Экземпляр AutoMapper для преобразования DTO</param>
public class AnalyticsService(
    IRepository<BookLoan, ObjectId> loanRepository,
    IRepository<Book, ObjectId> bookRepository,
    IRepository<Reader, ObjectId> readerRepository,
    IRepository<Publisher, ObjectId> publisherRepository,
    IMapper mapper) : IAnalyticsService
{
    private readonly StringComparer _ruComparer = StringComparer.Create(new CultureInfo("ru-RU"), false);

    /// <summary>
    /// Получает информацию о всех выданных книгах, упорядоченных по названию
    /// </summary>
    /// <returns>Список BookDto выданных книг</returns>
    public async Task<IList<BookDto>> GetIssuedBookTitlesOrderedByTitle()
    {
        var allLoans = await loanRepository.ReadAll();
        var allBooks = await bookRepository.ReadAll();

        var issuedBookIds = allLoans.Select(l => l.BookId).Distinct().ToList();

        var issuedBooks = allBooks
            .Where(b => issuedBookIds.Contains(b.Id))
            .OrderBy(b => b.Title, _ruComparer)
            .ToList();

        return mapper.Map<IList<BookDto>>(issuedBooks);
    }

    /// <summary>
    /// Получает топ-5 читателей, прочитавших наибольшее количество книг за заданный период
    /// </summary>
    /// <param name="startDate">Дата начала периода</param>
    /// <param name="endDate">Дата окончания периода</param>
    /// <returns>Список ReaderDto топ-5 читателей</returns>
    public async Task<IList<ReaderDto>> GetTopReadersByBooksRead(DateTime startDate, DateTime endDate)
    {
        var allLoans = await loanRepository.ReadAll();
        var allReaders = await readerRepository.ReadAll();

        var topLoanIds = allLoans
            .Where(l => l.LoanDate >= startDate && l.LoanDate <= endDate)
            .GroupBy(l => l.ReaderId)
            .Select(g => new { ReaderId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToList();

        var top5ReaderIds = topLoanIds
            .Select(x => x.ReaderId)
            .Take(5)
            .ToList();

        var topReaders = allReaders
            .Where(r => top5ReaderIds.Contains(r.Id))
            .ToList();

        var finalSortedReaders = topReaders
            .OrderBy(r => r.FullName, _ruComparer)
            .ToList();

        return mapper.Map<IList<ReaderDto>>(finalSortedReaders);
    }

    /// <summary>
    /// Получает информацию о читателях, бравших книги на наибольший максимальный срок, упорядоченных по ФИО
    /// </summary>
    /// <returns>Список ReaderDto читателей</returns>
    public async Task<IList<ReaderDto>> GetReadersByLongestMaxLoanPeriod()
    {
        var allLoans = await loanRepository.ReadAll();
        var allReaders = await readerRepository.ReadAll();

        var topLoanIds = allLoans
            .GroupBy(l => l.ReaderId)
            .Select(g => new { ReaderId = g.Key, MaxDays = g.Max(l => l.LoanDays) })
            .OrderByDescending(x => x.MaxDays)
            .Take(5)
            .ToList();

        var topReaderIds = topLoanIds.Select(x => x.ReaderId).ToList();

        var topReaders = allReaders
            .Where(r => topReaderIds.Contains(r.Id))
            .OrderBy(r => r.FullName, _ruComparer)
            .ToList();

        return mapper.Map<IList<ReaderDto>>(topReaders);
    }

    /// <summary>
    /// Получает топ-5 наиболее популярных издательств, по количеству выданных книг, за заданный год
    /// </summary>
    /// <param name="year">Год для анализа</param>
    /// <returns>Список PublisherDto топ-5 издательств</returns>
    public async Task<IList<PublisherDto>> GetTop5PopularPublishersByYear(int year)
    {
        var allLoans = await loanRepository.ReadAll();
        var allBooks = await bookRepository.ReadAll();
        var allPublishers = await publisherRepository.ReadAll();

        var relevantLoanBookIds = allLoans
            .Where(l => l.LoanDate.Year == year)
            .Select(l => l.BookId)
            .ToList();

        var publisherIds = allBooks
            .Where(b => relevantLoanBookIds.Contains(b.Id))
            .Select(b => b.PublisherId)
            .ToList();

        var topPublisherIds = publisherIds
            .GroupBy(id => id)
            .Select(g => new { PublisherId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToList();

        var top5Ids = topPublisherIds
            .Select(x => x.PublisherId)
            .Take(5)
            .ToList();

        var topPublishers = allPublishers
            .Where(p => top5Ids.Contains(p.Id))
            .OrderBy(p => p.Name, _ruComparer)
            .ToList();

        return mapper.Map<IList<PublisherDto>>(topPublishers);
    }

    /// <summary>
    /// Получает топ-5 наименее популярных книг по количеству выдач за заданный год
    /// </summary>
    /// <param name="year">Год для анализа</param>
    /// <returns>Список BookDto топ-5 наименее популярных книг</returns>
    public async Task<IList<BookDto>> GetTop5LeastPopularBooksByYear(int year)
    {
        var allBooks = await bookRepository.ReadAll();
        var relevantLoans = await loanRepository.ReadAll();

        var relevantLoansThisYear = relevantLoans
            .Where(l => l.LoanDate.Year == year)
            .ToList();

        var rankedBooks = allBooks
            .GroupJoin(
                relevantLoansThisYear,
                book => book.Id,
                loan => loan.BookId,
                (book, loans) => new { Book = book, Count = loans.Count() }
            )
            .OrderBy(x => x.Count)
            .ThenBy(x => x.Book.Title, _ruComparer)
            .Take(5)
            .ToList();

        var leastPopularBooks = rankedBooks.Select(x => x.Book).ToList();

        return mapper.Map<IList<BookDto>>(leastPopularBooks);
    }
}