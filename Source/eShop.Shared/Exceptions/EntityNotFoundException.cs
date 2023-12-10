using System.Net;

namespace eShop.Shared.Exceptions;

[Serializable]
public class EntityNotFoundException : AppException
{
    public EntityNotFoundException(object entityId, Type entityType, HttpStatusCode httpStatusCode = HttpStatusCode.Conflict)
        : base(
            "Entity not found",
            $"Entity {entityType.FullName} with identifier: {entityId} does not exist",
            HttpStatusCode.NotFound)
    {
    }
}
