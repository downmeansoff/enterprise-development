using Library.Application.Contracts.Readers;

namespace Library.Application.Contracts.Analytics;

/// <summary>
/// Представляет аналитические данные о читателе, включая общее количество прочитанных им книг за определенный период
/// </summary>
public class TopReaderAnalyticsDto
{
    /// <summary>
    /// Информация о читателе
    /// </summary>
    public required ReaderDto Reader { get; set; }

    /// <summary>
    /// Количество книг, прочитанных читателем за период
    /// </summary>
    public int BooksReadCount { get; set; }
}