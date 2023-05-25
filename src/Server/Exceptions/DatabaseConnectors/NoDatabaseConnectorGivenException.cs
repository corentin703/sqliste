namespace Sqliste.Server.Exceptions.DatabaseConnectors;

public class NoDatabaseConnectorGivenException : InvalidOperationException
{
    public NoDatabaseConnectorGivenException() 
        : base("You must provide the database's connector you want to use in config file")
    {
        //
    }
}