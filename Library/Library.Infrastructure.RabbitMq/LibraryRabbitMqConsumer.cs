using Library.Application.Contracts;
using Library.Application.Contracts.BookLoans;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace Library.Infrastructure.RabbitMq;

/// <summary>
/// Фоновый consumer RabbitMQ для получения сообщений из очереди с данными о выдаче книг
/// Создаёт канал к брокеру, подписывается на очередь и обрабатывает входящие сообщения
/// Каждое сообщение десериализуется и передаётся в application слой для создания записей
/// Корректно освобождает ресурсы при остановке сервиса
/// </summary>
public class LibraryRabbitMqConsumer(IConnection connection, IServiceScopeFactory scopeFactory, IConfiguration configuration, JsonSerializerOptions jsonOptions, ILogger<LibraryRabbitMqConsumer> logger) : BackgroundService
{
    private readonly string _queueName = configuration.GetSection("RabbitMq")["QueueName"] ?? throw new KeyNotFoundException("QueueName section of RabbitMq is missing");

    /// <summary>
    /// Запускает фоновую обработку сообщений из очереди RabbitMQ
    /// Создаёт канал, объявляет очередь, регистрирует consumer и удерживает выполнение до токена отмены
    /// Закрывает и освобождает канал при остановке сервиса
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Establishing channel to queue {queue}", _queueName);

        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);
        try
        {
            await channel.QueueDeclareAsync(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null, cancellationToken: stoppingToken);

            logger.LogInformation("Began listening to queue {queue}", _queueName);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (_, ea) => await ReceiveMessage(ea, stoppingToken);

            await channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer, cancellationToken: stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        finally
        {
            try
            {
                await channel.CloseAsync(cancellationToken: CancellationToken.None); 
            }
            catch 
            {

            }

            await channel.DisposeAsync();
        }
    }

    /// <summary>
    /// Обрабатывает одно входящее сообщение из очереди
    /// Десериализует список контрактов и создаёт записи через application сервис в отдельном scope
    /// Логирует ошибки обработки и не прерывает работу consumer
    /// </summary>
    private async Task ReceiveMessage(BasicDeliverEventArgs args, CancellationToken stoppingToken)
    {
        logger.LogInformation("Received a message from queue {queue}", _queueName);

        try
        {
            stoppingToken.ThrowIfCancellationRequested();

            var contracts = JsonSerializer.Deserialize<List<BookLoanCreateUpdateDto>>(args.Body.Span, jsonOptions)
                ?? throw new FormatException("Unable to parse contracts from message body");

            using var scope = scopeFactory.CreateScope();
            var bookLoanService = scope.ServiceProvider.GetRequiredService<IApplicationService<BookLoanDto, BookLoanCreateUpdateDto, ObjectId>>();

            foreach (var contract in contracts)
            {
                try
                {
                    await bookLoanService.Create(contract);
                }
                catch (KeyNotFoundException ex)
                {
                    logger.LogWarning(ex, "Skipping contract due to missing related entity in {queue} with BookId {bookId} and ReaderId {readerId}", _queueName, contract.BookId, contract.ReaderId);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occured during receiving contracts from {queue}", _queueName);
        }
    }
}