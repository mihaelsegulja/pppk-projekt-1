namespace Orm.Core.Models;

internal class EntityMetadata
{
    public Type RuntimeType { get; set; }
    public string TableName { get; set; }
    public List<ColumnMetadata> Columns { get; set; }
    public ColumnMetadata PrimaryKey { get; set; }
}