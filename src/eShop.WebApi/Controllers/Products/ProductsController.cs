using Convey.CQRS.Queries;
using eShop.Application.Models;
using eShop.Application.Products;
using eShop.Application.Products.Models;
using eShop.Shared.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Controllers.Products;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;

    public ProductsController(IQueryDispatcher queryDispatcher)
    {
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Gets the paged products.
    /// </summary>
    /// <param name="pagingModel">The paging data.</param>
    /// <returns>A 200 OK response containing products.</returns>
    [HttpGet(Name = ProductsControllerRoute.GetProducts)]
    [ProducesResponseType(typeof(PagedResult<ProductModel>), StatusCodes.Status200OK, ContentType.Json)]
    public async Task<IActionResult> GetProducts([FromQuery] PagingRequestModel pagingModel)
    {
        var pagedResult = await _queryDispatcher.QueryAsync(
            new GetPagedProductsQuery(pagingModel.OrderBy, pagingModel.CurrentPage, pagingModel.ResultsPerPage, pagingModel.SortOrder));

        return Ok(pagedResult);
    }
}
