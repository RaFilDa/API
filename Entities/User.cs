using System;

namespace RaFilDaAPI.Entities
{
    public record User
    {
        public int Id { get; init; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}