using System.Diagnostics;
namespace myTodoList.Middlewares;

public class FileLogMiddleware
{
    private readonly string logFilePath;
    private readonly RequestDelegate next;

    public FileLogMiddleware(RequestDelegate next, string logFilePath)
    {
        this.next = next;
        this.logFilePath = logFilePath;
    }

    public async Task Invoke(HttpContext c)
    {
        var sw = new Stopwatch();
        sw.Start();
        await next(c);
        WriteLogToFile($"{c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms."
            + $" User: {c.User?.FindFirst("name")?.Value ?? "unknown"}");
    }   
     private void WriteLogToFile(string logMessage)
    {
        using(StreamWriter sw = File.AppendText(logFilePath))
        {
            sw.WriteLine(logMessage);
        }
    } 
}

public static partial class MiddleExtensions
{
    public static IApplicationBuilder UseFileLogMiddleware(this IApplicationBuilder builder,string logFilePath)
    {
        return builder.UseMiddleware<FileLogMiddleware>(logFilePath);
    }
}