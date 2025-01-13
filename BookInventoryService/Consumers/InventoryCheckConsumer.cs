using Library.Shared.Messages;
using MassTransit;

namespace BookInventoryService.Consumers
{
    public class InventoryCheckConsumer : IConsumer<InventoryCheck>
    {
        public async Task Consume(ConsumeContext<InventoryCheck> context)
        {
            // Process the InventoryCheck event, for example, check if books are available
            Console.WriteLine($"Inventory check for Order: {context.Message.OrderId}, InStock: {context.Message.InStock}");

            // Handle inventory check logic here
            await Task.CompletedTask;
        }
    }
}