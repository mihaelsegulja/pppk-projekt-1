using Orm.Core.Attributes;

namespace Orm.Console;

[Table]
public class DemoEntity
{
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }
    [Unique]
    public string Name { get; set; }
    [Default(18)]
    public int Age { get; set; }
}
