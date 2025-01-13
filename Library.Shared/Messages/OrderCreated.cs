namespace Library.Shared.Messages;

public class OrderCreated
{
    public Guid OrderId { get; set; }
    public string CustomerId { get; set; }
    public List<string> BookTitles { get; set; }
}