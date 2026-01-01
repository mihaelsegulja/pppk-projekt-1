using Orm.Console.Models;
using Orm.Core;
using Spectre.Console;

namespace Orm.Console.Demo;

public static class IncludeDemo
{
    public static void Run(OrmContext orm)
    {
        AnsiConsole.MarkupLine("[yellow]Running Include demo...[/]");

        var patient = new Patient
        {
            FirstName = "Ivana",
            LastName = "Novak",
            OIB = "98765432109",
            DateOfBirth = new DateTime(1995, 3, 20),
            Gender = "F"
        };

        orm.Insert(patient);

        var doctor = orm.GetAll<Doctor>().First();

        var appointment = new Appointment
        {
            PatientId = patient.Id,
            DoctorId = doctor.Id,
            Type = AppointmentType.EKG,
            ScheduledAt = DateTime.Now.AddDays(2)
        };

        orm.Insert(appointment);

        var appointments = orm.GetAll<Appointment>(
            include: i =>
            {
                i.Include<Patient>();
                i.Include<Doctor>();
            }).ToList();

        foreach (var a in appointments)
        {
            AnsiConsole.MarkupLine(
                $"Appointment: {a.Type}, Patient: {a.Patient?.FirstName}, Doctor: {a.Doctor?.LastName}");
        }

        AnsiConsole.MarkupLine("[green]Include demo completed[/]");
    }
}
