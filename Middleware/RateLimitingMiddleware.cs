using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System;

namespace OTPService.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, DateTime> _requestTracker = new();

        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIp = context.Connection.RemoteIpAddress?.ToString();

            if (_requestTracker.TryGetValue(clientIp, out DateTime lastRequestTime))
            {
                if ((DateTime.UtcNow - lastRequestTime).TotalMinutes < 1)
                {
                    context.Response.StatusCode = 429; // Too Many Requests
                    await context.Response.WriteAsync("You can only make 1 request per minute.");
                    return;
                }
                _requestTracker.TryRemove(clientIp, out _);
            }

            _requestTracker[clientIp] = DateTime.UtcNow;
            await _next(context);
        }
    }
}