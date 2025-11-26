using Library.Application.Contracts;
using Library.Application.Contracts.BookLoans;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Library.Api.Host.Controllers;

/// <summary>
/// Контроллер для выполнения CRUD операций над сущностью BookLoan
/// </summary>
/// <param name="service">Сервис приложения, реализующий IBookLoanService</param>
/// <param name="logger">Логгер для BookLoanController</param>
[Route("api/[controller]")]
[ApiController]
public class BookLoanController(IApplicationService<BookLoanDto, BookLoanCreateUpdateDto, ObjectId> service, ILogger<BookLoanController> logger)
    : CrudControllerBase<BookLoanDto, BookLoanCreateUpdateDto, ObjectId>(service, logger);