using Library.Generator.RabbitMq.Host;
using Library.Generator.RabbitMq.Host.Generator;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRabbitMQClient("library-rabbitmq");

builder.Services.AddScoped<LibraryRabbitMqProducer>();

builder.Services.Configure<GeneratorOptions>(builder.Configuration.GetSection("Generator"));

builder.Services.AddSingleton<BookLoanGenerator>();

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
builder.Services.AddSwaggerGen(options =>
{
    var assemblies = AppDomain.CurrentDomain.GetAssemblies()
    .Where(a => a.GetName().Name!.StartsWith("Library"))
    .Distinct();

    foreach (var assembly in assemblies)
    {
        var xmlFile = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
            options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
