# OTP Verification Service with Twilio Integration  

A secure and scalable OTP (One-Time Password) verification system built with ASP.NET Core and Twilio. This service handles OTP generation, SMS delivery, and validation, with fallback mechanisms for development/testing environments.  

---

## Features  
- **Twilio SMS Integration**: Send OTPs via SMS using Twilio's API.  
- **Static OTP Fallback**: Auto-generates a static OTP (`1234`) when Twilio fails (e.g., unsupported phone numbers).  
- **Rate Limiting**: Restrict OTP requests to **3 per minute** per IP address.  
- **Phone Number Validation**: Ensure phone numbers are in [E.164 format](https://en.wikipedia.org/wiki/E.164).  
- **Error Handling**: Gracefully catch Twilio exceptions and log details for debugging.  


**Solution**:  
- If Twilio fails (e.g., due to an unsupported number), the service:  
  1. Logs the error.  
  2. Generates a **static OTP (`1234`)** for testing.  
  3. Stores the OTP in-memory for validation.  

---

## Tech Stack  
- **Backend**: ASP.NET Core 8.0  
- **SMS Gateway**: Twilio  
- **Caching**: `IMemoryCache` for OTP storage  
- **Middleware**: Custom rate limiting and phone number validation  

---

## Getting Started  

### Prerequisites  
- [.NET 8 SDK](https://dotnet.microsoft.com/download)  
- [Twilio Account](https://www.twilio.com/try-twilio) (Free Trial)  
- Verified Twilio Phone Number (e.g., `+1xxx`)  

### Installation  
1. Clone the repository:  
   ```bash  
   https://github.com/AungPyae-Phyo/OTPService.git
