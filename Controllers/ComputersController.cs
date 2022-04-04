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
    public class ComputersController : ControllerBase
    {
        //private readonly IComputersRepository repository;
        private readonly MyContext myContext;

        /*
        public ComputersController(IComputersRepository repository)
        {
            this.repository = repository;
        } */

        public ComputersController(MyContext myContext)
        {
            this.myContext = myContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Computer>>> GetComputers()
        {
            //return repository.GetComputers().Select(computer => computer.AsDTO());

            return Ok(await myContext.Computers.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Computer>> GetComputer(int id)
        {
            var comp = await myContext.Computers.FindAsync(id);
            if (comp == null)
                { return NotFound(); }

            //return comp.AsDTO();
            return Ok(comp);
        }

        
        [HttpGet]
        [Route("GetComputersByConfigID")]
        public IQueryable<CompConf> GetComputers_ByConfigID(int confId)
        {
            return myContext.CompConfs.FromSqlRaw("select * from CompConfs where ConfigID = {0}", confId);
        }

        [HttpPost]
        public async Task<ActionResult<List<Computer>>> AddComputer(Computer computer)
        {
            /* Computer computer = new(){
                Id = Guid.NewGuid(),
                Name = computerDTO.Name,
                MAC = computerDTO.MAC,
                IP = computerDTO.MAC,
                LastSeen = DateTimeOffset.UtcNow    //temp
            };

            repository.AddComputer(computer); 

            return CreatedAtAction(nameof(AddComputer), new {id = computer.Id}, computer.AsDTO()); */

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

            //myContext.CompGroups.FromSqlRaw("insert into CompGroup values(0, {0}, {1})", compID, groupID);

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
            /*var existingComputer = repository.GetComputer(id);
            if (existingComputer == null)
                return NotFound();
            
            Computer updatedComputer = existingComputer with {
                Name = computerDTO.Name,
                MAC = computerDTO.MAC,
                IP = computerDTO.MAC
            };

            repository.UpdateComputer(updatedComputer); 
            
            return NoContent();
            */

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
            /*var existingComputer = repository.GetComputer(id);
            if (existingComputer == null)
                return NotFound();

            repository.DeleteComputer(id);

            return NoContent();*/

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