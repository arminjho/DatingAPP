namespace DatingWebApp.DTOs
{
    public class UserDto
    {
        public required string Username { get; set; }
        public required string KnownUs { get; set; }
        public required string Token { get; set; }
        public required string Gender { get; set; }
        public string ? PhotoUrl { get; set; }
    }
}
