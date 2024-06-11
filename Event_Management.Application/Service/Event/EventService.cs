using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Event_Management.Application.Dto.EventDTO.RequestDTO;
using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Domain;
using Event_Management.Domain.Constants;
using Event_Management.Domain.Enum;
using Event_Management.Domain.Helper;
using Event_Management.Domain.Message;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.System;
using Event_Management.Domain.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;


namespace Event_Management.Application.Service
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = configuration;
        }

        public async Task<Event> AddEvent(EventRequestDto eventDto)
        {

            bool validate = DateTimeHelper.ValidateStartTimeAndEndTime(eventDto.StartDate, eventDto.EndDate);
            if (!validate)
            {
                throw new InvalidOperationException("Start date must after current time and end date must after start date 30 mins!!");
            }
            var eventEntity = _mapper.Map<Event>(eventDto);
            eventEntity.EventId = Guid.NewGuid();
            if (eventDto.Image != null)
            {
                eventEntity.Image = await UploadImage2(eventDto.Image, eventEntity.EventId);
            }
            eventEntity.CreatedAt = DateTime.Now;
            eventEntity.Status = EventStatus.NotYet.ToString();
            await _unitOfWork.EventRepository.Add(eventEntity);
            if (await _unitOfWork.SaveChangesAsync())
            {
                return eventEntity;
            }
            throw new Exception("Error while creating event!");
        }

        public async Task<bool> DeleteEvent(Guid eventId)
        {
            await _unitOfWork.EventRepository.Delete(eventId);
            return await _unitOfWork.SaveChangesAsync();
        }

        //	_distributedCache = distributedCache;
        //}
        public async Task<PagedList<EventResponseDto>> GetAllEvents(EventFilterObject filter, int pageNo, int elementEachPage)
        {

            var result = await _unitOfWork.EventRepository.GetAllEvents(filter, pageNo, elementEachPage);
            List<EventResponseDto> response = _mapper.Map<List<EventResponseDto>>(result);

            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, result.TotalItems, pageNo, elementEachPage);
            return pages;
        }

        public async Task<PagedList<EventResponseDto>> GetUserParticipatedEvents(EventFilterObject filter, string userId, int pageNo, int elementEachPage)
        {
            var result = await _unitOfWork.EventRepository.GetUserParticipatedEvents(filter, userId, pageNo, elementEachPage);
            List<EventResponseDto> response = _mapper.Map<List<EventResponseDto>>(result);
            PagedList<EventResponseDto> pages = new PagedList<EventResponseDto>
                (response, response.Count, pageNo, elementEachPage);
            return pages;

        }

        public async Task<bool> UpdateEvent(EventRequestDto eventDto)
        {
            var eventEntity = _mapper.Map<Event>(eventDto);

            eventEntity.UpdatedAt = DateTime.Now;
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
            if (eventExist != null)
            {
                throw new Exception("Event Image duplicated!");
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
    }
}
