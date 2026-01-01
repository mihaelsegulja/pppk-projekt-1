using Orm.Console.Models;
using Orm.Core;
using Spectre.Console;

namespace Orm.Console.Demo;

public static class SeedDemo
{
    public static void Run(OrmContext orm)
    {
        AnsiConsole.MarkupLine("[yellow]Seeding doctors...[/]");

        var doctors = new[]
        {
            new Doctor { FirstName = "Ivan", LastName = "Horvat", Specialization = "Radiology" },
            new Doctor { FirstName = "Ana", LastName = "Kovač", Specialization = "Cardiology" }
        };

        foreach (var d in doctors)
            orm.Insert(d);

        AnsiConsole.MarkupLine("[green]Doctors seeded[/]");
    }
}
