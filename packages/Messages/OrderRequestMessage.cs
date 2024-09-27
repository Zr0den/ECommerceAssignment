using System.ComponentModel.DataAnnotations.Schema;

namespace Messages;

[NotMapped]
public class OrderRequestMessage
{
    public string CustomerId { get; set; }
    public string Status { get; set; }
    public List<Tuple<int, int>> Orders { get; set; }
    public int OrderId { get; set; }
}