using System.Diagnostics;

namespace FullstackMVC.Middlewares
{
    public class LoggingMiddleware //serilog || nLog
    {
        RequestDelegate _next;
        Stopwatch sw;
        ILogger<LoggingMiddleware> logger;
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> _logger)
        {
            _next = next;
            sw = new Stopwatch();
            logger = _logger;
        }
        public async Task Invoke(HttpContext context) 
        {
            sw.Start();
            Console.WriteLine($"Request Path => {context.Request.Path} , Method => {context.Request.Method}");
            logger.LogCritical("Logging Middleware");
            //logger.LogError("Error");
            //logger.LogInformation("trying logger");
            await _next(context);
            sw.Stop();
            Console.WriteLine($"Response Status Code => {context.Response.StatusCode} , Time taken =>{sw.ElapsedMilliseconds}ms");

        }

    }
}
