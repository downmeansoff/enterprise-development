using Library.Application.Contracts;
using Library.Application.Contracts.Publishers;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для выполнения стандартных CRUD операций над сущностью Publisher
/// Наследует всю CRUD-логику от CrudControllerBase
/// </summary>
/// <param name="service">Сервис приложения для PublisherDto и PublisherCreateUpdateDto</param>
/// <param name="logger">Логгер для PublisherController</param>
[Route("api/[controller]")]
[ApiController]
public class PublisherController(IApplicationService<PublisherDto, PublisherCreateUpdateDto, ObjectId> service, ILogger<PublisherController> logger)
    : CrudControllerBase<PublisherDto, PublisherCreateUpdateDto, ObjectId>(service, logger);