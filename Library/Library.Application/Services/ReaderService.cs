using AutoMapper;
using Library.Application.Contracts;
using Library.Application.Contracts.BookLoans;
using Library.Application.Contracts.Readers;
using Library.Domain;
using Library.Domain.Model;
using MongoDB.Bson;

namespace Library.Application.Services;

/// <summary>
/// Сервис для выполнения CRUD операций с сущностью Reader
/// </summary>
/// <param name="readerRepository">Репозиторий для доступа к данным Reader</param>
/// <param name="loanRepository">Репозиторий для доступа к данным Loan</param>
/// <param name="mapper">Экземпляр AutoMapper для преобразования DTO</param>
public class ReaderService(IRepository<Reader, ObjectId> readerRepository, IRepository<BookLoan, ObjectId> loanRepository, IMapper mapper) : IReaderService
{
    /// <summary>
    /// Создает нового читателя
    /// </summary>
    /// <param name="dto">DTO с данными для создания читателя</param>
    /// <returns>Созданный ReaderDto</returns>
    public async Task<ReaderDto> Create(ReaderCreateUpdateDto dto)
    {
        var entity = mapper.Map<Reader>(dto);

        var createdEntity = await readerRepository.Create(entity);

        return mapper.Map<ReaderDto>(createdEntity);
    }

    /// <summary>
    /// Удаляет читателя по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор читателя для удаления</param>
    /// <returns>True если удаление успешно</returns>
    public async Task<bool> Delete(ObjectId dtoId)
    {
        return await readerRepository.Delete(dtoId);
    }

    /// <summary>
    /// Получает читателя по идентификатору
    /// </summary>
    /// <param name="dtoId">Идентификатор искомого читателя</param>
    /// <returns>Найденный ReaderDto или null</returns>
    public async Task<ReaderDto?> Get(ObjectId dtoId)
    {
        var entity = await readerRepository.Read(dtoId);

        return mapper.Map<ReaderDto>(entity);
    }

    /// <summary>
    /// Получает список всех читателей
    /// </summary>
    /// <returns>Список всех ReaderDto</returns>
    public async Task<IList<ReaderDto>> GetAll()
    {
        var entities = await readerRepository.ReadAll();

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
        var existingEntity = await readerRepository.Read(dtoId) ?? throw new KeyNotFoundException($"Читатель с ID {dtoId} не найден");
        mapper.Map(dto, existingEntity);

        var updatedEntity = await readerRepository.Update(existingEntity);
        return mapper.Map<ReaderDto>(updatedEntity);
    }

    /// <summary>
    /// Получает все записи о выдаче, связанные с данным читателем
    /// </summary>
    /// <param name="readerId">Идентификатор читателя</param>
    /// <returns>Список DTO записей о выдаче</returns>
    public async Task<IList<BookLoanDto>> GetLoans(ObjectId readerId)
    {
        var loans = await loanRepository.ReadAll();

        var relatedLoans = loans
            .Where(l => l.ReaderId == readerId)
            .ToList();

        if (relatedLoans.Count == 0)
            return [];

        return mapper.Map<IList<BookLoanDto>>(relatedLoans);
    }
}