namespace Orm.Core.Migration;

public interface IMigration
{
    string Id { get; }
    void Up(OrmContext db);
}