namespace Orm.Core.Migration;

public class MigrationRunner
{
    private readonly OrmContext _db;

    public MigrationRunner(OrmContext db)
    {
        _db = db;
    }

    private void EnsureMigrationTable()
    {
        _db.CreateTable<OrmMigration>();
    }

    public void Migrate(IEnumerable<IMigration> migrations)
    {
        EnsureMigrationTable();

        var applied = GetAppliedMigrationIds();

        var pending = migrations
            .OrderBy(m => m.Id)
            .Where(m => !applied.Contains(m.Id));

        foreach (var migration in pending)
        {
            migration.Up(_db);

            _db.Insert(new OrmMigration
            {
                Id = migration.Id,
                AppliedAt = DateTime.UtcNow
            });
        }
    }

    private HashSet<string> GetAppliedMigrationIds()
    {
        try
        {
            return _db.GetAll<OrmMigration>()
                .Select(m => m.Id)
                .ToHashSet();
        }
        catch
        {
            return new HashSet<string>();
        }
    }
}
