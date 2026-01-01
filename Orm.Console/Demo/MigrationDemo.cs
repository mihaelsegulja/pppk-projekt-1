using Orm.Console.Migrations;
using Orm.Core;
using Orm.Core.Migration;
using Spectre.Console;

namespace Orm.Console.Demo;

public static class MigrationDemo
{
    public static void Run(OrmContext orm)
    {
        AnsiConsole.MarkupLine("[yellow]Running migrations...[/]");

        var runner = new MigrationRunner(orm);

        runner.Migrate(new IMigration[]
        {
            new Migration_001_CreateInitialSchema(),
            new Migration_002_EmptyMigration()
        });

        AnsiConsole.MarkupLine("[green]Migrations completed[/]");
    }
}