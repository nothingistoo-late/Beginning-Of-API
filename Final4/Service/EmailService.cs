using System.Text;
using Final4.Configuration;
using Final4.Model.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

public class EmailService
{
    private readonly SmtpSettings? _smtpSettings;

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

    public string GenerateOrderEmailBody(Order order)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<h2>Thank you for your purchase!</h2>");
        sb.AppendLine("<h3>Order Details:</h3>");

        // Thông tin người dùng
        sb.AppendLine("<p><strong>Order Name:</strong> " + order.OrderName + "</p>");
        sb.AppendLine("<p><strong>Order ID:</strong> " + order.OrderId + "</p>");

        // Danh sách sản phẩm trong đơn hàng
        sb.AppendLine("<h4>Items Purchased:</h4>");
        sb.AppendLine("<ul>");

        foreach (var orderDetail in order.OrderDetails)
        {
            if (orderDetail.Flower != null)
            {
                sb.AppendLine($"<li>{orderDetail.Flower.FlowerName} - Quantity: {orderDetail.Quantity} - Price: {orderDetail.Flower.FlowerPrice * orderDetail.Quantity} VND</li>");
            }
            else
            {
                sb.AppendLine($"<li>Flower not found - Quantity: {orderDetail.Quantity} </li>");
            }
        }

        sb.AppendLine("</ul>");

        // Tổng giá trị
        var totalAmount = order.OrderDetails.Sum(od => (od.Flower?.FlowerPrice ?? 0) * od.Quantity);
        sb.AppendLine("<p><strong>Total Amount:</strong> " + totalAmount + " VND</p>");

        // Thông báo trạng thái đơn hàng
        sb.AppendLine("<p><strong>Order Status:</strong> " + order.OrderStatus + "</p>");

        sb.AppendLine("<p>If you have any questions, feel free to contact us.</p>");
        sb.AppendLine("<p>Thank you for shopping with us!</p>");

        return sb.ToString();
    }

}
