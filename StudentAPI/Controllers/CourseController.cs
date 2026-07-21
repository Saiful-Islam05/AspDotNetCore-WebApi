using Microsoft.AspNetCore.Mvc;
using StudentAPI.Repositories;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : Controller
    {
        private readonly ICourseRepository _repository;

        public CourseController(ICourseRepository repository)
        {
            _repository = repository;
        }

        // =====================================================
        // ✅ GET ALL — সব Course আনো
        // URL: GET /api/course
        // =====================================================

        [HttpGet]
        public async Task<ActionResult<List<CourseResponseDTO>>> GetAllCourses()
        {
            var courses = await _repository.GetAllAsync();

            // Course → CourseResponseDTO convert
            var response = courses.Select(c => new CourseResponseDTO
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CreditHours = c.CreditHours,
                CreatedAt = c.CreatedAt
            }).ToList();

            return Ok(response);
        }


        // =====================================================
        // ✅ GET BY ID — একটা Course আনো
        // URL: GET /api/course/1
        // =====================================================

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseResponseDTO>> GetCourseById(int id)
        {
            var course = await _repository.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound(new { Message = $"ID {id}  Course not found" });
            }

            var response = new CourseResponseDTO
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                CreditHours = course.CreditHours,
                CreatedAt = course.CreatedAt
            };

            return Ok(response);
        }


        // =====================================================
        // ✅ SEARCH — Title দিয়ে Course খোঁজো
        // URL: GET /api/course/search?title=csharp
        // =====================================================
        [HttpGet("search")]
        public async Task<ActionResult<List<CourseResponseDTO>>> SearchCourses([FromQuery] string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return BadRequest(new { Message = "Title query parameter is required." });
            }


            var courses = await _repository.SearchByTitleAsync(title);

            if (courses.Count == 0)
                return NotFound(new { Message = $"'{title}' by this not find any course" });

            // Course → CourseResponseDTO convert
            var response = courses.Select(c => new CourseResponseDTO
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CreditHours = c.CreditHours,
                CreatedAt = c.CreatedAt
            }).ToList();
            return Ok(response);
        }



        // =====================================================
        // ✅ POST — নতুন Course তৈরি
        // URL: POST /api/course
        // =====================================================
        [HttpPost]
        public async Task<ActionResult<CourseResponseDTO>> CreateCourse(
            [FromBody] CourseCreateDTO createDto)
        {
            // DTO → Course convert
            var course = new Course
            {
                Title = createDto.Title,
                Description = createDto.Description,
                CreditHours = createDto.CreditHours,
                CreatedAt = DateTime.Now
            };

            var created = await _repository.CreateAsync(course);

            var response = new CourseResponseDTO
            {
                Id = created.Id,
                Title = created.Title,
                Description = created.Description,
                CreditHours = created.CreditHours,
                CreatedAt = created.CreatedAt
            };

            return CreatedAtAction(nameof(GetCourseById), new { id = created.Id }, response);
        }






        // =====================================================
        // ✅ PUT — Course Update করো
        // URL: PUT /api/course/1
        // =====================================================

        [HttpPut("{id}")]
        public async Task<ActionResult<CourseResponseDTO>> UpdateCourse(int id, [FromBody] CourseUpdateDTO updateDto)
        {
            var course = new Course
            {
                Title = updateDto.Title,
                Description = updateDto.Description,
                CreditHours = updateDto.CreditHours
            };
            if (course == null)
            {
                return NotFound(new { Message = $"ID {id} Course not found" });
            }


            var updated = await _repository.UpdateAsync(id, course);

            if (updated == null)
            {
                return NotFound(new { Message = $"ID {id} Course not found" });
            }

            var response = new CourseResponseDTO
            {
                Id = updated.Id,
                Title = updated.Title,
                Description = updated.Description,
                CreditHours = updated.CreditHours,
                CreatedAt = updated.CreatedAt
            };

            return Ok(response);
        }




        // =====================================================
        // ✅ DELETE — Course মুছে ফেলো
        // URL: DELETE /api/course/1
        // =====================================================

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
            {
                return NotFound(new { Message = $"ID {id} Course not found" });
            }
            return Ok(new { Message = $"ID {id} Course deleted successfully" });
        }
    }
}
