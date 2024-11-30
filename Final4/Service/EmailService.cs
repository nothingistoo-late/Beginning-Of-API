using Final4.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

public class EmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IConfiguration configuration)
    {
        _smtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Sender Name", _smtpSettings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart("html")
        {
            Text = body
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_smtpSettings.SenderEmail, _smtpSettings.SenderPassword);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }

}
