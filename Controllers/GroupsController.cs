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
    [Route("Groups")]
    public class GroupsController : ControllerBase
    {
        private readonly MyContext myContext;

        public GroupsController(MyContext myContext)
        {
            this.myContext = myContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Group>>> GetGroups()
        {
            return Ok(await myContext.Groups.ToListAsync());
        }

        [HttpGet("{computerId}")]
        public IQueryable<CompGroup> GetGroups_ByComputerID(int computerId)
        {
            return myContext.CompGroups.FromSqlRaw("select * from CompGroups where CompID = {0}", computerId);
        }

        [HttpPost]
        public async Task<ActionResult<List<Group>>> AddGroup(Group group)
        {
            myContext.Groups.Add(group);
            await myContext.SaveChangesAsync();

            return Ok(await myContext.Groups.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Group>>> DeleteGroup(int id)
        {
            var group = await myContext.Groups.FindAsync(id);
            if (group == null)
                return NotFound();

            myContext.Groups.Remove(group);
            myContext.SaveChanges();

            return Ok(await myContext.Groups.ToListAsync());
        }
    }
}