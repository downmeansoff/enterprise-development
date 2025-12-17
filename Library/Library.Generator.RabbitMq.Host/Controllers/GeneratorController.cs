using Library.Application.Contracts.BookLoans;
using Library.Generator.RabbitMq.Host.Generator;
using Microsoft.AspNetCore.Mvc;

namespace Library.Generator.RabbitMq.Host.Controllers;

/// <summary>
/// Контроллер генерации тестовых данных для выдачи книг и отправки в RabbitMQ
/// Генерирует данные пачками и публикует их в очередь через продюсер
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class GeneratorController(ILogger<GeneratorController> logger, LibraryRabbitMqProducer producerService, BookLoanGenerator bookLoanGenerator) : ControllerBase
{
    /// <summary>
    /// Генерирует заданное количество контрактов и отправляет их в RabbitMQ батчами
    /// </summary>
    /// <param name="batchSize">Задаёт размер одного батча</param>
    /// <param name="payloadLimit">Задаёт общее количество элементов</param>
    /// <param name="waitTime">Задаёт паузу между отправками в секундах</param>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<ActionResult<List<BookLoanCreateUpdateDto>>> Get([FromQuery] int batchSize, [FromQuery] int payloadLimit, [FromQuery] int waitTime, CancellationToken cancellationToken)
    {
        if (batchSize <= 0)
            return BadRequest("batchSize must be greater than 0");

        if (payloadLimit <= 0)
            return BadRequest("payloadLimit must be greater than 0");

        if (waitTime < 0)
            return BadRequest("waitTime must be greater than or equal to 0");

        logger.LogInformation("Generating {limit} contracts via {batchSize} batches and {waitTime}s delay", payloadLimit, batchSize, waitTime);

        try
        {
            var list = new List<BookLoanCreateUpdateDto>(payloadLimit);
            var counter = 0;

            while (counter < payloadLimit)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var remaining = payloadLimit - counter;
                var currentBatchSize = Math.Min(batchSize, remaining);

                var batch = bookLoanGenerator.GenerateContracts(currentBatchSize);

                await producerService.SendAsync(batch, cancellationToken);

                logger.LogInformation("Batch of {batchSize} items has been sent", batch.Count);

                counter += batch.Count;
                list.AddRange(batch);

                if (waitTime > 0 && counter < payloadLimit)
                    await Task.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken);
            }

            logger.LogInformation("{method} method of {controller} executed successfully", nameof(Get), GetType().Name);
            return Ok(list);
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Request was cancelled during {method} method of {controller}", nameof(Get), GetType().Name);
            return Problem(statusCode: 499, title: "Client Closed Request");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception happened during {method} method of {controller}", nameof(Get), GetType().Name);
            return StatusCode(500, $"{ex.Message}\n\r{ex.InnerException?.Message}");
        }
    }
}