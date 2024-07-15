using AutoMapper;
using Event_Management.Application.Dto.NotificationDTO;
using Event_Management.Application.Dto.NotificationDTO.Response;
using Event_Management.Application.Dto.UserDTO.Response;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service.Notification
{
    public class NotificationService : INotificationService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponse> GetAllNotificationByUserId(Guid userId)
        {
            var notifications = await _unitOfWork.NotificationRepository.GetAllNotiByUserId(userId);
            var notificationResponses = _mapper.Map<IEnumerable<NotificationResponseDto>>(notifications);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = Message.MessageCommon.GetSuccesfully,
                Data = notificationResponses,
            };
        }
    }
}
