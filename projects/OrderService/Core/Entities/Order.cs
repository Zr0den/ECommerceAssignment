using Messages;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Core.Entities;

public class Order
{
    public int OrderId { get; set; }
    public string CustomerId { get; set; }
    public string Status { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}