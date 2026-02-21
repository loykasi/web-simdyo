namespace Infrastructure.Options
{
    public class AccountOptions
    {
        public const string OptionsKey = "Admin";

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
