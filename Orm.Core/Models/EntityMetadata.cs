namespace Orm.Core.Models;

public record EntityMetadata
{ 
    public Type RuntimeType { get; set; }
    public string TableName { get; set; }
    public List<ColumnMetadata> Columns { get; set; }
    public ColumnMetadata PrimaryKey { get; set; }
}