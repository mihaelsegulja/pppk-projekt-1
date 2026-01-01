using Orm.Core.Models;

namespace Orm.Core.ChangeTracking;

internal class ChangeTracker
{
    private readonly Dictionary<object, TrackedEntity> _tracked = new();

    public void Track(object entity, EntityMetadata metadata)
    {
        var snapshot = new EntitySnapshot();

        foreach (var col in metadata.Columns)
        {
            snapshot.Values[col.ColumnName] = col.Property.GetValue(entity);
        }

        _tracked[entity] = new TrackedEntity(entity, metadata, snapshot);
    }

    public IReadOnlyDictionary<string, (object? Old, object? New)> DetectChanges(object entity)
    {
        if (!_tracked.TryGetValue(entity, out var tracked))
            return new Dictionary<string, (object?, object?)>();

        var changes = new Dictionary<string, (object?, object?)>();

        foreach (var col in tracked.Metadata.Columns)
        {
            var oldValue = tracked.Snapshot.Values[col.ColumnName];
            var newValue = col.Property.GetValue(entity);

            if (!Equals(oldValue, newValue))
                changes[col.ColumnName] = (oldValue, newValue);
        }

        if (changes.Count > 0 && tracked.State == EntityState.Unchanged)
            tracked.State = EntityState.Modified;

        return changes;
    }

    public IEnumerable<TrackedEntity> TrackedEntities()
        => _tracked.Values;

    public IEnumerable<TrackedEntity> DeletedEntities()
        => _tracked.Values.Where(t => t.State == EntityState.Deleted);

    public void MarkDeleted(object entity)
    {
        if (_tracked.TryGetValue(entity, out var tracked))
            tracked.State = EntityState.Deleted;
    }

    public void AcceptChanges(TrackedEntity tracked)
    {
        var snapshot = new EntitySnapshot();

        foreach (var col in tracked.Metadata.Columns)
        {
            snapshot.Values[col.ColumnName] = col.Property.GetValue(tracked.Entity);
        }

        tracked.Snapshot = snapshot;
        tracked.State = EntityState.Unchanged;
    }

    public void Detach(object entity)
    {
        _tracked.Remove(entity);
    }
}
