using Library.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Базовый контроллер для работы с сущностями через CRUD-операции
/// Предоставляет стандартные методы для создания, получения, обновления и удаления данных
/// </summary>
/// <typeparam name="TDto">Тип DTO для операций чтения</typeparam>
/// <typeparam name="TCreateUpdateDto">Тип DTO для операций создания и обновления</typeparam>
/// <typeparam name="TKey">Тип идентификатора DTO</typeparam>
/// <param name="appService">Сервис приложения, реализующий операции CRUD над DTO</param>
/// <param name="logger">Логгер для записи информации о выполнении методов и исключениях</param>
[Route("api/[controller]")]
[ApiController]
public abstract class CrudControllerBase<TDto, TCreateUpdateDto, TKey>(IApplicationService<TDto, TCreateUpdateDto, TKey> appService,
    ILogger<CrudControllerBase<TDto, TCreateUpdateDto, TKey>> logger) : ControllerBase
    where TDto : class
    where TCreateUpdateDto : class
    where TKey : struct
{
    /// <summary>
    /// Создание новой записи сущности
    /// </summary>
    /// <param name="newDto">DTO с данными для создания новой сущности</param>
    /// <returns>Созданный объект DTO с присвоенным идентификатором</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<TDto>> Create(TCreateUpdateDto newDto)
    {
        logger.LogInformation("{method} method of {controller} is called with {@dto} parameter", nameof(Create), GetType().Name, newDto);
        try
        {
            var res = await appService.Create(newDto);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(Create), GetType().Name);
            return CreatedAtAction(nameof(this.Create), res);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(Create), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }

    /// <summary>
    /// Обновление существующей записи сущности по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности для обновления</param>
    /// <param name="newDto">DTO с обновлёнными данными</param>
    /// <returns>Обновлённый объект DTO</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<TDto>> Edit(TKey id, TCreateUpdateDto newDto)
    {
        logger.LogInformation("{method} method of {controller} is called with {key},{@dto} parameters", nameof(Edit), GetType().Name, id, newDto);
        try
        {
            var res = await appService.Update(newDto, id);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(Edit), GetType().Name);
            return Ok(res);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound($"{ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(Edit), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }

    /// <summary>
    /// Удаление существующей записи сущности по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности для удаления</param>
    /// <returns>true, если удаление выполнено успешно, иначе false</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete(TKey id)
    {
        logger.LogInformation("{method} method of {controller} is called with {id} parameter", nameof(Delete), GetType().Name, id);
        try
        {
            var res = await appService.Delete(id);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(Delete), GetType().Name);
            return res ? Ok() : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(Delete), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }

    /// <summary>
    /// Получение списка всех сущностей
    /// </summary>
    /// <returns>Список DTO всех сущностей</returns>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<IList<TDto>>> GetAll()
    {
        logger.LogInformation("{method} method of {controller} is called", nameof(GetAll), GetType().Name);
        try
        {
            var res = await appService.GetAll();
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(GetAll), GetType().Name);
            return Ok(res);
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(GetAll), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }

    /// <summary>
    /// Получение одной сущности по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор искомой сущности</param>
    /// <returns>DTO сущности, если она найдена, иначе NotFound</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<TDto>> Get(TKey id)
    {
        logger.LogInformation("{method} method of {controller} is called with {id} parameter", nameof(Get), GetType().Name, id);
        try
        {
            var res = await appService.Get(id);
            logger.LogInformation("{method} method of {controller} executed successfully", nameof(Get), GetType().Name);
            return res != null ? Ok(res) : NoContent();
        }
        catch (Exception ex)
        {
            logger.LogError("An exception happened during {method} method of {controller}: {@exception}", nameof(Get), GetType().Name, ex);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }
}