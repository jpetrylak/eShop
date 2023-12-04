using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using eShop.Application.Orders.Commands;
using eShop.Application.Orders.Models;
using eShop.Application.Orders.Queries;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public OrdersController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [HttpGet("{id}")]
    public async Task<OrderModel> Get(long id)
    {
        return await _queryDispatcher.QueryAsync(new GetOrderQuery(id));
    }

    [HttpGet]
    public async Task<IEnumerable<OrderModel>> GetAll()
    {
        return await _queryDispatcher.QueryAsync(new GetAllOrderQuery());
    }
    
    [HttpGet("{id}/history")]
    public async Task<IEnumerable<OrderHistoryModel>> History(long id)
    {
        return await _queryDispatcher.QueryAsync(new GetOrderHistoryQuery(id));
    }
    
    [HttpPost]
    public async Task<long> Create(CreateOrderModel input)
    {
        var orderGuid = Guid.NewGuid();
        await _commandDispatcher.SendAsync(Map(input, orderGuid));
        
        return await _queryDispatcher.QueryAsync(new GetOrderIdQuery(orderGuid));
    }

    [HttpPut("addPosition")]
    public async Task AddPosition(AddOrderPositionModel input)
    {
        await _commandDispatcher.SendAsync(new AddOrderPositionCommand(input.OrderId, input.ProductId, input.Amount));
    }

    [HttpPut("removePosition")]
    public async Task RemovePosition(RemoveOrderPositionModel input)
    {
        await _commandDispatcher.SendAsync(new RemoveOrderPositionCommand(input.OrderId, input.ProductId));
    }

    [HttpPost("pay")]
    public async Task Pay(PayForOrderModel order)
    {
        await _commandDispatcher.SendAsync(new PayForOrderCommand(order.OrderId));
    }
    
    [HttpPost("ship")]
    public async Task Pay(ShipOrderModel order)
    {
        await _commandDispatcher.SendAsync(new ShipOrderCommand(order.OrderId));
    }

    private CreateOrderCommand Map(CreateOrderModel input, Guid orderGuid)
    {
        List<CreateOrderPositionModel> positions = input.Positions
            .Select(p => new CreateOrderPositionModel
            {
                ProductId = p.ProductId,
                Amount = p.Amount
            })
            .ToList();

        var command = new CreateOrderCommand(orderGuid, input.UserEmail, input.ShippingAddress, positions);
        return command;
    }
}