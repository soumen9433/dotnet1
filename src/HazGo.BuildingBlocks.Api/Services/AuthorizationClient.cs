using System.Threading.Tasks;
using HazGo.BuildingBlocks.Core.Common;
using HazGo.BuildingBlocks.Core.Domain;
using HazGo.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Module = HazGo.BuildingBlocks.Core.Common.Module;

namespace HazGo.BuildingBlocks.Api.Services
{
    internal class AuthorizationClient : IAuthorizationClient
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleModuleRepository _roleModulesRepository;

        public AuthorizationClient(RoleManager<IdentityRole> roleManager, IRoleModuleRepository roleModulesRepository)
        {
            _roleManager = roleManager;
            _roleModulesRepository = roleModulesRepository;
        }

        public async Task<bool> HasPermission(string role, Module module, Rights rights)
        {
            var userRole = await _roleManager.FindByNameAsync(role.ToUpper());

            var permission = await _roleModulesRepository.FindFirstOrDefaultAsync(x => x.RoleId == userRole.Id && x.ModuleId == (int)module);

            return rights switch
            {
                Rights.View => permission.ViewRights,
                Rights.Add => permission.AddRights,
                Rights.Edit => permission.EditRights,
                Rights.Delete => permission.DeleteRights,
                _ => false,
            };
        }
    }
}
