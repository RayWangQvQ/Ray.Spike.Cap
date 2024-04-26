using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace Ray.Spike.Cap.Controllers
{
    public class ConsumerController : Controller
    {
        private readonly ILogger<ConsumerController> _logger;

        public ConsumerController(ILogger<ConsumerController> logger)
        {
            _logger = logger;
        }

        [NonAction]
        [CapSubscribe("test.show.time")]
        [HttpGet]
        public void ReceiveMessage(DateTime time)
        {
            _logger.LogInformation("开始接收");
            _logger.LogInformation("message time is:{time}", time);
            _logger.LogInformation("接收结束");
        }
    }
}
