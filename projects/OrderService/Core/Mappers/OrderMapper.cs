using Mapper;
using Messages;
using OrderService.Core.Entities;

namespace OrderService.Core.Mappers;

// DONE: Modify or create mappers as you see fit
public class OrderRequestMapper : IMapper<OrderRequestMessage, Order>
{
    public OrderRequestMessage Map(Order model)
    {
        return new OrderRequestMessage
        {
            OrderId = model.OrderId,
            CustomerId = model.CustomerId,
            Status = model.Status,
        };
    }

    public Order Map(OrderRequestMessage model)
    {
        return new Order
        {
            OrderId = model.OrderId,
            CustomerId = model.CustomerId,
            Status = model.Status
        };
    }
}

public class OrderResponseMapper : IMapper<OrderResponseMessage, Order>
{
    public OrderResponseMessage Map(Order model)
    {
        return new OrderResponseMessage
        {
            OrderId = model.OrderId,
            CustomerId = model.CustomerId,
            Status = model.Status
        };
    }

    public Order Map(OrderResponseMessage model)
    {
        return new Order
        {
            OrderId = model.OrderId,
            CustomerId = model.CustomerId,
            Status = model.Status
        };
    }
}