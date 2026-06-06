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


        // =====================================================
        // 🔵 TYPE 1 — IActionResult (যেকোনো কিছু return করে)
        // সমস্যা: Swagger বুঝতে পারে না কী আসবে
        // URL: GET /api/student/iaction/2
        // =====================================================
        [HttpGet("iaction/{id}")]
        public IActionResult GetWithIActionResult(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if(student==null)
            {
                return NotFound(new { Message = $"Id {id} not Found" });
            }

            return Ok(student);
            // Swagger know something will come but it doesn't know what type of data will come
        }


        // =====================================================
        // 🟢 TYPE 2 — ActionResult<T> (Specific type বলা আছে)
        // সুবিধা: Swagger জানে Student object আসবে ✅
        // URL: GET /api/student/action/2
        // =====================================================

        [HttpGet("action/{id}")]
        public ActionResult<Student> GetWithActionResult(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if(student == null)
            {
                return NotFound(new {Message=$"ID {id} not Found"});
            }


            return Ok(student);
            // Swagger know Student Object will come
        }

        // =====================================================
        // 🟡 TYPE 3 — ActionResult<List<T>> (List return)
        // URL: GET /api/student
        // =====================================================
        [HttpGet]
        public ActionResult<List<Student>> GetAllStudents()
        {
            if(_students.Count == 0)
            {
                return NotFound(new {Message="No Student Found"});
            }

            return Ok(_students);
        }

        // =====================================================
        // 🟣 TYPE 4 — সব Return Types দেখি
        // URL: GET /api/student/status/{code}
        // =====================================================
        [HttpGet("status/{code}")]
        public IActionResult GetByStatusCode(int code)
        {
            return code switch
            {
                200 => Ok(new { Message = "Success" }),
                201 => Created("", new { Message = "201- Created! something new create" }),
                204 => NoContent(), // No content return
                400 => BadRequest(new { Message = "00 — Bad Request! তুমি ভুল data পাঠিয়েছো ❌" }),
                401 => Unauthorized(new { Message = "401 — Unauthorized! Login করো ❌" }),
                403 => Forbid(), // Permission denied
                404 => NotFound(new { Message = "404 — Not Found! খুঁজে পাওয়া যায়নি ❌" }),
                500 => StatusCode(500, new { Message = "500 — Internal Server Error! সার্ভারে সমস্যা ❌" }),
                _ => BadRequest(new { Message = "Invalid Status Code! তুমি ভুল code পাঠিয়েছো ❌" })
            };
        }

        // =====================================================
        // 🔴 TYPE 5 — NoContent (204)
        // Update বা Delete এর পর data না পাঠালে
        // URL: DELETE /api/student/nocontent/{id}
        // =====================================================

        [HttpDelete("nocontent/{id}")]
        public ActionResult DeleteWithNoContent(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if(student == null)
            {
                return NotFound(new { Message = $"ID {id} not Found" });
            }

            _students.Remove(student);

            return NoContent();  // 204 — সফল কিন্তু কোনো data নেই
        }

        // =====================================================
        // আগের methods গুলো ঠিকঠাক রাখো
        // =====================================================
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudentById(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return NotFound(new { Message = $"ID {id} এর student পাওয়া যায়নি" });

            return Ok(student);
        }

        [HttpPost]
        public ActionResult<Student> CreateStudent([FromBody] Student newStudent)
        {
            newStudent.Id = _students.Max(s => s.Id) + 1;
            _students.Add(newStudent);

            return CreatedAtAction(
                nameof(GetStudentById),
                new { id = newStudent.Id },
                newStudent
            );
        }

        [HttpPut("{id}")]
        public ActionResult<Student> UpdateStudent(int id, [FromBody] Student updatedStudent)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return NotFound(new { Message = $"ID {id} পাওয়া যায়নি" });

            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.City = updatedStudent.City;

            return Ok(student);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return NotFound(new { Message = $"ID {id} পাওয়া যায়নি" });

            _students.Remove(student);

            return Ok(new { Message = $"'{student.Name}' deleted successfully!" });
        }

    }
}
