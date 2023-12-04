using eShop.Shared.DDD.Validation;
using eShop.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace eShop.Shared.WebApi.ErrorHandling;

public class EShopExceptionHandler : IExceptionHandler
{
    private readonly IExceptionHandler<EntityNotFoundException> _entityNotFoundExceptionHandler;
    private readonly IExceptionHandler<BusinessRuleException> _businessRuleExceptionHandler;

    public EShopExceptionHandler(
        IExceptionHandler<EntityNotFoundException> entityNotFoundExceptionHandler,
        IExceptionHandler<BusinessRuleException> businessRuleExceptionHandler)
    {
        _entityNotFoundExceptionHandler = entityNotFoundExceptionHandler;
        _businessRuleExceptionHandler = businessRuleExceptionHandler;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        switch (exception)
        {
            case EntityNotFoundException e:
                await _entityNotFoundExceptionHandler.HandleAsync(httpContext, e, ct);
                return true;
            case BusinessRuleException e:
                await _businessRuleExceptionHandler.HandleAsync(httpContext, e, ct);
                return true;
            default:
                return false;
        }
    }
}