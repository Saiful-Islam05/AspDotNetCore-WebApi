using Microsoft.AspNetCore.Mvc;
using StudentAPI.Models;  // Import the Student model (if needed for future use)

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]  // Base route for the controller, e.g., api/Student
    [ApiController]
    public class StudentController : ControllerBase
    {
        // Fake Database of students (for demonstration purposes)
        private static List<Student> _students = new List<Student>
        {
            new Student {Id = 1, Name = "Rahim", Age = 20, City = "Dhaka"},
            new Student {Id = 2, Name = "Karim", Age = 22, City = "Chittagong"},
            new Student {Id = 3, Name = "Salam", Age = 21, City = "Khulna"},
            new Student {Id = 4, Name = "Jamal", Age = 23, City = "Rajshahi"}
        };


        // 🔵 GET — Data পড়া (কোনো change করে না)
        // URL: GET/api/Student
        // কাজ: সব student এর list দেখাও
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            return Ok(_students);
        }

        // URL: GET /api/student/2
        // কাজ: শুধু একজন student দেখাও
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound($"Student with ID {id} not found.");
            return Ok(student);
        }

        // 🟢 POST — নতুন data তৈরি করা
      
        // URL: POST /api/student
        // কাজ: নতুন student যোগ করো
        [HttpPost]
        public IActionResult CreateStudent([FromBody] Student newStudent)
        {
            // ✅ নতুন ID দাও (last id + 1)
            newStudent.Id = _students.Max(s=>s.Id)+1; // Auto-increment ID

            // ✅ নতুন student কে list এ যোগ করো
            _students.Add(newStudent);

            // ✅ 201 Created + নতুন student return
            return CreatedAtAction(
                nameof(GetStudentById),
                new { id = newStudent.Id },
                newStudent
                );
        }

        // 🟡 PUT — পুরো data update করা
        // URL: PUT /api/student/2
        // কাজ: ID 2 এর student এর সব তথ্য বদলাও

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] Student updatedStudent)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if(student == null)
            {
                return NotFound(new {Message=$"Id {id} not found" });
            }

            // ✅ student এর সব তথ্য update করো
            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.City = updatedStudent.City;

            return Ok(student); // 200 + Updated student
        }


        // 🔴 DELETE — data মুছে ফেলা

        // URL: DELETE /api/student/2
        // কাজ: ID 2 এর student মুছে ফেলো
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if(student ==null)
            {
                return NotFound(new {Message=$"Id {id} Not found" }); //404 + error message
            }

            _students.Remove(student); // student মুছে ফেলো

            return Ok(new {Message=$"'{student.Name}' deleted successfully." }); // 200 + success message
        }
    }
}
