namespace Infrastructure.Options
{
    public class EmailOptions
    {
        public const string EmailOptionsKey = "SmtpSettings";

        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }
}
