using System;

namespace RaFilDaAPI.Entities
{
    public record Computer
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string MAC { get; init; }
        public string IP { get; init; }
        public DateTimeOffset LastSeen { get; init; }
    }
}