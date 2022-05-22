using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RaFilDaAPI.Entities;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RaFilDaAPI.Controllers
{
    [ApiController]
    [Route("Users")]
    [Authorize(Role = "admin")]
    public class UsersController : ControllerBase
    {
        private readonly MyContext myContext;

        public UsersController(MyContext myContext)
        {
            this.myContext = myContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            return Ok(await myContext.Users.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<List<User>>> AddUser(User user)
        {
            string salt = BCrypt.Net.BCrypt.GenerateSalt(6);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);
            Console.WriteLine(user.Password);
            myContext.Users.Add(user);
            await myContext.SaveChangesAsync();

            return Ok(await myContext.Users.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<User>>> DeleteUser(int id)
        {
            var user = await myContext.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            myContext.Users.Remove(user);
            myContext.SaveChanges();

            return Ok(await myContext.Users.ToListAsync());
        }
    }
}