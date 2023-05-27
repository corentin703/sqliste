using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Sqliste.Core.Constants;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Exceptions.Services.HttpModelFactoryService;
using Sqliste.Core.Extensions;
using Sqliste.Core.Models.Http.FormData;
using Sqliste.Core.Models.Pipeline;
using Sqliste.Core.Models.Sql;
using Sqliste.Core.Utils.Uri;

namespace Sqliste.Infrastructure.Services;

internal class PipelineModelsFactory : IPipelineModelsFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<PipelineModelsFactory> _logger;
    private readonly IProcedureResolver _procedureResolver;
    
    public PipelineModelsFactory(IHttpContextAccessor httpContextAccessor, ILogger<PipelineModelsFactory> logger, IProcedureResolver procedureResolver)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _procedureResolver = procedureResolver;
    }

    public async Task<PipelineBag> BuildRequestModelAsync(CancellationToken cancellationToken = default)
    {
        HttpRequest request = _httpContextAccessor.HttpContext.Request;

        HttpMethod httpMethod = HttpMethodExtensions.Parse(request.Method);
        string? bodyContent = await ParseRequestBodyAsync(request, cancellationToken);
        Dictionary<string, FormDataItem>? formData = await ParseRequestFormDataAsync(request, cancellationToken);
        
        Dictionary<string, string> cookies =
            request.Cookies.ToDictionary(cookie => cookie.Key, cookie => cookie.Value);

        Dictionary<string, string> headers =
            request.Headers.ToDictionary(header => header.Key, header => header.Value.ToString());
        
        string? queryString = request.QueryString.HasValue
            ? request.QueryString.Value
            : null
        ;

        Dictionary<string, string> queryParams = queryString != null
            ? UriQueryParamsParser.ParseQueryParams(queryString)
            : new()
        ;

        ProcedureModel? procedure = await _procedureResolver.ResolveProcedureAsync(request.Path, httpMethod, cancellationToken);
        
        Dictionary<string, string> pathParams = procedure != null 
            ? ParseUriParams(request.Path, procedure)
            : new();
        
        PipelineRequestBag requestModel = new()
        {
            Path = request.Path,
            QueryString = queryString,
            QueryParams = queryParams,
            PathParams = pathParams,
            Method = httpMethod,
            Body = bodyContent,
            FormData = formData,
            Cookies = JsonSerializer.Serialize(cookies),
            Headers = JsonSerializer.Serialize(headers),
        };

        PipelineBag pipelineModel = new PipelineBag()
        {
            Request = requestModel,
            Procedure = procedure,
        };

        return pipelineModel;
    }

    private bool IsFormDataRequest(HttpRequest request)
    {
        return request.ContentType.StartsWith(MimeTypes.FormUrlEncoded) || request.ContentType.StartsWith(MimeTypes.FormData);
    }
    
    private async Task<string?> ParseRequestBodyAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Method == HttpMethods.Get || request.Method == HttpMethods.Delete)
            return null;
        
        if (IsFormDataRequest(request))
            return null;
        
        if (request.ContentLength == 0)
            return null;

        try
        {
            byte[] buffer = new byte[Convert.ToInt32(request.ContentLength)];
            int readResult = await request.Body.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

            if (readResult != request.ContentLength)
                throw new RequestBodyReadingException();
            
            return Encoding.UTF8.GetString(buffer);
        }
        catch(Exception exception) 
        {
            _logger.LogError(exception: exception, "Unable to parse body");
            throw new RequestBodyParsingException();
        }
    }
    
    private async Task<Dictionary<string, FormDataItem>?> ParseRequestFormDataAsync(HttpRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Method == HttpMethods.Get || request.Method == HttpMethods.Delete)
            return null;

        if (!IsFormDataRequest(request))
            return null;
        
        if (request.ContentLength == 0)
            return null;

        Dictionary<string, FormDataItem> formContent = new();

        foreach (IFormFile file in request.Form.Files)
        {
            await ProcessFileAsync(formContent, file, cancellationToken);
        }

        foreach (KeyValuePair<string, StringValues> formItem in request.Form)
        {
            formContent.TryAdd(formItem.Key, new FormDataString(formItem.Key, formItem.Value));
        }

        return formContent;
    }

    private async Task ProcessFileAsync(Dictionary<string, FormDataItem> formContent, IFormFile file, CancellationToken cancellationToken)
    {
        if (formContent.ContainsKey(file.Name))
            return;

        try
        {
            byte[] fileContent = new byte[file.Length];
            await using Stream fileStream = file.OpenReadStream();
            int readResult = await fileStream.ReadAsync(fileContent, cancellationToken);
        
            if (readResult != file.Length)
                throw new RequestBodyReadingException();
            
            formContent.Add(file.Name, new FormDataFile(file, fileContent));
        }
        catch(Exception exception) 
        {
            _logger.LogError(exception: exception, "Unable to parse body");
            throw new RequestBodyParsingException();
        }
    } 
    
    private Dictionary<string, string> ParseUriParams(string uri, ProcedureModel procedure)
    {
        Dictionary<string, string> pathParams = new();

        Match paramsMatch = Regex.Match(uri, procedure.RoutePattern);
        if (!paramsMatch.Success)
        {
            _logger.LogDebug("Path params's pattern didn't match for {ProcedureName} (path: {Path})", procedure.Name, uri);
            return pathParams;
        }

        procedure.UriParams.ForEach(routeParam =>
        {
            string paramValue = paramsMatch.Groups[routeParam.Name].Value;
            if (string.IsNullOrEmpty(paramValue))
                return;

            pathParams.Add(routeParam.Name, paramValue);
        });

        _logger.LogDebug("Found {ParamCount} path params for {ProcedureName} (path: {Path})", pathParams.Count, procedure.Name, uri);
        return pathParams;
    }
}