using AutoMapper;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.EditionTypes;
using Library.Application.Contracts.Publishers;
using Library.Domain;
using Library.Domain.Model;
using MongoDB.Bson;

namespace Library.Application.Services;

/// <summary>
/// Сервис для выполнения CRUD операций с сущностью Book
/// </summary>
/// <param name="repository">Репозиторий для доступа к данным Book</param>
/// <param name="editionTypeRepository">Репозиторий для доступа к данным EditionType</param>
/// <param name="publisherRepository">Репозиторий для доступа к данным Publisher</param>
/// <param name="mapper">Экземпляр AutoMapper для преобразования DTO</param>
public class BookService(
    IRepository<Book, ObjectId> repository,
    IRepository<EditionType, ObjectId> editionTypeRepository,
    IRepository<Publisher, ObjectId> publisherRepository,
    IMapper mapper) : IBookService
{
    /// <summary>
    /// Создает новую книгу
    /// </summary>
    /// <param name="dto">DTO с данными для создания книги</param>
    /// <returns>Созданный BookDto</returns>
    public async Task<BookDto> Create(BookCreateUpdateDto dto)
    {
        var entity = mapper.Map<Book>(dto);

        var createdEntity = await repository.Create(entity);

        return mapper.Map<BookDto>(createdEntity);
    }

    /// <summary>
    /// Удаляет книгу по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор книги для удаления</param>
    /// <returns>True если удаление успешно</returns>
    public async Task<bool> Delete(ObjectId dtoId)
    {
        return await repository.Delete(dtoId);
    }

    /// <summary>
    /// Получает книгу по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор искомой книги</param>
    /// <returns>Найденный BookDto или null</returns>
    public async Task<BookDto?> Get(ObjectId dtoId)
    {
        var entity = await repository.Read(dtoId);

        return mapper.Map<BookDto>(entity);
    }

    /// <summary>
    /// Получает список всех книг
    /// </summary>
    /// <returns>Список всех BookDto</returns>
    public async Task<IList<BookDto>> GetAll()
    {
        var entities = await repository.ReadAll();

        return mapper.Map<IList<BookDto>>(entities);
    }

    /// <summary>
    /// Обновляет существующую книгу
    /// </summary>
    /// <param name="dto">DTO с обновленными данными книги</param>
    /// <param name="dtoId">Идентификатор обновляемой книги</param>
    /// <returns>Обновленный BookDto</returns>
    public async Task<BookDto> Update(BookCreateUpdateDto dto, ObjectId dtoId)
    {
        var existingEntity = await repository.Read(dtoId) ?? throw new KeyNotFoundException($"Книга с ID {dtoId} не найдена");
        mapper.Map(dto, existingEntity);

        var updatedEntity = await repository.Update(existingEntity);
        return mapper.Map<BookDto>(updatedEntity);
    }

    /// <summary>
    /// Получает вид издания связанный с книгой
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <returns>DTO вида издания</returns>
    public async Task<EditionTypeDto> GetEditionType(ObjectId bookId)
    {
        var book = await repository.Read(bookId) ?? throw new KeyNotFoundException($"Книга с ID {bookId} не найдена");

        var editionType = await editionTypeRepository.Read(book.EditionTypeId)
            ?? throw new KeyNotFoundException($"Вид издания с ID {book.EditionTypeId} не найден для книги {bookId}");

        return mapper.Map<EditionTypeDto>(editionType);
    }

    /// <summary>
    /// Получает издательство связанное с книгой
    /// </summary>
    /// <param name="bookId">Идентификатор книги</param>
    /// <returns>DTO издательства</returns>
    public async Task<PublisherDto> GetPublisher(ObjectId bookId)
    {
        var book = await repository.Read(bookId) ?? throw new KeyNotFoundException($"Книга с ID {bookId} не найдена");

        var publisher = await publisherRepository.Read(book.PublisherId)
            ?? throw new KeyNotFoundException($"Издательство с ID {book.PublisherId} не найдено для книги {bookId}");

        return mapper.Map<PublisherDto>(publisher);
    }
}