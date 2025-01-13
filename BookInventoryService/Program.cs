using MassTransit;
using BookInventoryService.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add MassTransit and RabbitMQ configuration
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        // RabbitMQ host configuration
        cfg.Host(builder.Configuration["RabbitMQ:HostName"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:UserName"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });

        // Bind consumers to specific queues
        cfg.ReceiveEndpoint("order-created-queue", e =>
        {
            e.Consumer<OrderCreatedConsumer>(context);
        });

        cfg.ReceiveEndpoint("inventory-checked-queue", e =>
        {
            e.Consumer<InventoryCheckConsumer>(context);
        });
    });
});

builder.Services.AddMassTransitHostedService();

var app = builder.Build();
app.UseRouting();

app.Run();