using Event_Management.Domain.Models.User;

namespace Event_Management.Application.Service
{
    public interface IEmailService
    {
        Task<bool> SendEmailWithTemplate(string template, string title, UserMailDto userMail);
    }
}
