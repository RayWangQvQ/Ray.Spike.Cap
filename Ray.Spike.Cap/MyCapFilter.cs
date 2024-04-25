using DotNetCore.CAP.Filter;

namespace Ray.Spike.Cap
{
    public class MyCapFilter : SubscribeFilter
    {
        private readonly ILogger<MyCapFilter> _logger;

        public MyCapFilter(ILogger<MyCapFilter> logger)
        {
            _logger = logger;
        }

        public override void OnSubscribeExecuting(ExecutingContext context)
        {
            _logger.LogInformation("SubscribeFilter: {0}", "订阅方法执行前");
        }

        public override void OnSubscribeExecuted(ExecutedContext context)
        {
            _logger.LogInformation("SubscribeFilter: {0}", "订阅方法执行后");
        }

        public override void OnSubscribeException(ExceptionContext context)
        {
            _logger.LogInformation("SubscribeFilter: {0}", "订阅方法执行异常");
        }
    }
}
