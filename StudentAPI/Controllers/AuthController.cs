using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentAPI.Data;
using StudentAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(
            AppDbContext context,
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // ✅ REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterDTO dto)
        {
            // Already আছে?
            if (await _context.Users.AnyAsync(
                u => u.Username == dto.Username))
                return BadRequest(new { Message = "Username already নেওয়া!" });

            // Password Hash করো
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt
                               .HashPassword(dto.Password),
                Role = dto.Role ?? "Student"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = $"✅ {user.Username} registered!" });
        }

        // ✅ LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDTO dto)
        {
            // User খোঁজো
            var user = await _context.Users
                .FirstOrDefaultAsync(
                    u => u.Username == dto.Username);

            // Password check করো
            if (user == null || !BCrypt.Net.BCrypt
                .Verify(dto.Password, user.PasswordHash))
                return Unauthorized(new { Message = "❌ ভুল Username বা Password!" });

            // Token বানাও
            var token = GenerateToken(user);

            return Ok(new
            {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                Message = $"✅ Welcome {user.Username}!"
            });
        }

        // ✅ PROTECTED — Token লাগবে
        [HttpGet("profile")]
        [Authorize]
        public IActionResult Profile()
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Message = "🔒 তুমি Authorized!",
                Username = username,
                Role = role
            });
        }

        // ✅ ADMIN ONLY
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly()
        {
            return Ok(new { Message = "👑 Admin Only Area!" });
        }

        // Token Generate করার Method
        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _config["JwtSettings:SecretKey"]!));

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username!),
                new Claim(ClaimTypes.Role, user.Role!)
            };

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}