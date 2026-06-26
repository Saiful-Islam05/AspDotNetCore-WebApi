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
    }
}
