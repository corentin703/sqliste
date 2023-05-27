using System.CommandLine;
using Sqliste.Server.Cli.Handlers;

namespace Sqliste.Server.Cli;

public class CliApplication
{
    private readonly SwaggerCommandHandler _swaggerCommandHandler;

    public CliApplication(SwaggerCommandHandler swaggerCommandHandler)
    {
        _swaggerCommandHandler = swaggerCommandHandler;
    }

    public async Task RunAsync(string[] args)
    {
        RootCommand rootCommand = new("Run SQListe server")
        {
            Name = "SQListe",
        };

        AddSwaggerCommand(rootCommand);
        
        // Command sub1aCommand = new("sub1a", "Second level subcommand");
        // swaggerCommand.Add(sub1aCommand);

        await rootCommand.InvokeAsync(args);
    }

    public void AddSwaggerCommand(RootCommand rootCommand)
    {
        Command swaggerCommand = new("swagger", "Generate swagger json");

        #region OutputOption

        Option<string> outputOption = new(
            name: "--output",
            description: "Output path"
        )
        {
            IsRequired = true,
        };
        
        outputOption.AddAlias("-o");
        swaggerCommand.AddOption(outputOption);

        #endregion
        
        swaggerCommand.SetHandler(async (outputArgumentValue) =>
        {
            await _swaggerCommandHandler.HandleAsync(outputArgumentValue);
        }, outputOption);
        
        rootCommand.Add(swaggerCommand);
    }
}