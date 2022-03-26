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
    }
}