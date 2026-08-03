using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Data;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LinqController : ControllerBase
    {
        private readonly AppDbContext _context;
      public LinqController(AppDbContext context)
      {
              _context = context;
      }


        // =====================================================
        // 🔵 LINQ 1 — WHERE (Filter করা)
        // URL: GET /api/linq/by-city?city=Dhaka
        // =====================================================

        [HttpGet("by-city")]
        public async Task<IActionResult> GetByCity([FromQuery] string city)
        {
            var students = await _context.Students
                .Where(s => s.City == city)  // Filter by city
                .ToListAsync();

            if(students.Count == 0)
            {
                return NotFound(new { Message = $"No students found in city: {city}" });
            }

            return Ok(new
            {
                City = city,
                Count = students.Count,
                Students = students
            });
        }


        // =====================================================
        // 🟢 LINQ 2 — ORDERBY (Sort করা)
        // URL: GET /api/linq/sorted?sortBy=name
        // =====================================================

        [HttpGet("sorted")]
        public async Task<IActionResult> GetSorted([FromQuery] string sortBy = "name")
        {
            // sortBy এর value দেখে sort করো
            var query = _context.Students.AsQueryable();

            query = sortBy.ToLower() switch
            {
                "name" => query.OrderBy(s => s.Name),    //A-Z
                "age" => query.OrderBy(s => s.Age),     // Small->Big
                "city" => query.OrderBy(s => s.City),  // City A-Z
                "age_desc" => query.OrderByDescending(s => s.Age),   // Big->Small
                _ => query.OrderBy(s => s.Id) // default sort by name
            };

            var students = await query.ToListAsync();

            return Ok(new
            {
                SortedBy = sortBy,
                Students = students
            });
        }




    }
}
