using System.Reflection;
using MassTransit;
using PizzaMaui.API.Orders.Kitchen.Consumers;
using PizzaMaui.API.Orders.Kitchen.StateMachines;
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
        //cfg.ConfigureEndpoints(context);
        cfg.ReceiveEndpoint("kitchen-order-created", z =>
        {
            z.BindDeadLetterQueue("kitchen-order-created-dead");
            z.ConfigureConsumer<KitchenOrderCreatedConsumer>(context);
        });
        cfg.ReceiveEndpoint("kitchen-order-accepted", z =>
        {
            z.BindDeadLetterQueue("kitchen-order-accepted-dead");
            z.ConfigureConsumer<KitchenOrderAcceptedConsumer>(context);
        });
        cfg.ReceiveEndpoint("kitchen-order-rejected", z =>
        {
            z.BindDeadLetterQueue("kitchen-order-rejected-dead");
            z.ConfigureConsumer<KitchenOrderRejectedConsumer>(context);
        });
        cfg.ReceiveEndpoint("kitchen-order-is-cooking", z =>
        {
            z.BindDeadLetterQueue("kitchen-order-is-cooking-dead");
            z.ConfigureConsumer<KitchenOrderIsCookingConsumer>(context);
        });
        cfg.ReceiveEndpoint("saga-order-queue", z =>
        {
            z.BindDeadLetterQueue("saga-order-queue-dead");
            z.ConfigureSaga<KitchenCookerState>(context);
        });
    }); 
});

var app = builder.Build();

app.Run();

