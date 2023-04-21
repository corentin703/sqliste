namespace Sqliste.Core.Exceptions.Url;

public class MissingParameterException : InvalidOperationException
{
    public MissingParameterException() 
        : base($"Some parameters are missing")
    {
        //
    }

    public MissingParameterException(string paramName) 
        : base($"{paramName} is missing")
    {
        //
    }
}