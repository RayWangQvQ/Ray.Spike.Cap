using DotNetCore.CAP;using Microsoft.AspNetCore.Mvc;using Newtonsoft.Json;using Ray.Spike.Cap.Common;namespace Ray.Spike.Cap.Consumer.Controllers{    public class ConsumerController : Controller    {        private static int Count = 0;        private readonly ILogger<ConsumerController> _logger;        public ConsumerController(ILogger<ConsumerController> logger)        {            _logger = logger;        }        [NonAction]        [CapSubscribe("test.show.time")]        [HttpGet]        public void ReceiveMessage(DateTime time)        {            _logger.LogInformation("-------开始接收-------");            _logger.LogInformation("message time is:{time}", time);            _logger.LogInformation("-------接收结束-------");        }        [NonAction]        [CapSubscribe("test.show.obj")]        [HttpGet]        public void ReceiveObjMessage(MyEvt myEvt)        {            _logger.LogInformation("-------开始接收-------");
            _logger.LogInformation("message obj is:{obj}", JsonConvert.SerializeObject(myEvt));

            Count++;
            _logger.LogInformation($"{Count}");

            if (Count < 4)
            {
                throw new Exception();            }

            _logger.LogInformation("-------接收结束-------");        }    }}