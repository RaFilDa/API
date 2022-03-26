using System.ComponentModel.DataAnnotations;

namespace RaFilDaAPI.DTOs
{
    public record AddComputerDTO
    {
        [Required]
        public string Name { get; init; }
        [Required]
        public string MAC { get; init; }
        [Required]
        public string IP { get; init; }
    }
}