var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddMongoDB("mongo").AddDatabase("mongo-db");

var rabbitMqQueue = builder.AddParameter("RabbitMQQueue");
var rabbitUserName = builder.AddParameter("RabbitMQLogin");
var rabbitPassword = builder.AddParameter("RabbitMQPassword");
var rabbitMq = builder.AddRabbitMQ("library-rabbitmq", userName: rabbitUserName, password: rabbitPassword)
    .WithManagementPlugin();

builder.AddProject<Projects.Library_Api_Host>("library-api-host")
    .WithReference(db, "libraryClient")
    .WithReference(rabbitMq)
    .WithEnvironment("RabbitMq:QueueName", rabbitMqQueue)
    .WaitFor(db)
    .WaitFor(rabbitMq);

builder.AddProject<Projects.Library_Generator_RabbitMq_Host>("library-generator-rabbitmq-host")
    .WithReference(rabbitMq)
    .WaitFor(rabbitMq)
    .WithEnvironment("RabbitMq:QueueName", rabbitMqQueue);

builder.Build().Run();