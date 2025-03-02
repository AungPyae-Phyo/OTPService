using Microsoft.AspNetCore.Mvc;
using OTPService.Interfaces;
using OTPService.DTO;
using OTPService.DTO.Request;


namespace OTPService.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    public class OtpController : ControllerBase
        {
            private readonly ITwilioService _twilioService;
            public OtpController(ITwilioService twilioService)
            {
                _twilioService = twilioService;
            }

            [HttpPost("send")]
            public async Task<OTPResponse> SendOTP([FromBody] OTPRequest request)
            {
                try
                {
                    await _twilioService.GenerateAndSendOtpAsync(request.PhoneNumber);
                    return new OTPResponse { IsSuccess = true, Message = "Sent Successful OTP" };
                }
                catch (Exception ex)
                {
                    return new OTPResponse { IsSuccess = false, Message = ex.Message };
                }
            }

            [HttpPost("verify")]
            public OTPResponse VerifyOTP([FromBody] VerifyOtpRequest request)
            {
                try
                {
                    var result = _twilioService.VerifyOtp(request.PhoneNumber, request.Otp);
                return new OTPResponse { IsSuccess = result, Message = result ? "OTP Verified" : "Invalid OTP" };
                }
                catch (Exception ex)
                {
                    return new OTPResponse { IsSuccess = false, Message = ex.Message };
                }
            }

        }
    }

