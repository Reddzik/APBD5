using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zajecia5.MIddlewear
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string PathToFile = @"D:\Proejkty\APBD\zajecia5\APBD5\requestsLog.txt";
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            if (context.Request != null)
            {
                string path = context.Request.Path;
                string method = context.Request.Method;
                string queryString = context.Request.QueryString.ToString();
                string bodyStr = "";
                using(var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;
                }
                string stringToLog = $"{method}\n{path}\n{bodyStr}\n{queryString}";

                if(File.Exists(PathToFile)) await File.WriteAllTextAsync(PathToFile, stringToLog, Encoding.UTF8);
            }

            if(_next!=null) await _next(context);
        }
    }
}
