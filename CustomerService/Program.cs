using MassTransit;
using CustomerService.Consumers;

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
        cfg.ReceiveEndpoint("order-confirmed-queue", e =>
        {
            e.Consumer<OrderConfirmedConsumer>(context);
        });

        cfg.ReceiveEndpoint("order-denied-queue", e =>
        {
            e.Consumer<OrderDeniedConsumer>(context);
        });
    });
});

builder.Services.AddMassTransitHostedService();

var app = builder.Build();
app.UseRouting();

app.Run();