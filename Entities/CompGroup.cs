using System;

namespace RaFilDaAPI.Entities
{
    public record CompGroup
    {
        public int Id { get; init; }
        public int CompID { get; set; }
        public int GroupID { get; set; }
    }
}