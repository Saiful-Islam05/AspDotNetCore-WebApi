using Microsoft.AspNetCore.Mvc;
using StudentAPI.Models;  // Import the Student model (if needed for future use)

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentDTOController : Controller
    {
        // Fake Database - Include sensitive fields
        private static List<Student> _students = new List<Student>
        {
            new Student {Id = 1, Name = "Rahim", Age = 20, City = "Dhaka", Password="pass123",BankAccount="Bd111"},
            new Student {Id = 2, Name = "Karim", Age = 22, City = "Chittagong", Password="pass456",BankAccount="Bd222"},
            new Student {Id = 3, Name = "Salam", Age = 21, City = "Khulna", Password="pass789",BankAccount="Bd333"},
            new Student {Id = 4, Name = "Jamal", Age = 23, City = "Rajshahi", Password="pass012",BankAccount="Bd444"}
        };

        // =====================================================
        // ❌ WITHOUT DTO — সমস্যা দেখো
        // URL: GET /api/studentdto/without-dto
        // Password, BankAccount সব দেখা যাচ্ছে!
        // =====================================================
        [HttpGet("without-dto")]
        public IActionResult GetWithoutDTO()
        {
            // ⚠️ সরাসরি Student return করছি
            // Password, BankAccount সব client এ যাচ্ছে!
            return Ok(_students);
        }

        // =====================================================
        // ✅ WITH DTO — সমস্যার সমাধান
        // URL: GET /api/studentdto/with-dto
        // Password, BankAccount hidden ✅
        // =====================================================
        [HttpGet("with-dto")]
        public ActionResult<List<StudentResponseDTO>> GetWithDTO()
        {
            // Student থেকে StudentResponseDTO তে Convert করছি
            var studentDTOs = _students.Select(s => new StudentResponseDTO
            {
                Id = s.Id,
                Name = s.Name,
                Age = s.Age,
                City = s.City
                // Password, BankAccount নেই!
            }).ToList();
            return Ok(studentDTOs);
        }


        // =====================================================
        // ✅ GET by ID with DTO
        // URL: GET /api/studentdto/5
        // =====================================================
        [HttpGet("{id}")]
        public ActionResult<StudentResponseDTO> GetByIdWithDTO(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound(new { Message = $"ID {id} not Found" });
            // Student থেকে StudentResponseDTO তে Convert করছি
            var dto = new StudentResponseDTO
            {
                Id = student.Id,
                Name = student.Name,
                Age = student.Age,
                City = student.City

            };
            return Ok(dto);
        }


        // =====================================================
        // ✅ POST with CreateDTO
        // Client শুধু Name, Age, City পাঠাবে
        // Id server নিজে দেবে
        // URL: POST /api/studentdto
        // =====================================================
        [HttpPost]
        public ActionResult<StudentResponseDTO> CreateWithDTO([FromBody] StudentCreateDTO createDTO)
        {
            // DTO → Student (Database Model) এ convert করো
            var newStudent = new Student
            {
                Id = _students.Max(s => s.Id) + 1, // Auto-increment ID
                Name = createDTO.Name,
                Age = createDTO.Age,
                City = createDTO.City,
                Password = "defaultPass", // Server will handle this, client doesn't know about it
                BankAccount = "BD000"  // Server will handle this, client doesn't know about it
            };
            _students.Add(newStudent);

            // Response এ DTO পাঠাও — sensitive data নয়

            var responseDTO = new StudentResponseDTO
            {
                Id = newStudent.Id,
                Name = newStudent.Name,
                Age = newStudent.Age,
                City = newStudent.City
            };
            return CreatedAtAction(nameof(GetByIdWithDTO), new { id = newStudent.Id }, responseDTO); // DTO return করছি, Student নয়
        }

    }
}
