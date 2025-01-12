using MassTransit;
using Library.Shared.Messages;

namespace OrderService.StateMachines;

public class OrderSaga : MassTransitStateMachine<OrderSagaState>
{
    public OrderSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderCreatedEvent, x => x.CorrelateById(m => m.Message.OrderId));
        Event(() => InventoryCheck, x => x.CorrelateById(m => m.Message.OrderId));

        Initially(
            When(OrderCreatedEvent)
                .Then(context => context.Instance.BookTitles = context.Message.BookTitles)
                .Then(context => context.Instance.CustomerId = context.Message.CustomerId)
                .TransitionTo(OrderCreated));

        During(OrderCreated,
            When(InventoryCheck)
                .IfElse(context => context.Message.InStock,
                    binder => binder
                        .Then(context => context.Instance.Status = "Confirmed")
                        .Publish(context => new OrderConfirmed { OrderId = context.Instance.CorrelationId })
                        .TransitionTo(Confirmed),
                    binder => binder
                        .Then(context => context.Instance.Status = "Denied")
                        .Publish(context => new OrderDenied { OrderId = context.Instance.CorrelationId })
                        .TransitionTo(Denied)));
    }

    public State OrderCreated { get; private set; }
    public State Confirmed { get; private set; }
    public State Denied { get; private set; }

    public Event<OrderCreated> OrderCreatedEvent { get; private set; }
    public Event<InventoryCheck> InventoryCheck { get; private set; }
}