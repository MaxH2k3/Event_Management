using AutoMapper;
using Event_Management.Application.Dto;
using Event_Management.Application.Dto.PackageDto;
using Event_Management.Domain;
using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Domain.Models.Common;

namespace Event_Management.Application.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            //Mapper User

            //Mapper Event
            CreateMap<Event, EventResponseDto>().ReverseMap();
            CreateMap<EventRequestDto, Event>().ReverseMap();

            //Mapper Tag
            CreateMap<TagDTO, Tag>().ReverseMap();

            //Mapper Packgae
            CreateMap<PackageDto, Package>();
            CreateMap<Event, EventPreview>().ReverseMap();
            CreateMap<PagedList<Event>, PagedList<EventPreview>>().ReverseMap();

        }
        
    }
}
