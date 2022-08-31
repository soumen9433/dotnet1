using HazGo.BuildingBlocks.Persistence.EF;
using HazGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HazGo.Infrastructure.EntityConfiguration
{
    public class CityEntityConfiguration : AuditableEntityBaseConfiguration<City,int>
    {
        public override void Configure(EntityTypeBuilder<City> builder)
        {
            base.Configure(builder);
            builder.ToTable(nameof(City));
            builder.Property(t => t.Code)
           .HasMaxLength(25)
           .IsRequired(true);
            builder.Property(t => t.OtherComments)
           .HasMaxLength(200);
            builder.Property(t => t.Name)
           .HasMaxLength(100)
           .IsRequired(true);
            builder.Property(t => t.StatusId)
                .IsRequired();
        }
    }
}
