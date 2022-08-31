using HazGo.Domain.Entities;
using HazGo.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using HazGo.BuildingBlocks.Persistence.EF;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HazGo.Infrastructure.Repositories
{
    public class CityRepository : RepositoryBase<City, int>, ICityRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CityRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
         
        public override async Task<City> GetByIdAsync(int id)
        {
            var city = await _applicationDbContext.City
                        .Where(s => s.Id == id)
                        .AsSplitQuery()
                        .FirstOrDefaultAsync();

            return city;
        }
    }
}
