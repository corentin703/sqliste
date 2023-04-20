namespace Sqliste.Core.Models;

public class ArgumentModel
{
    public string Name { get; set; } = string.Empty;
    public string SqlDataType { get; set; } = string.Empty;
    public int Order { get; set; }
}