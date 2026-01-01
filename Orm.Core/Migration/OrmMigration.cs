using Orm.Core.Attributes;

namespace Orm.Core.Migration;

[Table(Name = "__orm_migration")]
internal class OrmMigration
{
    [PrimaryKey]
    public string Id { get; set; } = null!;

    [NotNull]
    public DateTime AppliedAt { get; set; }
}