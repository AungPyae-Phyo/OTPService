using Microsoft.Extensions.Caching.Memory;
using OTPService.Interfaces;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace OTPService.Services
{
    public class TwilioService : ITwilioService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TwilioService> _logger;
        private readonly IMemoryCache _cache;

        public TwilioService(
            IConfiguration configuration,
            IMemoryCache cache,
            ILogger<TwilioService> logger)
        {
            _configuration = configuration;
            _cache = cache;
            _logger = logger;
            
            // Initialize Twilio client with credentials
            TwilioClient.Init(
                _configuration["Twilio:AccountSid"],
                _configuration["Twilio:AuthToken"]
            );
        }

        public async Task<string> GenerateAndSendOtpAsync(string phoneNumber)
        {
            string otp = "123456"; 

            try
            {
              
                await MessageResource.CreateAsync(
                    body: $"Your OTP: {otp}",
                    from: new PhoneNumber(_configuration["Twilio:PhoneNumber"]),
                    to: new PhoneNumber(phoneNumber)
                );
            }
            catch (RestException ex)
            {
           
                _logger.LogError($"Twilio Error (Code={ex.Code}): {ex.Message}");
                _logger.LogInformation($"Static OTP for testing: {otp}");
            }
            catch (Exception ex)
            {
                // Log general errors
                _logger.LogError(ex, "Unexpected error");
            }

            // Store OTP in cache (5 minutes expiry)
            _cache.Set(phoneNumber, otp, TimeSpan.FromMinutes(5));
            return otp;
        }

        public bool VerifyOtp(string phoneNumber, string otp)
        {
            bool isValid = _cache.TryGetValue(phoneNumber, out string storedOtp) 
                           && storedOtp == otp;

            if (isValid)
                _cache.Remove(phoneNumber); // Clear OTP after successful verification

            return isValid;
        }
    }
}