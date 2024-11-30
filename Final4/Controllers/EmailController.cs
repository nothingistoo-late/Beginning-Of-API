using Final4.Configuration;
using Final4.DTO.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace Final4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            if (string.IsNullOrEmpty(request.To) || string.IsNullOrEmpty(request.Subject) || string.IsNullOrEmpty(request.Body))
            {
                return BadRequest("Invalid email request.");
            }

            try
            {
                await _emailService.SendEmailAsync(request.To, request.Subject, request.Body);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while sending the email: {ex.Message}");
            }
        }
    }

}
