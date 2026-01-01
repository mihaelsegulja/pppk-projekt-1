using Orm.Core.Models;

namespace Orm.Core.ChangeTracking;

internal class TrackedEntity
{
    public object Entity { get; }
    public EntityMetadata Metadata { get; }
    public EntitySnapshot Snapshot { get; set; }
    public EntityState State { get; set; }

    public TrackedEntity(object entity, EntityMetadata metadata, EntitySnapshot snapshot)
    {
        Entity = entity;
        Metadata = metadata;
        Snapshot = snapshot;
        State = EntityState.Unchanged;
    }
}