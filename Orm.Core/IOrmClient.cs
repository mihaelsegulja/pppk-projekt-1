using Orm.Core.Models;

namespace Orm.Core;

public interface IOrmClient
{
    public void CreateTable<T>();
    public void Insert<T>(T entity);
    public void DeleteById<T>(object id);
    public void Delete<T>(DeleteOptions? options);
    public T? GetById<T>(object id) where T : new();
    public IEnumerable<T> GetAll<T>(SelectOptions? options) where T : new();
    void Update<T>(T entity);
}