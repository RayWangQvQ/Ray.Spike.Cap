using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Ray.Spike.Cap.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublishController : ControllerBase
    {
        private readonly ILogger<PublishController> _logger;

        public PublishController(ILogger<PublishController> logger)
        {
            _logger = logger;
        }

        [Route("~/send")]
        [HttpGet]
        public IActionResult SendMessage([FromServices] ICapPublisher capBus)
        {
            capBus.Publish("test.show.time", DateTime.Now);
            _logger.LogInformation("Already send.");
            return Ok();
        }
    }
}
