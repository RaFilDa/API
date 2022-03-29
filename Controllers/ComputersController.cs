using Microsoft.AspNetCore.Mvc;
using RaFilDaAPI.Repositories;
using System.Collections.Generic;
using RaFilDaAPI.Entities;
using System;
using System.Linq;
using RaFilDaAPI.DTOs;
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

        [HttpPut("{id}")]
        public async Task<ActionResult<List<Computer>>> UpdateComputer(Computer computer)
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

            var dbComputer = await myContext.Computers.FindAsync(computer.Id);
            if (dbComputer == null)
                return NotFound();

            dbComputer.Name = computer.Name;
            dbComputer.MAC = computer.MAC;
            dbComputer.IP = computer.MAC;

            await myContext.SaveChangesAsync();

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
    }
}