using System.CommandLine;
using Sqliste.Server.Cli.Handlers;

namespace Sqliste.Server.Cli;

public class CliApplication
{
    private readonly SwaggerCommandHandler _swaggerCommandHandler;
    private readonly DatabaseMigrationCommandHandler _databaseMigrationCommandHandler;

    public CliApplication(SwaggerCommandHandler swaggerCommandHandler, DatabaseMigrationCommandHandler databaseMigrationCommandHandler)
    {
        _swaggerCommandHandler = swaggerCommandHandler;
        _databaseMigrationCommandHandler = databaseMigrationCommandHandler;
    }

    public async Task RunAsync(string[] args)
    {
        RootCommand rootCommand = new("Run SQListe server")
        {
            Name = "SQListe",
        };

        AddSwaggerCommand(rootCommand);
        AddMigrationCommand(rootCommand);
        
        await rootCommand.InvokeAsync(args);
    }

    private void AddSwaggerCommand(RootCommand rootCommand)
    {
        Command swaggerCommand = new("swagger", "Generate swagger json");
        swaggerCommand.AddAlias("s");

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

    private void AddMigrationCommand(RootCommand rootCommand)
    {
        Command migrationCommand = new("migrate", "Migrate the database");
        migrationCommand.AddAlias("m");
        
        migrationCommand.SetHandler(async () =>
        {
            await _databaseMigrationCommandHandler.HandleAsync();
        });
        
        rootCommand.Add(migrationCommand);
    }
}