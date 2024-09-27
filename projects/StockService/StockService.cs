using Messages;
using StockService.Core.Services;

namespace StockService;

using global::StockService.Core.Entities;
using MessageClient;
using System.Text;

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
        Action<OrderResponseMessage> callback = HandleNewOrder;
        _messageClient.Connect();
        _messageClient.ListenUsingTopic(callback, "", "Stock");
    }

    private void HandleNewOrder(OrderResponseMessage order)
    {
        // DONE: Handle the new orders
        /*
         * DONE: Handle new orders
         * - Check the stock of the products in the order
         * - Create a new order response with the stock status of the products
         * - Send the order response so the shipping service can calculate the shipping cost
         */

        //Do we even have the requested items?
        IEnumerable<Product> products = _productService.GetOrderProducts(order.Orders);

        if (order.Orders.Count == products.Count())
        {
            var orderResponse = new OrderResponseMessage
            {
                CustomerId = order.CustomerId,
                Status = "Stock processed",
                Orders = order.Orders
            };

            // Send the order completion to the shipping service
            Console.WriteLine($"Sending stock processing to shipping service {orderResponse.CustomerId}");
            _messageClient.SendUsingTopic<OrderResponseMessage>(orderResponse, "Calculate Shipping");
        }
        else
        {
            StringBuilder sb = new(" - ");
            foreach (var item in order.Orders)
            {
                sb.Append($"{{ ProductId: {item.ProductId}, Quantity: {item.Quantity} }}");
            }
            sb.Append(" - ");

            Console.WriteLine($"Invalid Order. Products either do not exist or ordered quantity exceeds stock: CustomerId = {order.CustomerId}, Order = {sb.ToString()}");
        }

    }
}