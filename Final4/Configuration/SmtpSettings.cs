namespace Final4.Configuration
{
    public class SmtpSettings
    {
        public required string Host { get; set; }
        public int Port { get; set; }
        public required string SenderEmail { get; set; }
        public required string SenderPassword { get; set; }
    }
}
