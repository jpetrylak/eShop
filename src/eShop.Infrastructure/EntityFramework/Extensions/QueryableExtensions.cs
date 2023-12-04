using System.Linq.Expressions;
using eShop.Domain.Orders;
using eShop.Shared.DDD;
using eShop.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace eShop.Infrastructure.EntityFramework.Extensions;

public static class QueryableExtensions
{
    public static async Task<Order> GetAsync(this IQueryable<Order> query, long orderId)
    {
        return await query
            .Include(x => x.Positions)
            .FirstOrDefaultAsync(x => x.Id == orderId) ?? throw new EntityNotFoundException(orderId, typeof(Order));
    }

    public static async Task<TEntity> GetAsync<TEntity, TKey>(this IQueryable<TEntity> query, TKey entityId)
        where TEntity : EntityBase<TKey>
    {
        return await query
                   .FirstOrDefaultAsync(CreateEqualityExpressionForId<TEntity, TKey>(entityId))
               ?? throw new EntityNotFoundException(entityId, typeof(TEntity));
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