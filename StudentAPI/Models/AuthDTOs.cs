namespace StudentAPI.Models
{
    public class RegisterDTO
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; } = "Student";
    }

    public class LoginDTO
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}