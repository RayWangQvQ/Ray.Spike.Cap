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
            _logger.LogDebug("1");
            _logger.LogDebug("2");
            _logger.LogDebug("3");
            capBus.PublishDelay(TimeSpan.FromSeconds(5), "test.show.time", DateTime.Now);
            _logger.LogInformation("Already send.");
            _logger.LogDebug("4");
            _logger.LogDebug("5");
            _logger.LogDebug("6");
            return Ok();
        }
    }
}
