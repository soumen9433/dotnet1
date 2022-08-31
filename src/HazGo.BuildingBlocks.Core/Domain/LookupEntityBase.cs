namespace HazGo.BuildingBlocks.Core.Domain
{
    public abstract class LookupEntityBase<TPrimaryKey> : EntityBase<TPrimaryKey>
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public string Shortdescription { get; set; }
    }
}
