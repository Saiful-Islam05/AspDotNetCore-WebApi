using System.Security.Principal;
using System.ComponentModel.DataAnnotations;

namespace StudentAPI.Models
{
    public class Student
    {
        public int Id { get; set; }


        [Required(ErrorMessage ="Name have to give must!")]
        [StringLength(50,MinimumLength =2,ErrorMessage ="Name should from 2 to 50 character!")]
        public string? Name { get; set; }


        [Range(1,100,ErrorMessage ="Age should from 1 to 100 years!")]
        public int Age { get; set; }


        [Required(ErrorMessage ="City name have to given!")]
        public string? City { get; set; }

        // Sensitive fields- we dont' give client
        public string Password { get; set; } = "secret123";
        public string BankAccount { get; set; } = "BD123456789";
    }
}
