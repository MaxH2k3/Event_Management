using Event_Management.Application.Dto.User;
using Event_Management.Domain.Models.Mail;
using FluentEmail.Core;
using Microsoft.VisualBasic;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service.Email
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
    }
}
