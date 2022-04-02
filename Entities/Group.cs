using System;

namespace RaFilDaAPI.Entities
{
    public record Group
    {
        public int Id { get; init; }
        public string Name { get; set; }
    }
}