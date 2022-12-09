using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UrlSaver.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly ILogger<DemoController> _logger;

        public DemoController(
            ILogger<DemoController> logger)
        {
            _logger = logger;
        }

        [HttpGet("latency", Name = nameof(LatencyDemo))]
        public ActionResult LatencyDemo ()
        {
            var latency = new Random().Next(4000);

            _logger.LogInformation($"--> Latency simulation with {latency} ms");
            Thread.Sleep(latency);

            return Ok($"Action simulated latency {latency}");
        }

        [HttpGet("notdetermineted", Name = nameof(NotDeterminetedDemo))]
        public ActionResult NotDeterminetedDemo()
        {
            return (new Random().Next(4000) > 2000 ? Ok() : NotFound());
        }

        [HttpGet("notfound", Name = nameof(NotFoundDemo))]
        public ActionResult NotFoundDemo ()
        {
            return NotFound();
        }

        [HttpGet("badrequest", Name = nameof(BadRequestDemo))]
        public ActionResult BadRequestDemo()
        {
            return BadRequest();
        }
    }
}
