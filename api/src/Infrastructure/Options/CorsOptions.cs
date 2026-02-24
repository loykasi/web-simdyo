namespace Infrastructure.Options
{
    public class CorsOptions
    {
        public const string OptionsKey = "CorsOptions";

        public string[] AllowedOrigins { get; set; }
    }
}
