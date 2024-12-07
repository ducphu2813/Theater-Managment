namespace Shared.Interfaces;

public interface IRepository <TEntity> : IDisposable where TEntity : class
{
    Task<Dictionary<string, object>> GetAll(int page, int limit);
    Task<TEntity> GetById(string id);
    Task<TEntity> Add(TEntity obj);
    Task<TEntity> Update(string id, TEntity obj);
    Task<bool> Remove(string id);
    Task<List<TEntity>> GetAllByIds(List<string> ids);
}