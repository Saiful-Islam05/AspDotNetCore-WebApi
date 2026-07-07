using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Data;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EFStudentController : ControllerBase
    {
        // ✅ DbContext inject করছি — Fake List নয়!
        private readonly AppDbContext _context;

        public EFStudentController(AppDbContext context)
        {
            _context = context;  // DI (Dependency Injection) এর মাধ্যমে আসছে
        }

        // =====================================================
        // ✅ GET — Database থেকে সব Student আনো
        // URL: GET /api/efstudent
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            // ✅ Database থেকে সব student আনো
            var students = await _context.Students.ToListAsync();
            return Ok(students);
        }


        // =====================================================
        // ✅ GET by ID — Database থেকে একজন Student আনো
        // URL: GET /api/efstudent/1
        // =====================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            // ✅ ID দিয়ে Database থেকে খোঁজো
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound(new { Message = $"ID {id} পাওয়া যায়নি" });

            return Ok(student);
        }


        // =====================================================
        // ✅ POST — Database তে নতুন Student যোগ করো
        // URL: POST /api/efstudent
        // =====================================================
        [HttpPost]
        public async Task<IActionResult> CreateStudent(
           [FromBody] StudentCreateDTO createDTO)
        {
            // DTO → Student convert
            var newStudent = new Student
            {
                Name = createDTO.Name,
                Age = createDTO.Age,
                City = createDTO.City,
                Password = "defaultPass",
                BankAccount = "BD000"
            };

            // ✅ Database তে Add করো
            _context.Students.Add(newStudent);

            // ✅ Save করো — এটা ছাড়া DB তে যাবে না!
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetStudentById),
                new { id = newStudent.Id },
                newStudent
            );
        }
    }
}
