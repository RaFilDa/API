using RaFilDaAPI.DTOs;
using RaFilDaAPI.Entities;

namespace RaFilDaAPI
{
    public static class Extensions{
        public static ComputerDTO AsDTO(this Computer computer)
        {
            return new ComputerDTO{
                Id = computer.Id,
                Name = computer.Name,
                MAC = computer.MAC,
                IP = computer.IP,
                LastSeen = computer.LastSeen
            };
        }
    }
}