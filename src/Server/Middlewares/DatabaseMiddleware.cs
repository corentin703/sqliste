using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Net.Http.Headers;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Http;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Sqliste.Core.Models.Pipeline;
using SameSiteMode = Microsoft.AspNetCore.Http.SameSiteMode;

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

        _logger.LogDebug("Handling request for path {Path}", context.Request.Path);
        IRequestHandlerService requestHandlerService = context.RequestServices.GetRequiredService<IRequestHandlerService>();
        IHttpModelsFactory httpModelsFactory = context.RequestServices.GetRequiredService<IHttpModelsFactory>();

        PipelineBag request = await httpModelsFactory.BuildRequestModelAsync(context.RequestAborted);
        PipelineBag response = await requestHandlerService.HandleRequestAsync(request, context.RequestAborted);
        await ApplyResponseAsync(context, response, context.RequestAborted);
    }

    private async Task ApplyResponseAsync(
        HttpContext context, 
        PipelineBag pipelineBag,
        CancellationToken cancellationToken
    )
    {
        ApplyResponseCookies(context, pipelineBag);
        ApplyResponseHeaders(context, pipelineBag);

        // Set status and Content-Type
        if (pipelineBag.Response.Body != null)
        {
            if (context.Response.Headers[HeaderNames.ContentType].IsNullOrEmpty())
                context.Response.Headers[HeaderNames.ContentType] = pipelineBag.Response.ContentType ?? MediaTypeNames.Text.Plain;

            pipelineBag.Response.Status ??= HttpStatusCode.OK;
        }
        else 
            pipelineBag.Response.Status ??= HttpStatusCode.NoContent;

        context.Response.StatusCode = (int)pipelineBag.Response.Status;

        // Write body if not null
        if (pipelineBag.Response.Body != null)
            await context.Response.WriteAsync(pipelineBag.Response.Body, cancellationToken: cancellationToken);
    }

    private void ApplyResponseHeaders(HttpContext context, PipelineBag response)
    {
        if (response.Response.Headers == null)
            return;

        try
        {
            Dictionary<string, string>? headers =
                JsonSerializer.Deserialize<Dictionary<string, string>>(response.Response.Headers);

            if (headers == null)
                return;

            foreach (KeyValuePair<string, string> header in headers)
            {
                context.Response.Headers[header.Key] = header.Value;
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception: exception, "Error during headers parsing");
        }
    }

    private void ApplyResponseCookies(HttpContext context, PipelineBag response)
    {
        if (string.IsNullOrEmpty(response.Response.Cookies))
            return;

        List<HttpCookieModel>? cookieModels;

        try
        {
            cookieModels = JsonSerializer.Deserialize<List<HttpCookieModel>>(response.Response.Cookies);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception: exception, "Failed to parse response cookies");
            cookieModels = null;
        }
        
        if (cookieModels == null)
            return;

        foreach (HttpCookieModel cookieModel in cookieModels)
        {
            context.Response.Cookies.Append(cookieModel.Name, cookieModel.Value, new CookieOptions()
            {
                Domain = cookieModel.Domain,
                Path = cookieModel.Path,
                Secure = cookieModel.Secure ?? false,
                SameSite = cookieModel.SameSite ?? SameSiteMode.Unspecified,
                HttpOnly = cookieModel.HttpOnly ?? false,
                IsEssential = cookieModel.IsEssential ?? false,
                Expires = cookieModel.Expires,
                MaxAge = cookieModel.MaxAge,
            });
        }
    }
}