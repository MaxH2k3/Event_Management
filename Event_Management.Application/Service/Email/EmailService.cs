using Event_Management.Domain.Models.ParticipantDTO;
using Event_Management.Domain.Models.User;
using FluentEmail.Core;

namespace Event_Management.Application.Service
{
	public class EmailService : IEmailService
    {

        private readonly IFluentEmail _fluentEmail;

        public EmailService (IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }

        public async Task<bool> SendEmailWithTemplate(string template, string title, UserMailDto userMail)
        {
            var response = await _fluentEmail.To(userMail.Email)
                .Subject(title)
                .UsingTemplateFromFile(template, userMail, true)
                .SendAsync();
            return response.Successful;
        }

        public async Task<bool> SendEmailTicket(string template, string title, TicketModel ticket)
        {
            var response = await _fluentEmail.To(ticket.Email)
                .Subject(title)
                .UsingTemplateFromFile(template, ticket, true)
                .SendAsync();
            return response.Successful;
        }
    }
}
