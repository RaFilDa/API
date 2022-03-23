using System.Collections.Generic;
using RaFilDaAPI.Entities;
using System;
using System.Linq;

namespace RaFilDaAPI.Repositories
{
    public class InMemoryComputersRepository
    {
        private readonly List<Computer> computers = new()
        {
            new Computer { Id = 1, Name = "TestComputer_1", MAC = "2C:54:91:88:C9:E3", IP = "69.89.31.226", LastSeen = DateTimeOffset.UtcNow },
            new Computer { Id = 2, Name = "TestComputer_2", MAC = "2C:54:91:88:C9:E3", IP = "69.89.31.226", LastSeen = DateTimeOffset.UtcNow },
            new Computer { Id = 3, Name = "TestComputer_3", MAC = "2C:54:91:88:C9:E3", IP = "69.89.31.226", LastSeen = DateTimeOffset.UtcNow }
        };

        public IEnumerable<Computer> GetComputers()
        {
            return computers;
        }

        public Computer GetComputer(int id)
        {
            return computers.Where(computer => computer.Id == id).SingleOrDefault();
        }
    }
}