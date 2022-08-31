using System;
using System.Threading.Tasks;
using HazGo.BuildingBlocks.Api.Helper;
using HazGo.BuildingBlocks.Core.Common;
using HazGo.BuildingBlocks.Core.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HazGo.BuildingBlocks.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizationAttribute
          : ActionFilterAttribute
    {
        private readonly Module _module;
        private readonly Rights _rights;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationClient _authorizationClient;

        public AuthorizationAttribute(
             Module module,
             Rights rights,
             IHttpContextAccessor httpContextAccessor,
             IAuthorizationClient authorizationClient)
        {
            _module = module;
            _rights = rights;
            _httpContextAccessor = httpContextAccessor;
            _authorizationClient = authorizationClient;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string role = _httpContextAccessor.HttpContext.GetRole();
            
            var result = await _authorizationClient.HasPermission(role, _module, _rights);
            if (result)
            {
                await base.OnActionExecutionAsync(context, next);
            }
            else
            {
                throw new UnauthorizedAccessException("You are not authorized.");
            }           
        }

    }

}

