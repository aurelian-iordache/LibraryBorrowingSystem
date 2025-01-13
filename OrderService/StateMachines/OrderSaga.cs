using Library.Shared.Messages;
using Automatonymous;

namespace OrderService.StateMachines
{
    public class OrderSaga : MassTransitStateMachine<OrderSagaState>
    {
        // Define states
        public State Rejected { get; private set; }
        public State ReadyToShip { get; private set; }
        public State Completed { get; private set; }

        // Define events (messages)
        public Event<OrderCreated> OrderCreated { get; private set; }
        public Event<OrderProcessed> OrderProcessed { get; private set; }
        public Event<OrderConfirmed> OrderConfirmed { get; private set; }
        public Event<OrderDenied> OrderDenied { get; private set; }

        public OrderSaga()
        {
            // This tells MassTransit which property on the saga state tracks the current state
            InstanceState(x => x.CurrentState);

            Initially(
                When(OrderCreated)
                    .Then(context =>
                    {
                        // Initialize the saga's state using data from the event
                        context.Instance.CorrelationId = context.Data.OrderId; // Set the CorrelationId
                        context.Instance.CustomerId = context.Data.CustomerId;
                        context.Instance.Status = "Order Received";  // Initialize status
                        context.Instance.OrderCreatedDate = DateTime.UtcNow;
                    })
                    .IfElse(
                        context => context.Data.BookTitles.Any(item => item.Contains("notinstock", StringComparison.OrdinalIgnoreCase)),
                        rejected => rejected
                            .Then(context =>
                            {
                                // Mark the order as rejected if any item is out of stock
                                context.Instance.Status = "Order Rejected";
                                context.Instance.RejectionReason = "One or more items are not in stock.";
                                context.Instance.OrderProcessedDate = DateTime.UtcNow;
                                Console.WriteLine($"Order {context.Instance.OrderId} rejected due to stock issues.");
                            })
                            .TransitionTo(Rejected)
                            .Publish(context => new OrderDenied
                            {
                                OrderId = context.Instance.CorrelationId
                            }),
                        readyToShip => readyToShip
                            .Then(context =>
                            {
                                // Mark the order as ready to ship
                                context.Instance.Status = "Ready to Ship";
                                context.Instance.OrderProcessedDate = DateTime.UtcNow;
                                Console.WriteLine($"Order {context.Instance.OrderId} is ready to ship.");
                            })
                            .TransitionTo(ReadyToShip)
                            .Publish(context => new OrderConfirmed
                            {
                                OrderId = context.Instance.CorrelationId,
                                CustomerId = context.Instance.CustomerId
                            })
                    )
            );

            // Handle the 'Rejected' state
            During(Rejected,
                When(OrderProcessed)
                    .ThenAsync(context =>
                    {
                        // When the order is processed after rejection, mark it as completed
                        Console.WriteLine($"Order {context.Instance.CorrelationId} marked as completed after rejection.");
                        return Task.CompletedTask; // No need to explicitly return null, just an empty task
                    })
                    .TransitionTo(Completed)
                    .Publish(context => new OrderProcessed
                    {
                        OrderId = context.Instance.CorrelationId,
                        Status = "Completed"
                    })
            );

            // Handle the 'ReadyToShip' state
            During(ReadyToShip,
                When(OrderProcessed)
                    .ThenAsync(context =>
                    {
                        // When the order is processed after being ready to ship, mark it as completed
                        Console.WriteLine($"Order {context.Instance.CorrelationId} marked as completed after being ready to ship.");
                        return Task.CompletedTask; // No need to explicitly return null, just an empty task
                    })
                    .TransitionTo(Completed)
                    .Publish(context => new OrderProcessed
                    {
                        OrderId = context.Instance.CorrelationId,
                        Status = "Completed"
                    })
            );
        }
    }
}
