using Microsoft.Extensions.Logging;

namespace Peak.App.Web.IDP
{
    public class TraceController : ITraceController
    {
        private readonly ILogger<TraceController> _logger;
        public TraceController(ILogger<TraceController> logger)
        {
            _logger = logger;
        }

        public void TraceDebug(string key, string message, params object[] args)
        {
            _logger.LogDebug($"{key} | {message}", args);
        }

        public void TraceInformation(string key, string message, params object[] args)
        {
            _logger.LogInformation($"{key} | {message}", args);
        }

        public void TraceError(string key, string message, params object[] args)
        {
            _logger.LogError($"{key} | {message}", args);
        }
    }
}
