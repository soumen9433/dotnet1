namespace HazGo.BuildingBlocks.Persistence.EF
{
    using HazGo.BuildingBlocks.Core.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    public static class DbContextExtension
    {
        public static void MarkChildEntitiesAsDelete(this DbContext dbContext)
        {
            dbContext.ChangeTracker.DetectChanges();

            foreach (EntityEntry<IHardDeleteEntity> entry in dbContext.ChangeTracker.Entries<IHardDeleteEntity>())
            {
                if (entry.Entity.IsPermanentDelete)
                {
                    dbContext.Entry(entry.Entity).State = EntityState.Deleted;
                }
            }
        }
    }
}