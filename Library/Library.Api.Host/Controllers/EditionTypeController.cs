using Library.Application.Contracts;
using Library.Application.Contracts.EditionTypes;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для выполнения стандартных CRUD операций над сущностью EditionType
/// Наследует всю CRUD-логику от CrudControllerBase
/// </summary>
/// <param name="service">Сервис приложения для EditionTypeDto и EditionTypeCreateUpdateDto</param>
/// <param name="logger">Логгер для EditionTypeController</param>
[Route("api/[controller]")]
[ApiController]
public class EditionTypeController(IApplicationService<EditionTypeDto, EditionTypeCreateUpdateDto, ObjectId> service, ILogger<EditionTypeController> logger)
    : CrudControllerBase<EditionTypeDto, EditionTypeCreateUpdateDto, ObjectId>(service, logger);