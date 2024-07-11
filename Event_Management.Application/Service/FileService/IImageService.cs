using Event_Management.Application.Dto.EventDTO.RequestDTO;
using Event_Management.Application.Dto.SponsorLogoDTO;
using Event_Management.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Application.Service.FileService
{
    public interface IImageService
    {
        public Task<string?> UploadImage(String base64, Guid EventId);
        public Task<List<SponsorLogoDto>> GetAllBlobUris();
        public Task<Dictionary<string, List<string>>> GetAllEventBlobUris(Guid eventId);
        public Task<SponsorLogoDto> GetBlobUri(string blobName);
        public Task<string?> UploadEventSponsorLogo(string base64, Guid EventId, string sponsorName);
        public Task<bool> DeleteBlob(string blobName);
    }
}
