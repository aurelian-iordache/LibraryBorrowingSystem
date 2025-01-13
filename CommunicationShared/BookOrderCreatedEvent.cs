namespace CommunicationShared;

public record BookOrderCreatedEvent(
    Guid OrderId,
    Guid CustomerId,
    string CustomerName,
    DateTime OrderDate,
    string Status,
    bool OrderConfirmed
);
