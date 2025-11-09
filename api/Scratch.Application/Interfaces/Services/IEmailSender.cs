namespace Scratch.Application.Interfaces.Repositories
{
    public interface IEmailSender
    {
        Task Send(string toName, string toEmail, string subject, string body);
    }
}
