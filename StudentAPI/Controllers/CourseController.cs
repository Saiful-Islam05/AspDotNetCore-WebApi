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

    }
}
