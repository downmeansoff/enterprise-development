namespace Library.Domain;

/// <summary>
/// Абстракция репозитория для выполнения стандартных операций CRUD над коллекцией сущностей
/// </summary>
/// <typeparam name="TEntity">Тип сущности, которой управляет репозиторий</typeparam>
/// <typeparam name="TKey">Тип идентификатора сущности</typeparam>
public interface IRepository<TEntity, TKey>
    where TEntity : class
    where TKey : struct
{
    /// <summary>
    /// Создание новой сущности
    /// </summary>
    /// <param name="entity">Объект сущности для сохранения</param>
    /// <returns>Сохраненный объект сущности</returns>
    public Task<TEntity> Create(TEntity entity);

    /// <summary>
    /// Получение сущности по идентификатору
    /// </summary>
    /// <param name="entityId">Уникальный идентификатор сущности</param>
    /// <returns>Найденная сущность, или null, если сущность не найдена</returns>
    public Task<TEntity?> Read(TKey entityId);

    /// <summary>
    /// Получение всего списка сущностей
    /// </summary> 
    /// <returns>Список всех сущностей в коллекции</returns>
    public Task<IList<TEntity>> ReadAll();

    /// <summary>
    /// Обновление сущности в коллекции
    /// </summary>
    /// <param name="entity">Сущность с обновленными данными</param>
    /// <returns>Обновленный объект сущности</returns>
    public Task<TEntity> Update(TEntity entity);

    /// <summary>
    /// Удаление сущности из коллекции
    /// </summary>
    /// <param name="entityId">Уникальный идентификатор сущности для удаления</param>
    /// <returns>True, если удаление прошло успешно; False, если сущность не была найдена или удаление не удалось</returns>
    public Task<bool> Delete(TKey entityId);
}