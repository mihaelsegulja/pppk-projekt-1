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
    using var orm = new OrmClient(connStr);

    AnsiConsole.Status()
        .Start("Running ORM demo...", _ =>
        {
            SchemaDemo.Run(orm);
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