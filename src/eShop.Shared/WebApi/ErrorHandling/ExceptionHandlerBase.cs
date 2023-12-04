using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Shared.WebApi.ErrorHandling;

public abstract class ExceptionHandlerBase<T>
{
    protected async Task HandleAsync(HttpContext httpContext, HttpStatusCode statusCode, string title, string detail, CancellationToken ct)
    {
        httpContext.Response.StatusCode = (int)statusCode; 
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = (int)statusCode,
            Type = typeof(T).Name,
            Title = title,
            Detail = detail,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
        }, ct);
    }
}