using Library.Shared.Messages;
using MassTransit;

namespace CustomerService.Consumers
{
    public class OrderDeniedConsumer : IConsumer<OrderDenied>
    {
        public async Task Consume(ConsumeContext<OrderDenied> context)
        {
            // Process the OrderDenied event, e.g., notify the customer or take other actions
            Console.WriteLine($"Order Denied: {context.Message.OrderId}");

            // Notify the customer or update their status
            await Task.CompletedTask;
        }
    }
}