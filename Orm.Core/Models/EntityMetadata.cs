namespace Orm.Core.Models;

internal class EntityMetadata
{
    public Type RuntimeType { get; set; }
    public string TableName { get; set; }
    public List<ColumnMetadata> Columns { get; set; } = new();
    public ColumnMetadata PrimaryKey { get; set; }
    public List<ColumnMetadata> ForeignKeys { get; set; } = new();
}