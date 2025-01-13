namespace Library.Shared.Messages
{
    public class OrderProcessed
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; }
        // You can add additional properties based on your requirements
    }
}