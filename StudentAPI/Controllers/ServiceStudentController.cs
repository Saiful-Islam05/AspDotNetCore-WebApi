using Microsoft.AspNetCore.Mvc;
using StudentAPI.Models;
using StudentAPI.Services;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceStudentController : ControllerBase
    {
        // ✅ Repository নয় — Service inject করছি
        private readonly IStudentService _service;

        public ServiceStudentController(IStudentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _service.GetAllAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _service.GetByIdAsync(id);

            if (student == null)
                return NotFound(new { Message = $"ID {id} পাওয়া যায়নি" });

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] StudentCreateDTO createDTO)
        {
            var created = await _service.CreateAsync(createDTO);
            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                created
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] StudentUpdateDTO updateDTO)
        {
            var updated = await _service.UpdateAsync(id, updateDTO);

            if (updated == null)
                return NotFound(new { Message = $"ID {id} পাওয়া যায়নি" });

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (!result)
                return NotFound(new { Message = $"ID {id} পাওয়া যায়নি" });

            return Ok(new { Message = $"ID {id} deleted!" });
        }
    }
}