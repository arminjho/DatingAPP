using System.ComponentModel.DataAnnotations;

namespace DatingWebApp.DTOs
{
    public class RegisterDto
    {
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
        public required string KnownAs { get; set; }
        public required string Gender { get; set; }
    }
}
