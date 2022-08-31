using HazGo.BuildingBlocks.Api.Helper;
using HazGo.BuildingBlocks.Core.Common;
using HazGo.BuildingBlocks.Core.Domain;
using Microsoft.AspNetCore.Http;

namespace HazGo.BuildingBlocks.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        #region Private properties
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGetIpAddress _getIpAddress;
        #endregion

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IGetIpAddress getIpAddress)
        {
            _httpContextAccessor = httpContextAccessor;
            _getIpAddress = getIpAddress;
            if (httpContextAccessor.HttpContext != null)
            {
                SetProperties();
            }
        }

        public string UserId { get; private set; }

        public string ClientId { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }

        private void SetProperties()
        {
            UserId = IdentityExtensions.GetUsername(_httpContextAccessor.HttpContext);
            ClientId = IdentityExtensions.GetClientId(_httpContextAccessor.HttpContext);
            FirstName = IdentityExtensions.GetFirstname(_httpContextAccessor.HttpContext);
            LastName = IdentityExtensions.GetLastname(_httpContextAccessor.HttpContext);
        }
    }
}
