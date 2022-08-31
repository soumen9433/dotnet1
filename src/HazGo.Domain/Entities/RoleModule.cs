using System;
using HazGo.BuildingBlocks.Core.Domain;

namespace HazGo.Domain.Entities
{
    public class RoleModule : AuditableEntityBase<int>
    {
        public string RoleId { get; set; }
        public int ModuleId { get; set; }
        public bool ViewRights { get; set; }
        public bool EditRights { get; set; }
        public bool DeleteRights { get; set; }
        public bool AddRights { get; set; }

        // Navigation Properties
        public Module Module { get; set; }
    }
}
