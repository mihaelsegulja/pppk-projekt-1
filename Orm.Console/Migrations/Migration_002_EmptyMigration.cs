using Orm.Console.Models;
using Orm.Core;
using Orm.Core.Migration;

namespace Orm.Console.Migrations;

public class Migration_002_EmptyMigration : IMigration
{
    public string Id => "002_EmptyMigration";

    public void Up(OrmContext db)
    {
        
    }
}