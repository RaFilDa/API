using System.Collections.Generic;
using RaFilDaAPI.Entities;
using System;
using System.Linq;

namespace RaFilDaAPI.Repositories
{


    public class InMemoryComputersRepository : IComputersRepository
    {
        private readonly List<Computer> computers = new()
        {
            new Computer { Id = Guid.NewGuid(), Name = "TestComputer_1", MAC = "2C:54:91:88:C9:E3", IP = "69.89.31.226", LastSeen = DateTimeOffset.UtcNow },
            new Computer { Id = Guid.NewGuid(), Name = "TestComputer_2", MAC = "2C:54:91:88:C9:E3", IP = "69.89.31.226", LastSeen = DateTimeOffset.UtcNow },
            new Computer { Id = Guid.NewGuid(), Name = "TestComputer_3", MAC = "2C:54:91:88:C9:E3", IP = "69.89.31.226", LastSeen = DateTimeOffset.UtcNow }
        };

        public IEnumerable<Computer> GetComputers()
        {
            return computers;
        }

        public Computer GetComputer(Guid id)
        {
            return computers.Where(computer => computer.Id == id).SingleOrDefault();
        }

        public void AddComputer(Computer computer)
        {
            computers.Add(computer);
        }

        public void UpdateComputer(Computer computer)
        {
            computers[computers.FindIndex(c => c.Id == computer.Id)] = computer;
        }

        public void DeleteComputer(Guid id)
        {
            computers.RemoveAt(computers.FindIndex(c => c.Id == id));
        }
    }
}