using Microsoft.AspNetCore.Mvc;
using StudentAPI.Models;
using StudentAPI.Repositories;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepoStudentController : ControllerBase
    {
        // ✅ DbContext নয় — Repository inject করছি
        private readonly IStudentRepository _repository;

        public RepoStudentController(IStudentRepository repository)
        {
            _repository = repository;
        }


        // =====================================================
        // ✅ GET — সব Student
        // URL: GET /api/repostudent
        // =====================================================
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _repository.GetAllAsync();
            return Ok(students);
        }

        // =====================================================
        // ✅ GET BY ID
        // URL: GET /api/repostudent/1
        // =====================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _repository.GetByIdAsync(id);

            if (student == null)
                return NotFound(new { Message = $"ID {id} Not Found" });

            return Ok(student);
        }


        // =====================================================
        // ✅ POST — নতুন Student তৈরি
        // URL: POST /api/repostudent
        // =====================================================
        [HttpPost]
        public async Task<IActionResult> CreateStudent(
            [FromBody] StudentCreateDTO createDTO)
        {
            // DTO → Student convert
            var student = new Student
            {
                Name = createDTO.Name,
                Age = createDTO.Age,
                City = createDTO.City,
                Email = createDTO.Email,
                Phone = createDTO.Phone,
                CreatedAt = DateTime.Now,
                Password = "defaultPass",
                BankAccount = "BD000"
            };

            // Repository দিয়ে Create করো
            var created = await _repository.CreateAsync(student);

            // Response DTO বানাও
            var responseDTO = new StudentResponseDTO
            {
                Id = created.Id,
                Name = created.Name,
                Age = created.Age,
                City = created.City,
                Email = created.Email,
                Phone = created.Phone,
                CreatedAt = created.CreatedAt
            };

            return CreatedAtAction(
                nameof(GetStudentById),
                new { id = created.Id },
                responseDTO
            );
        }


        // =====================================================
        // ✅ PUT — Student Update
        // URL: PUT /api/repostudent/1
        // =====================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(
            int id,
            [FromBody] StudentUpdateDTO updateDTO)
        {
            // DTO → Student convert
            var student = new Student
            {
                Name = updateDTO.Name,
                Age = updateDTO.Age,
                City = updateDTO.City,
                Email = updateDTO.Email,
                Phone = updateDTO.Phone
            };

            // Repository দিয়ে Update করো
            var updated = await _repository.UpdateAsync(id, student);

            if (updated == null)
                return NotFound(new { Message = $"ID {id} Not Found" });

            // Response DTO বানাও
            var responseDTO = new StudentResponseDTO
            {
                Id = updated.Id,
                Name = updated.Name,
                Age = updated.Age,
                City = updated.City,
                Email = updated.Email,
                Phone = updated.Phone,
                CreatedAt = updated.CreatedAt
            };

            return Ok(responseDTO);
        }


        // =====================================================
        // ✅ DELETE — Student মুছে ফেলো
        // URL: DELETE /api/repostudent/1
        // =====================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await _repository.DeleteAsync(id);

            if (!result)
                return NotFound(new { Message = $"ID {id} Not Found" });

            return Ok(new { Message = $"ID {id} deleted successfully!" });
        }

    }
}
