using System.Net;
using eShop.Shared.DDD.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace eShop.Shared.WebApi.ErrorHandling;

public class BusinessRuleExceptionHandler : ExceptionHandlerBase<BusinessRuleException>, IExceptionHandler<BusinessRuleException>
{
    private readonly ILogger<BusinessRuleExceptionHandler> _logger;

    public BusinessRuleExceptionHandler(ILogger<BusinessRuleExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(HttpContext httpContext, BusinessRuleException exception, CancellationToken ct)
    {
        _logger.LogError("Business rule exception. Message: {exceptionMessage}, Time of occurrence (UTC) {time}",
            exception.ErrorMessage, DateTime.UtcNow);

        await HandleAsync(httpContext, HttpStatusCode.BadRequest, "Business rule exception", exception.ErrorMessage, ct);
    }
}