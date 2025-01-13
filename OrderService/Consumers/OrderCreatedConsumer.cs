using Library.Shared.Messages;
using MassTransit;

namespace OrderService.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            // Logic to handle the OrderCreated event
            var orderCreated = context.Message;
            Console.WriteLine($"Order created: {orderCreated.OrderId}, Customer: {orderCreated.CustomerId}");

            // Process the order, maybe check inventory, etc.
            // You might want to send an InventoryCheck message or update order status in your DB.

            // Example: Sending an InventoryCheck event
            var inventoryCheck = new InventoryCheck
            {
                OrderId = orderCreated.OrderId,
                InStock = true // Assume we check inventory here
            };

            // Publish the InventoryCheck message to another service (or queue)
            await context.Publish(inventoryCheck);
        }
    }
}