namespace Sqliste.Core.Exceptions.Procedures;

public class RouteDeductionException : InvalidOperationException
{
    public RouteDeductionException(string? procedureName) 
        : base($"{procedureName}'s name doesn't match the fallback format")
    {
        //
    }
}