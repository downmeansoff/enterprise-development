using Library.Api.Host;
using Library.Application;
using Library.Application.Contracts;
using Library.Application.Contracts.Analytics;
using Library.Application.Contracts.BookLoans;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.EditionTypes;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.Readers;
using Library.Application.Services;
using Library.Domain;
using Library.Domain.Data;
using Library.Domain.Model;
using Library.Infrastructure.EfCore;
using Library.Infrastructure.EfCore.Repositories;
using Library.Infrastructure.RabbitMq;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddSingleton(sp =>
{
    var o = new JsonSerializerOptions(JsonSerializerDefaults.Web);
    o.Converters.Add(new ObjectIdJsonConverter());
    return o;
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new ObjectIdJsonConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.MapType<ObjectId>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "24-character hex string",
        Example = new OpenApiString(ObjectId.GenerateNewId().ToString())
    });

    var assemblies = AppDomain.CurrentDomain.GetAssemblies()
        .Where(a => a.GetName().Name!.StartsWith("Library"))
        .Distinct();

    foreach (var assembly in assemblies)
    {
        var xmlFile = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
            c.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new LibraryProfile());
});

builder.Services.AddSingleton<DataSeeder>();

builder.Services.AddTransient<IRepository<BookLoan, ObjectId>, BookLoanRepository>();
builder.Services.AddTransient<IRepository<Book, ObjectId>, BookRepository>();
builder.Services.AddTransient<IRepository<EditionType, ObjectId>, EditionTypeRepository>();
builder.Services.AddTransient<IRepository<Publisher, ObjectId>, PublisherRepository>();
builder.Services.AddTransient<IRepository<Reader, ObjectId>, ReaderRepository>();

builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IApplicationService<BookLoanDto, BookLoanCreateUpdateDto, ObjectId>, BookLoanService>();
builder.Services.AddScoped<IApplicationService<EditionTypeDto, EditionTypeCreateUpdateDto, ObjectId>, EditionTypeService>();
builder.Services.AddScoped<IApplicationService<PublisherDto, PublisherCreateUpdateDto, ObjectId>, PublisherService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IReaderService, ReaderService>();

builder.AddMongoDBClient("libraryClient");

builder.Services.AddDbContext<LibraryDbContext>((services, o) =>
{
    var db = services.GetRequiredService<IMongoDatabase>();
    o.UseMongoDB(db.Client, db.DatabaseNamespace.DatabaseName);
});

builder.Services.AddHostedService<LibraryRabbitMqConsumer>();
builder.AddRabbitMQClient("library-rabbitmq");

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
var dataSeed = scope.ServiceProvider.GetRequiredService<DataSeeder>();

if (!dbContext.Loans.Any())
{
    foreach (var family in dataSeed.Loans)
        await dbContext.Loans.AddAsync(family);

    foreach (var model in dataSeed.Books)
        await dbContext.Books.AddAsync(model);

    foreach (var flight in dataSeed.EditionTypes)
        await dbContext.EditionTypes.AddAsync(flight);

    foreach (var passenger in dataSeed.Publishers)
        await dbContext.Publishers.AddAsync(passenger);

    foreach (var ticket in dataSeed.Readers)
        await dbContext.Readers.AddAsync(ticket);

    await dbContext.SaveChangesAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
