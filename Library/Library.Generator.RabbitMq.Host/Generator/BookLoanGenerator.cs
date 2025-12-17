using Bogus;
using Library.Application.Contracts.BookLoans;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace Library.Generator.RabbitMq.Host.Generator;

/// <summary>
/// Генератор тестовых данных для выдачи книг
/// Выбирает BookId и ReaderId случайно из конфигурируемых пулов
/// </summary>
public class BookLoanGenerator
{
    private readonly ObjectId[] _bookIds;
    private readonly ObjectId[] _readerIds;

    public BookLoanGenerator(IOptions<GeneratorOptions> options)
    {
        var opt = options.Value;

        if (opt.BookIds.Count == 0)
            throw new InvalidOperationException("Generator BookIds must be configured");

        if (opt.ReaderIds.Count == 0)
            throw new InvalidOperationException("Generator ReaderIds must be configured");

        _bookIds = [.. opt.BookIds.Select(ObjectId.Parse)];
        _readerIds = [.. opt.ReaderIds.Select(ObjectId.Parse)];
    }

    /// <summary>
    /// Генерирует указанное количество DTO для выдачи книг
    /// Выбирает идентификаторы книги и читателя из пулов настроек и формирует остальные поля через Bogus
    /// </summary>
    public List<BookLoanCreateUpdateDto> GenerateContracts(int count)
    {
        return new Faker<BookLoanCreateUpdateDto>()
            .WithRecord()
            .RuleFor(bl => bl.BookId, f => f.PickRandom(_bookIds))
            .RuleFor(bl => bl.ReaderId, f => f.PickRandom(_readerIds))
            .RuleFor(bl => bl.LoanDate, f => f.Date.Between(new DateTime(2023, 1, 1), DateTime.UtcNow))
            .RuleFor(bl => bl.LoanDays, f => f.Random.Int(1, 90))
            .RuleFor(bl => bl.ReturnDate, (f, bl) =>
            {
                if (f.Random.Bool(0.5f))
                    return null;

                return bl.LoanDate.Date.AddDays(bl.LoanDays ?? 0);
            })
            .Generate(count);
    }
        
}