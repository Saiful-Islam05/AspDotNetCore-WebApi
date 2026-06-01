using Microsoft.AspNetCore.Mvc;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]  // Base route for the controller, e.g., api/Student
    [ApiController]
    public class StudentController : ControllerBase
    {
        // Type 1: Simple Route
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

        // Type 2: Route with Parameter
        // Get: api/Student/1
        [HttpGet("{id}")]
        public IActionResult GetStudentById(int id)
        {
            var student = new { Id = id, Name = "Rahim", Age = 20, City = "Dhaka" };
            return Ok(student);
        }


        // Type 3: Query Parameter
        // URL: GET/api/Student/search?name=Rahim & age=20
        [HttpGet("search")]
        public IActionResult SearchStudent(
            [FromQuery] string name,
            [FromQuery] int age)
        {
            return Ok(new
            {
                Message = $"'{name}' name er {age} bochor boyos searched",
                SearchedName = name,
                SearchedAge = age
            });
        }

        // Type 4: Custom Route Name
        // URL: GET/api/Student/top-students
        [HttpGet("top-students")]  // Give custom name
        public IActionResult GetTopStudents()
        {
            var top = new List<object>
            {
                new { Id = 1, Name = "Rahim", GPA = 3.9 },
                new { Id = 2, Name = "Karim", GPA = 3.8 }
            };
            return Ok(top);
        }

        // Type 5: Route Constraint
        // URL: GET/api/Student/details/5
        // This route will only match if the id is an integer
        [HttpGet("details/{id:int}")]  // Route constraint for integer id
        public IActionResult GetDetails(int id)
        {
            return Ok(new
                {
                    Id = id,
                    Message =  $"Student {id} details retrieved successfully!"
            });
        }


        // Type 6: Multiple Parameters in Route
        // URL: GET/api/Student/class/10/section/A
        [HttpGet("class/{classNumber}/section/{section}")]
        public IActionResult GetByClassAndSection(string className, string sectionName)
        {
            return Ok(new
            {
                Class = className,
                Section = sectionName,
                Message = $"Class {className}, Section {sectionName} er Students"
            });
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
