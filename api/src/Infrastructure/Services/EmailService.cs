using Application.Interfaces.Services;
using Application.Models.Emails;
using Hangfire;
using Infrastructure.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Services
{
    public class EmailService(IOptions<EmailOptions> options) : IEmailService
    {
        private readonly EmailOptions _options = options.Value;

        public void Send(EmailMessage emailMessage)
        {
            BackgroundJob.Enqueue(() => SendMail(emailMessage));
        }

        public async Task SendMail(EmailMessage emailMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
            message.To.Add(new MailboxAddress(emailMessage.ToName, emailMessage.ToEmail));
            message.Subject = emailMessage.Subject;
            message.Body = new TextPart("html")
            {
                Text = emailMessage.GetBody()
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_options.SmtpServer, _options.SmtpPort, false);
            //client.Authenticate("joey", "password");
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
