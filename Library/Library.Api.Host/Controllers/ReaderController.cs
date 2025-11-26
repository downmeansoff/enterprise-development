using Library.Application.Contracts.BookLoans;
using Library.Application.Contracts.Readers;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для выполнения стандартных CRUD операций над сущностью Reader
/// </summary>
/// <param name="service">Сервис приложения для ReaderDto и ReaderCreateUpdateDto</param>
/// <param name="logger">Логгер для ReaderController</param>
[Route("api/[controller]")]
[ApiController]
public class ReaderController(IReaderService service, ILogger<ReaderController> logger)
    : CrudControllerBase<ReaderDto, ReaderCreateUpdateDto, ObjectId>(service, logger)
{
    /// <summary>
    /// Получает все записи о выдаче , связанные с данным читателем
    /// </summary>
    /// <param name="readerId">Идентификатор читателя</param>
    /// <returns>Список DTO записей о выдаче</returns>
    [HttpGet("{readerId}/loans")]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IList<BookLoanDto>>> GetLoans(ObjectId readerId)
    {
        logger.LogInformation("{method} method of {controller} is called with {readerId}", nameof(GetLoans), GetType().Name, readerId);
        try
        {
            var res = await service.GetLoans(readerId);
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