using Application.Models.Emails;

namespace Application.Interfaces.Services
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);
    }
}
