namespace OrdersManagement.Application.Dtos;

public record CreateBookOrderDto(
    Guid OrderId,
    DateTime OrderDate,
    string Status, // "Pending", "Confirmed", "Cancelled"
    Guid CustomerId,
    string CustomerName,
    string CustomerEmail,
    string CustomerPhone,
    string ShippingAddress,
    string City,
    string State,
    string Country,
    string PostalCode,
    DateTime? EstimatedDeliveryDate,
    bool OrderConfirmed,
    List<OrderItemDTO> Items,
    string Notes
);
public record OrderItemDTO(
    Guid ProductId,
    string ProductName
);