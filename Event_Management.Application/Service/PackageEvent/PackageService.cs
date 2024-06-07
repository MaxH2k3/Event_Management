using AutoMapper;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain;
using Event_Management.Domain.UnitOfWork;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Event_Management.Application.Dto.PackageDto;
using Microsoft.EntityFrameworkCore;

namespace Event_Management.Application.Service.PackageEvent
{
    public class PackageService : IPackageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PackageService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<bool> AddPackage(PackageDto packageDto)
        {
            
                var packakeEntity = _mapper.Map<Package>(packageDto);
                packakeEntity.UpdatedAt = DateTime.Now;  // or DateTime.UtcNow

                await _unitOfWork.PackageRepository.Add(packakeEntity);
                return await _unitOfWork.SaveChangesAsync();

        }

        public async Task<bool> DeletePackage(Guid packageId)
        {
            await _unitOfWork.PackageRepository.Delete(packageId);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedList<Package>> GetAllPackage(int page, int eachPage)
        {
            return await _unitOfWork.PackageRepository.GetAll(page, eachPage, "Budget");
        }

        public async Task<PagedList<Package>> GetPackageByEventId(Guid eventId, int page, int eachPage)
        {
            
            return await _unitOfWork.PackageRepository.GetAll(p => p.EventId == eventId, page, eachPage, "Budget");
        }

        public async Task<bool> UpdatePackage(PackageDto packageDto)
        {
            var packakeEntity = _mapper.Map<Package>(packageDto);

           
            packakeEntity.UpdatedAt = DateTime.Now;  // or DateTime.UtcNow

            await _unitOfWork.PackageRepository.Update(packakeEntity);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
