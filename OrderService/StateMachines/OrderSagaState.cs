using Automatonymous;

namespace OrderService.StateMachines
{
    public class OrderSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }  // Required for correlation
        public string CurrentState { get; set; } // Tracks the current state

        // Order properties
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public DateTime OrderCreatedDate { get; set; }
        public DateTime? OrderProcessedDate { get; set; }
        public string RejectionReason { get; set; }

        // For tracking order stages
        public string OrderId => CorrelationId.ToString();
    }
}