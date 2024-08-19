using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace SerilogDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _log;

        public TestController(ILogger<TestController> logger)
        {
            _log = logger;
        }

        [HttpGet]
        public string Get()
        {
            _log.LogInformation("hello");

            _log.LogInformation("hello2 {orderId}", "a");

            _log.LogInformation("hello3 {orderId}", "b");

            // https://github.com/serilog/serilog/wiki/Enrichment
            using (LogContext.PushProperty("A", 1))
            {
                _log.LogInformation("Carries property A = 1");
            }

            return "ok";
        }
    }
}
