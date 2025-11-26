namespace Library.Application.Contracts;

/// <summary>
/// Интерфейс службы приложения для CRUD операций
/// </summary>
/// <typeparam name="TDto">DTO для Get-запросов</typeparam>
/// <typeparam name="TCreateUpdateDto">DTO для Post/Put-запросов</typeparam>
/// <typeparam name="TKey">Тип идентификатора DTO</typeparam>
public interface IApplicationService<TDto, TCreateUpdateDto, TKey>
    where TDto : class
    where TCreateUpdateDto : class
    where TKey : struct
{
    /// <summary>
    /// Создание DTO
    /// </summary>
    /// <param name="dto">DTO для создания</param>
    /// <returns>Созданный объект DTO</returns>
    public Task<TDto> Create(TCreateUpdateDto dto);

    /// <summary>
    /// Получение DTO по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор DTO</param>
    /// <returns>Объект DTO, если найден, иначе null</returns>
    public Task<TDto?> Get(TKey dtoId);

    /// <summary>
    /// Получение всего списка DTO
    /// </summary>
    /// <returns>Список всех DTO</returns>
    public Task<IList<TDto>> GetAll();

    /// <summary>
    /// Обновление DTO
    /// </summary>
    /// <param name="dto">DTO с обновленными данными</param>
    /// <param name="dtoId">Идентификатор DTO для обновления</param> 
    /// <returns>Обновленный объект DTO</returns>
    public Task<TDto> Update(TCreateUpdateDto dto, TKey dtoId);

    /// <summary>
    /// Удаление DTO
    /// </summary>
    /// <param name="dtoId">Идентификатор DTO для удаления</param>
    /// <returns>true если удаление прошло успешно, иначе false</returns>
    public Task<bool> Delete(TKey dtoId);
}