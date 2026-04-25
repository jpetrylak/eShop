using System.Net;

namespace eShop.Shared.Exceptions;

[Serializable]
public class NotRecognizedFieldException : AppException
{
    public NotRecognizedFieldException(string fieldName)
        : base("Not recognized field", $"Field {fieldName} could not be not be recognized", HttpStatusCode.BadRequest)
    {
    }
}
