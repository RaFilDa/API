using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaFilDaAPI;
using RaFilDaAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        
        private readonly MyContext myContext;
        private RaFilDaAPI.Entities.AuthenticationService auth;

        public SessionsController(MyContext myContext)
        {
            this.myContext = myContext;
            this.auth = new RaFilDaAPI.Entities.AuthenticationService(myContext);
        }

        [HttpPost]
        public JsonResult Login(Credentials credentials)
        {
            try
            {
                return new JsonResult(this.auth.Authenticate(credentials));
            }
            catch
            {
                return new JsonResult("Invalid username or password") { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
