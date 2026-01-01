using Orm.Console.Models;
using Orm.Core;
using Spectre.Console;

namespace Orm.Console.Demo;

public static class CrudDemo
{
    public static void Run(OrmContext orm)
    {
        AnsiConsole.MarkupLine("[yellow]Running CRUD demo...[/]");

        var patient = new Patient
        {
            FirstName = "Miki",
            LastName = "Mikić",
            OIB = "12345678901",
            DateOfBirth = new DateTime(2004, 9, 22),
            Gender = "M",
            ResidenceAddress = "Zagreb",
            PermanentAddress = "Zagreb"
        };

        orm.Insert(patient);

        var loaded = orm.GetAll<Patient>().First();
        loaded.LastName = "Perić";

        orm.SaveChanges();

        var updated = orm.GetById<Patient>(loaded.Id);
        AnsiConsole.MarkupLine($"Updated patient: {updated!.FirstName} {updated.LastName}");

        orm.DeleteById<Patient>(updated.Id);

        AnsiConsole.MarkupLine("[green]CRUD demo completed[/]");
    }
}
