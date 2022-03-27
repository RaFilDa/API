using System;

namespace RaFilDaAPI.Entities
{
    public record Computer
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string MAC { get; set; }
        public string IP { get; set; }
        public DateTimeOffset LastSeen { get; init; }
    }
}