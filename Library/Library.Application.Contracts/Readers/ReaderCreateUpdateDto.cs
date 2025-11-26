namespace Library.Application.Contracts.Readers;

/// <summary>
/// DTO для создания или обновления читателя
/// </summary>
/// <param name="FullName">ФИО читателя</param>
/// <param name="Address">Адрес читателя</param>
/// <param name="Phone">Телефон читателя</param>
/// <param name="RegistrationDate">Дата регистрации читателя</param>
public record ReaderCreateUpdateDto(
    string FullName,
    string? Address,
    string Phone,
    DateTime RegistrationDate
);