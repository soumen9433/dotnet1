using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HazGo.Application.Cities.Dto;

namespace HazGo.Application.City.Mapper
{
    public class CityMapper
    {
        public static Domain.Entities.City Map(Domain.Entities.City city, CityDto cityDto)
        {
            city.Code = cityDto.Code;
            city.Name = cityDto.Name;
            city.OtherComments = cityDto.OtherComments;

            return city;
        }
    }
}
