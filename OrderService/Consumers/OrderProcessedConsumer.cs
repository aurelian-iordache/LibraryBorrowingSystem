using Library.Shared.Messages;
using MassTransit;

namespace OrderService.Consumers
{
    public class OrderProcessedConsumer : IConsumer<OrderProcessed>
    {
        public async Task Consume(ConsumeContext<OrderProcessed> context)
        {
            // Logic to handle OrderProcessed event
            var orderProcessed = context.Message;
            Console.WriteLine($"Order processed: {orderProcessed.OrderId}");

            // Here, you could update the order status to "Completed" or publish a "ReadyToShip" event.
            // This could trigger a follow-up action or state transition in the saga.
        }
    }
}