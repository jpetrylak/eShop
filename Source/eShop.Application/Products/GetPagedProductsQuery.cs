using Convey.CQRS.Queries;
using eShop.Application.Products.Models;

namespace eShop.Application.Products;

public record GetPagedProductsQuery(string OrderBy, int Page, int Results, string SortOrder)
    : IQuery<PagedResult<ProductModel>>, IPagedQuery;

