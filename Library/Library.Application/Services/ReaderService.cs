using AutoMapper;
using Library.Application.Contracts;
using Library.Application.Contracts.Readers;
using Library.Domain;
using Library.Domain.Model;
using MongoDB.Bson;

namespace Library.Application.Services;

/// <summary>
/// Сервис для выполнения CRUD операций с сущностью Reader
/// </summary>
/// <param name="repository">Репозиторий для доступа к данным Reader</param>
/// <param name="mapper">Экземпляр AutoMapper для преобразования DTO</param>
public class ReaderService(IRepository<Reader, ObjectId> repository, IMapper mapper) : IApplicationService<ReaderDto, ReaderCreateUpdateDto, ObjectId>
{
    /// <summary>
    /// Создает нового читателя
    /// </summary>
    /// <param name="dto">DTO с данными для создания читателя</param>
    /// <returns>Созданный ReaderDto</returns>
    public async Task<ReaderDto> Create(ReaderCreateUpdateDto dto)
    {
        var entity = mapper.Map<Reader>(dto);

        var createdEntity = await repository.Create(entity);

        return mapper.Map<ReaderDto>(createdEntity);
    }

    /// <summary>
    /// Удаляет читателя по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор читателя для удаления</param>
    /// <returns>True если удаление успешно</returns>
    public async Task<bool> Delete(ObjectId dtoId)
    {
        return await repository.Delete(dtoId);
    }

    /// <summary>
    /// Получает читателя по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор искомого читателя</param>
    /// <returns>Найденный ReaderDto или null</returns>
    public async Task<ReaderDto?> Get(ObjectId dtoId)
    {
        var entity = await repository.Read(dtoId);

        return mapper.Map<ReaderDto>(entity);
    }

    /// <summary>
    /// Получает список всех читателей
    /// </summary>
    /// <returns>Список всех ReaderDto</returns>
    public async Task<IList<ReaderDto>> GetAll()
    {
        var entities = await repository.ReadAll();

        return mapper.Map<IList<ReaderDto>>(entities);
    }

    /// <summary>
    /// Обновляет существующего читателя
    /// </summary>
    /// <param name="dto">DTO с обновленными данными читателя</param>
    /// <param name="dtoId">Идентификатор обновляемого читателя</param>
    /// <returns>Обновленный ReaderDto</returns>
    public async Task<ReaderDto> Update(ReaderCreateUpdateDto dto, ObjectId dtoId)
    {
        var existingEntity = await repository.Read(dtoId) ?? throw new KeyNotFoundException($"Читатель с ID {dtoId} не найден");
        mapper.Map(dto, existingEntity);

        var updatedEntity = await repository.Update(existingEntity);
        return mapper.Map<ReaderDto>(updatedEntity);
    }
}