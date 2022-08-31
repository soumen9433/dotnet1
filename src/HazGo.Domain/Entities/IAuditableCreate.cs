namespace HazGo.Domain.Entities
{
    using System;
    public interface IAuditableAppendOnly
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int StatusId { get; set; }
    }
}
