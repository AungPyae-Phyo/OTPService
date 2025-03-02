using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace OTPService.Middlewares
{
    public class PhoneNumberValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private const string PhoneNumberPattern = @"^\+[1-9]\d{1,14}$"; 

        public PhoneNumberValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/Otp"))
            {
                var phoneNumber = context.Request.Query["phoneNumber"].ToString();
                if (!Regex.IsMatch(phoneNumber, PhoneNumberPattern))
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Phone Number must be start with: +959");
                    return;
                }
            }

            await _next(context);
        }
    }
}