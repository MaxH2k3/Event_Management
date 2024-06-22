using AutoMapper;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Event_Management.Application.Dto.EventDTO.RequestDTO;
using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Application.Message;
using Event_Management.Application.Service.Job;
using Event_Management.Domain;
using Event_Management.Domain.Constants;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.UnitOfWork;
using FluentEmail.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;


namespace Event_Management.Application.Service
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IQuartzService _quartzService;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IQuartzService quartzService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = configuration;
            _quartzService = quartzService;
        }

        public async Task<EventResponseDto> AddEvent(EventRequestDto eventDto, string userId)// HttpContext context)
        {
            DateTime startDate = DateTimeHelper.ToDateTime(eventDto.StartDate);
            DateTime endDate = DateTimeHelper.ToDateTime(eventDto.EndDate);
            bool validate = DateTimeHelper.ValidateStartTimeAndEndTime(startDate, endDate);
            if (!validate)
            {
                throw new InvalidOperationException(MessageEvent.StartEndTimeValidation);
            }
            var eventEntity = _mapper.Map<Event>(eventDto);
            eventEntity.EventId = Guid.NewGuid();
            eventEntity.StartDate = startDate;
            eventEntity.EndDate = endDate;
            if (eventDto.Image != null)
            {
                eventEntity.Image = await UploadImage2(eventDto.Image, eventEntity.EventId);
            }
            //string userId = IndentityExtension.GetUserIdFromToken2(context);
            //string userId = IndentityExtension.GetUserIdFromToken();
            /*if (string.IsNullOrEmpty(userId))
            {
                throw new Exception(MessageCommon.SessionTimeout);
            }
            else
            {
                eventEntity.CreatedBy = Guid.Parse(userId);
            }*/

            eventEntity.CreatedAt = DateTime.Now;
            eventEntity.Location = eventDto.Location.Location;
            eventEntity.LocationId = eventDto.Location.LocationId;
            eventEntity.LocationAddress = eventDto.Location.LocationAddress;
            eventEntity.LocationUrl = eventDto.Location.LocationUrl;
            eventEntity.LocationCoord = eventDto.Location.LocationCoord;
            eventEntity.Status = EventStatus.NotYet.ToString();
            await _unitOfWork.EventRepository.Add(eventEntity);
            if (await _unitOfWork.SaveChangesAsync())
            {
                await _quartzService.StartEventStatusToOngoingJob(eventEntity.EventId.ToString(), eventEntity.StartDate);
                await _quartzService.StartEventStatusToEndedJob(eventEntity.EventId.ToString(), eventEntity.EndDate);
                EventResponseDto response = ToResponseDto(eventEntity);
                return response;
            }
            throw new Exception(MessageCommon.CreateFailed);
        }
        private EventResponseDto ToResponseDto(Event eventEntity)
        { 
            long epochTime = DateTimeHelper.epochTime;
            EventResponseDto response = new EventResponseDto();
            response.StartDate = (eventEntity.StartDate.Ticks - epochTime) / 10000;
            response.EndDate = (eventEntity.EndDate.Ticks - epochTime) / 10000;
            long createAt = 0;
            createAt = DateTime.Parse(eventEntity.StartDate.ToString()).Ticks;
            response.CreatedAt = (createAt - epochTime) / 10000;
            response.Status = eventEntity.Status;
            response.Approval = eventEntity.Approval;
            response.Description = eventEntity.Description;
            response.Location = eventEntity.Location;
            response.LocationId = eventEntity.LocationId;
            response.LocationCoord = eventEntity.LocationCoord;
            response.LocationAddress = eventEntity.LocationAddress;
            response.LocationUrl = eventEntity.LocationUrl;
            response.EventId = eventEntity.EventId;
            response.EventName = eventEntity.EventName;
            response.CreatedBy = eventEntity.CreatedBy;
            response.Image = eventEntity.Image;
            if(eventEntity.StartDate.ToString() != null)
            {
                response.UpdatedAt = (DateTime.Parse(eventEntity.StartDate.ToString()).Ticks
                    - epochTime) / 10000;
            }
            else
            {
                response.UpdatedAt = null;
            }
            response.Ticket = eventEntity.Ticket;
            response.Capacity = eventEntity.Capacity;
            return response;
        }
        public async Task<bool> DeleteEvent(Guid eventId)
        {
            Event? existEvent = await _unitOfWork.EventRepository.GetById(eventId);
            if (existEvent == null || existEvent.Status.Equals(EventStatus.OnGoing))
            {
                return false;
            }
            return await _unitOfWork.EventRepository.DeleteEvent(eventId);
        }

        //	_distributedCache = distributedCache;
        //}
        public async Task<PagedList<EventResponseDto>> GetAllEvents(EventFilterObject filter, int pageNo, int elementEachPage)
        {

            var result = await _unitOfWork.EventRepository.GetAllEvents(filter, pageNo, elementEachPage);
            List<EventResponseDto> response = new List<EventResponseDto>(); //_mapper.Map<List<EventResponseDto>>(result);
                foreach(var r in result)
            {
                response.Add(ToResponseDto(r));
            }
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, result.TotalItems, pageNo, elementEachPage);
            return pages;
        }

        public async Task<PagedList<EventResponseDto>> GetUserParticipatedEvents(EventFilterObject filter, string userId, int pageNo, int elementEachPage)
        {
            var result = await _unitOfWork.EventRepository.GetUserParticipatedEvents(filter, userId, pageNo, elementEachPage);
            //List<EventResponseDto> response = _mapper.Map<List<EventResponseDto>>(result);
            List<EventResponseDto> response = new List<EventResponseDto>();
            foreach (var r in result)
            {
                response.Add(ToResponseDto(r));
            }
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, response.Count, pageNo, elementEachPage);
            return pages;

        }

        public async Task<bool> UpdateEvent(EventRequestDto eventDto, string userId)
        {
            var eventEntity = _mapper.Map<Event>(eventDto);
            if (!string.IsNullOrEmpty(userId))
            {
                eventEntity.CreatedBy = Guid.Parse(userId);
            }
            eventEntity.UpdatedAt = DateTime.Now;
            eventEntity.StartDate = DateTimeOffset.FromUnixTimeMilliseconds(eventDto.StartDate).DateTime;
            eventEntity.EndDate = DateTimeOffset.FromUnixTimeMilliseconds(eventDto.EndDate).DateTime;
            await _unitOfWork.EventRepository.Update(eventEntity);
            return await _unitOfWork.SaveChangesAsync();


        }
        public async Task<string?> UploadImage(FileUploadDto dto)
        {
            return await UploadImage2(dto.formFile, dto.eventId);
        }
        public async Task<string?> UploadImage2(String base64, Guid EventId)
        {
            var eventExist = await _unitOfWork.EventRepository.GetById(EventId);
            if (eventExist == null)
            {
                throw new Exception(MessageEvent.EventIdNotExist);
            }

            var httpHeaders = new BlobHttpHeaders
            {
                ContentType = "image/png" // file type
            };
            BlobContainerClient blobContainerClient = GetBlobContainerClient();
            BlobClient blobClient = blobContainerClient.GetBlobClient(EventId.ToString());

            // Decode base64 string to byte array
            byte[] imageBytes = Convert.FromBase64String(base64);

            using (var memoryStream = new MemoryStream(imageBytes))
            {
                await blobClient.UploadAsync(memoryStream, httpHeaders);
            }

            string absPath = blobClient.Uri.AbsoluteUri;
            //eventExist.Image = absPath;
            return absPath;
        }
        public void UpdateEventStatusEnded()
        {
            _unitOfWork.EventRepository.UpdateEventStatusToEnded();
        }
        public void UpdateEventStatusOngoing()
        {
            _unitOfWork.EventRepository.UpdateEventStatusToOnGoing();
        }
        public async Task<string?> GetBlobUri(string blobName)
        {
            BlobContainerClient blobContainerClient = GetBlobContainerClient();
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
            if (!await blobClient.ExistsAsync())
            {
                return null;
            }

            string absPath = blobClient.Uri.AbsoluteUri;
            return absPath;
        }
        public async Task<List<string>> GetAllBlobUris()
        {
            BlobContainerClient blobContainerClient = GetBlobContainerClient();
            List<string> blobUris = new List<string>();

            await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
            {
                BlobClient blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
                string absPath = blobClient.Uri.AbsoluteUri;
                blobUris.Add(absPath);
            }

            return blobUris;
        }
        private BlobContainerClient GetBlobContainerClient()
        {
            string? containerName = _config["AzureStorageSettings:ContainerName"];
            string? connectionString = _config["AzureStorageSettings:ConnectionString"];
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            return blobContainerClient;
        }

        public async Task<Event?> GetEventById(Guid eventId)
        {
            return await _unitOfWork.EventRepository.GetById(eventId);
        }
    }
}
