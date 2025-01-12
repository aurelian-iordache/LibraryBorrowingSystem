namespace Library.Shared.Messages;

public class OrderCreated
{
    public Guid OrderId { get; set; }
    public string CustomerId { get; set; }
    public List<string> BookTitles { get; set; }
}

public class InventoryCheck
{
    public Guid OrderId { get; set; }
    public bool InStock { get; set; }
}

public class OrderConfirmed
{
    public Guid OrderId { get; set; }
}

public class OrderDenied
{
    public Guid OrderId { get; set; }
}