using Library.Application.Contracts.Publishers;

namespace Library.Application.Contracts.Analytics;

/// <summary>
/// Представляет аналитические данные об издательстве, включая общее количество выданных книг этого издательства за определенный период
/// </summary>
/// <param name="Publisher">Информация об издательстве</param>
/// <param name="IssuedBookCount">Количество выданных книг этого издательства</param>
public record TopPublisherAnalyticsDto(
    PublisherDto Publisher,
    int IssuedBookCount
);