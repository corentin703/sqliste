using System.Text.Json;
using Microsoft.Extensions.Logging;
using Sqliste.Core.Constants;
using Sqliste.Core.Contracts.Services;
using Sqliste.Core.Models.Http.FormData;
using Sqliste.Core.Models.Pipeline;
using Sqliste.Core.Models.Sql;
using Sqliste.Database.Common.Contracts.Services;

namespace Sqliste.Infrastructure.Services;

public class ParametersResolver : IParametersResolver
{
    private readonly ILogger<ParametersResolver> _logger;
    private readonly IDatabaseSessionAccessor _sessionAccessor;

    private Dictionary<string, object?>? _requestParams;

    public ParametersResolver(ILogger<ParametersResolver> logger, IDatabaseSessionAccessor sessionAccessor)
    {
        _logger = logger;
        _sessionAccessor = sessionAccessor;
    }

    public async Task<Dictionary<string, object?>> GetParamsAsync(PipelineBag pipeline, ProcedureModel procedure, CancellationToken cancellationToken)
    {
        Dictionary<string, object?> sqlParams = new(GetRequestParams(pipeline));
        AddResponseParams(sqlParams, pipeline, procedure);
        
        if (procedure.Arguments.Any(arg => arg.Name == SystemQueryParametersConstants.Session))
        {
            string? session = await _sessionAccessor.GetSessionAsync(cancellationToken);
            sqlParams.TryAdd(SystemQueryParametersConstants.Session, session);
        }

        if (pipeline.Response.Error != null)
            sqlParams.TryAdd(SystemQueryParametersConstants.Error, JsonSerializer.Serialize(pipeline.Response.Error));

        _logger.LogDebug("Added {ParamCount} for {ProcedureName}", sqlParams.Count, procedure.Name);

        return sqlParams;
    }
    
    private Dictionary<string, object?> GetRequestParams(PipelineBag pipeline)
    {
        if (_requestParams != null)
            return _requestParams;
        
        Dictionary<string, object?> requestParams = new();
        
        foreach (KeyValuePair<string, string> uriParam in pipeline.Request.PathParams)
        {
            requestParams.TryAdd(uriParam.Key, uriParam.Value);
        }

        foreach (KeyValuePair<string, string> queryParam in pipeline.Request.QueryParams)
        {
            requestParams.TryAdd(queryParam.Key, queryParam.Value);
        }

        requestParams.TryAdd(SystemQueryParametersConstants.RequestBody, pipeline.Request.Body);
        requestParams.TryAdd(SystemQueryParametersConstants.RequestContentType, pipeline.Request.ContentType);
        requestParams.TryAdd(SystemQueryParametersConstants.RequestCookies, pipeline.Request.Cookies ?? "{}");
        requestParams.TryAdd(SystemQueryParametersConstants.RequestHeaders, pipeline.Request.Headers ?? "{}");
        requestParams.TryAdd(SystemQueryParametersConstants.RequestPath, pipeline.Request.Path);
        requestParams.TryAdd(SystemQueryParametersConstants.RequestMethod, pipeline.Request.Method.ToString());
        requestParams.TryAdd(SystemQueryParametersConstants.PathParams, JsonSerializer.Serialize(pipeline.Request.PathParams));
        requestParams.TryAdd(SystemQueryParametersConstants.QueryParams, JsonSerializer.Serialize(pipeline.Request.QueryParams));
        requestParams.TryAdd(SystemQueryParametersConstants.RequestModel, JsonSerializer.Serialize(pipeline.Request));
        AddFormDataParameters(requestParams, pipeline);
        
        _requestParams = requestParams;
        return _requestParams;
    }
    
    private void AddResponseParams(
        Dictionary<string, object?> sqlParams,
        PipelineBag pipeline, 
        ProcedureModel procedure
    )
    {
        sqlParams.TryAdd(SystemQueryParametersConstants.ResponseBody, pipeline.Response.Body);
        sqlParams.TryAdd(SystemQueryParametersConstants.ResponseCookies, pipeline.Response.Cookies ?? "{}");
        sqlParams.TryAdd(SystemQueryParametersConstants.PipelineStorage, pipeline.Response.PipelineStorage);
        sqlParams.TryAdd(SystemQueryParametersConstants.ResponseHeaders, pipeline.Response.Headers ?? "{}");
        sqlParams.TryAdd(SystemQueryParametersConstants.ResponseStatus, pipeline.Response.Status);
        sqlParams.TryAdd(SystemQueryParametersConstants.ResponseContentType, pipeline.Response.ContentType ?? procedure.ContentType);
        sqlParams.TryAdd(SystemQueryParametersConstants.ResponseModel, JsonSerializer.Serialize(pipeline.Response));
    }

    private void AddFormDataParameters(Dictionary<string, object?> sqlParams, PipelineBag pipeline)
    {
        if (pipeline.Request.FormData == null) 
            return;
        
        sqlParams.TryAdd(SystemQueryParametersConstants.RequestFormData, JsonSerializer.Serialize(pipeline.Request.FormData));

        List<FormDataFile> files = pipeline.Request.FormData
            .Select(item => item.Value)
            .Where(value => value is FormDataFile).Cast<FormDataFile>().ToList();
            
        foreach (FormDataFile file in files)
        {
            sqlParams.TryAdd($"{SystemQueryParametersConstants.RequestFormFilePrefix}{file.Name}", file.Content);
        }
    }
}