using Library.Application.Contracts.Books;

namespace Library.Application.Contracts.Analytics;

/// <summary>
/// Представляет аналитические данные о книге, включая общее количество ее выдач за определенный период
/// </summary>
/// <param name="Book">Информация о книге</param>
/// <param name="LoanCount">Общее количество выдач книги за период</param>
public record BookPopularityAnalyticsDto(
    BookDto Book,
    int LoanCount
);