using MessageClient;
using Messages;
using OrderService.Core.Entities;
using OrderService.Core.Mappers;

namespace OrderService;

public class OrderService
{
    private readonly MessageClient<OrderRequestMessage> _newOrderClient;
    private readonly MessageClient<OrderResponseMessage> _orderCompletionClient;
    private readonly Core.Services.OrderService _orderService;
    private readonly OrderResponseMapper _orderResponseMapper;
    public OrderService(MessageClient<OrderRequestMessage> newOrderClient, MessageClient<OrderResponseMessage> orderCompletionClient, Core.Services.OrderService orderService, OrderResponseMapper orderResponseMapper)
    {
        _newOrderClient = newOrderClient;
        _orderCompletionClient = orderCompletionClient;
        _orderService = orderService;
        _orderResponseMapper = orderResponseMapper;
    }

    public void Start()
    {
        // Start listening for new orders
        //_newOrderClient.ConnectAndListen(HandleNewOrder);
        Action<OrderRequestMessage> callback = HandleNewOrder;
        _newOrderClient.Connect();
        _newOrderClient.ListenUsingTopic(callback, "", "newOrder");

        // Connect to the order completion topic
        Action<OrderResponseMessage> callback2 = HandleOrderCompletion;
        _orderCompletionClient.Connect();
        _orderCompletionClient.ListenUsingTopic(callback2, "ShippingService", "orderCompletion");
    }

    private void HandleNewOrder(OrderRequestMessage order)
    {
        /*
         * DONE: Handle new orders
         * - Check if the order is valid
         * - Create the order in the database (optional)
         * - Send the order to the stock service
         */
        bool valid = order.Status == "Order received.";

        if (valid)
        {
            List<OrderItem> orders = new List<OrderItem>();
            foreach (var item in order.Orders)
            {
                orders.Add(new OrderItem()
                {
                    ProductId = item.Item1,
                    Quantity = item.Item2
                });
            }

            //Save the order in the database
            var orderId = Guid.NewGuid().GetHashCode(); //We should be giving this through an identity in our database or something, not this but oh well
            _orderService.CreateOrder(new Order()
            {
                OrderId = orderId,
                CustomerId = order.CustomerId,
                Status = order.Status,
                OrderItems = orders
            });

            // Create new OrderResponseMessage
            Console.WriteLine($"Received new order from customer {order.CustomerId}");
            var orderResponse = new OrderResponseMessage
            {
                CustomerId = order.CustomerId,
                Status = "Order started",
                Orders = orders,
                OrderId = orderId
            };

            //Sending to stock service
            _orderCompletionClient.SendUsingTopic(orderResponse, "Stock");

            // Send the order completion to the customer using the customer ID as the topic
            //Console.WriteLine($"Sending order completion to customer {order.CustomerId}");
            //_orderCompletionClient.SendUsingTopic<OrderResponseMessage>(orderResponse,
            // orderResponse.CustomerId);
        }
        else
        {
            Console.WriteLine($"Invalid Order");
        }
    }

    private void HandleOrderCompletion(OrderResponseMessage dto)
    {
        /*
         * DONE: Handle the order completion, e.g. change the order status
         * - Update the order status in the database
         * - Notify the customer
         */

        OrderResponseMapper mapper = new();
        Order order = mapper.Map(dto);

        order.Status = "Order completed";
        _orderService.CompleteOrder(order);

        Console.WriteLine($"Sending order completion to customer {order.CustomerId}");
        _orderCompletionClient.SendUsingTopic<OrderResponseMessage>(dto,
         dto.CustomerId);

        //It is probably here we should send a message to StockService again to subtract the ordered quantity from our stock
        //There is a bigger unresolved problem here though - if the subtraction is first done here, then we open up the possibility of allowing multiple concurrent orders that may together exceed our stock
        //Solution would maybe be to do it when we first check our stock, and somehow make sure that we can revert it in case a problem arises before the order is fully complete
        //We would need to implement a unit of work that spans all our microservices with rollback functions though, which is a little complicated and i think is out of the scope of this assignment?
    }
}