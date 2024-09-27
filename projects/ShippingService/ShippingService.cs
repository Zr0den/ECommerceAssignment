using MessageClient;
using Messages;

namespace ShippingService;

public class ShippingService
{
    private readonly MessageClient<OrderResponseMessage> _messageClient;
    
    public ShippingService(MessageClient<OrderResponseMessage> messageClient)
    {
        _messageClient = messageClient;
    }
    
    public void Start()
    {
        // DONE: Start listening for orders that need to be shipped
        Action<OrderResponseMessage> callback = HandleOrderShippingCalculation;
        _messageClient.Connect();
        _messageClient.ListenUsingTopic(callback, "", "Calculate Shipping");
    }
    
    private void HandleOrderShippingCalculation(OrderResponseMessage orderResponse)
    {
        /*
         * DONE: Handle the calculation of the shipping cost for the order
         * - Calculate the shipping cost
         * - Change the status of the order and apply the shipping cost
         * - Send the processed order to the order service for completion
         */

        var cost = ShippingCalculator.CalculateShippingCost(orderResponse);

        var newOrderResponse = new OrderResponseMessage
        {
            CustomerId = orderResponse.CustomerId,
            Status = "Costs calculated",
            Cost = cost
        };

        Console.WriteLine($"Sending processed order to OrderService {newOrderResponse.CustomerId}");
        _messageClient.SendUsingTopic<OrderResponseMessage>(newOrderResponse, "orderCompletion");

    }
}