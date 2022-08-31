namespace HazGo.BuildingBlocks.Core.Domain
{
    using System;

    public abstract class AuditableEntityBase<TPrimaryKey> : EntityBase<TPrimaryKey>
    {
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
