namespace Sqliste.Core.Models.Http;

public class HttpCookieModel
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}