using Library.Shared.Messages;
using MassTransit;

namespace OrderService.Consumers
{
    public class OrderConfirmedConsumer : IConsumer<OrderConfirmed>
    {
        public async Task Consume(ConsumeContext<OrderConfirmed> context)
        {
            // Logic to handle OrderConfirmed event
            var orderConfirmed = context.Message;
            Console.WriteLine($"Order confirmed: {orderConfirmed.OrderId}");

            // You could update the order status in the database or publish other events.
            // For example, after confirmation, you might publish an OrderProcessed event.

            await context.Publish(new OrderProcessed
            {
                OrderId = orderConfirmed.OrderId
            });
        }
    }
}