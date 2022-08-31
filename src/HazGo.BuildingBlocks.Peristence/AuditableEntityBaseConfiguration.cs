namespace HazGo.BuildingBlocks.Persistence.EF
{
    using HazGo.BuildingBlocks.Core.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AuditableEntityBaseConfiguration<T, TPrimaryKey> : IEntityTypeConfiguration<T>
        where T : AuditableEntityBase<TPrimaryKey>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(t => t.CreatedBy)
              .HasMaxLength(50)
              .IsRequired();
            builder.Property(t => t.CreatedDate)
                    .IsRequired();
            builder.Property(t => t.UpdatedBy)
                    .HasMaxLength(50);
        }
    }
}
