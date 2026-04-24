using System.Linq.Expressions;
using Convey.CQRS.Queries;
using eShop.Application.Products.Models;
using eShop.Domain.Products;
using eShop.Infrastructure.EntityFramework;
using eShop.Infrastructure.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace eShop.Application.Products.Queries.Handlers;

public class GetPagedProductsQueryHandler : IQueryHandler<GetPagedProductsQuery, PagedResult<ProductModel>>
{
    private readonly EShopDbContext _dbContext;

    public GetPagedProductsQueryHandler(EShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<ProductModel>> HandleAsync(GetPagedProductsQuery query, CancellationToken ct)
    {
        PagedResult<ProductModel> pagedResult = await _dbContext.Products
            .AsNoTracking()
            .Select(SelectExpression())
            .PaginateAsync(query.OrderBy, query.SortOrder, query.Page, query.Results);

        return pagedResult;
    }

    private Expression<Func<Product, ProductModel>> SelectExpression() =>
        p => new ProductModel
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price
        };
}
