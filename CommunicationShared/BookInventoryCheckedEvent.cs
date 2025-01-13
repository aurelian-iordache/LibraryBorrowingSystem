namespace CommunicationShared;

public record BookInventoryCheckedEvent(Guid OrderId, bool OrderConfirmed);