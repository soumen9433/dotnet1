using HazGo.BuildingBlocks.Core.Common;

namespace HazGo.Application.City.Dto
{
    public class CityDtoBase
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string OtherComments { get; set; }

        public EntityStatus StatusId { get; set; }
    }
}
