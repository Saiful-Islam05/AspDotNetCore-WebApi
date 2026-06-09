using Microsoft.AspNetCore.Mvc;
using StudentAPI.Models;  // Import the Student model (if needed for future use)

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelBindingController : Controller
    {
        //Dummy data for demonstration
        private static List<Student> _students = new List<Student>
        {
            new Student {Id = 1, Name = "Rahim", Age = 20, City = "Dhaka"},
            new Student {Id = 2, Name = "Karim", Age = 22, City = "Chittagong"},
            new Student {Id = 3, Name = "Salam", Age = 21, City = "Khulna"},
            new Student {Id = 4, Name = "Jamal", Age = 23, City = "Rajshahi"}
        };

        // =====================================================
        // 🔵 TYPE 1 — [FromRoute]
        // URL এর ভেতর থেকে data নেওয়া
        // URL: GET /api/modelbinding/route/3
        // =====================================================

        [HttpGet("route/{id}")]
        public IActionResult FromRouteExample([FromRoute] int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if(student == null)
            {
                return NotFound(new { Message = $"ID {id} not Found" });
            }

            return Ok(new
            {
                Source = "FromRoute",
                GotId = id,
                Student = student
            });
        }

        // =====================================================
        // 🟢 TYPE 2 — [FromQuery]
        // URL এর ? এর পরে থেকে data নেওয়া
        // URL: GET /api/modelbinding/query?name=rahim&age=20
        // =====================================================


        [HttpGet("query")]
        public IActionResult FromQueryExample(
            [FromQuery] string? name,
            [FromQuery] int? age,
            [FromQuery] string? city)
            {
            // যে filter দেওয়া আছে সেটা apply করো

            var result = _students.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                result = result.Where(s => s.Name.ToLower().Contains(name.ToLower()));

            if (age.HasValue)
                result = result.Where(s => s.Age == age.Value);

            if (!string.IsNullOrEmpty(city))
                result = result.Where(s => s.City.ToLower().Contains(city.ToLower()));

            return Ok(new
            {
                Source = "FromQuery",
                FilterUsed = new {name,age,city },
                Result = result.ToList()
            });

            }

        // =====================================================
        // 🟡 TYPE 3 — [FromBody]
        // Request এর JSON body থেকে data নেওয়া
        // URL: POST /api/modelbinding/body
        // =====================================================

        [HttpPost("body")]
        public IActionResult FromBodyExample(
            [FromBody] StudentRequest request)  // From JSON body
        {
            // Create new Student
            var newStudent = new Student
            {
                Id = _students.Max(s => s.Id) + 1,
                Name = request.Name,
                Age = request.Age,
                City = request.City
            };

            _students.Add(newStudent);

            return Ok(new
            {
                Source = "FromBody",
                GotData = request,
                NewStudent = newStudent
            });
        }

        // =====================================================
        // 🟣 TYPE 4 — [FromHeader]
        // Request এর Header থেকে data নেওয়া
        // URL: GET /api/modelbinding/header
        // Swagger এ Headers section এ দিতে হবে
        // =====================================================

        [HttpGet("header")]
        public IActionResult FromHeaderExample(

            [FromHeader(Name ="X-Student-Name")] string? studentName,
            [FromHeader(Name = "X-Student-City")] string? studentCity)
        {
            if(string.IsNullOrEmpty(studentName))
                return BadRequest(new { Message = "X-Student-Name header is required" });

            return Ok(new
            {
                Source = "FromHeader",
                GotName = studentName,
                GotCity = studentCity,
                Message = $"Header received: Name = {studentName}, City = {studentCity}"
            });
        }

        // =====================================================
        // 🔴 TYPE 5 — Mixed Binding
        // একসাথে Route + Query + Body থেকে data নেওয়া
        // URL: PUT /api/modelbinding/mixed/2?notify=true
        // =====================================================

        [HttpPut("mixed/{id}")]
        public IActionResult MixedBindingExample(
            [FromRoute] int id,               // From URL route
            [FromQuery] bool notify,          //?notify=true থেকে
            [FromBody] StudentRequest updatedData)  // From JSON body
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return NotFound(new { Message = $"ID {id} not Found" });
                    
                    //Do Update ;

                    student.Name = updatedData.Name;
                    student.Age = updatedData.Age;
                    student.City = updatedData.City;

            return Ok(new
            {
                Source = "Mixed Binding",
                FromRoute_id = id,
                FromQuery_notify = notify,
                FromBody_Data = updatedData,
                UpdatedStudnts = student,
                Message = notify? $"{student.Name} updated and notification sent!" : $"{student.Name} updated without notification."
            });

        }
    }
}
