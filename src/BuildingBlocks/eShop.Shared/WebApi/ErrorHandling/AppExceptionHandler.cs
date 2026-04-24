using System.Diagnostics;
using System.Text.Json;
using eShop.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace eShop.Shared.WebApi.ErrorHandling;

public class AppExceptionHandler : IExceptionHandler
{
    private readonly ILogger<AppExceptionHandler> _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public AppExceptionHandler(
        ILogger<AppExceptionHandler> logger,
        IWebHostEnvironment environment,
        ProblemDetailsFactory problemDetailsFactory)
    {
        _environment = environment;
        _problemDetailsFactory = problemDetailsFactory;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not AppException appException)
        {
            return false;
        }

        _logger.LogError(appException, appException.Message);
        await HandleAppExceptionAsync(httpContext, appException);

        return true;
    }

    private async Task HandleAppExceptionAsync(HttpContext context, AppException ex)
    {
        ProblemDetails problem = _problemDetailsFactory.CreateProblemDetails(
            context,
            title: ex.Title,
            detail: ex.Message,
            instance: $"{context.Request.Method} {context.Request.Path}",
            type: ex.GetType().Name,
            statusCode: (int)ex.HttpStatusCode);

        problem.Extensions["traceId"] = GetTraceId(context);

        if (_environment.IsDevelopment())
            problem.Extensions["stackTrace"] = ex.StackTrace;

        context.Response.StatusCode = (int)ex.HttpStatusCode;
        await context.Response.WriteAsJsonAsync(problem, (JsonSerializerOptions)null, ContentType.ProblemJson);
    }

    private static string GetTraceId(HttpContext context)
    {
        return Activity.Current?.Id ?? context.TraceIdentifier;
    }
}
