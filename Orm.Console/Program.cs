using dotenv.net;
using Spectre.Console;
using Orm.Core;
using Orm.Console.Demo;

DotEnv.Load();

AnsiConsole.MarkupLine("[bold cyan]*** ORM Demo ***[/]");

var connStr = Environment.GetEnvironmentVariable("PPPK_CONN");
if (string.IsNullOrWhiteSpace(connStr))
{
    AnsiConsole.MarkupLine("[red]Missing PPPK_CONN[/]");
    return;
}

try
{
    using var orm = new OrmContext(connStr);

    AnsiConsole.Status()
        .Start("Running ORM demo...", _ =>
        {
            MigrationDemo.Run(orm);
            // SchemaDemo.Run(orm);
            SeedDemo.Run(orm);
            CrudDemo.Run(orm);
            IncludeDemo.Run(orm);
        });

    AnsiConsole.MarkupLine("[bold green]Demo finished successfully[/]");
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex);
}