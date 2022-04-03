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


        [HttpPut]
        [Route("UpdateGroup")]
        public async Task<ActionResult<List<Group>>> UpdateGroup(Group group, int id)
        {

            var dbGroup = await myContext.Groups.FindAsync(id);
            if (dbGroup == null)
                return NotFound();

            dbGroup.Name = group.Name;

            await myContext.SaveChangesAsync();

            return Ok(await myContext.Groups.ToListAsync());
        }

        [HttpPost]
        [Route("AddGroupToConfig")]
        public ActionResult<IQueryable<ConfGroup>> AddGroupToConfig(int groupId, int configId)
        {
            if(groupId == 0 || configId == 0)
                return BadRequest();

            var confGroup = new ConfGroup{
                Id = 0, 
                GroupID = groupId,
                ConfigID = configId
            };
            myContext.ConfGroups.Add(confGroup);
            myContext.SaveChanges();

            return Ok(myContext.ConfGroups.FromSqlRaw("select * from ConfGroups"));
        }

        [HttpPost]
        [Route("AddGroupToComputer")]
        public ActionResult<IQueryable<CompGroup>> AddGroupToComputer(int groupId, int compId)
        {
            if(groupId == 0 || compId == 0)
                return BadRequest();

            var compGroup = new CompGroup{
                Id = 0, 
                GroupID = groupId,
                CompID = compId
            };
            myContext.CompGroups.Add(compGroup);
            myContext.SaveChanges();

            return Ok(myContext.CompGroups.FromSqlRaw("select * from CompGroups"));
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

        [HttpDelete]
        [Route("RemoveGroupFromConfig")]
        public ActionResult<IQueryable<ConfGroup>> RemoveGroupFromConfig(int groupID, int configID)
        {
            if(groupID == 0 || configID == 0)
                return BadRequest();         

            try {
            var deleted =
                myContext.ConfGroups.FromSqlRaw("select * from ConfGroups where GroupID = {0} and ConfigID = {1}",
                    groupID, configID).First();
                myContext.ConfGroups.Remove(deleted);
            }
            catch { return NotFound(); }
            myContext.SaveChanges();

            return Ok(myContext.ConfGroups.FromSqlRaw("select * from ConfGroups"));
        }

        [HttpDelete]
        [Route("RemoveGroupFromComputer")]
        public ActionResult<IQueryable<CompGroup>> RemoveComputerFromConfig(int groupID, int compID)
        {
            if(groupID == 0 || compID == 0)
                return BadRequest();         

            try {
            var deleted =
                myContext.CompGroups.FromSqlRaw("select * from CompGroups where GroupID = {0} and CompID = {1}",
                    groupID, compID).First();
                myContext.CompGroups.Remove(deleted);
            }
            catch { return NotFound(); }
            myContext.SaveChanges();

            return Ok(myContext.CompGroups.FromSqlRaw("select * from CompGroups"));
        }
    }
}