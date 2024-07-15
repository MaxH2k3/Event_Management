using AutoMapper;
using Azure;
using Event_Management.Application.Dto.EventDTO.SponsorDTO;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum.Sponsor;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.Sponsor;
using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service
{
    public class SponsorEventService : ISponsorEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public SponsorEventService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = configuration;
        }

		public async Task<SponsorEvent> AddSponsorEventRequest(SponsorDto sponsorEvent)
		{
            
            
           
            var sponsorEntity = _mapper.Map<SponsorEvent>(sponsorEvent);

            sponsorEntity.CreatedAt = DateTimeHelper.GetDateTimeNow();
            sponsorEntity.UpdatedAt = DateTimeHelper.GetDateTimeNow();
            sponsorEntity.Status = SponsorRequest.PROCESSING.ToString();
            await _unitOfWork.SponsorEventRepository.Add(sponsorEntity);
            await _unitOfWork.SaveChangesAsync();
            return sponsorEntity;


        }

        public async Task<PagedList<SponsorEvent>> GetSponsoredEvent(Guid userId, int page, int eachPage)
        {
            return await _unitOfWork.SponsorEventRepository.GetSponsoredEvent(userId, page, eachPage);
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

        public async Task<SponsorEvent> UpdateSponsorEventRequest(SponsorDto sponsorEvent)
		{
			var sponsorEntity = _mapper.Map<SponsorEvent>(sponsorEvent);
            sponsorEntity.UpdatedAt = DateTimeHelper.GetDateTimeNow();
            sponsorEntity.Status = SponsorRequest.PROCESSING.ToString();
            await _unitOfWork.SponsorEventRepository.Update(sponsorEntity);
            await _unitOfWork.SaveChangesAsync();
            return sponsorEntity;
		}
	}
}
