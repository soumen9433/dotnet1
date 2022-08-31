using HazGo.BuildingBlocks.Persistence.EF;
using HazGo.Domain.Entities;
using HazGo.Domain.Repositories;

namespace HazGo.Infrastructure.Repositories
{
    public class RoleModuleRepository : RepositoryBase<RoleModule, int>, IRoleModuleRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public RoleModuleRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
    }
}
