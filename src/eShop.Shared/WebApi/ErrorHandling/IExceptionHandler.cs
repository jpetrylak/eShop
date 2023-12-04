using Microsoft.AspNetCore.Http;

namespace eShop.Shared.WebApi.ErrorHandling;

public interface IExceptionHandler<T>
{
    Task HandleAsync(HttpContext httpContext, T exception, CancellationToken ct);
}