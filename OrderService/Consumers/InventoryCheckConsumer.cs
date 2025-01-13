using Library.Shared.Messages;
using MassTransit;

namespace OrderService.Consumers
{
    public class InventoryCheckConsumer : IConsumer<InventoryCheck>
    {
        public async Task Consume(ConsumeContext<InventoryCheck> context)
        {
            // Logic to handle InventoryCheck event
            var inventoryCheck = context.Message;
            Console.WriteLine($"Inventory check for order {inventoryCheck.OrderId}: In stock - {inventoryCheck.InStock}");

            // You could do some logic here, like calling another service or updating the order status.
            // If in stock, you might publish an OrderConfirmed event, else OrderDenied.

            if (inventoryCheck.InStock)
            {
                // Send OrderConfirmed event
                await context.Publish(new OrderConfirmed
                {
                    OrderId = inventoryCheck.OrderId
                });
            }
            else
            {
                // Send OrderDenied event
                await context.Publish(new OrderDenied
                {
                    OrderId = inventoryCheck.OrderId
                });
            }
        }
    }
}