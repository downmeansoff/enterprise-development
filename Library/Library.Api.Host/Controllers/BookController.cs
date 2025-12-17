using Library.Application.Contracts.BookLoans;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.EditionTypes;
using Library.Application.Contracts.Publishers;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для выполнения CRUD операций над сущностью Book
/// </summary>
/// <param name="service">Сервис приложения, реализующий IBookService</param>
/// <param name="logger">Логгер для BookController</param>
[Route("api/[controller]")]
[ApiController]
public class BookController(IBookService service, ILogger<BookController> logger)
    : CrudControllerBase<BookDto, BookCreateUpdateDto, ObjectId>(service, logger)
{
    /// <summary>
    /// Получает вид издания для указанной книги
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <returns>DTO вида издания</returns>
    [HttpGet("{bookId}/EditionType")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<EditionTypeDto>> GetEditionType(ObjectId bookId)
    {
        logger.LogInformation("{method} method of {controller} is called with {bookId}", nameof(GetEditionType), GetType().Name, bookId);
        try
        {
            var res = await service.GetEditionType(bookId);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetEditionType), GetType().Name);
            return Ok(res);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Not found during {method} method of {controller} with {@bookId} parameter", nameof(Create), GetType().Name, bookId);
            return NotFound($"{ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetEditionType), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }

    /// <summary>
    /// Получает издательство для указанной книги
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <returns>DTO издательства</returns>
    [HttpGet("{bookId}/Publisher")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<PublisherDto>> GetPublisher(ObjectId bookId)
    {
        logger.LogInformation("{method} method of {controller} is called with {bookId}", nameof(GetPublisher), GetType().Name, bookId);
        try
        {
            var res = await service.GetPublisher(bookId);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetPublisher), GetType().Name);
            return Ok(res);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Not found during {method} method of {controller} with {@bookId} parameter", nameof(Create), GetType().Name, bookId);
            return NotFound($"{ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetPublisher), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }

    /// <summary>
    /// Получает все записи о выдаче, связанные с данной книгой
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <returns>Список DTO записей о выдаче</returns>
    [HttpGet("{bookId}/Loans")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IList<BookLoanDto>>> GetLoans(ObjectId bookId)
    {
        logger.LogInformation("{method} method of {controller} is called with {bookId}", nameof(GetLoans), GetType().Name, bookId);
        try
        {
            var res = await service.GetLoans(bookId);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetLoans), GetType().Name);
            return Ok(res);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetLoans), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }
}