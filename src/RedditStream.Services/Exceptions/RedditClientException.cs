using System.Net;

namespace RedditStream.Services.Exceptions;

internal class RedditClientException : ClientException
{
    public RedditClientException(HttpStatusCode httpStatusCode, string errorMessage) 
        : base(httpStatusCode, errorMessage)
    {
    }
}
