using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Net.Http.Headers;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Http;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        _logger.LogDebug("Handling request for path {path}", context.Request.Path);
        IRequestHandlerService requestHandlerService = context.RequestServices.GetRequiredService<IRequestHandlerService>();
        IHttpModelsFactory httpModelsFactory = context.RequestServices.GetRequiredService<IHttpModelsFactory>();

        HttpRequestModel request = await httpModelsFactory.BuildRequestModelAsync(context.RequestAborted);
        HttpRequestModel response = await requestHandlerService.HandleRequestAsync(request, context.RequestAborted);
        await ApplyResponseAsync(context, response, context.RequestAborted);
    }

    private async Task ApplyResponseAsync(
        HttpContext context, 
        HttpRequestModel response,
        CancellationToken cancellationToken = default
    )
    {
        Dictionary<string, string>? headers = ApplyHeaders(context, response);

        if (response.Body != null)
        {
            if (headers == null || !headers.ContainsKey(HeaderNames.ContentType))
                context.Response.Headers[HeaderNames.ContentType] = response.ContentType ?? MediaTypeNames.Text.Plain;

            response.Status ??= HttpStatusCode.OK;
        }
        else 
            response.Status ??= HttpStatusCode.NoContent;

        context.Response.StatusCode = (int)response.Status;

        if (response.Body != null)
            await context.Response.WriteAsync(response.Body, cancellationToken: cancellationToken);
    }

    private Dictionary<string, string>? ApplyHeaders(HttpContext context, HttpRequestModel response)
    {
        if (response.Headers == null)
            return null;

        try
        {
            Dictionary<string, string>? headers =
                JsonSerializer.Deserialize<Dictionary<string, string>>(response.Headers);

            if (headers != null)
                return null;

            foreach (KeyValuePair<string, string> header in headers)
            {
                context.Response.Headers[header.Key] = header.Value;
            }

            return headers;
        }
        catch (Exception exception)
        {
            _logger.LogError("Error during headers parsing {exception}", exception);
            return null;
        }
    }
}