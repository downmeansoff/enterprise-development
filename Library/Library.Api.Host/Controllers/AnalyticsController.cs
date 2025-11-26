using Library.Application.Contracts.Analytics;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.Readers;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для предоставления аналитической информации о библиотеке
/// </summary>
/// <param name="service">Сервис аналитики, реализующий IAnalyticsService</param>
/// <param name="logger">Логгер для AnalyticsController</param>
[Route("api/[controller]")]
[ApiController]
public class AnalyticsController(IAnalyticsService service, ILogger<AnalyticsController> logger) : ControllerBase
{
    /// <summary>
    /// Выводит информацию о всех выданных книгах, упорядоченных по названию
    /// </summary>
    /// <returns>Список BookDto выданных книг</returns>
    [HttpGet("issued-books")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IList<BookDto>>> GetIssuedBooksOrderedByTitle()
    {
        logger.LogInformation("{method} method of {controller} is called", nameof(GetIssuedBooksOrderedByTitle), GetType().Name);
        try
        {
            var res = await service.GetIssuedBookTitlesOrderedByTitle();
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetIssuedBooksOrderedByTitle), GetType().Name);
            return Ok(res);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetIssuedBooksOrderedByTitle), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }

    /// <summary>
    /// Выводит топ-5 читателей, прочитавших наибольшее количество книг за заданный период
    /// </summary>
    /// <param name="startDate">Дата начала периода (yyyy-MM-dd)</param>
    /// <param name="endDate">Дата окончания периода (yyyy-MM-dd)</param>
    /// <returns>Список DTO топ-5 читателей с количеством прочитанных книг.</returns>
    [HttpGet("top-readers-by-books-read")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IList<TopReaderAnalyticsDto>>> GetTopReadersByBooksRead(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        logger.LogInformation("{method} method of {controller} is called with dates: {start} to {end}", nameof(GetTopReadersByBooksRead), GetType().Name, startDate, endDate);
        try
        {
            var res = await service.GetTopReadersByBooksRead(startDate, endDate);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetTopReadersByBooksRead), GetType().Name);
            return Ok(res);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetTopReadersByBooksRead), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }

    /// <summary>
    /// Выводит информацию о читателях, бравших книги на наибольший максимальный срок, упорядоченных по ФИО
    /// </summary>
    /// <returns>Список ReaderDto читателей</returns>
    [HttpGet("readers-by-longest-loan")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IList<ReaderDto>>> GetReadersByLongestMaxLoanPeriod()
    {
        logger.LogInformation("{method} method of {controller} is called", nameof(GetReadersByLongestMaxLoanPeriod), GetType().Name);
        try
        {
            var res = await service.GetReadersByLongestMaxLoanPeriod();
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetReadersByLongestMaxLoanPeriod), GetType().Name);
            return Ok(res);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetReadersByLongestMaxLoanPeriod), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }

    /// <summary>
    /// Выводит топ-5 наиболее популярных издательств, по количеству выданных книг, за заданный год
    /// </summary>
    /// <param name="year">Год для анализа</param>
    /// <returns>Список DTO топ-5 издательств с количеством выданных книг</returns>
    [HttpGet("top-publishers-by-year")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IList<TopPublisherAnalyticsDto>>> GetTop5PopularPublishersByYear([FromQuery] int year)
    {
        logger.LogInformation("{method} method of {controller} is called with year: {year}", nameof(GetTop5PopularPublishersByYear), GetType().Name, year);
        try
        {
            var res = await service.GetTop5PopularPublishersByYear(year);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetTop5PopularPublishersByYear), GetType().Name);
            return Ok(res);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetTop5PopularPublishersByYear), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }

    /// <summary>
    /// Выводит топ-5 наименее популярных книг по количеству выдач за заданный год
    /// </summary>
    /// <param name="year">Год для анализа</param>
    /// <returns>Список DTO топ-5 наименее популярных книг с количеством выдач</returns>
    [HttpGet("least-popular-books-by-year")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IList<BookPopularityAnalyticsDto>>> GetTop5LeastPopularBooksByYear([FromQuery] int year)
    {
        logger.LogInformation("{method} method of {controller} is called with year: {year}", nameof(GetTop5LeastPopularBooksByYear), GetType().Name, year);
        try
        {
            var res = await service.GetTop5LeastPopularBooksByYear(year);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetTop5LeastPopularBooksByYear), GetType().Name);
            return Ok(res);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetTop5LeastPopularBooksByYear), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }
}