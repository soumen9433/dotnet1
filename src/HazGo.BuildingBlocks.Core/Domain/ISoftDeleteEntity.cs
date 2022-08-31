namespace HazGo.BuildingBlocks.Core.Domain
{
    using HazGo.BuildingBlocks.Core.Common;

    public interface ISoftDeleteEntity
    {
        public EntityStatus StatusId { get; set; }
    }
}
