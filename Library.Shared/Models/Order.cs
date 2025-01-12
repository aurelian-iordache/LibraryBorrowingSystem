namespace Library.Shared.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public string CustomerId { get; set; }
        public List<string> BookTitles { get; set; }
        public string Status { get; set; }
    }
}
