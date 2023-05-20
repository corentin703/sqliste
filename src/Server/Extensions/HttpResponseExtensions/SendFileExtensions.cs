using Microsoft.Net.Http.Headers;

namespace Sqliste.Server.Extensions.HttpResponseExtensions;

public static class SendFileExtensions
{
    public static async Task SendFileAsync(this HttpResponse response, byte[] fileContent, string fileName, bool inline = false, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(response.Headers[HeaderNames.ContentDisposition]))
        {
            string disposition = inline ? "inline" : "attachment";
            response.Headers[HeaderNames.ContentDisposition] = $"{disposition}; filename=\"{fileName}\";";
        }

        await response.Body.WriteAsync(fileContent, cancellationToken);
    }
}