using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Sqliste.Core.Models.Http;

public class HttpCookieModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;

    [JsonPropertyName("expires")]
    public DateTimeOffset? Expires { get; set; }
    
    [JsonPropertyName("domain")]
    public string? Domain { get; set; }
    
    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("secure")]
    public bool? Secure { get; set; }

    [JsonPropertyName("sameSite")]
    public SameSiteMode? SameSite { get; set; }

    [JsonPropertyName("httpOnly")]
    public bool? HttpOnly { get; set; }

    [JsonPropertyName("maxAge")]
    public int? MaxAge { get; set; }
}