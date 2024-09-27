using System.ComponentModel.DataAnnotations.Schema;

namespace Messages
{
    [NotMapped]
    public class OrderResponseMessage
    {
        public string CustomerId { get; set; }
        public string Status { get; set; }
        public List<OrderItem> Orders { get; set; }
        public double Cost { get; set; }
        public int OrderId { get; set; }
    }
}