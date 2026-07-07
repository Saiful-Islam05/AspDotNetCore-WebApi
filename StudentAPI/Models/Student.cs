using System.Security.Principal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAPI.Models
{
    // এই class টা Database এর "Students" Table হবে
    [Table("Students")]  // Table এর নাম define করছি
    public class Student
    {

        // ✅ Primary Key — Auto Increment
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // ✅ Required + Max 100 character
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        // ✅ 1 থেকে 100 এর মধ্যে
        [Range(1, 100)]
        public int Age { get; set; }

        // ✅ Required + Max 100 character
        [Required]
        [MaxLength(100)]
        public string? City { get; set; }

        // Sensitive fields — Client দেখবে না
        [MaxLength(200)]
        public string? Password { get; set; }

        [MaxLength(50)]
        public string? BankAccount { get; set; }
    }
        
}
