using System;

namespace RaFilDaAPI.Entities
{
    public record Source
    {
        public int Id { get; init; }
        public int ConfigID { get; set; }
        public string Path { get; set; }
    }
}