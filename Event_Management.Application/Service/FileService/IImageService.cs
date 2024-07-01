using Event_Management.Application.Dto.EventDTO.RequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service.FileService
{
    public interface IImageService
    {
        public Task<string?> UploadImage2(FileUploadDto dto);
        public Task<string?> UploadImage(String base64, Guid EventId);
        public Task<List<string>> GetAllBlobUris();
        public Task<Dictionary<string, List<string>>> GetAllEventBlobUris(Guid eventId);
        public Task<string?> GetBlobUri(string blobName);
        public Task<string?> UploadEventSponsorLogo(string base64, Guid EventId);
    }
}
