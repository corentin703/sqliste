﻿using DapperCodeFirstMappings.Attributes;
using Sqliste.Core.Contracts;
using System.Net.Mime;

namespace Sqliste.Core.Models.Sql;

[DapperEntity]
public class ProcedureModel
{
    [DapperColumn("name")]
    public string Name { get; set; } = string.Empty;

    [DapperColumn("content")]
    public string Content { get; set; } = string.Empty;

    [DapperColumn("schema")]
    public string Schema { get; set; } = string.Empty;

    public string Route { get; set; } = string.Empty;
    public string RoutePattern { get; set; } = string.Empty;

    public List<ProcedureArgumentModel> Arguments { get; set; } = new();

    public List<ISqlAnnotation> Annotations { get; set; } = new();
    public List<string> RouteParamNames { get; set; } = new();

    #region FromAnnotations
    public HttpMethod[] HttpMethods { get; set; } = new [] { HttpMethod.Post, };

    public string ContentType { get; set; } = MediaTypeNames.Text.Plain;
    #endregion
}