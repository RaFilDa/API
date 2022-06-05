using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaFilDaAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuthenticationService = RaFilDaAPI.Entities.AuthenticationService;

namespace RaFilDaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        
        private readonly MyContext myContext;
        private AuthenticationService auth;

        public SessionsController(MyContext myContext)
        {
            this.myContext = myContext;
            this.auth = new AuthenticationService(myContext);
        }

        [HttpPost]
        public JsonResult Login(Credentials credentials)
        {
            try
            {
                return new JsonResult(auth.Authenticate(credentials));
            }
            catch(Exception e)
            {
                return new JsonResult(e.Message) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
        
        [HttpGet]
        [Authorize(Role = "admin")]
        public async Task<ActionResult<List<Session>>> GetSessions()
        {
            var output = await myContext.Sessions.Select(x => new SessionInfo() {session = x, expired = this.auth.CheckExpired(x.token)}).ToListAsync();
            return Ok(output);
        }
        
        [HttpPost("add")]
        [Authorize(Role = "admin")]
        public async Task<ActionResult<List<Session>>> AddSession(string name, int days, bool unlimited)
        {
            string token;
            if(unlimited)
                token = JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                    .WithSecret(AuthenticationService.SECRET)
                    .AddClaim("exp", DateTimeOffset.MaxValue.ToUnixTimeSeconds())
                    .AddClaim("role", "daemon")
                    .Encode();
            else
                token = JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                    .WithSecret(AuthenticationService.SECRET)
                    .AddClaim("exp", DateTimeOffset.UtcNow.AddDays(days).ToUnixTimeSeconds())
                    .AddClaim("role", "daemon")
                    .Encode();
            
            Session session = new Session
            {
                id = 0, 
                name = name, 
                token = token
            };

            myContext.Sessions.Add(session);
            await myContext.SaveChangesAsync();

            return Ok(await myContext.Sessions.ToListAsync());
        }
        
        [HttpPost("banned")]
        public bool IsBanned(string token)
        {
            return myContext.BannedSessions.Any(x => x.token == token);
        }
        
        [HttpDelete]
        [Authorize(Role = "admin")]
        public async Task<ActionResult<List<Session>>> BanSession(string token)
        {
            Session session = myContext.Sessions.First(x => x.token == token);
            
            myContext.Sessions.Remove(session);
            myContext.BannedSessions.Add(new BannedSession
            {
                id = 0,
                token = session.token
            });
            
            await myContext.SaveChangesAsync();

            return Ok(await myContext.BannedSessions.ToListAsync());
        }
    }
}
