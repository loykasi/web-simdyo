namespace Application.Models.Emails
{
    public class EmailMessage
    {
        public required string ToName { get; set; }
        public required string ToEmail { get; set; }
        public required string Subject { get; set; }
        public required string TemplateName { get; set; }
        public required IEnumerable<EmailTemplatePlaceholder> Placeholders { get; set; }

        public string GetBody()
        {
            string path = $"{Directory.GetCurrentDirectory()}/MailTemplates/{TemplateName}";
            path = path.Replace('/', Path.DirectorySeparatorChar);

            string body = string.Empty;
            using (StreamReader reader = new(path))
            {
                body = reader.ReadToEnd();
            }

            foreach (var placeholder in Placeholders)
            {
                body = body.Replace($"{{{placeholder.Name}}}", placeholder.Value);
            }
            Console.WriteLine(body);
            return body;
        }
    }
}
