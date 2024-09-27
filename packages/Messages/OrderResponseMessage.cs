namespace Messages
{
    public class OrderResponseMessage
    {
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public List<OrderItem> Orders { get; set; }
        public double Cost { get; set; }
    }
}