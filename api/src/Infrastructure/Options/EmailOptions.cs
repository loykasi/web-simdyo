namespace Infrastructure.Options
{
    public class EmailOptions
    {
        public const string OptionsKey = "SmtpSettings";

        public string Host { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string From { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseSsl { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
