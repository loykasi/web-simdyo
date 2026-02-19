namespace Infrastructure.Options
{
    public class S3Options
    {
        public const string OptionsKey = "S3Settings";

        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string URL { get; set; }
        public string Bucket { get; set; }
    }
}
