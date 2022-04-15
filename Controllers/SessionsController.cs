using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private RaFilDaAPI.Entities.AuthenticationService auth = new RaFilDaAPI.Entities.AuthenticationService();

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
