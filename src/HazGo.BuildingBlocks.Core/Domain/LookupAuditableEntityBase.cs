namespace HazGo.BuildingBlocks.Core.Domain
{
    public class LookupAuditableEntityBase<TPrimaryKey> : AuditableEntityBase<TPrimaryKey>
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public string Shortdescription { get; set; }
    }
}
