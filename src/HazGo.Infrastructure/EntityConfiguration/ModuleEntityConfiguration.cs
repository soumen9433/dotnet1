using HazGo.BuildingBlocks.Persistence.EF;
using HazGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HazGo.Infrastructure.EntityConfiguration
{
    public class ModuleEntityConfiguration : AuditableEntityBaseConfiguration<Module, int>
    {
        public override void Configure(EntityTypeBuilder<Module> builder)
        {
            base.Configure(builder);
            builder.ToTable(nameof(Module));
            builder.Property(t => t.Name)
           .IsRequired(true);
            builder.Property(t => t.Description)
           .IsRequired(true);
        }
    }
}
