using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Domain.Model;

/// <summary>
/// Класс читателя содержит сведения о пользователе библиотеки
/// </summary>
public class Reader
{
    /// <summary>
    /// Идентификатор читателя
    /// </summary>
    [BsonId]
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

    /// <summary>
    /// ФИО читателя
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Адрес читателя
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Телефон читателя
    /// </summary>
    public required string Phone { get; set; }

    /// <summary>
    /// Дата регистрации читателя
    /// </summary>
    public required DateTime RegistrationDate { get; set; }
}