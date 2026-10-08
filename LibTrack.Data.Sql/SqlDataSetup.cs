using Dapper;

namespace LibTrack.Data.Sql;

public static class SqlDataSetup
{
    private static bool _initialized;

    /// <summary>Registers Dapper handlers and creates the schema. Safe to call more than once.</summary>
    public static SqliteConnectionFactory Initialize(string dbPath)
    {
        if (!_initialized)
        {
            SqlMapper.AddTypeHandler(new DateTimeHandler());
            _initialized = true;
        }

        var factory = new SqliteConnectionFactory(dbPath);
        new DatabaseInitializer(factory).Initialize();
        return factory;
    }
}