namespace Library.Shared.Messages
{
    public class OrderConfirmed
    {
        public Guid OrderId { get; set; }
        public string CustomerId { get; set; }
        // You can add additional properties as needed
    }
}