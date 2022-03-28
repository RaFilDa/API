using System;

namespace RaFilDaAPI.Entities
{
    public record Computer
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public string MAC { get; set; }
        public string IP { get; set; }
        public DateTime LastSeen { get; init; }
    }
}