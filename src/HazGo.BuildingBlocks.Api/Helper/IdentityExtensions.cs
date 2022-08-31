using HazGo.BuildingBlocks.Api.Constant;
using HazGo.BuildingBlocks.Core.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace HazGo.BuildingBlocks.Api.Helper
{
    public static class IdentityExtensions
    {
        /// <summary>
        ///  Get Login user Username
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetUsername(this HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;

            if (identity != null)
                return identity.FindFirst(ClaimTypes.Name)?.Value;
            return string.Empty;
        }


        /// <summary>
        /// Get Login user firstName
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetFirstname(this HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            if (identity != null)
                return identity.FindFirst(ClaimsConstant.FirstName)?.Value;
            return string.Empty;
        }

        /// <summary>
        /// Get Login user lastName
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetLastname(this HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            if (identity != null)
                return identity.FindFirst(ClaimsConstant.LastName)?.Value;
            return string.Empty;
        }
        /// <summary>
        /// Get Login user OrganizationId
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int? GetOrganizationId(this HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var result = identity.FindFirst(ClaimsConstant.OrganisationId)?.Value;
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }

            }
            return null;
        }

        /// <summary>
        /// Get Client Id
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetClientId(this HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;

            if (identity != null)
                return identity.FindFirst(ClaimsConstant.ClientId)?.Value;
            return string.Empty;
        }

        public static string GetRole(this HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;

            if (identity != null)
                return identity.FindFirst(ClaimTypes.Role)?.Value;
            return string.Empty;

        }
    }
}
