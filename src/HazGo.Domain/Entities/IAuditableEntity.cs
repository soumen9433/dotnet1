namespace HazGo.Domain.Entities
{
    using System;
    public interface IAuditableEntity
    {
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int StatusId { get; set; }
    }
}
