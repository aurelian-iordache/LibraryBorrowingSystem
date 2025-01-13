namespace Library.Shared.Messages;

public class InventoryCheck
{
    public Guid OrderId { get; set; }
    public bool InStock { get; set; }
}