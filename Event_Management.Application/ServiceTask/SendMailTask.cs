using Event_Management.Application.BackgroundTask;
using Event_Management.Application.Helper;
using Event_Management.Application.Service;
using Event_Management.Domain.Constants;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.ParticipantDTO;
using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Event_Management.Application.ServiceTask
{
    public class SendMailTask
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SendMailTask(IBackgroundTaskQueue taskQueue, IServiceScopeFactory serviceScopeFactory)
        {
            _taskQueue = taskQueue;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void SendMail(RegisterEventModel registerEventModel)
        {
            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                await SendTicketForVisitor(registerEventModel);
                Console.WriteLine("Send mail complete!");
            });
        }

        public async Task SendTicketForVisitor(RegisterEventModel registerEventModel)
        {
            var _unitOfWork = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();

            var currentEvent = await _unitOfWork.EventRepository.GetEventById(registerEventModel.EventId);

            if (currentEvent == null)
            {
                return;
            }

            var user = await _unitOfWork.UserRepository.GetById(registerEventModel.UserId);

            if (user == null)
            {
                return;
            }

            //var bytes = QRCodeHelper.GenerateQRCode("okokok");
            var _emailService = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IEmailService>();
            await _emailService.SendEmailTicket(MailConstant.TicketMail.PathTemplate, MailConstant.TicketMail.Title, new TicketModel()
            {
                Email = user.Email,
                RoleEventId = registerEventModel.RoleEventId,
                FullName = user.FullName,
                Avatar = currentEvent?.CreatedByNavigation?.Avatar,
                EventName = currentEvent?.EventName,
                Location = currentEvent?.Location,
                LocationAddress = currentEvent?.LocationAddress,
                LogoEvent = currentEvent?.Image,
                OrgainzerName = currentEvent?.CreatedByNavigation?.FullName,
                StartDate = currentEvent!.StartDate.ToOnlyDate(),
                Time = DateTimeHelper.GetTimeRange(currentEvent.StartDate, currentEvent.EndDate),
                Message = TicketMailConstant.MessageMail.ElementAt(registerEventModel.RoleEventId),
                TypeButton = Utilities.GetTypeButton(registerEventModel.RoleEventId),
            });
        }

    }
}
