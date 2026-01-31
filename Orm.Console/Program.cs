using Microsoft.Extensions.Configuration;
using Spectre.Console;
using Orm.Core;
using Orm.Console.Demo;

AnsiConsole.MarkupLine("[bold cyan]*** ORM Demo ***[/]");

var config = new ConfigurationBuilder() 
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var connStr = config.GetConnectionString("Default");
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