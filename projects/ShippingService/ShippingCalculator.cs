using Messages;

namespace ShippingService;

public static class ShippingCalculator
{
    // DONE: Implement this method, calculate shipping costs as you see fit
    //These values should ideally not be stored in the code itself, but rather in a database of some sort. These values would in the real world likely be fluid and would therefore require constant releases to have these values updated which is not sustainable
    private static readonly Dictionary<int, double> ProductValues = new()
    {
        {1, 5.5},
        {2, 12.5},
        {3, 51.75},
    };

    public static double CalculateShippingCost(OrderResponseMessage orderResponse)
    {
        double totalCost = 0;

        foreach (var item in orderResponse.Orders)
        {
            totalCost += ProductValues.TryGetValue(item.ProductId, out double value) ? value * item.Quantity : 0; // If item does not exist in the table just ignore it for now
        }

        return totalCost;
    }
}