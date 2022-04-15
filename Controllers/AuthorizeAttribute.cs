using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RaFilDaAPI.Entities;

namespace RaFilDaAPI.Controllers
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string Role { get; set; }

        private AuthenticationService auth = new AuthenticationService();

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers["Authorization"].ToString().Split(' ').Last();

            if (!this.auth.VerifyToken(token))
            {
                context.Result = new JsonResult("authentication failed") { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
