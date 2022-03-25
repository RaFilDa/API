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
        public ActionResult<ComputerDTO> GetComputer(int id)
        {
            var comp = repository.GetComputer(id);
            if (comp == null)
                { return NotFound(); }

            return comp.AsDTO();
        }
    }
}