using Orm.Core.Attributes;

namespace Orm.Console.Models;

[Table]
public class Patient
{
    [PrimaryKey(AutoIncrement = true)]
    public int Id { get; set; }
    [Unique]
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Column(Name = "gender")]
    [Default("male")]
    public string Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
}
