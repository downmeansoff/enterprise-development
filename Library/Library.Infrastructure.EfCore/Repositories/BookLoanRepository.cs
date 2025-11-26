using Library.Domain;
using Library.Domain.Model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Library.Infrastructure.EfCore.Repositories;

/// <summary>
/// Репозиторий для управления сущностью BookLoan
/// </summary>
public class BookLoanRepository(LibraryDbContext context) : IRepository<BookLoan, ObjectId>
{
    private readonly DbSet<BookLoan> _dbSet = context.Loans;

    /// <summary>
    /// Создание новой записи о выдаче
    /// </summary>
    public async Task<BookLoan> Create(BookLoan entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Получение записи о выдаче по Id
    /// </summary>
    public async Task<BookLoan?> Read(ObjectId entityId)
    {
        return await _dbSet.FirstOrDefaultAsync(l => l.Id == entityId);
    }

    /// <summary>
    /// Получение всех записей о выдаче
    /// </summary>
    public async Task<IList<BookLoan>> ReadAll()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Обновление записи о выдаче
    /// </summary>
    public async Task<BookLoan> Update(BookLoan entity)
    {
        _dbSet.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удаление записи о выдаче по Id
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