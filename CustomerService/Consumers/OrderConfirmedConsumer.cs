using Library.Shared.Messages;
using MassTransit;

namespace CustomerService.Consumers
{
    public class OrderConfirmedConsumer : IConsumer<OrderConfirmed>
    {
        public async Task Consume(ConsumeContext<OrderConfirmed> context)
        {
            // Process the OrderConfirmed event, e.g., update the customer order status
            Console.WriteLine($"Order Confirmed: {context.Message.OrderId}");

            // Update the customer's order status or trigger further actions
            await Task.CompletedTask;
        }
    }
}