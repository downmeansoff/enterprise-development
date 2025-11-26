using AutoMapper;
using Library.Application.Contracts;
using Library.Application.Contracts.EditionTypes;
using Library.Domain;
using Library.Domain.Model;
using MongoDB.Bson;

namespace Library.Application.Services;

/// <summary>
/// Сервис для выполнения CRUD операций с сущностью EditionType
/// </summary>
/// <param name="repository">Репозиторий для доступа к данным EditionType</param>
/// <param name="mapper">Экземпляр AutoMapper для преобразования DTO</param>
public class EditionTypeService(IRepository<EditionType, ObjectId> repository, IMapper mapper) : IApplicationService<EditionTypeDto, EditionTypeCreateUpdateDto, ObjectId>
{
    /// <summary>
    /// Создает новый вид издания
    /// </summary>
    /// <param name="dto">DTO с данными для создания вида издания</param>
    /// <returns>Созданный EditionTypeDto</returns>
    public async Task<EditionTypeDto> Create(EditionTypeCreateUpdateDto dto)
    {
        var entity = mapper.Map<EditionType>(dto);

        var createdEntity = await repository.Create(entity);

        return mapper.Map<EditionTypeDto>(createdEntity);
    }

    /// <summary>
    /// Удаляет вид издания по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор вида издания для удаления</param>
    /// <returns>True если удаление успешно</returns>
    public async Task<bool> Delete(ObjectId dtoId)
    {
        return await repository.Delete(dtoId);
    }

    /// <summary>
    /// Получает вид издания по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор искомого вида издания</param>
    /// <returns>Найденный EditionTypeDto или null</returns>
    public async Task<EditionTypeDto?> Get(ObjectId dtoId)
    {
        var entity = await repository.Read(dtoId);

        return mapper.Map<EditionTypeDto>(entity);
    }

    /// <summary>
    /// Получает список всех видов изданий
    /// </summary>
    /// <returns>Список всех EditionTypeDto</returns>
    public async Task<IList<EditionTypeDto>> GetAll()
    {
        var entities = await repository.ReadAll();

        return mapper.Map<IList<EditionTypeDto>>(entities);
    }

    /// <summary>
    /// Обновляет существующий вид издания
    /// </summary>
    /// <param name="dto">DTO с обновленными данными вида издания</param>
    /// <param name="dtoId">Идентификатор обновляемого вида издания</param>
    /// <returns>Обновленный EditionTypeDto</returns>
    public async Task<EditionTypeDto> Update(EditionTypeCreateUpdateDto dto, ObjectId dtoId)
    {
        var existingEntity = await repository.Read(dtoId) ?? throw new KeyNotFoundException($"Вид издания с ID {dtoId} не найден");
        mapper.Map(dto, existingEntity);

        var updatedEntity = await repository.Update(existingEntity);
        return mapper.Map<EditionTypeDto>(updatedEntity);
    }
}