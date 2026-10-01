namespace LibTrack.Core.Common;

public interface IRepository<TEntity> where TEntity : Entity
{
    TEntity? Get(int id);
    List<TEntity> GetAll();
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}