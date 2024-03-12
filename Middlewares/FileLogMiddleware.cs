using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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
            const int maxAttempts = 3;
            const int delayMs = 100;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    using (StreamWriter sw = File.AppendText(logFilePath))
                    {
                        sw.WriteLine(logMessage);
                    }
                    return; 
                }
                catch (IOException)
                {
                    if (attempt < maxAttempts)
                    {
                        Task.Delay(delayMs).Wait(); 
                    }
                    else
                    {
                        Console.WriteLine($"Failed to write to log file after {maxAttempts} attempts.");
                    }
                }
            }
        }
    }


public static partial class MiddleExtensions
{
    public static IApplicationBuilder UseFileLogMiddleware(this IApplicationBuilder builder, string logFilePath)
    {
        return builder.UseMiddleware<FileLogMiddleware>(logFilePath);
    }
}

