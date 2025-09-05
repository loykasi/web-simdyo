namespace Scratch.Application.Abstracts
{
    public interface IEmailSender
    {
        Task Send(string toName, string toEmail, string subject, string body);
    }
}
