namespace Sqliste.Core.Configuration;

public class SessionConfiguration
{
    public int IdleTimeout { get; set; } = 20; // Timeout for session (in minutes)
}