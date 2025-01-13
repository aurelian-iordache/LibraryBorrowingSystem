using Library.Shared.Messages;
using MassTransit;

namespace BookInventoryService.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            // Process the OrderCreated event, e.g., check if books are in stock
            Console.WriteLine($"Received Order Created: {context.Message.OrderId} for Customer: {context.Message.CustomerId}");

            // Here, you can trigger inventory checks or other actions
            await Task.CompletedTask;
        }
    }
}