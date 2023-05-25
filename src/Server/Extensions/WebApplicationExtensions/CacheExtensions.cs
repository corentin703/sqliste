namespace Sqliste.Server.Extensions.WebApplicationExtensions;

public static class CacheExtensions
{
    public static WebApplicationBuilder AddSqlisteCache(this WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.Services.AddDistributedMemoryCache();

        return builder;
    }
}