using HazGo.BuildingBlocks.Core.Common;
using HazGo.BuildingBlocks.Core.Domain;
using System;

namespace HazGo.Domain.Entities
{
    public class City : AuditableEntityBase<int>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string OtherComments { get; set; }
        public EntityStatus StatusId { get; set; }
    }
}
