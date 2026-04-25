using System.Net;

namespace eShop.Shared.Exceptions;

[Serializable]
public class AppException : Exception
{
    public string Title { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }

    public AppException(string title, string message, HttpStatusCode httpStatusCode) : base(message)
    {
        Title = title;
        HttpStatusCode = httpStatusCode;
    }
}
