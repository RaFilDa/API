using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RaFilDaAPI.Entities;

namespace RaFilDaAPI.Controllers
{
    
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
         public string Role { get; set; }
         private readonly MyContext myContext;
         // private AuthenticationService auth;
         // public AuthorizeAttribute(MyContext myContext)
         // {
         //     this.myContext = myContext;
         //     this.auth = new AuthenticationService(myContext);
         // }


        private AuthenticationService auth = new (null);

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers["Authorization"].ToString().Split(' ').Last();

            string result = this.auth.VerifyToken(token);
            
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
