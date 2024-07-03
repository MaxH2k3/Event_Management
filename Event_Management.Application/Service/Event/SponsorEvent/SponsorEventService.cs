using AutoMapper;
using Azure;
using Event_Management.Application.Dto.EventDTO.SponsorDTO;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum.Sponsor;
using Event_Management.Domain.Models.Common;
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

		public async Task<bool> AddSponsorEventRequest(SponsorDto sponsorEvent)
		{
            sponsorEvent.Status = SponsorRequest.PROCESSING;
            
            sponsorEvent.UpdatedAt = DateTime.Now;
            var sponsorEntity = _mapper.Map<SponsorEvent>(sponsorEvent);
            await _unitOfWork.SponsorEventRepository.Add(sponsorEntity);
            return await _unitOfWork.SaveChangesAsync();

		}

		public async Task<SponsorEvent?> CheckSponsorEvent(Guid eventId, Guid userId)
		{
			return await _unitOfWork.SponsorEventRepository.CheckSponsorEvent(eventId, userId);

		}

		//public async Task<PagedList<SponsorEvent>> GetSponsorByEventId(Expression<Func<Guid, bool>> eventId, int page, int eachPage)
  //      {
  //          return await _unitOfWork.SponsorEventRepository.GetAll(page, eachPage);
  //      }

		public async Task<bool> UpdateSponsorEventRequest(SponsorDto sponsorEvent)
		{
            sponsorEvent.UpdatedAt = DateTime.Now;
			var sponsorEntity = _mapper.Map<SponsorEvent>(sponsorEvent);
			await _unitOfWork.SponsorEventRepository.Update(sponsorEntity);
            return await _unitOfWork.SaveChangesAsync();
		}
	}
}
