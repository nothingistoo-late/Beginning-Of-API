using Final4.Data;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Microsoft.Extensions.Logging;

public class SendEmailJob : IJob
{
    private readonly ApplicationDBContext _dbContext;
    private readonly EmailService _emailService;
    private readonly ILogger<SendEmailJob> _logger;

    public SendEmailJob(ApplicationDBContext dbContext, EmailService emailService, ILogger<SendEmailJob> logger)
    {
        _dbContext = dbContext;
        _emailService = emailService;
        _logger = logger;
    }

    // execute hosted gmail reminder, dont mind on it, trust me bro 
    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("SendEmailJob started at {Time}", DateTime.Now);

        try
        {
            // Lấy danh sách email từ DB
            var emails = await _dbContext.Accounts.
                                           Where(o => o.AccountRoleID == "User")
                                          .Select(a => a.AccountEmail)
                                          .ToListAsync();

            if (emails.Any())
            {
                // Gửi email đến danh sách
                await _emailService.SendEmailAsync(emails, "Reminder Mail", "Đây chỉ là mail test, nếu bạn đọc dc mail này thì chứng tỏ 1 thằng nào đó trong nhóm đã test thành công chức năng gửi mail reminder!!");
                _logger.LogInformation("Emails successfully sent at {Time}", DateTime.Now);
            }
            else
            {
                _logger.LogWarning("No email addresses found in the database at {Time}", DateTime.Now);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending email at {Time}", DateTime.Now);
        }
    }
}
