using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Net.Http.Headers;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Http;
using System.Net;
using System.Net.Mime;

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
        HttpResponseModel response = await requestHandlerService.HandleRequestAsync(request, context.RequestAborted);
        await ApplyResponseAsync(context, response, context.RequestAborted);
    }

    private async Task ApplyResponseAsync(
        HttpContext context, 
        HttpResponseModel response,
        CancellationToken cancellationToken = default
    )
    {
        foreach (KeyValuePair<string, string> header in response.Headers)
        {
            context.Response.Headers[header.Key] = header.Value;
        }

        if (response.Body != null)
        {
            response.Headers.TryAdd(HeaderNames.ContentType, MediaTypeNames.Text.Plain);
            response.Status ??= HttpStatusCode.OK;
        }
        else 
            response.Status ??= HttpStatusCode.NoContent;

        context.Response.StatusCode = (int)response.Status;

        if (response.Body != null)
            await context.Response.WriteAsync(response.Body, cancellationToken: cancellationToken);
    }
}