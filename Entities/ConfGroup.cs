using System;

namespace RaFilDaAPI.Entities
{
    public record ConfGroup
    {
        public int Id { get; init; }
        public int ConfigID { get; set; }
        public int GroupID { get; set; }
    }
}