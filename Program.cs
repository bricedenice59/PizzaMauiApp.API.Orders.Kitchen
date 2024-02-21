using System.Reflection;
using MassTransit;
using PizzaMauiApp.API.Shared.Environment;

var builder = WebApplication.CreateBuilder(args);

//decode configuration environment variables;
var rabbitMqConnectionConfig = new DbConnectionConfig(builder.Configuration, "RabbitMq");
//check if secrets data are correctly read and binded to object
ArgumentException.ThrowIfNullOrEmpty(rabbitMqConnectionConfig.Host);
ArgumentException.ThrowIfNullOrEmpty(rabbitMqConnectionConfig.Port);
ArgumentException.ThrowIfNullOrEmpty(rabbitMqConnectionConfig.Username);
ArgumentException.ThrowIfNullOrEmpty(rabbitMqConnectionConfig.Password);

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    
    x.SetInMemorySagaRepositoryProvider();
    
    var entryAssembly = Assembly.GetEntryAssembly();
    
    x.AddConsumers(entryAssembly);
    x.AddSagaStateMachines(entryAssembly);
    x.AddSagas(entryAssembly);
    x.AddActivities(entryAssembly);
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host($"rabbitmq://{rabbitMqConnectionConfig.Host}:{rabbitMqConnectionConfig.Port}", hostconfig =>
        {
            hostconfig.Username(rabbitMqConnectionConfig.Username);
            hostconfig.Password(rabbitMqConnectionConfig.Password);
        });
        cfg.ConfigureEndpoints(context);
    }); 
});

var app = builder.Build();

app.Run();

