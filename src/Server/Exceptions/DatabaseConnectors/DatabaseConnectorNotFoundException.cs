namespace Sqliste.Server.Exceptions.DatabaseConnectors;

public class DatabaseConnectorNotFoundException : InvalidOperationException
{
    public DatabaseConnectorNotFoundException() : base("The provided database's connector hasn't been found")
    {
        //
    }
}