using Library.Application.Contracts.Readers;

namespace Library.Application.Contracts.Analytics;

/// <summary>
/// Представляет аналитические данные о читателе, включая общее количество прочитанных им книг за определенный период
/// </summary>
/// <param name="Reader">Информация о читателе</param>
/// <param name="BooksReadCount">Количество книг, прочитанных читателем за период</param>
public record TopReaderAnalyticsDto(
    ReaderDto Reader,
    int BooksReadCount
);