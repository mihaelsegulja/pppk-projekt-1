using Orm.Core.Attributes;

namespace Orm.Console.Models;

[Table]
public class DiseaseHistory
{
    [PrimaryKey(AutoIncrement = true)]
    public int Id { get; set; }

    [ForeignKey(typeof(Patient))]
    public int PatientId { get; set; }
    
    [NotNull]
    public string DiseaseName { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    // navigation props
    public Patient? Patient { get; set; }
}
