namespace Orm.Core;

public interface IOrmClient
{
    public void CreateTable<T>();
    public void Insert<T>(T entity);
    public void DeleteById<T>(object id);
    public void DeleteWhere<T>(string columnName, object value);
    public IEnumerable<T> GetAll<T>() where T : new();
    public T? GetById<T>(object id) where T : new();
    void Update<T>(T entity);
}