namespace HazGo.BuildingBlocks.Persistence.EF
{
    using Microsoft.EntityFrameworkCore;

    public static class ModelBuilderExtension
    {
        public static void UseLowerCaseForTableName(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entity.ClrType.Name.ToLower();

                modelBuilder.Entity(entity.Name).ToTable(tableName);
            }
        }
    }
}
