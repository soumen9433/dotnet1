namespace HazGo.BuildingBlocks.Persistence.EF
{
    using HazGo.BuildingBlocks.Core.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ReferenceDataEntityBaseConfiguration<T, TPrimaryKey> : IEntityTypeConfiguration<T>
        where T : ReferenceDataEntityBase<TPrimaryKey>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(s => s.Id)
             .ValueGeneratedNever();
        }
    }
}
