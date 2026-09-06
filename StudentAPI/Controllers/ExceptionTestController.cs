using Microsoft.AspNetCore.Mvc;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExceptionTestController : ControllerBase
    {
        // ✅ 500 — Unhandled Error
        [HttpGet("server-error")]
        public IActionResult ServerError()
        {
            throw new Exception("Something went wrong!");
        }

        // ✅ 404 — Not Found Error
        [HttpGet("not-found")]
        public IActionResult NotFoundError()
        {
            throw new KeyNotFoundException("Data পাওয়া যায়নি!");
        }

        // ✅ 400 — Bad Request Error
        [HttpGet("bad-request")]
        public IActionResult BadRequestError()
        {
            throw new ArgumentException("ভুল argument দিয়েছো!");
        }

        // ✅ 401 — Unauthorized Error
        [HttpGet("unauthorized")]
        public IActionResult UnauthorizedError()
        {
            throw new UnauthorizedAccessException("Login করো!");
        }
    }
}