using Microsoft.AspNetCore.Mvc;
using RaFilDaAPI.Repositories;
using System.Collections.Generic;
using RaFilDaAPI.Entities;
using System;
using System.Linq;
using RaFilDaAPI.DTOs;

namespace RaFilDaAPI.Controllers
{
    [ApiController]
    [Route("Computers")]
    public class ComputersController : ControllerBase
    {
        private readonly IComputersRepository repository;

        public ComputersController(IComputersRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IEnumerable<ComputerDTO> GetComputers()
        {
            return repository.GetComputers().Select(computer => computer.AsDTO());
        }

        [HttpGet("{id}")]
        public ActionResult<ComputerDTO> GetComputer(Guid id)
        {
            var comp = repository.GetComputer(id);
            if (comp == null)
                { return NotFound(); }

            return comp.AsDTO();
        }

        [HttpPost]
        public ActionResult<ComputerDTO> AddComputer(AddComputerDTO computerDTO)
        {
            Computer computer = new(){
                Id = Guid.NewGuid(),
                Name = computerDTO.Name,
                MAC = computerDTO.MAC,
                IP = computerDTO.MAC,
                LastSeen = DateTimeOffset.UtcNow    //temp
            };

            repository.AddComputer(computer);

            return CreatedAtAction(nameof(AddComputer), new {id = computer.Id}, computer.AsDTO());
        }

        [HttpPut("{id}")]
        public ActionResult UpdateComputer(Guid id, UpdateComputerDTO computerDTO)
        {
            var existingComputer = repository.GetComputer(id);
            if (existingComputer == null)
                return NotFound();
            
            Computer updatedComputer = existingComputer with {
                Name = computerDTO.Name,
                MAC = computerDTO.MAC,
                IP = computerDTO.MAC
            };

            repository.UpdateComputer(updatedComputer);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteComputer(Guid id)
        {
            var existingComputer = repository.GetComputer(id);
            if (existingComputer == null)
                return NotFound();

            repository.DeleteComputer(id);

            return NoContent();
        }
    }
}