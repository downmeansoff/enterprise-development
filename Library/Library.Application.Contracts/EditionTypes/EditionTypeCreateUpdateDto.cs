namespace Library.Application.Contracts.EditionTypes;

/// <summary>
/// DTO для создания или обновления вида издания
/// </summary>
/// <param name="Name">Название вида издания</param>
public record EditionTypeCreateUpdateDto(
    string Name
);