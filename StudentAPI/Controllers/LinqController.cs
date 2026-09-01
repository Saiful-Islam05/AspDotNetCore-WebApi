using Microsoft.AspNetCore.Mvc;
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


        // =====================================================
        // 🟡 LINQ 3 — SELECT (নির্দিষ্ট field নেওয়া)
        // URL: GET /api/linq/select-fields
        // =====================================================
        [HttpGet("select-fields")]
        public async Task<IActionResult> GetSelectedFields()
        {
            // শুধু Id আর Name নাও — বাকি field দরকার নেই
            var students = await _context.Students
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.City
                    // Age, Password, BankAccount নেই
                })
                .ToListAsync();

            return Ok(students);
        }


        // =====================================================
        // 🟣 LINQ 4 — TAKE & SKIP (Pagination)
        // URL: GET /api/linq/paged?page=1&size=2
        // =====================================================
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int size = 2)
        {
            // মোট কতজন student আছে
            var total = await _context.Students.CountAsync();

            var students = await _context.Students
                .Skip((page - 1) * size)  // আগের page গুলো skip
                .Take(size)               // এই page এর data নাও
                .ToListAsync();

            return Ok(new
            {
                CurrentPage = page,
                PageSize = size,
                TotalStudents = total,
                TotalPages = (int)Math.Ceiling(
                                (double)total / size),
                Students = students
            });
        }


        // =====================================================
        // 🔴 LINQ 5 — FIRST, SINGLE, ANY, ALL
        // URL: GET /api/linq/aggregates
        // =====================================================
        [HttpGet("aggregates")]
        public async Task<IActionResult> GetAggregates()
        {
            // FirstOrDefault — প্রথম student
            var first = await _context.Students
                .FirstOrDefaultAsync();

            // Any — কোনো student কি Dhaka তে আছে?
            var anyInDhaka = await _context.Students
                .AnyAsync(s => s.City == "Dhaka");

            // All — সব student কি 18+ ?
            var allAdult = await _context.Students
                .AllAsync(s => s.Age >= 18);

            // Count — মোট কতজন
            var total = await _context.Students
                .CountAsync();

            // Max — সবচেয়ে বেশি বয়স
            var maxAge = await _context.Students
                .MaxAsync(s => s.Age);

            // Min — সবচেয়ে কম বয়স
            var minAge = await _context.Students
                .MinAsync(s => s.Age);

            // Average — গড় বয়স
            var avgAge = await _context.Students
                .AverageAsync(s => s.Age);

            return Ok(new
            {
                FirstStudent = first?.Name,
                AnyInDhaka = anyInDhaka,
                AllAdult = allAdult,
                TotalStudents = total,
                MaxAge = maxAge,
                MinAge = minAge,
                AverageAge = Math.Round(avgAge, 2)
            });
        }


        // =====================================================
        // 🔵 LINQ 6 — WHERE + ORDERBY + SELECT একসাথে
        // URL: GET /api/linq/combined?minAge=20&city=Dhaka
        // =====================================================
        [HttpGet("combined")]
        public async Task<IActionResult> GetCombined(
            [FromQuery] int minAge = 0,
            [FromQuery] string? city = null)
        {
            var query = _context.Students.AsQueryable();

            // Condition অনুযায়ী filter যোগ করো
            if (minAge > 0)
                query = query.Where(s => s.Age >= minAge);

            if (!string.IsNullOrEmpty(city))
                query = query.Where(s => s.City == city);

            var students = await query
                .OrderBy(s => s.Name)      // Name অনুযায়ী sort
                .Select(s => new           // শুধু দরকারি field
                {
                    s.Id,
                    s.Name,
                    s.Age,
                    s.City
                })
                .ToListAsync();

            return Ok(new
            {
                Filter = new { minAge, city },
                Count = students.Count,
                Students = students
            });
        }


        // =====================================================
        // 🟢 LINQ 7 — GROUPBY (Group করা)
        // URL: GET /api/linq/group-by-city
        // =====================================================
        [HttpGet("group-by-city")]
        public async Task<IActionResult> GetGroupByCity()
        {
            var grouped = await _context.Students
                .GroupBy(s => s.City)       // City অনুযায়ী group করো
                .Select(g => new
                {
                    City = g.Key,       // City এর নাম
                    Count = g.Count(),   // এই city তে কতজন
                    Students = g.Select(s => s.Name).ToList() // নামের list
                })
                .ToListAsync();

            return Ok(grouped);
        }


        // =====================================================
        // 🟡 LINQ 8 — SEARCH (Contains দিয়ে)
        // URL: GET /api/linq/search?keyword=ra
        // =====================================================
        [HttpGet("search")]
        public async Task<IActionResult> SearchStudents(
            [FromQuery] string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return BadRequest(new { Message = "keyword দাও!" });

            var students = await _context.Students
                .Where(s =>
                    s.Name!.ToLower().Contains(keyword.ToLower()) ||
                    s.City!.ToLower().Contains(keyword.ToLower())
                )
                .OrderBy(s => s.Name)
                .ToListAsync();

            return Ok(new
            {
                Keyword = keyword,
                Count = students.Count,
                Students = students
            });
        }

    }
}
