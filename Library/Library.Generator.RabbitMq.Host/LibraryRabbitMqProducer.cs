using Library.Application.Contracts.BookLoans;
using RabbitMQ.Client;
using System.Text.Json;

namespace Library.Generator.RabbitMq.Host;

/// <summary>
/// RabbitMQ producer для отправки пачек DTO о выдаче книг в заданную очередь
/// Сериализует входные данные в JSON и публикует сообщение с routing key равным имени очереди
/// Создаёт канал на время отправки и корректно освобождает ресурсы
/// </summary>
public class LibraryRabbitMqProducer(IConfiguration configuration, IConnection rabbitMqConnection, JsonSerializerOptions jsonOptions, ILogger<LibraryRabbitMqProducer> logger)
{
    private readonly string _queueName = configuration.GetSection("RabbitMq")["QueueName"] ?? throw new KeyNotFoundException("QueueName section of RabbitMq is missing");

    /// <summary>
    /// Отправляет одну пачку контрактов в очередь RabbitMQ
    /// Объявляет очередь если она ещё не существует и публикует сообщение в default exchange
    /// Логирует успешную отправку и ошибки сериализации или публикации
    /// </summary>
    public async Task SendAsync(IList<BookLoanCreateUpdateDto> batch, CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Sending a batch of {count} contracts to {queue}", batch.Count, _queueName);

            var payload = JsonSerializer.SerializeToUtf8Bytes(batch, jsonOptions);

            await using var channel = await rabbitMqConnection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null, cancellationToken: cancellationToken);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: _queueName, mandatory: false, body: payload, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occured during sending a batch of {count} contracts to {queue}", batch.Count, _queueName);
        }
    }
}