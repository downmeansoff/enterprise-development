namespace Library.Generator.RabbitMq.Host.Generator;

/// <summary>
/// Настройки генератора тестовых данных
/// Хранит пулы идентификаторов книг и читателей для подмешивания валидных и фиктивных значений
/// </summary>
public sealed class GeneratorOptions
{
    public List<string> BookIds { get; init; } = [];
    public List<string> ReaderIds { get; init; } = [];
}