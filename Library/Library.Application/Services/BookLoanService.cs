using AutoMapper;
using Library.Application.Contracts;
using Library.Application.Contracts.BookLoans;
using Library.Domain;
using Library.Domain.Model;
using MongoDB.Bson;

namespace Library.Application.Services;

/// <summary>
/// Сервис для выполнения CRUD операций с сущностью BookLoan
/// </summary>
/// <param name="repository">Репозиторий для доступа к данным BookLoan</param>
/// <param name="bookRepository">Репозиторий для доступа к данным Book</param>
/// <param name="readerRepository">Репозиторий для доступа к данным Reader</param>
/// <param name="mapper">Экземпляр AutoMapper для преобразования DTO</param>
public class BookLoanService(
    IRepository<BookLoan, ObjectId> repository,
    IRepository<Book, ObjectId> bookRepository,
    IRepository<Reader, ObjectId> readerRepository,
    IMapper mapper) : IApplicationService<BookLoanDto, BookLoanCreateUpdateDto, ObjectId>
{
    /// <summary>
    /// Создает новую запись о выдаче книги
    /// </summary>
    /// <param name="dto">DTO с данными для создания записи о выдаче</param>
    /// <returns>Созданный BookLoanDto</returns>
    public async Task<BookLoanDto> Create(BookLoanCreateUpdateDto dto)
    {
        _ = await bookRepository.Read(dto.BookId) ?? throw new KeyNotFoundException($"Книга с ID {dto.BookId} не найдена");
        _ = await readerRepository.Read(dto.ReaderId) ?? throw new KeyNotFoundException($"Читатель с ID {dto.ReaderId} не найден");

        var entity = mapper.Map<BookLoan>(dto);

        var createdEntity = await repository.Create(entity);

        return mapper.Map<BookLoanDto>(createdEntity);
    }

    /// <summary>
    /// Удаляет запись о выдаче книги по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор записи о выдаче для удаления</param>
    /// <returns>True если удаление успешно</returns>
    public async Task<bool> Delete(ObjectId dtoId)
    {
        return await repository.Delete(dtoId);
    }

    /// <summary>
    /// Получает запись о выдаче книги по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор искомой записи о выдаче</param>
    /// <returns>Найденный BookLoanDto или null</returns>
    public async Task<BookLoanDto?> Get(ObjectId dtoId)
    {
        var entity = await repository.Read(dtoId);

        return mapper.Map<BookLoanDto>(entity);
    }

    /// <summary>
    /// Получает список всех записей о выдаче книг
    /// </summary>
    /// <returns>Список всех BookLoanDto</returns>
    public async Task<IList<BookLoanDto>> GetAll()
    {
        var entities = await repository.ReadAll();

        return mapper.Map<IList<BookLoanDto>>(entities);
    }

    /// <summary>
    /// Обновляет существующую запись о выдаче книги
    /// </summary>
    /// <param name="dto">DTO с обновленными данными записи о выдаче</param>
    /// <param name="dtoId">Идентификатор обновляемой записи о выдаче</param>
    /// <returns>Обновленный BookLoanDto</returns>
    public async Task<BookLoanDto> Update(BookLoanCreateUpdateDto dto, ObjectId dtoId)
    {
        var existingEntity = await repository.Read(dtoId) ?? throw new KeyNotFoundException($"Запись о выдаче с ID {dtoId} не найдена");
        mapper.Map(dto, existingEntity);

        var updatedEntity = await repository.Update(existingEntity);
        return mapper.Map<BookLoanDto>(updatedEntity);
    }
}