using Library.Domain;
using Library.Domain.Model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Library.Infrastructure.EfCore.Repositories;

/// <summary>
/// Репозиторий для управления сущностью Reader
/// </summary>
public class ReaderRepository(LibraryDbContext context) : IRepository<Reader, ObjectId>
{
    private readonly DbSet<Reader> _dbSet = context.Readers;

    /// <summary>
    /// Создание нового читателя
    /// </summary>
    public async Task<Reader> Create(Reader entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Получение читателя по Id
    /// </summary>
    public async Task<Reader?> Read(ObjectId entityId)
    {
        return await _dbSet.FindAsync(entityId);
    }

    /// <summary>
    /// Получение всех читателей
    /// </summary>
    public async Task<IList<Reader>> ReadAll()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Обновление читателя
    /// </summary>
    public async Task<Reader> Update(Reader entity)
    {
        _dbSet.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удаление читателя по Id
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