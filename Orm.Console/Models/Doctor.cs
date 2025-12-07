using Orm.Core.Attributes;

namespace Orm.Console.Models;

[Table(Name = "doctor")]
public class Doctor
{
    [PrimaryKey(AutoIncrement = true)]
    public int Id { get; set; }

    [NotNull]
    public string FirstName { get; set; }

    [NotNull]
    public string LastName { get; set; }

    [NotNull]
    public string Specialization { get; set; }
}
