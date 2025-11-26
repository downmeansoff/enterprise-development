using Library.Domain;
using Library.Domain.Model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Library.Infrastructure.EfCore.Repositories;

/// <summary>
/// Репозиторий для управления сущностью Publisher
/// </summary>
public class PublisherRepository(LibraryDbContext context) : IRepository<Publisher, ObjectId>
{
    private readonly DbSet<Publisher> _dbSet = context.Publishers;

    /// <summary>
    /// Создание нового издательства
    /// </summary>
    public async Task<Publisher> Create(Publisher entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Получение издательства по Id
    /// </summary>
    public async Task<Publisher?> Read(ObjectId entityId)
    {
        return await _dbSet.FindAsync(entityId);
    }

    /// <summary>
    /// Получение всех издательств
    /// </summary>
    public async Task<IList<Publisher>> ReadAll()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Обновление издательства
    /// </summary>
    public async Task<Publisher> Update(Publisher entity)
    {
        _dbSet.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удаление издательства по Id
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