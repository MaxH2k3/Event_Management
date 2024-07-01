using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Event_Management.Application.Dto.EventDTO.RequestDTO;
using Event_Management.Application.Service.Job;
using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service.FileService
{
    public class ImageService : IImageService
    {      
        private readonly IConfiguration _config;

        public ImageService(IConfiguration configuration)
        {           
            _config = configuration;
        }
        private BlobContainerClient GetBlobContainerClient()
        {
            string? containerName = _config["AzureStorageSettings:ContainerName"];
            string? connectionString = _config["AzureStorageSettings:ConnectionString"];
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
            return blobContainerClient;
        }
        public async Task<string?> UploadImage(String base64, Guid EventId)
        {
            /*var eventExist = await _unitOfWork.EventRepository.GetById(EventId);
            if (eventExist == null)
            {
                throw new Exception(MessageEvent.EventIdNotExist);
            }*/

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

        public async Task<Dictionary<string, List<string>>> GetAllEventBlobUris(Guid eventId)
        {
            BlobContainerClient blobContainerClient = GetBlobContainerClient();
            List<string> blobUris = new List<string>();
            List<string> eventTheme = new List<string>();
            await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
            {
                BlobClient blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
                string absPath = blobClient.Uri.AbsoluteUri;
                if (absPath.Contains(eventId.ToString()) && absPath.Contains("logo"))
                {
                    blobUris.Add(absPath);
                }
                if (absPath.Contains(eventId.ToString()) && !absPath.Contains("logo"))
                {
                    eventTheme.Add(absPath);
                }
            }
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>
            {
                { "event avatar", eventTheme },
                { "event sponsor logos", blobUris }
            };
            return result;
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

        public async Task<string?> UploadImage2(FileUploadDto dto)
        {
            return await UploadImage(dto.formFile, dto.eventId);
        }

        // blob name format of the image uploaded by this function: eventId + sponsor name + "logo"
        public Task<string?> UploadEventSponsorLogo(string base64, Guid EventId)
        {
            throw new NotImplementedException();
        }
    }
}
