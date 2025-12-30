using Orm.Core.Attributes;

namespace Orm.Console.Models;

[Table(Name = "patient")]
public class Patient
{
    [PrimaryKey(AutoIncrement = true)]
    public int Id { get; set; }

    [NotNull]
    public string FirstName { get; set; }

    [NotNull]
    public string LastName { get; set; }

    [Unique]
    [NotNull]
    public string OIB { get; set; }

    [NotNull]
    public DateTime DateOfBirth { get; set; }

    [NotNull]
    public string Gender { get; set; }

    public string ResidenceAddress { get; set; }
    public string PermanentAddress { get; set; }
}
