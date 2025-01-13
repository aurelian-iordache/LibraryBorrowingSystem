using CommunicationShared;
using MassTransit;

namespace OrdersManagement.Application.Sagas;

public class OrderSaga: MassTransitStateMachine<OrderSagaState>
{
    
    public State Created { get; set; }
    public State Checked { get; set; }
    public State Confirmed { get; set; }
    public State Canceled { get; set; }

    public DateTime OrderCreatedDate { get; set; }
    public DateTime OrderProcessedDate { get; set; }    
    
    public Event<BookOrderCreatedEvent> OrderCreated { get; set; }
    public Event<BookInventoryCheckedEvent> InventoryChecked { get; set; }

    public OrderSaga()
    {
        InstanceState(s=>s.CurrentState);
        
        Event(()=> OrderCreated, x =>
        {
            x.CorrelateById(ctx => ctx.Message.OrderId);
            x.SelectId(ctx => ctx.Message.OrderId);
        });
        
        Event(()=> InventoryChecked, x =>
        {
            x.CorrelateById(ctx => ctx.Message.OrderId);
        });
        
        Initially(
            When(OrderCreated)
                .Then(ctx =>
                {
                    //check if all the books are available in the inventory
                    Console.WriteLine($"Check if there is any book missing from the inventory");
                    var orderCanBeConfirmed = ctx.Message.Books.All(b => b.IsInStock);

                    ctx.Saga.OrderCreatedDate = ctx.Message.OrderDate;
                    ctx.Saga.OrderConfirmed = orderCanBeConfirmed;
                })
                .ThenAsync(ctx=>Console.Out.WriteAsync("Order Created"))
                .TransitionTo(Created)
                .Publish(ctx => new BookInventoryCheckEvent(ctx.Saga.CorrelationId, Guid.NewGuid(), ctx.Saga.OrderConfirmed))
        );
        
        
        During(Created,
            When(InventoryChecked)
                .Then(ctx =>
                {
                    ctx.Saga.OrderProcessedDate = DateTime.UtcNow;
                })
                .ThenAsync(ctx=>Console.Out.WriteAsync("Inventory Checked"))
                .TransitionTo(Checked));

        When(OrderCreated)
            .ThenAsync(ctx => Console.Out.WriteAsync($"Duplicate OrderCreatedEvent received for OrderId: {ctx.Saga.CorrelationId}"));

        During(Checked,
            When(InventoryChecked)
                .IfElse(ctx => ctx.Saga.OrderConfirmed,
                    thenClause => thenClause
                        .ThenAsync(ctx => Console.Out.WriteLineAsync("Order Confirmed"))
                        .TransitionTo(Confirmed)
                        .Then(ctx =>
                        {
                            ctx.Saga.OrderProcessedDate = DateTime.UtcNow;
                        })
                        .ThenAsync(ctx => Console.Out.WriteLineAsync("Transitioning to Finalized"))
                        .Finalize(),
                    elseClause => elseClause
                        .ThenAsync(ctx => Console.Out.WriteLineAsync("Order Canceled"))
                        .TransitionTo(Canceled)
                        .Then(ctx =>
                        {
                            ctx.Saga.OrderProcessedDate = DateTime.UtcNow;
                        })
                        .ThenAsync(ctx => Console.Out.WriteLineAsync("Transitioning to Finalized"))
                        .Finalize()
                )
        );
    }
}