using MongoDB.Bson;

namespace Library.Application.Contracts.Readers;

/// <summary>
/// DTO для представления читателя
/// </summary>
/// <param name="Id">Идентификатор читателя</param>
/// <param name="FullName">ФИО читателя</param>
/// <param name="Address">Адрес читателя</param>
/// <param name="Phone">Телефон читателя</param>
/// <param name="RegistrationDate">Дата регистрации читателя</param>
public record ReaderDto(
    ObjectId Id,
    string FullName,
    string? Address,
    string Phone,
    DateTime RegistrationDate
);