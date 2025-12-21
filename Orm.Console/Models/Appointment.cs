using Orm.Core.Attributes;

namespace Orm.Console.Models;

public enum AppointmentType
{
    CT, MR, ULTRA, EKG, ECHO, OKO, DERM, DENTA, MAMMO, EEG
}

[Table(Name = "appointment")]
public class Appointment
{
    [PrimaryKey(AutoIncrement = true)]
    public int Id { get; set; }

    [ForeignKey(typeof(Patient))]
    public int PatientId { get; set; }

    [ForeignKey(typeof(Doctor))]
    public int DoctorId { get; set; }

    [NotNull]
    public AppointmentType Type { get; set; }

    [NotNull]
    public DateTime ScheduledAt { get; set; }
    
    // navigation props
    public Patient? Patient { get; set; }
    public Doctor? Doctor { get; set; }
}
