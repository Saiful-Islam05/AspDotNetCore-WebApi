using System.ComponentModel.DataAnnotations;

namespace StudentAPI.Models
{
    // =====================================================
    // ✅ Response DTO — GET এ use হবে (validation নেই)
    // =====================================================
    public class StudentResponseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public string? City { get; set; }
        public string? Email { get; set; } //New
        public string? Phone { get; set; } //New
        public DateTime CreatedAt { get; set; } //New
    }

    // =====================================================
    // ✅ Create DTO — POST এ use হবে (validation আছে)
    // =====================================================

    public class StudentCreateDTO
    {
        [Required(ErrorMessage ="Name have to give")]
        [StringLength(50,MinimumLength =2, ErrorMessage ="Name should 2 to 50 character")]
        public string? Name { get; set; }

        [Range(1, 100, ErrorMessage = "Age should have from 1 to 100 years")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Give City name!")]
        public string? City { get; set; }

        [EmailAddress(ErrorMessage = "Give valid email address")]
        public string? EmailAddress { get; set; }


        [Phone(ErrorMessage = "Give right phone number")]
        public string? Phone { get; set; }
        public string Email { get; internal set; }
    }

    // =====================================================
    // ✅ Update DTO — PUT এ use হবে (validation আছে)
    // =====================================================

    public class  StudentUpdateDTO
    {
       [Required(ErrorMessage ="Name have to give!")]
       [StringLength(50,MinimumLength =2,ErrorMessage ="Name should from 2 to 50 character!")]
       public string? Name { get; set; }

        [Range(1, 100, ErrorMessage = "Age should from 1 to 100 years!")]
        public int Age { get; set; }

        [Required(ErrorMessage = "City name have to give!")]
        public string? City { get; set; }

        [EmailAddress(ErrorMessage = "সঠিক Email দাও!")]
        public string? Email { get; set; }      // নতুন ✅

        [Phone(ErrorMessage = "সঠিক Phone দাও!")]
        public string? Phone { get; set; }      // নতুন ✅

    }



    // =====================================================
    // Course DTOs
    // =====================================================

    // ✅ Response DTO — GET এ use হবে (validation নেই)
    public class  CourseResponseDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int CreditHours { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ✅ Create DTO — POST এ use হবে (validation আছে)
    public class CourseCreateDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title should be between 2 and 100 characters.")]
        public string? Title { get; set; }


        [StringLength(500, ErrorMessage = "Description should not exceed 500 characters.")]
        public string? Description { get; set; }


        [Range(1, 24, ErrorMessage = "Credit hours should be between 1 and 24.")]
        public int CreditHours { get; set; }
    }


    // ✅ Update DTO — PUT এ use হবে (validation আছে)

    public class CourseUpdateDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title should be between 2 and 100 characters.")]
        public string? Title { get; set; }


        [StringLength(500, ErrorMessage = "Description should not exceed 500 characters.")]
        public string? Description { get; set; }


        [Range(1, 24, ErrorMessage = "Credit hours should be between 1 and 24.")]
        public int CreditHours { get; set; }
    }
}
