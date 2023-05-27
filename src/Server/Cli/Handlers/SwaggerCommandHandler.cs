using Sqliste.Core.Contracts.Services;

namespace Sqliste.Server.Cli.Handlers;

public class SwaggerCommandHandler
{
    private readonly ISqlisteOpenApiGenerator _openApiGenerator;
    private readonly ILogger<SwaggerCommandHandler> _logger;

    public SwaggerCommandHandler(ISqlisteOpenApiGenerator openApiGenerator, ILogger<SwaggerCommandHandler> logger)
    {
        _openApiGenerator = openApiGenerator;
        _logger = logger;
    }

    public async Task HandleAsync(string path)
    {
        try
        {
            string openApiJson = await _openApiGenerator.GenerateOpenApiJsonAsync();
            await File.WriteAllTextAsync(path, openApiJson);
            _logger.LogInformation("Output generated at {Path}", path);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception: exception, "An error occured during the command execution");
            throw;
        }
    }
}