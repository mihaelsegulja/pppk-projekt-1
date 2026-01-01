using Orm.Console.Models;
using Orm.Core;
using Spectre.Console;

namespace Orm.Console.Demo;

public static class SchemaDemo
{
    public static void Run(OrmContext orm)
    {
        AnsiConsole.MarkupLine("[yellow]Creating tables...[/]");

        orm.CreateTable<Doctor>();
        orm.CreateTable<Patient>();
        orm.CreateTable<Medication>();
        orm.CreateTable<DiseaseHistory>();
        orm.CreateTable<Appointment>();
        orm.CreateTable<MedicationPrescription>();

        AnsiConsole.MarkupLine("[green]Schema created[/]");
    }
}