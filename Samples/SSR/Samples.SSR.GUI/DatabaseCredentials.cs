namespace Samples.SSR.GUI;

public class DatabaseCredentials
{
    public DatabaseCredentials(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public string ConnectionString { get; }
}
