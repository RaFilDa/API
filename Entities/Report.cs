using System;

namespace RaFilDaAPI.Entities
{
    public record Report
    {
        public int Id { get; init; }
        public int CompConfID { get; set; }
        public DateTime Date { get; init; } = DateTime.UtcNow;
        public string Type { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }
    }
}