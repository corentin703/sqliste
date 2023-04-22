using Microsoft.AspNetCore.Mvc.Infrastructure;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace Sqliste.Server.Middlewares;

public class DatabaseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
    private readonly ILogger<DatabaseMiddleware> _logger;

    public DatabaseMiddleware(RequestDelegate next, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider, ILogger<DatabaseMiddleware> logger)
    {
        _next = next;
        _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? path = context.Request.Path.Value;
        IEnumerable<string> routes = _actionDescriptorCollectionProvider
            .ActionDescriptors
            .Items
            .Select(actionDescriptor => $"/{actionDescriptor.AttributeRouteInfo?.Template}");

        if (routes.Any(route => path?.Equals(route, StringComparison.InvariantCultureIgnoreCase) ?? false)) {
            await _next(context);
            return;
        }

        IRequestHandlerService requestHandlerService = context.RequestServices.GetRequiredService<IRequestHandlerService>();

        string? bodyContent = await ParseRequestBodyAsync(context);

        HttpRequestModel request = new HttpRequestModel()
        {
            Path = context.Request.Path,
            Method = GetHttpMethodFromString(context.Request.Method),
            Body = bodyContent,
            Cookies = context.Request.Cookies.ToDictionary(cookie => cookie.Key, cookie => cookie.Value),
            Headers = context.Request.Headers.ToDictionary(header => header.Key, header => header.Value.ToString()),
        };

        HttpResponseModel response = await requestHandlerService.HandleRequestAsync(request);

        context.Response.StatusCode = (int)response.Status;
        if (response.Data != null)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            string result = JsonSerializer.Serialize(response.Data);
            await context.Response.WriteAsync(result);
        }

        //await _next(context);
    }

    private HttpMethod GetHttpMethodFromString(string method)
    {
        return method switch
        {
            "GET" => HttpMethod.Get,
            "POST" => HttpMethod.Post,
            "PATCH" => HttpMethod.Patch,
            "PUT" => HttpMethod.Put,
            "DELETE" => HttpMethod.Delete,
            _ => HttpMethod.Get
        };
    }

    private async Task<string?> ParseRequestBodyAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        HttpRequest request = context.Request;

        if (request.Method == HttpMethods.Get || request.Method == HttpMethods.Delete)
            return null;

        if (request.ContentLength == 0)
            return null;

        if (request.ContentType != MediaTypeNames.Application.Json)
            return null;

        try
        {
            byte[] buffer = new byte[Convert.ToInt32(request.ContentLength)];
            int readResult = await request.Body.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

            return Encoding.UTF8.GetString(buffer);
        }
        catch(Exception exception) 
        {
            _logger.LogError(exception.ToString());
            return null;
        }
    }
}