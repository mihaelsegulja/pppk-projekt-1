using Orm.Core.Attributes;

namespace Orm.Console.Models;

[Table]
public class MedicationPrescription
{
    [PrimaryKey(AutoIncrement = true)]
    public int Id { get; set; }

    [ForeignKey(typeof(Patient))]
    public int PatientId { get; set; }

    [ForeignKey(typeof(Medication))]
    public int MedicationId { get; set; }

    [NotNull]
    public string Dose { get; set; }

    [NotNull]
    [Default("once per day")]
    public string Frequency { get; set; }
}
