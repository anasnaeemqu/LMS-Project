namespace FullstackMVC.Middlewares
{
    public static class useLoggingMiddlewreExtension
    {
        public static IApplicationBuilder UseCustomLogging(this IApplicationBuilder app)
        { 
            return app.UseMiddleware<LoggingMiddleware>();
        }

    }
}
