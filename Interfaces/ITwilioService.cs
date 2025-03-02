namespace OTPService.Interfaces
{
    public interface ITwilioService
    {
        Task<string> GenerateAndSendOtpAsync(string phoneNumber);
        bool VerifyOtp(string phoneNumber, string otp);
    }
}
