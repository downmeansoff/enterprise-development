using Library.Domain;
using Library.Domain.Model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Library.Infrastructure.EfCore.Repositories;

/// <summary>
/// Репозиторий для управления сущностью EditionType
/// </summary>
public class EditionTypeRepository(LibraryDbContext context) : IRepository<EditionType, ObjectId>
{
    private readonly DbSet<EditionType> _dbSet = context.EditionTypes;

    /// <summary>
    /// Создание нового вида издания
    /// </summary>
    public async Task<EditionType> Create(EditionType entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Получение вида издания по Id
    /// </summary>
    public async Task<EditionType?> Read(ObjectId entityId)
    {
        return await _dbSet.FindAsync(entityId);
    }

    /// <summary>
    /// Получение всех видов изданий
    /// </summary>
    public async Task<IList<EditionType>> ReadAll()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Обновление вида издания
    /// </summary>
    public async Task<EditionType> Update(EditionType entity)
    {
        _dbSet.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удаление вида издания по Id
    /// </summary>
    public async Task<bool> Delete(ObjectId entityId)
    {
        var entityToRemove = await _dbSet.FindAsync(entityId);

        if (entityToRemove == null)
        {
            return false;
        }

        _dbSet.Remove(entityToRemove);
        await context.SaveChangesAsync();

        return true;
    }
}