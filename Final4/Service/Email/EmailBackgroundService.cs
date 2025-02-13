using Microsoft.Extensions.Hosting;
namespace Final4.Service.Email
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly EmailQueue _emailQueue;
        private readonly EmailService _emailService;
        private readonly ILogger<EmailBackgroundService> _logger;

        public EmailBackgroundService(EmailQueue emailQueue, EmailService emailService, ILogger<EmailBackgroundService> logger)
        {
            _emailQueue = emailQueue;
            _emailService = emailService;
            _logger = logger;
        }
        // start email background
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Email Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var emailRequest = await _emailQueue.DequeueEmailAsync(stoppingToken);
                    if (emailRequest != null)
                    {
                        await _emailService.SendEmailAsync(new List<string> { emailRequest.To }, emailRequest.Subject, emailRequest.Body);
                        _logger.LogInformation($"Email sent to {emailRequest.To}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred while sending email: {ex.Message}");
                }
            }

            _logger.LogInformation("Email Background Service is stopping.");
        }
    }
}
