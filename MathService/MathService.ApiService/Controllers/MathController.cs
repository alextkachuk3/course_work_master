using Microsoft.AspNetCore.Mvc;

namespace MathService.ApiService.Controllers
{    
    public class MathController : Controller
    {
        private readonly ILogger<MathController> _logger;

        public MathController(ILogger<MathController> logger)
        {
            _logger = logger;
        }

        [HttpPost("mult")]
        public async Task<ActionResult<string>> Multiplication(int A)
        {
            return Ok("test");
        }
    }
}
