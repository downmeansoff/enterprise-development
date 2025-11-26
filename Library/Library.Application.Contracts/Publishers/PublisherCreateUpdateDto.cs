namespace Library.Application.Contracts.Publishers;

/// <summary>
/// DTO для создания или обновления издательства
/// </summary>
/// <param name="Name">Название издательства</param>
public record PublisherCreateUpdateDto(
    string Name
);