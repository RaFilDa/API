using System;
using System.Collections.Generic;
using RaFilDaAPI.Entities;

namespace RaFilDaAPI.Repositories
{
    public interface IComputersRepository
    {
        Computer GetComputer(Guid id);
        IEnumerable<Computer> GetComputers();
        void AddComputer(Computer computer);
    }
}