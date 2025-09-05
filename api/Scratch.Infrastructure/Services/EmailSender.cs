using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using Scratch.Application.Abstracts;
using Scratch.Infrastructure.Options;

namespace Scratch.Infrastructure.Services
{
    public class EmailSender: IEmailSender
    {
        private readonly EmailOptions _options;

        public EmailSender(IOptions<EmailOptions> options)
        {
            _options = options.Value;
        }

        public async Task Send(string toName, string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_options.SmtpServer, _options.SmtpPort, false);

                //client.Authenticate("joey", "password");

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
