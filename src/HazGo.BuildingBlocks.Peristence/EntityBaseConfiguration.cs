namespace HazGo.BuildingBlocks.Persistence.EF
{
    using HazGo.BuildingBlocks.Core.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public partial class EntityBaseConfiguration<T, TPrimaryKey> : IEntityTypeConfiguration<T>
        where T : EntityBase<TPrimaryKey>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
        }
    }

    public partial class EntityBaseConfiguration<T> : IEntityTypeConfiguration<T>
        where T : EntityBase<int>
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
        }
    }
}
