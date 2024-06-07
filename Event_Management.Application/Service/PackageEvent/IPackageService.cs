using Event_Management.Domain.Models.Common;
using Event_Management.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event_Management.Application.Dto.PackageDto;
using Event_Management.Application.Dto;

namespace Event_Management.Application.Service.PackageEvent
{
    public interface IPackageService
    {
        Task<PagedList<Package>> GetAllPackage(int page, int eachPage);
        Task<PagedList<Package>> GetPackageByEventId(Guid eventId, int page, int eachPage);

        Task<bool> AddPackage(PackageDto packageDto);
        Task<bool> DeletePackage(Guid packageId);
        Task<bool> UpdatePackage(PackageDto packageDto);
    }
}
