using System.Collections.Generic;
using RaFilDaAPI.Entities;

namespace RaFilDaAPI.Repositories
{
    public interface IComputersRepository
    {
        Computer GetComputer(int id);
        IEnumerable<Computer> GetComputers();
    }
}