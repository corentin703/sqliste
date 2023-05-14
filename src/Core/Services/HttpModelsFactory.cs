﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Http;
using System.Text;
using System.Text.Json;
using Sqliste.Core.Utils.Uri;

namespace Sqliste.Core.Services;

public class HttpModelsFactory : IHttpModelsFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HttpModelsFactory> _logger;

    public HttpModelsFactory(IHttpContextAccessor httpContextAccessor, ILogger<HttpModelsFactory> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<HttpRequestModel> BuildRequestModelAsync(CancellationToken cancellationToken = default)
    {
        HttpRequest request = _httpContextAccessor.HttpContext.Request;

        HttpMethod httpMethod = GetHttpMethodFromString(request.Method);
        string? bodyContent = await ParseRequestBodyAsync(request, cancellationToken);

        Dictionary<string, string> cookies =
            request.Cookies.ToDictionary(cookie => cookie.Key, cookie => cookie.Value);

        Dictionary<string, string> headers =
            request.Headers.ToDictionary(header => header.Key, header => header.Value.ToString());
        
        string? queryString = request.QueryString.HasValue
            ? request.QueryString.Value
            : null
        ;

        HttpRequestModel requestModel = new HttpRequestModel()
        {
            Path = request.Path,
            QueryString = queryString,
            QueryParams = queryString != null ? UriQueryParamsParser.ParseQueryParams(queryString) : new(),
            Method = httpMethod,
            RequestBody = bodyContent,
            RequestCookies = JsonSerializer.Serialize(cookies),
            RequestHeaders = JsonSerializer.Serialize(headers),
        };

        return requestModel;
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

    private async Task<string?> ParseRequestBodyAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Method == HttpMethods.Get || request.Method == HttpMethods.Delete)
            return null;

        if (request.ContentLength == 0)
            return null;

        try
        {
            byte[] buffer = new byte[Convert.ToInt32(request.ContentLength)];
            int readResult = await request.Body.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

            return Encoding.UTF8.GetString(buffer);
        }
        catch(Exception exception) 
        {
            _logger.LogError(exception: exception, "Unable to parse body");
            return null;
        }
    }
}