using System.Net;

namespace RedditStream.Services.Exceptions;

internal class ClientException : Exception
{
    public HttpStatusCode HttpStatusCode { get; }
    public string ErrorMessage { get; }

    public ClientException(HttpStatusCode httpStatusCode, string errorMessage)
    {
        HttpStatusCode = httpStatusCode;
        ErrorMessage = errorMessage;
    }
}
