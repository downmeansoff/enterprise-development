using Library.Application.Contracts.Publishers;

namespace Library.Application.Contracts.Analytics;

/// <summary>
/// Представляет аналитические данные об издательстве, включая общее количество выданных книг этого издательства за определенный период
/// </summary>
public class TopPublisherAnalyticsDto
{
    /// <summary>
    /// Информация об издательстве
    /// </summary>
    public required PublisherDto Publisher { get; set; }

    /// <summary>
    /// Количество выданных книг этого издательства
    /// </summary>
    public int IssuedBookCount { get; set; }
}