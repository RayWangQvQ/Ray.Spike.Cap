using DotNetCore.CAP.Filter;
using Microsoft.Extensions.Logging;

namespace Ray.Spike.Cap.Common
{
    public class MyCapFilter : SubscribeFilter
    {
        private readonly ILogger<MyCapFilter> _logger;

        public MyCapFilter(ILogger<MyCapFilter> logger)
        {
            _logger = logger;
        }

        public override Task OnSubscribeExecutingAsync(ExecutingContext context)
        {
            //_logger.LogInformation("SubscribeFilter: {0}", "订阅方法执行前");
            return Task.CompletedTask;
        }

        public override Task OnSubscribeExecutedAsync(ExecutedContext context)
        {
            //_logger.LogInformation("SubscribeFilter: {0}", "订阅方法执行后");
            return Task.CompletedTask;
        }

        public override Task OnSubscribeExceptionAsync(ExceptionContext context)
        {
            //_logger.LogInformation("SubscribeFilter: {0}", "订阅方法执行异常");
            return Task.CompletedTask;
        }
    }
}
