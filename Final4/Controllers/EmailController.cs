using Final4.Configuration;
using Final4.DTO.Email;
using Final4.Service.Email;
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
        private readonly EmailQueue _emailQueue;

        public EmailController(EmailService emailService, EmailQueue emailQueue)
        {
            _emailService = emailService;
            _emailQueue = emailQueue;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            if (string.IsNullOrEmpty(request.To) || string.IsNullOrEmpty(request.Subject) || string.IsNullOrEmpty(request.Body))
            {
                return BadRequest("Invalid email request.");
            }

            _emailQueue.EnqueueEmail(request);
            return Ok("Email has been queued for sending.");
        }
    }

}
