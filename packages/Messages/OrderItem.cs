using System.ComponentModel.DataAnnotations.Schema;

namespace Messages;

[NotMapped]
public class OrderItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
      
}