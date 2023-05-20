using System.Net;

namespace Sqliste.Core.Exceptions.Services.HttpModelFactoryService;

public class RequestBodyReadingException : WebException
{
    public RequestBodyReadingException() : base(message: "Unable to read request body")
    {
        //
    }
}