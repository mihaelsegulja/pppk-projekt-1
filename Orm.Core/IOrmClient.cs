namespace Orm.Core;

public interface IOrmClient
{
    public void CreateTable<T>();
    public void Insert<T>(T entity);
    public void DeleteById<T>(object id);
    public void DeleteWhere<T>(string columnName, object value);
}