using Microsoft.AspNetCore.Mvc;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : Controller
    {
        // Get: api/Student
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            var students = new List<object>
            {
                new { Id = 1, Name = "Rahim", Age = 20 },
                new { Id = 2, Name = "Karim", Age = 22 },
                new { Id = 3, Name = "Jabbar", Age = 21 },
                new { Id = 4, Name = "Shafiq", Age = 23 }

            };

            return Ok(students);  // 200 Status Code with the list of students
        }

        // Get: api/Student/1
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = new { Id = id, Name = "Rahim", Age = 20, City = "Dhaka" };
            return Ok(student);
        }
    }
}
