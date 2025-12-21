using Orm.Core.Models;

namespace Orm.Core.ChangeTracking;

internal class ChangeTracker
{
    private readonly Dictionary<object, (EntityMetadata Metadata, EntitySnapshot Snapshot)> _tracked = new();
    private readonly HashSet<object> _deleted = new();
    
    public void Track(object entity, EntityMetadata metadata)
    {
        var snapshot = new EntitySnapshot();

        foreach (var col in metadata.Columns)
        {
            snapshot.Values[col.ColumnName] = col.Property.GetValue(entity);
        }

        _tracked[entity] = (metadata, snapshot);
    }


    public IReadOnlyDictionary<string, (object? Old, object? New)> DetectChanges(object entity)
    {
        if (!_tracked.TryGetValue(entity, out var entry))
            return new Dictionary<string, (object?, object?)>();

        var metadata = entry.Metadata;
        var snapshot = entry.Snapshot;

        var changes = new Dictionary<string, (object?, object?)>();

        foreach (var col in metadata.Columns)
        {
            var oldValue = snapshot.Values[col.ColumnName];
            var newValue = col.Property.GetValue(entity);

            if (!Equals(oldValue, newValue))
                changes[col.ColumnName] = (oldValue, newValue);
        }

        return changes;
    }
    
    public IEnumerable<(object Entity, EntityMetadata Metadata)> TrackedEntities()
    {
        foreach (var kvp in _tracked)
            yield return (kvp.Key, kvp.Value.Metadata);
    }


    public void AcceptChanges(object entity, EntityMetadata metadata)
    {
        Track(entity, metadata);
    }
    
    public void MarkDeleted(object entity)
    {
        _deleted.Add(entity);
    }

    public IEnumerable<object> DeletedEntities()
    {
        return _deleted;
    }
    
    public void ClearDeleted(object entity)
    {
        _deleted.Remove(entity);
        _tracked.Remove(entity);
    }
}
