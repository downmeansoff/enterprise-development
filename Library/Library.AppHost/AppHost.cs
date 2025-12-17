var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddMongoDB("mongo").AddDatabase("mongo-db");

var rabbitMqQueue = builder.AddParameter("RabbitMQQueue");

builder.AddProject<Projects.Library_Api_Host>("library-api-host")
    .WithReference(db, "libraryClient")
    .WithEnvironment("RabbitMq:QueueName", rabbitMqQueue)
    .WaitFor(db);

builder.Build().Run();