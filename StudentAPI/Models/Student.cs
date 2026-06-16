using System.Security.Principal;

namespace StudentAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string City { get; set; }

        // Sensitive fields- we dont' give client
        public string Password { get; set; } = "secret123";
        public string BankAccount { get; set; } = "BD123456789";
    }
}
