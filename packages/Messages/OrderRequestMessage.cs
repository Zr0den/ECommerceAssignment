namespace Messages;

public class OrderRequestMessage
{
    public string CustomerId { get; set; }
    public string Status { get; set; }
    public int[] ProductIds { get; set; }
}