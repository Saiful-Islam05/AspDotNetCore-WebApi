using Microsoft.AspNetCore.Mvc;
using StudentAPI.Services;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DIController : ControllerBase
    {
        private readonly ISingletonService _singleton;
        private readonly IScopedService _scoped;
        private readonly ITransientService _transient1;
        private readonly ITransientService _transient2; // ✅ দুটো Transient

        public DIController(
            ISingletonService singleton,
            IScopedService scoped,
            ITransientService transient1,
            ITransientService transient2)
        {
            _singleton = singleton;
            _scoped = scoped;
            _transient1 = transient1;
            _transient2 = transient2;
        }

        // =====================================================
        // ✅ Lifetime পার্থক্য দেখো
        // URL: GET /api/di/lifetime
        // =====================================================
        [HttpGet("lifetime")]
        public IActionResult GetLifetime()
        {
            _singleton.IncrementCount();

            return Ok(new
            {
                Singleton = new
                {
                    InstanceId = _singleton.GetInstanceId(),
                    RequestCount = _singleton.GetRequestCount(),
                    Note = "সব request এ same ID আসবে! ✅"
                },
                Scoped = new
                {
                    InstanceId = _scoped.GetInstanceId(),
                    Note = "প্রতিটা request এ নতুন ID আসবে ✅"
                },
                Transient = new
                {
                    // দুটো আলাদা ID আসবে!
                    Instance1 = _transient1.GetInstanceId(),
                    Instance2 = _transient2.GetInstanceId(),
                    AreSame = _transient1.GetInstanceId()
                                 == _transient2.GetInstanceId(),
                    Note = "প্রতিবার inject এ নতুন ID! ✅"
                }
            });
        }
    }
}