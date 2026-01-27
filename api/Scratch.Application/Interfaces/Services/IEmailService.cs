using Scratch.Application.Models.Emails;

namespace Scratch.Application.Interfaces.Repositories
{
    public interface IEmailService
    {
        void Send(EmailMessage emailMessage);
    }
}
