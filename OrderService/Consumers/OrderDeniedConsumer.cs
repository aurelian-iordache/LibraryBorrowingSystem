using Library.Shared.Messages;
using MassTransit;

namespace OrderService.Consumers
{
    public class OrderDeniedConsumer : IConsumer<OrderDenied>
    {
        public async Task Consume(ConsumeContext<OrderDenied> context)
        {
            // Logic to handle OrderDenied event
            var orderDenied = context.Message;
            Console.WriteLine($"Order denied: {orderDenied.OrderId}");

            // You could do something like update the order status to "Denied" or notify the customer.
        }
    }
}