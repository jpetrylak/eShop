using System.Net;
using eShop.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace eShop.Shared.WebApi.ErrorHandling;

public class EntityNotFoundExceptionHandler : ExceptionHandlerBase<EntityNotFoundException>, IExceptionHandler<EntityNotFoundException>
{
    private readonly ILogger<EntityNotFoundExceptionHandler> _logger;

    public EntityNotFoundExceptionHandler(ILogger<EntityNotFoundExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async Task HandleAsync(HttpContext httpContext, EntityNotFoundException exception, CancellationToken ct)
    {
        _logger.LogError($"Entity not found. EntityId: {exception.EntityId}, EntityType: {exception.EntityType.FullName}");

        var detail = $"Entity of type {exception.EntityType.Name} with id {exception.EntityId} not found";
        
        await HandleAsync(httpContext, HttpStatusCode.NotFound, "Entity not found", detail, ct);
    }
}