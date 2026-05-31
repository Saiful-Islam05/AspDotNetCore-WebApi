using Microsoft.AspNetCore.Mvc;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
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

        // New - POST (student creation)
        [HttpPost]
        public IActionResult CreateStudent([FromBody] string name)
        {
            // In a real application, we will save the student from the database. so give fake response
            var newStudent = new { Id = 5, Name = name, Age = 20 };
            return Created("", newStudent); // 201 Status Code with the created student
        }

        // New - PUT (student update)
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] string newName)
        {
            var updated = new { Id = id, Name = newName, Message = "Updated Successfully!" };
            return Ok(updated); // 200 Status Code with the updated student
        }

        // New - DELETE (student deletion)
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            return Ok($"Student {id} deleted successfully!"); // 200 Status Code with a deletion message
        }
    }
}
