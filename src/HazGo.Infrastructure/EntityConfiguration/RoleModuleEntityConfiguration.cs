using HazGo.BuildingBlocks.Persistence.EF;
using HazGo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HazGo.Infrastructure.EntityConfiguration
{
    public class RoleModuleEntityConfiguration : AuditableEntityBaseConfiguration<RoleModule, int>
    {
        public override void Configure(EntityTypeBuilder<RoleModule> builder)
        {
            base.Configure(builder);
            builder.ToTable(nameof(RoleModule));

            builder.HasOne<Module>(s => s.Module)
            .WithMany(g => g.RoleModules)
            .HasForeignKey(s => s.ModuleId);
        }
    }
}
