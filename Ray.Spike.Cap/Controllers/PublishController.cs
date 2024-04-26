using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Ray.Spike.Cap.Common;

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
            _logger.LogDebug("-------开始发送-------");
            capBus.PublishDelay(TimeSpan.FromSeconds(5), "test.show.time", DateTime.Now);
            _logger.LogDebug("-------发送结束-------");
            return Ok();
        }

        [Route("~/sendobj")]
        [HttpGet]
        public IActionResult SendObjMessage([FromServices] ICapPublisher capBus)
        {
            _logger.LogDebug("-------开始发送-------");
            capBus.PublishDelay(TimeSpan.FromSeconds(5), "test.show.obj", MyEvt.Create());
            _logger.LogDebug("-------发送结束-------");
            return Ok();
        }
    }
}
