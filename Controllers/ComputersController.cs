using Microsoft.AspNetCore.Mvc;
using RaFilDaAPI.Repositories;
using System.Collections.Generic;
using RaFilDaAPI.Entities;
using System;

namespace RaFilDaAPI.Controllers
{
    [ApiController]
    [Route("Computers")]
    public class ComputersController : ControllerBase
    {
        private readonly InMemoryComputersRepository repository;  // prozatím

        public ComputersController(){
            repository = new InMemoryComputersRepository();
        }

        [HttpGet]
        public IEnumerable<Computer> GetComputers()
        {
            return repository.GetComputers();
        }

        [HttpGet("{id}")]
        public ActionResult<Computer> GetComputer(int id)
        {
            var comp = repository.GetComputer(id);
            if (comp == null)
                { return NotFound(); }

            return comp;
        }
    }
}