using System;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Requests;
using Sqliste.Core.Models.Response;

namespace Sqliste.Server.Middlewares;

public class DatabaseMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

    public DatabaseMiddleware(RequestDelegate next, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
    {
        _next = next;
        _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value;
        var routes = _actionDescriptorCollectionProvider
            .ActionDescriptors
            .Items
            .Select(actionDescriptor => $"/{actionDescriptor.AttributeRouteInfo?.Template}");

        if (routes.Any(route => path?.Equals(route, StringComparison.InvariantCultureIgnoreCase) ?? false)) {
            await _next(context);
            return;
        }

        IRequestHandlerService requestHandlerService = context.RequestServices.GetRequiredService<IRequestHandlerService>();
        HttpRequestModel request = new HttpRequestModel()
        {
            Path = context.Request.Path,
            Method = GetHttpMethodFromString(context.Request.Method),
        };

        HttpResponseModel response = await requestHandlerService.HandleRequestAsync(request);

        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)response.Status;
        string result = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(result);

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
}