using MassTransit;

namespace OrderService.StateMachines;

public class OrderSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public List<string> BookTitles { get; set; }
    public string CustomerId { get; set; }
    public string Status { get; set; }
}