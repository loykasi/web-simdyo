using Scratch.Application.Models.Emails;

namespace Scratch.Application.Interfaces.Services
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);
    }
}
