﻿namespace OTPService.DTO
{
    public class VerifyOtpRequest
    {
        public string PhoneNumber { get; set; }
        public string Otp { get; set; } 
    }
}