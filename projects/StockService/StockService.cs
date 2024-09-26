using Messages;
using StockService.Core.Services;

namespace StockService;

using global::StockService.Core.Entities;
using MessageClient;

public class StockService
{
    private readonly MessageClient<OrderRequestMessage> _messageClient;
    private readonly ProductService _productService;

    public StockService(MessageClient<OrderRequestMessage> messageClient, ProductService productService)
    {
        _messageClient = messageClient;
        _productService = productService;
    }

    public void PopulateDb()
    {
        // Populate the database with some products
        _productService.PopulateDb();
    }

    public void Start()
    {
        // DONE: Start listening for new orders
        Action<OrderRequestMessage> callback = HandleNewOrder;
        _messageClient.ListenUsingTopic(callback, "", "Stock");
    }

    private void HandleNewOrder(OrderRequestMessage order)
    {
        // TODO: Handle the new orders
        /*
         * TODO: Handle new orders
         * - Check the stock of the products in the order
         * - Create a new order response with the stock status of the products
         * - Send the order response so the shipping service can calculate the shipping cost
         */

        IEnumerable<Product> products = _productService.GetOrderProducts(order.ProductIds);
        var orderResponse = new OrderResponseMessage
        {
            CustomerId = order.CustomerId,
            Status = "Stock processed"
        };

        // Send the order completion to the shipping service
        Console.WriteLine($"Sending stock processing to shipping service {orderResponse.CustomerId}");
        _messageClient.SendUsingTopic<OrderResponseMessage>(orderResponse,
            order.CustomerId);
    }
}