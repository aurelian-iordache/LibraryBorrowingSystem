using Library.Shared.Models;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

// Add services required for Minimal APIs
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

// Swagger endpoint configuration
app.UseHttpsRedirection();

app.Run();