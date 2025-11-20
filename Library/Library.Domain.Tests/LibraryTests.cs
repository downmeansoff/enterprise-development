using Library.Domain.Data;
using System.Globalization;

namespace Library.Domain.Tests;

public class LibraryTests(DataSeeder seeder) : IClassFixture<DataSeeder>
{
    private readonly StringComparer _ruComparer = StringComparer.Create(new CultureInfo("ru-RU"), false);

    /// <summary>
    /// Вывод информации о выданных книгах, упорядоченных по названию
    /// </summary>
    [Fact]
    public void GetIssuedBooksOrderedByTitle()
    {
        var actualTitles = seeder.Loans
                .Select(l => seeder.Books.First(b => b.Id == l.BookId).Title)
                .Distinct()
                .OrderBy(t => t, _ruComparer)
                .ToList();

        var expectedTitles = new[]
        {
                "1984",
                "Белая гвардия",
                "Война и мир",
                "Гарри Поттер и философский камень",
                "Мартин Иден",
                "Мастер и Маргарита",
                "Оно",
                "Преступление и наказание",
                "Сияние",
                "Три товарища",
        };

        Assert.Equal(expectedTitles, actualTitles);
    }


    /// <summary>
    /// Вывод топ 5 читателей, прочитавших больше всего книг за период
    /// </summary>
    [Fact]
    public void GetTop5ReadersByPeriod()
    {
        var start = new DateTime(2024, 1, 1);
        var end = new DateTime(2024, 12, 31);

        var actualNames = seeder.Loans
            .Where(l => l.LoanDate >= start && l.LoanDate <= end)
            .GroupBy(l => l.ReaderId)
            .Select(g => new { ReaderId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => seeder.Readers.First(r => r.Id == x.ReaderId).FullName, _ruComparer)
            .Take(5)
            .Select(x => seeder.Readers.First(r => r.Id == x.ReaderId).FullName)
            .ToList();

        var expectedNames = new[]
        {
                "Иванов Иван Иванович",
                "Петров Петр Петрович",
                "Кузнецов Алексей Сергеевич",
                "Орлова Мария Андреевна",
                "Сидорова Анна Михайловна"
        };

        Assert.Equal(expectedNames, actualNames);
    }


    /// <summary>
    /// Вывод читателей, бравших книги на наибольший срок, упорядоченных по ФИО
    /// </summary>
    [Fact]
    public void GetReadersByLongestLoanPeriod()
    {
        var topByMaxLoan = seeder.Loans
                .GroupBy(l => l.ReaderId)
                .Select(g => new { ReaderId = g.Key, MaxDays = g.Max(l => l.LoanDays) })
                .OrderByDescending(x => x.MaxDays)
                .Take(5)
                .ToList();

        var actualNames = topByMaxLoan
            .Select(x => seeder.Readers.First(r => r.Id == x.ReaderId).FullName)
            .OrderBy(n => n, _ruComparer)
            .ToList();

        var expectedNames = new[]
        {
                "Иванов Иван Иванович",
                "Кузнецов Алексей Сергеевич",
                "Орлова Мария Андреевна",
                "Петров Петр Петрович",
                "Сидорова Анна Михайловна"
        }
        .OrderBy(n => n, _ruComparer)
        .ToArray();

        Assert.Equal(expectedNames, actualNames);
    }


    /// <summary>
    /// Вывод топ 5 наиболее популярных издательств за последний год
    /// </summary>
    [Fact]
    public void GetTop5PopularPublishersLastYear()
    {
        var lastYear = 2024;

        var actualPublisherNames = seeder.Loans
            .Where(l => l.LoanDate.Year == lastYear)
            .Select(l => seeder.Books.First(b => b.Id == l.BookId).PublisherId)
            .GroupBy(id => id)
            .Select(g => new
            {
                PublisherId = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .ThenBy(x => seeder.Publishers.First(p => p.Id == x.PublisherId).Name, _ruComparer)
            .Take(5)
            .Select(x => seeder.Publishers.First(p => p.Id == x.PublisherId).Name)
            .ToList();

        var expectedPublisherNames = new[]
        {
                "Эксмо",
                "АСТ",
                "Вита",
                "Лабиринт",
                "Манн Иванов Фербер"
        };

        Assert.Equal(expectedPublisherNames, actualPublisherNames);
    }


    /// <summary>
    /// Вывод топ 5 наименее популярных книг за последний год
    /// </summary>
    [Fact]
    public void GetTop5LeastPopularBooksLastYear()
    {
        var lastYear = 2024;

        var actualTitles = seeder.Books
            .GroupJoin(
                seeder.Loans.Where(l => l.LoanDate.Year == lastYear),
                book => book.Id,
                loan => loan.BookId,
                (book, loans) => new { Book = book, Count = loans.Count() }
            )
            .OrderBy(x => x.Count)
            .ThenBy(x => x.Book.Title, _ruComparer)
            .Take(5)
            .Select(x => x.Book.Title)
            .ToList();

        var expectedTitles = new[]
        {
                "Война и мир",
                "Мартин Иден",
                "1984",
                "Белая гвардия",
                "Гарри Поттер и философский камень"
        };

        Assert.Equal(expectedTitles, actualTitles);
    }
}