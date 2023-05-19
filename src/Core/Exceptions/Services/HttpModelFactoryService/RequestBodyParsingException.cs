using System.Net;

namespace Sqliste.Core.Exceptions.Services.HttpModelFactoryService;

public class RequestBodyParsingException : WebException
{
    public RequestBodyParsingException() : base(message: "Unable to parse request body")
    {
        //
    }
}