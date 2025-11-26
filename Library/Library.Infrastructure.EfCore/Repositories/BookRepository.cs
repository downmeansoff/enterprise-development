using Library.Domain;
using Library.Domain.Model;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Library.Infrastructure.EfCore.Repositories;

/// <summary>
/// Репозиторий для управления сущностью Book
/// </summary>
public class BookRepository(LibraryDbContext context) : IRepository<Book, ObjectId>
{
    private readonly DbSet<Book> _dbSet = context.Books;

    /// <summary>
    /// Создание новой книги
    /// </summary>
    public async Task<Book> Create(Book entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Получение книги по Id
    /// </summary>
    public async Task<Book?> Read(ObjectId entityId)
    {
        return await _dbSet
            .Include(b => b.EditionType)
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(b => b.Id == entityId);
    }

    /// <summary>
    /// Получение всего списка книг
    /// </summary>
    public async Task<IList<Book>> ReadAll()
    {
        return await _dbSet
            .Include(b => b.EditionType)
            .Include(b => b.Publisher)
            .ToListAsync();
    }

    /// <summary>
    /// Обновление существующей книги
    /// </summary>
    public async Task<Book> Update(Book entity)
    {
        _dbSet.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Удаление книги по Id
    /// </summary>
    public async Task<bool> Delete(ObjectId entityId)
    {
        var bookToRemove = await _dbSet.FindAsync(entityId);

        if (bookToRemove == null)
        {
            return false;
        }

        _dbSet.Remove(bookToRemove);
        await context.SaveChangesAsync();

        return true;
    }
}