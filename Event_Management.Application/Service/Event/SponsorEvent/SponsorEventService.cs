using AutoMapper;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Enum.Sponsor;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.ParticipantDTO;
using Event_Management.Domain.Models.Sponsor;
using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;

namespace Event_Management.Application.Service
{
    public class SponsorEventService : ISponsorEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IRegisterEventService _registerEventService;
        

        public SponsorEventService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IRegisterEventService registerEventService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = configuration;
            _registerEventService = registerEventService;
        }

		public async Task<SponsorEvent> AddSponsorEventRequest(SponsorDto sponsorEvent, Guid userId)
		{

            var newSponsorRequest = new SponsorEvent();
            newSponsorRequest.EventId = sponsorEvent.EventId;

            var eventEntity = await _unitOfWork.EventRepository.GetById(sponsorEvent.EventId);
            if(sponsorEvent.Amount >= 2 * (eventEntity.Fare))
            {
                newSponsorRequest.UserId = userId;
                newSponsorRequest.SponsorType = sponsorEvent.SponsorType;
                newSponsorRequest.Message = sponsorEvent.Message;
                newSponsorRequest.Amount = sponsorEvent.Amount;

                newSponsorRequest.CreatedAt = DateTimeHelper.GetDateTimeNow();
                newSponsorRequest.UpdatedAt = DateTimeHelper.GetDateTimeNow();
                newSponsorRequest.Status = SponsorRequest.Processing.ToString();
                newSponsorRequest.IsSponsored = false;
                await _unitOfWork.SponsorEventRepository.Add(newSponsorRequest);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                return null;
            }
           
            return newSponsorRequest;


        }

        public async Task<SponsorEvent?> DeleteRequest(Guid eventId, Guid userId)
        {
            var requestEvent = await _unitOfWork.SponsorEventRepository.CheckSponsorEvent(eventId, userId);
            if(!requestEvent.Status.Equals(SponsorRequest.Confirmed.ToString()) || !requestEvent.Status.Equals(SponsorRequest.Reject.ToString()))
            {
                await _unitOfWork.SponsorEventRepository.DeleteSponsorRequest(eventId, userId);
            }
            return requestEvent;
        }

        public async Task<SponsorEvent?> GetRequestedDetail(Guid eventId, Guid userId)
        {
            return await _unitOfWork.SponsorEventRepository.CheckSponsorEvent(eventId, userId);
        }

        public async Task<PagedList<SponsorEvent>> GetRequestSponsor(Guid userId, string? status, int page, int eachPage)
        {
            return await _unitOfWork.SponsorEventRepository.GetRequestSponsor(userId, status, page, eachPage);
        }

        //public async Task<SponsorEvent?> CheckSponsorEvent(Guid eventId, Guid userId)
        //{
        //	return await _unitOfWork.SponsorEventRepository.CheckSponsorEvent(eventId, userId);

        //}

        public async Task<PagedList<SponsorEventDto>> GetSponsorEventsById(SponsorEventFilter sponsorFilter)
        {
            var list = await _unitOfWork.SponsorEventRepository.GetSponsorEvents(sponsorFilter);
            return _mapper.Map<PagedList<SponsorEventDto>>(list);
           
        }

        //public async Task<PagedList<SponsorEvent>> GetSponsorByEventId(Expression<Func<Guid, bool>> eventId, int page, int eachPage)
        //      {
        //          return await _unitOfWork.SponsorEventRepository.GetAll(page, eachPage);
        //      }

        public async Task<SponsorEvent> UpdateSponsorEventRequest(SponsorRequestUpdate sponsorRequestUpdate)
		{
            var sponsorRequest = await _unitOfWork.SponsorEventRepository.CheckSponsorEvent(sponsorRequestUpdate.EventId, sponsorRequestUpdate.UserId);
            if (sponsorRequest != null)
            {
                sponsorRequest.Status = sponsorRequestUpdate.Status;
                sponsorRequest.UpdatedAt = DateTimeHelper.GetDateTimeNow();
                if (sponsorRequest.Status.Equals(SponsorRequest.Confirmed.ToString()))
                {
                    var newParticipant = new RegisterEventModel((Guid)sponsorRequest.UserId, (Guid)sponsorRequest.EventId, (int)EventRole.Sponsor);
                    var result = await _registerEventService.AddToEvent(newParticipant);
                    if(result.Data == null)
                    {
                        return null;
                    }
                }
            }
            await _unitOfWork.SponsorEventRepository.Update(sponsorRequest);
            await _unitOfWork.SaveChangesAsync();
            return sponsorRequest;
		}
	}
}
