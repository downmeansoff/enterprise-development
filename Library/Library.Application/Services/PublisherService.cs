using AutoMapper;
using Library.Application.Contracts;
using Library.Application.Contracts.Publishers;
using Library.Domain;
using Library.Domain.Model;
using MongoDB.Bson;

namespace Library.Application.Services;

/// <summary>
/// Сервис для выполнения CRUD операций с сущностью Publisher
/// </summary>
/// <param name="repository">Репозиторий для доступа к данным Publisher</param>
/// <param name="mapper">Экземпляр AutoMapper для преобразования DTO</param>
public class PublisherService(IRepository<Publisher, ObjectId> repository, IMapper mapper) : IApplicationService<PublisherDto, PublisherCreateUpdateDto, ObjectId>
{
    /// <summary>
    /// Создает новое издательство
    /// </summary>
    /// <param name="dto">DTO с данными для создания издательства</param>
    /// <returns>Созданный PublisherDto</returns>
    public async Task<PublisherDto> Create(PublisherCreateUpdateDto dto)
    {
        var entity = mapper.Map<Publisher>(dto);

        var createdEntity = await repository.Create(entity);

        return mapper.Map<PublisherDto>(createdEntity);
    }

    /// <summary>
    /// Удаляет издательство по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор издательства для удаления</param>
    /// <returns>True если удаление успешно</returns>
    public async Task<bool> Delete(ObjectId dtoId)
    {
        return await repository.Delete(dtoId);
    }

    /// <summary>
    /// Получает издательство по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор искомого издательства</param>
    /// <returns>Найденный PublisherDto или null</returns>
    public async Task<PublisherDto?> Get(ObjectId dtoId)
    {
        var entity = await repository.Read(dtoId);

        return mapper.Map<PublisherDto>(entity);
    }

    /// <summary>
    /// Получает список всех издательств
    /// </summary>
    /// <returns>Список всех PublisherDto</returns>
    public async Task<IList<PublisherDto>> GetAll()
    {
        var entities = await repository.ReadAll();

        return mapper.Map<IList<PublisherDto>>(entities);
    }

    /// <summary>
    /// Обновляет существующее издательство
    /// </summary>
    /// <param name="dto">DTO с обновленными данными издательства</param>
    /// <param name="dtoId">Идентификатор обновляемого издательства</param>
    /// <returns>Обновленный PublisherDto</returns>
    public async Task<PublisherDto> Update(PublisherCreateUpdateDto dto, ObjectId dtoId)
    {
        var existingEntity = await repository.Read(dtoId) ?? throw new KeyNotFoundException($"Издательство с ID {dtoId} не найдено");
        mapper.Map(dto, existingEntity);

        var updatedEntity = await repository.Update(existingEntity);
        return mapper.Map<PublisherDto>(updatedEntity);
    }
}