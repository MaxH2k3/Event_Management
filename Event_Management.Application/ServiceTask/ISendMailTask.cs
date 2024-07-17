using Event_Management.Domain.Models.ParticipantDTO;
using Event_Management.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.ServiceTask
{
    public interface ISendMailTask
    {
        void SendMailTicket(RegisterEventModel registerEventModel);
        void SendMailReminder(Guid eventId);
        void SendMailVerify(UserMailDto userMailDto);
    }
}
