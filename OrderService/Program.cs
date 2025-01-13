using Library.Shared.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Consumers;
using OrderService.Database;
using OrderService.StateMachines;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<OrderStateDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddMassTransit(x =>
{
    // Configure MassTransit to use RabbitMQ
    x.UsingRabbitMq((context, cfg) =>
    {
        // Configure RabbitMQ connection
        cfg.Host(builder.Configuration["RabbitMQ:HostName"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:UserName"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });

        // Bind consumers to relevant RabbitMQ queues
        cfg.ReceiveEndpoint("order-created-queue", e =>
        {
            e.Consumer<OrderCreatedConsumer>(context);
        });

        cfg.ReceiveEndpoint("inventory-checked-queue", e =>
        {
            e.Consumer<InventoryCheckConsumer>(context);
        });

        cfg.ReceiveEndpoint("order-confirmed-queue", e =>
        {
            e.Consumer<OrderConfirmedConsumer>(context);
        });

        cfg.ReceiveEndpoint("order-denied-queue", e =>
        {
            e.Consumer<OrderDeniedConsumer>(context);
        });

        cfg.ReceiveEndpoint("order-processed-queue", e =>
        {
            e.Consumer<OrderProcessedConsumer>(context);
        });

        //// Optionally: Add a saga state machine if needed
        //cfg.ReceiveEndpoint("order-saga-queue", e =>
        //{
        //    e.StateMachineSaga<OrderSagaState>(context); // Attach the saga state machine to the queue
        //});
    });
});

//// Add the necessary services for MassTransit
//builder.Services.AddMassTransitHostedService();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Define Minimal API endpoint for creating orders
app.MapPost("/orders", async (Order newOrder, IPublishEndpoint publishEndpoint) =>
{
    // Publish the new order to the message bus
    await publishEndpoint.Publish(newOrder);

    // Return a Created response with the new order
    return Results.Created($"/orders/{newOrder.OrderId}", newOrder);
});

app.Run();