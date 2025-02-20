using System.Text;
using Final4.Configuration;
using Final4.Model.Entities;
using MailKit.Net.Smtp;
using MimeKit;

public class EmailService
{
    private readonly SmtpSettings? _smtpSettings;

    public EmailService(IConfiguration configuration)
    {
        _smtpSettings = configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
    }

    // này chỉ gửi mail thoi, nhập vào người nhận, tiêu đề, và body rồi gửi thoi

    public async Task SendEmailAsync(List<string> to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Admin", _smtpSettings.SenderEmail));
        email.To.Add(MailboxAddress.Parse("hctrung2k4@gmail.com"));
        foreach (var recipient in to)
        {
            email.Bcc.Add(MailboxAddress.Parse(recipient));
        }
        email.Subject = subject;
        email.Body = new TextPart("html")
        {
            Text = body
        };
        //mail

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_smtpSettings.SenderEmail, _smtpSettings.SenderPassword);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }


    // cái này chỉ là tạo body cho gửi bill gmail thôi, đừng cố đọc hiểu làm gì cho mệt 
    public string GenerateOrderEmailBody(Order order)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<h5>Sales Invoice!</h5>");
        sb.AppendLine("<h2>Thank you for your purchase!</h2>");
        sb.AppendLine("<h3>Order Details:</h3>");
        sb.AppendLine("<h1></h1>");
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
        sb.AppendLine("<p><strong>Date:</strong> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
        sb.AppendLine("<p>If you have any questions, feel free to contact us from out contact 0123456789.</p>");
        sb.AppendLine("<p>Or contact we from email example@gmail.com</p>");
        sb.AppendLine("<p>Thank you for shopping with us <3!</p>");

        return sb.ToString();
    }

}
