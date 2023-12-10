using System.Linq.Expressions;
using Convey.CQRS.Queries;
using eShop.Shared.DDD;
using eShop.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace eShop.Infrastructure.EntityFramework.Extensions;

public static class QueryableExtensions
{
    public static async Task<TEntity> GetAsync<TEntity, TKey>(this IQueryable<TEntity> query, TKey entityId)
        where TEntity : EntityBase<TKey>
    {
        return await query
                   .FirstOrDefaultAsync(CreateEqualityExpressionForId<TEntity, TKey>(entityId))
               ?? throw new EntityNotFoundException(entityId, typeof(TEntity));
    }

    public static async Task<PagedResult<T>> PaginateAsync<T>(this IQueryable<T> collection, string orderBy,
        string sortOrder, int page = 1, int resultsPerPage = 10)
    {
        if (page <= 0)
        {
            page = 1;
        }

        if (resultsPerPage <= 0)
        {
            resultsPerPage = 10;
        }

        var isEmpty = await collection.AnyAsync() == false;
        if (isEmpty)
        {
            return PagedResult<T>.Empty;
        }

        var totalResults = await collection.CountAsync();
        var totalPages = (int) Math.Ceiling((decimal) totalResults / resultsPerPage);

        List<T> data;
        if (string.IsNullOrWhiteSpace(orderBy))
        {
            data = await collection.Limit(page, resultsPerPage).ToListAsync();
            return PagedResult<T>.Create(data, page, resultsPerPage, totalPages, totalResults);
        }

        data = await collection.OrderByDynamic(orderBy, sortOrder).Limit(page, resultsPerPage).ToListAsync();;

        return PagedResult<T>.Create(data, page, resultsPerPage, totalPages, totalResults);
    }

    public static IQueryable<T> Limit<T>(this IQueryable<T> collection,
        int page = 1, int resultsPerPage = 10)
    {
        if (page <= 0)
        {
            page = 1;
        }
        if (resultsPerPage <= 0)
        {
            resultsPerPage = 10;
        }
        var skip = (page - 1) * resultsPerPage;
        var data = collection.Skip(skip)
            .Take(resultsPerPage);

        return data;
    }

    public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string orderByMember, string sortOrder)
    {
        var queryElementTypeParam = Expression.Parameter(typeof(T));
        MemberExpression memberAccess = default;
        try
        {
            memberAccess = Expression.PropertyOrField(queryElementTypeParam, orderByMember);
        }
        catch (ArgumentException e)
        {
            if (e.Message.Contains("is not a member of type"))
            {
                throw new NotRecognizedFieldException(orderByMember);
            }

            throw;
        }

        var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

        var orderBy = Expression.Call(
            typeof(Queryable),
            sortOrder?.ToLowerInvariant() == "asc" ? "OrderBy" : "OrderByDescending",
            new Type[] { typeof(T), memberAccess.Type },
            query.Expression,
            Expression.Quote(keySelector));

        return query.Provider.CreateQuery<T>(orderBy);
    }

    private static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId<TEntity, TKey>(TKey id)
    {
        var lambdaParam = Expression.Parameter(typeof(TEntity));
        var leftExpression = Expression.PropertyOrField(lambdaParam, "Id");

        Expression<Func<object>> closure = () => id;
        var rightExpression = Expression.Convert(closure.Body, leftExpression.Type);

        var lambdaBody = Expression.Equal(leftExpression, rightExpression);

        return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
    }
}
