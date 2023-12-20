using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using eShop.Application.Models;
using eShop.Application.Orders.Commands;
using eShop.Application.Orders.Models;
using eShop.Application.Orders.Queries;
using eShop.Controllers.Orders.Models;
using Microsoft.AspNetCore.Mvc;
using ContentType = eShop.Shared.WebApi.ContentType;

namespace eShop.Controllers.Orders;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public OrdersController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Gets the paged orders.
    /// </summary>
    /// <param name="pagingModel">The paging data.</param>
    /// <returns>A 200 OK response containing the orders.</returns>
    [HttpGet(Name = OrdersControllerRoute.GetOrders)]
    [ProducesResponseType(typeof(PagedResult<OrderModel>), StatusCodes.Status200OK, ContentType.Json)]
    public async Task<IActionResult> GetOrders([FromQuery] PagingRequestModel pagingModel)
    {
        var pagedResult = await _queryDispatcher.QueryAsync(
            new GetPagedOrdersQuery(pagingModel.OrderBy, pagingModel.CurrentPage, pagingModel.ResultsPerPage, pagingModel.SortOrder));

        return Ok(pagedResult);
    }

    /// <summary>
    /// Gets an order with the specified unique identifier.
    /// </summary>
    /// <param name="orderId">The order unique identifier.</param>
    /// <returns>A 200 OK response containing the order or a 404 Not Found if an order with the specified unique
    /// identifier was not found.</returns>
    [HttpGet("{orderId:long}", Name = OrdersControllerRoute.GetOrder)]
    [ProducesResponseType(typeof(OrderDetailsModel), StatusCodes.Status200OK, ContentType.Json)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, ContentType.ProblemJson)]
    public async Task<IActionResult> Get(long orderId) => Ok(await GetOrderDetailsModelAsync(orderId));

    /// <summary>
    /// Gets the paged order history logs for the specified unique identifier.
    /// </summary>
    /// <param name="orderId">The order unique identifier.</param>
    /// <param name="pagingModel">The paging data.</param>
    /// <returns>A 200 OK response containing the order history logs or a 404 Not Found if an order with the specified unique
    /// identifier was not found.</returns>
    [HttpGet("{orderId:long}/history", Name = OrdersControllerRoute.GetHistory)]
    [ProducesResponseType(typeof(PagedResult<OrderHistoryModel>), StatusCodes.Status200OK, ContentType.Json)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, ContentType.ProblemJson)]
    public async Task<IActionResult> GetHistory(long orderId, [FromQuery] PagingRequestModel pagingModel)
    {
        var pagedResult = await _queryDispatcher.QueryAsync(
            new GetPagedOrderHistoryQuery(
                orderId, pagingModel.OrderBy, pagingModel.CurrentPage, pagingModel.ResultsPerPage, pagingModel.SortOrder));

        return Ok(pagedResult);
    }

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="input">The order to create.</param>
    /// <returns>A 201 Created response containing the newly created order or 409 Conflict if the order is
    /// invalid.</returns>
    [HttpPost(Name = OrdersControllerRoute.CreateOrder)]
    [ProducesResponseType(typeof(OrderDetailsModel), StatusCodes.Status201Created, ContentType.Json)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict, ContentType.ProblemJson)]
    public async Task<IActionResult> Create(CreateOrderApiModel input)
    {
        var orderGuid = Guid.NewGuid();
        await _commandDispatcher.SendAsync(new CreateOrderCommand(orderGuid, input.UserEmail, input.ShippingAddress));

        long orderId = await _queryDispatcher.QueryAsync(new GetOrderIdQuery(orderGuid));
        OrderDetailsModel order = await GetOrderDetailsModelAsync(orderId);

        return CreatedAtRoute(OrdersControllerRoute.GetOrder, new { orderId }, order);
    }

    /// <summary>
    /// Gets the order positions.
    /// </summary>
    /// <param name="orderId">The order unique identifier.</param>
    /// <param name="pagingModel">The paging data.</param>
    /// <returns>A 201 OK response containing the order positions or 404 Not Found
    /// if an order with the specified unique identifier was not found.</returns>
    [HttpGet("{orderId:long}/positions", Name = OrdersControllerRoute.GetPositions)]
    [ProducesResponseType(typeof(PagedResult<OrderPositionModel>), StatusCodes.Status200OK, ContentType.Json)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound, ContentType.ProblemJson)]
    public async Task<IActionResult> GetPositions(long orderId, [FromQuery] PagingRequestModel pagingModel)
    {
        var pagedResult = await _queryDispatcher.QueryAsync(
            new GetPagedOrderPositionsQuery(
                orderId, pagingModel.OrderBy, pagingModel.CurrentPage, pagingModel.ResultsPerPage, pagingModel.SortOrder));

        return Ok(pagedResult);
    }

    /// <summary>
    /// Creates an order position.
    /// </summary>
    /// <param name="orderId">The order unique identifier.</param>
    /// <param name="input">The order position to create.</param>
    /// <returns>A 201 Created response containing the order with newly created position or 409 Conflict if the order
    /// position is invalid.</returns>
    [HttpPost("{orderId:long}/positions", Name = OrdersControllerRoute.CreatePosition)]
    [ProducesResponseType(typeof(OrderDetailsModel), StatusCodes.Status201Created, ContentType.Json)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict, ContentType.ProblemJson)]
    public async Task<IActionResult> CreatePosition(long orderId, CreateOrderPositionApiModel input)
    {
        await _commandDispatcher.SendAsync(new CreateOrderPositionCommand(orderId, input.ProductId, input.Amount));
        OrderDetailsModel order = await GetOrderDetailsModelAsync(orderId);

        return CreatedAtRoute(OrdersControllerRoute.GetOrder, new { orderId }, order);
    }

    /// <summary>
    /// Deletes an order position.
    /// </summary>
    /// <param name="orderId">The order unique identifier.</param>
    /// <param name="productId">The product unique identifier of the order position to delete.</param>
    /// <returns>A 204 NoContent response if the position was deleted or 409 Conflict if the position is invalid.</returns>
    [HttpDelete("{orderId:long}/positions/{productId:long}", Name = OrdersControllerRoute.DeletePosition)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict, ContentType.ProblemJson)]
    public async Task<IActionResult> DeletePosition(long orderId, long productId)
    {
        await _commandDispatcher.SendAsync(new RemoveOrderPositionCommand(orderId, productId));

        return NoContent();
    }

    /// <summary>
    /// Pays the order position.
    /// </summary>
    /// <param name="orderId">The order unique identifier.</param>
    /// <returns>A 204 NoContent response if the order was paid or 409 Conflict if the operation failed.</returns>
    [HttpPost("{orderId:long}/pay", Name = OrdersControllerRoute.PayOrder)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Pay(long orderId)
    {
        await _commandDispatcher.SendAsync(new PayForOrderCommand(orderId));

        return NoContent();
    }

    /// <summary>
    /// Ships the order position.
    /// </summary>
    /// <param name="orderId">The order unique identifier.</param>
    /// <returns>A 204 NoContent response if the order was shipped or 409 Conflict if the operation failed.</returns>
    [HttpPost("{orderId:long}/ship", Name = OrdersControllerRoute.ShipOrder)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Ship(long orderId)
    {
        await _commandDispatcher.SendAsync(new ShipOrderCommand(orderId));

        return NoContent();
    }

    private async Task<OrderDetailsModel> GetOrderDetailsModelAsync(long orderId)
    {
        return await _queryDispatcher.QueryAsync(new GetOrderQuery(orderId));
    }
}
