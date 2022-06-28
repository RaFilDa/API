using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RaFilDaAPI.Entities;

namespace RaFilDaAPI.Controllers
{
    
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
         public string Role { get; set; }

         public void OnAuthorization(AuthorizationFilterContext context)
         {
             AuthenticationService auth = context.HttpContext.RequestServices.GetService<AuthenticationService>();
            
            string token = context.HttpContext.Request.Headers["Authorization"].ToString().Split(' ').Last();

            string result = auth.VerifyToken(token);
            
            if (result == "")
            {
                context.Result = new JsonResult("authentication failed") { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            if(!Role.Split(',').Contains(JsonConvert.DeserializeObject<Result>(result).role))
                context.Result = new JsonResult("authentication failed") { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
    
    record Result
    {
        public string? exp { get; set; }
        public string? user_id { get; set; }
        public string role { get; set; }
    }
}
