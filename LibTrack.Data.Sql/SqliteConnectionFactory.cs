namespace LibTrack.Data.Sql;

using Microsoft.Data.Sqlite;

public class SqliteConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory(string dbPath)
    {
        // "Foreign Keys=True" makes SQLite enforce FKs on every connection
        // (SQLite doesn't enforce them unless asked)
        _connectionString = $"Data Source={dbPath};Foreign Keys=True";
    }

    public SqliteConnection Create()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }
}