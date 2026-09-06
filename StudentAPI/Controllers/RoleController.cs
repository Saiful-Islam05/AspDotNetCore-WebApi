using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        // =====================================================
        // 🔓 PUBLIC — Token লাগবে না
        // =====================================================
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok(new { Message = "🔓 সবাই দেখতে পারবে!" });
        }

        // =====================================================
        // 🔒 ANY LOGIN — যেকোনো Role
        // =====================================================
        [HttpGet("any-login")]
        [Authorize]
        public IActionResult AnyLogin()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Message = "🔒 Login থাকলেই দেখা যাবে!",
                Username = username,
                Role = role
            });
        }

        // =====================================================
        // 👑 ADMIN ONLY
        // =====================================================
        [HttpGet("admin-only")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            return Ok(new
            {
                Message = "👑 শুধু Admin দেখতে পারবে!",
                Username = username,
                Secret = "এটা Admin এর Secret Data!"
            });
        }

        // =====================================================
        // 🎓 STUDENT ONLY
        // =====================================================
        [HttpGet("student-only")]
        [Authorize(Roles = "Student")]
        public IActionResult StudentOnly()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            return Ok(new
            {
                Message = "🎓 শুধু Student দেখতে পারবে!",
                Username = username
            });
        }

        // =====================================================
        // 👥 ADMIN + STUDENT — দুজনেই
        // =====================================================
        [HttpGet("both")]
        [Authorize(Roles = "Admin,Student")]
        public IActionResult Both()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Message = "👥 Admin আর Student দুজনেই দেখতে পারবে!",
                Username = username,
                Role = role
            });
        }

        // =====================================================
        // 📊 ROLE CHECK — কোড দিয়ে Role check করা
        // =====================================================
        [HttpGet("check-role")]
        [Authorize]
        public IActionResult CheckRole()
        {
            // User এর Role code দিয়ে check করো
            var isAdmin = User.IsInRole("Admin");
            var isStudent = User.IsInRole("Student");
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            if (isAdmin)
            {
                return Ok(new
                {
                    Username = username,
                    IsAdmin = true,
                    Message = "👑 তুমি Admin! সব access আছে।",
                    CanDo = new[]
                    {
                        "Student দেখতে পারবে",
                        "Student Delete করতে পারবে",
                        "Course বানাতে পারবে",
                        "Report দেখতে পারবে"
                    }
                });
            }

            return Ok(new
            {
                Username = username,
                IsStudent = true,
                Message = "🎓 তুমি Student! Limited access আছে।",
                CanDo = new[]
                {
                    "নিজের Profile দেখতে পারবে",
                    "Course দেখতে পারবে"
                }
            });
        }
    }
}