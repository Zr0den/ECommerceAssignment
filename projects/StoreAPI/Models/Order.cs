namespace StoreAPI.Models;

public class Order
{
    public string CustomerId { get; set; }
    public string Status { get; set; }
    public List<Tuple<int, int>> Orders { get; set; }
}