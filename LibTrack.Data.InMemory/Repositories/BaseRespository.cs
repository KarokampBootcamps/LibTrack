using LibTrack.Core.Common;

namespace LibTrack.Data.InMemory.Repositories;

public abstract class BaseRepository<TEntity>
    : IRepository<TEntity> where TEntity : Entity
{
    public TEntity? Get(int id)
    {
        throw new NotImplementedException();
    }

    public List<TEntity> GetAll()
    {
        throw new NotImplementedException();
    }

    public void Add(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void Update(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public void Remove(TEntity entity)
    {
        throw new NotImplementedException();
    }
}