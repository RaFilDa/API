using System;

namespace RaFilDaAPI.DTOs
{
    public record ComputerDTO
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string MAC { get; init; }
        public string IP { get; init; }
        public DateTimeOffset LastSeen { get; init; }
    }
}