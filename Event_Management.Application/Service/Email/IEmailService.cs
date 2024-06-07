using Event_Management.Application.Dto.User;
using Event_Management.Domain.Models.Mail;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service.Email
{
    public interface IEmailService
    {
        Task<bool> SendEmailWithTemplate(string template, string title, UserMailDto userMail);
    }
}
