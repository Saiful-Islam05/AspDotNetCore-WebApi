using Microsoft.AspNetCore.Mvc;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusCodeController : ControllerBase
    {
        //Dummy Database
        private static List<Student> _students = new List<Student>
        {
            new Student { Id = 1,Name = "Rahim", Age = 20, City = "Dhaka"},
            new Student { Id = 2, Name = "Karim", Age = 22, City = "Chittagong" },
            new Student { Id = 3, Name = "Salam", Age = 24, City = "Chandpur" }
        };


        // =====================================================
        // ✅ 200 — OK
        // সফলভাবে data পাওয়া গেছে
        // URL: GET /api/statuscode/200-demo
        // =====================================================
        [HttpGet("200-demo")]
        public IActionResult Demo200()
        {
            return Ok(
                new {
                    StatusCode = 200,
                    Message = "✅ সফল! সব student পাওয়া গেছে।",
                    Data = _students
                });
        }


        // =====================================================
        // ✅ 201 — Created
        // নতুন কিছু তৈরি হয়েছে
        // URL: POST /api/statuscode/201-demo
        // =====================================================
        [HttpPost("201-demo")]
        public IActionResult Demo201([FromBody] StudentCreateDTO createDTO)
        {
            var newStudent = new Student
            {
                Id = _students.Max(s => s.Id) + 1,
                Name = createDTO.Name,
                Age = createDTO.Age,
                City = createDTO.City,
            };

            _students.Add(newStudent);

            // ✅ 201 — Location header সহ
            return CreatedAtAction(
                nameof(Demo200),
                new { id = newStudent.Id },
                new
                {
                    StatusCode = 201,
                    Message = "✅ নতুন student তৈরি হয়েছে!",
                    Data = newStudent
                }
                );
        }

        // =====================================================
        // ✅ 204 — No Content
        // সফল কিন্তু return করার কিছু নেই
        // URL: DELETE /api/statuscode/204-demo/1
        // =====================================================
        [HttpDelete("204-demo/{id}")]
        public IActionResult Demo204(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound(new { Message = $"ID {id} not Found" });
            }

            _students.Remove(student);

            return NoContent();  // 204 - No body, only Success
        }


        // =====================================================
        // ❌ 400 — Bad Request
        // Client ভুল data পাঠিয়েছে
        // URL: POST /api/statuscode/400-demo
        // =====================================================
        [HttpPost("400-Demo")]
        public IActionResult Demo400([FromBody] StudentCreateDTO createDTO)
        {
            // Manual validation Check
            if (string.IsNullOrEmpty(createDTO.Name))
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "❌ Bad Request! নাম দিতে হবে।",
                    Field = "Name",
                    YourInput = createDTO.Name
                });
            }

            if (createDTO.Age < 1 || createDTO.Age > 100)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "❌ Bad Request! বয়স 1-100 এর মধ্যে হতে হবে।",
                    Field = "Age",
                    YourInput = createDTO.Age
                });
            }

            return Ok(new { Message = "✅ Data সঠিক আছে!", Data = createDTO });
        }


        // =====================================================
        // ❌ 401 — Unauthorized
        // Login করা নেই
        // URL: GET /api/statuscode/401-demo
        // =====================================================
        [HttpGet("401-Demo")]
        public IActionResult Demo401([FromHeader(Name ="X-Auth-Token")]string? token)
        {
            // Token check করছি
            if(string.IsNullOrEmpty(token))
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "❌ Unauthorized! Login করো।",
                    Hint = "Header এ X-Auth-Token দাও"
                });
            }

            if(token!= "mySecretToken123")
            {
                return Unauthorized(new
                {
                    StatusCode = 401,
                    Message = "❌ Token ভুল! আবার Login করো।"
                });
            }

            return Ok(new
            {
                StatusCode = 200,
                Message = "✅ Welcome! তুমি authorized।"
            });
        }

        // =====================================================
        // ❌ 403 — Forbidden
        // Login আছে কিন্তু Permission নেই
        // URL: GET /api/statuscode/403-demo
        // =====================================================

        [HttpGet("403-demo")]
        public IActionResult Demo403([FromHeader(Name ="X-User-Role")] string? role)
        {
            if(string.IsNullOrEmpty(role))
            {
                return BadRequest(new { Message = "Header a X-User-Role Daw!" });
            }

            if(role.ToLower()!="admin")
            {
                return StatusCode(403, new
                {
                    StatusCode = 403,
                    Message = "Forbidden! You have no permission",
                    YourRole = role,
                    Required = "Admin"
                });
            }

            return Ok(new
            {
                StatusCode = 200,
                Message = "Welcome to Admin area",
                SecretData = "Only admin can see it"
            });
        }


        // =====================================================
        // ❌ 404 — Not Found
        // খুঁজে পাওয়া যায়নি
        // URL: GET /api/statuscode/404-demo/99
        // =====================================================

        [HttpGet("404-demo/{id}")]
        public IActionResult Demo404(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if(student==null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    Message = $"X Not Found! Nothing of student {id} ",
                    SearchedId = id,
                    Hint = "Try by 1,2,or 3"
                });
            }

            return Ok(new
            {
                StatusCode = 200,
                Message = "Student Found",
                Data = student
            });
        }


        // =====================================================
        // ❌ 500 — Internal Server Error
        // Server এ সমস্যা হয়েছে
        // URL: GET /api/statuscode/500-demo
        // =====================================================

        [HttpGet("500-demo")]
        public IActionResult Demo500([FromQuery] bool triggerError = false)
        {
            try
            { 
                if(triggerError)
                {
                    throw new Exception("Database connection Failed!");
                }

                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Server is Ok"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    Message = "Server Error! Something is wrong",
                    ErrorDetails = ex.Message
                });
            }
        }


        // =====================================================
        // 🔵 সব Status Code একসাথে দেখো
        // URL: GET /api/statuscode/all-codes
        // =====================================================

        [HttpGet("all-codes")]
        public IActionResult AllStatusCodes()
        {
            return Ok(new
            {
                Sucess = new
                {
                    _200 = "Ok - Success! Data founded",
                    _201 = "Created- Something new Created",
                    _204 = "No Content - Succeess but no data"
                },

                ClientError = new
                {
                    _400 = "Bad Request - Wrong data sent",
                    _401 = "Unauthorized - Do Login ",
                    _403 = "Forbidden - No Permission",
                    _404 = "Not Foun - Didn't find"
                },

                ServerError = new
                {
                    _500 = "Internal Server Error - Problem is in Server",
                    _503 = "Service Unavailable - Server is now Busy"
                }

            });
        }
    }
}
