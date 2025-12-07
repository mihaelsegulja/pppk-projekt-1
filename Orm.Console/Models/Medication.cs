using Orm.Core.Attributes;

namespace Orm.Console.Models;

[Table]
public class Medication
{
    [PrimaryKey(AutoIncrement = true)]
    public int Id { get; set; }

    [NotNull]
    public string Name { get; set; }
}
