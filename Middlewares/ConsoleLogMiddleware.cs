using System.Diagnostics;

namespace myTodoList.Middlewares;

public class ConsoleLogMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ConsoleLogMiddleware> logger;

    public ConsoleLogMiddleware(RequestDelegate next, ILogger<ConsoleLogMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext c)
    {
        var sw = new Stopwatch();
        sw.Start();
        await next(c);
        logger.LogDebug($"{c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms."
            + $" User: {c.User?.FindFirst("userId")?.Value ?? "unknown"}");     
    }    
}

public static partial class MiddleExtensions
{
    public static IApplicationBuilder UseConsoleLogMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ConsoleLogMiddleware>();
    }
}