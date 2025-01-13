using CommunicationShared;

namespace InventoryManagement.Consumers;
using MassTransit;

public class BookOrderCreatedConsumer : IConsumer<BookOrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<BookOrderCreatedEvent> context)
    {
        var message = context.Message;

        //process the OrderCreatedEvent
        Console.WriteLine($"Order Received: {message.OrderId}");
        Console.WriteLine($"Customer: {message.CustomerName}");
        Console.WriteLine($"Status: {message.Status}");

        await context.Publish<BookInventoryCheckEvent>(new BookInventoryCheckedEvent(context.Message.OrderId));

        await Task.CompletedTask;
    }
}