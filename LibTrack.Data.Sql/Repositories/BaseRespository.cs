using Dapper;
using LibTrack.Core.Common;
using LibTrack.Core.Entities;

namespace LibTrack.Data.Sql.Repositories;

public abstract class BaseRepository<TEntity> : IRepository<TEntity>
    where TEntity : Entity
{
    protected readonly SqliteConnectionFactory Factory;

    protected BaseRepository(SqliteConnectionFactory factory) => Factory = factory;

    // Constants supplied by subclasses, never user input, so interpolating them into SQL is safe
    protected abstract string TableName { get; }

    /// <summary>INSERT statement ending with RETURNING Id.</summary>
    protected abstract string InsertSql { get; }

    /// <summary>UPDATE statement with WHERE Id = @Id.</summary>
    protected abstract string UpdateSql { get; }

    public TEntity? Get(int id)
    {
        using var connection = Factory.Create();
        return connection.QuerySingleOrDefault<TEntity>(
            $"SELECT * FROM {TableName} WHERE Id = @id", new { id });
    }

    public List<TEntity> GetAll()
    {
        using var connection = Factory.Create();
        return connection.Query<TEntity>($"SELECT * FROM {TableName}").ToList();
    }

    public void Add(TEntity entity)
    {
        using var connection = Factory.Create();
        entity.Id = connection.QuerySingle<int>(InsertSql, entity);
    }

    public void Update(TEntity entity)
    {
        using var connection = Factory.Create();
        var rows = connection.Execute(UpdateSql, entity);

        if (rows == 0)
            throw new InvalidOperationException(
                $"{typeof(TEntity).Name} with Id {entity.Id} was not found.");
    }

    public void Remove(TEntity entity)
    {
        using var connection = Factory.Create();
        connection.Execute($"DELETE FROM {TableName} WHERE Id = @Id", entity);
    }
}