using Microsoft.AspNetCore.Mvc;
//using RaFilDaAPI.Repositories;
using System.Collections.Generic;
using RaFilDaAPI.Entities;
using System;
using System.Linq;
//using RaFilDaAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace RaFilDaAPI.Controllers
{
    [ApiController]
    [Route("Computers")]
    [Authorize(Role = "admin")]
    public class ComputersController : ControllerBase
    {
        private readonly MyContext myContext;

        public ComputersController(MyContext myContext)
        {
            this.myContext = myContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Computer>>> GetComputers()
        {
            return Ok(await myContext.Computers.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Computer>> GetComputer(int id)
        {
            var comp = await myContext.Computers.FindAsync(id);
            if (comp == null)
                { return NotFound(); }

            return Ok(comp);
        }
        
        [HttpGet("GetComputersByMAC/{MAC}")]
        [Authorize(Role = "admin,daemon")]
        public IQueryable<Computer> GetComputers_ByMac(string MAC)
        {
            return myContext.Computers.FromSqlRaw("select c.* from Computers c where c.MAC = {0}", MAC);
        }
        
        [HttpGet("GetComputersByConfigID/{confId}")]
        public IQueryable<Computer> GetComputers_ByConfigID(int confId)
        {
            return myContext.Computers.FromSqlRaw("select c.* from CompConfs cc inner join Computers c on c.id = cc.compId where cc.ConfigID = {0}", confId);
        }
        
        [HttpGet("GetComputersByGroupID/{groupId}")]
        public IQueryable<Computer> GetComputers_ByGroupID(int groupId)
        {
            return myContext.Computers.FromSqlRaw("select c.* from CompGroups cc inner join Computers c on c.id = cc.compId where cc.groupId = {0}", groupId);
        }

        [HttpPost]
        [Authorize(Role = "admin,daemon")]
        public async Task<ActionResult<List<Computer>>> AddComputer(Computer computer)
        {
            myContext.Computers.Add(computer);
            await myContext.SaveChangesAsync();

            return Ok(await myContext.Computers.ToListAsync());
        }

        [HttpPost]
        [Route("AddComputerToGroup")]
        public ActionResult<IQueryable<CompGroup>> AddComputerToGroup(int compID, int groupID)
        {
            if(compID == 0 || groupID == 0)
                return BadRequest();

            var compGroup = new CompGroup{
                Id = 0, 
                CompID = compID,
                GroupID = groupID
            };
            myContext.CompGroups.Add(compGroup);
            myContext.SaveChanges();

            return Ok(myContext.CompGroups.FromSqlRaw("select * from CompGroups"));
        }

        [HttpPost]
        [Route("AddComputerToConfig")]
        public ActionResult<IQueryable<CompConf>> AddComputerToConfig(int compID, int configID)
        {
            if(compID == 0 || configID == 0)
                return BadRequest();

            var compConfig = new CompConf{
                Id = 0, 
                CompID = compID,
                ConfigID = configID
            };
            myContext.CompConfs.Add(compConfig);
            myContext.SaveChanges();

            return Ok(myContext.CompConfs.FromSqlRaw("select * from CompConfs"));
        }

        [HttpPut]
        [Route("UpdateComputer")]
        public async Task<ActionResult<List<Computer>>> UpdateComputer(Computer computer, int id)
        {
            var dbComputer = await myContext.Computers.FindAsync(id);
            if (dbComputer == null)
                return NotFound();

            dbComputer.Name = computer.Name;
            dbComputer.MAC = computer.MAC;
            dbComputer.IP = computer.IP;

            await myContext.SaveChangesAsync();

            return Ok(await myContext.Computers.ToListAsync());
        }

        [HttpPut]
        [Route("UpdateLastSeen")]
        public async Task<ActionResult<List<Computer>>> UpdateLastSeen(int id)
        {
            var computer = await myContext.Computers.FindAsync(id);
            computer.LastSeen = DateTime.UtcNow;
            return Ok(await myContext.Computers.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Computer>>> DeleteComputer(int id)
        {
            var computer = await myContext.Computers.FindAsync(id);
            if (computer == null)
                return NotFound();

            myContext.Computers.Remove(computer);
            myContext.SaveChanges();

            return Ok(await myContext.Computers.ToListAsync());
        }

        [HttpDelete]
        [Route("RemoveComputerFromGroup")]
        public ActionResult<IQueryable<CompGroup>> RemoveComputerFromGroup(int compID, int groupID)
        {
            if(compID == 0 || groupID == 0)
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

        [HttpDelete]
        [Route("RemoveComputerFromConfig")]
        public ActionResult<IQueryable<CompConf>> RemoveComputerFromConfig(int compID, int configID)
        {
            if(compID == 0 || configID == 0)
                return BadRequest();         

            try {
            var deleted =
                myContext.CompConfs.FromSqlRaw("select * from CompConfs where ConfigID = {0} and CompID = {1}",
                    configID, compID).First();
                myContext.CompConfs.Remove(deleted);
            }
            catch { return NotFound(); }
            myContext.SaveChanges();

            return Ok(myContext.CompConfs.FromSqlRaw("select * from CompConfs"));
        }
    }
}