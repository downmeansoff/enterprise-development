using Library.Application.Contracts.Books;

namespace Library.Application.Contracts.Analytics;

/// <summary>
/// Представляет аналитические данные о книге, включая общее количество ее выдач за определенный период
/// </summary>
public class BookPopularityAnalyticsDto
{
    /// <summary>
    /// Информация о книге
    /// </summary>
    public required BookDto Book { get; set; }

    /// <summary>
    /// Общее количество выдач книги за период
    /// </summary>
    public int LoanCount { get; set; }
}