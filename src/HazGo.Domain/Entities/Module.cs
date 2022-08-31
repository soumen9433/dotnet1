using System.Collections.Generic;
using HazGo.BuildingBlocks.Core.Domain;

namespace HazGo.Domain.Entities
{
    public class Module : AuditableEntityBase<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<RoleModule> RoleModules { get; set; }
    }
}
