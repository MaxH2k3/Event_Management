using AutoMapper;
using Event_Management.Application;
using Event_Management.Application.Dto;
using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Application.Dto.UserDTO.Response;
using Event_Management.Domain.Models.Common;
using Event_Management.Domain.Models.EventDTO.ResponseDTO;
using Event_Management.Domain.Models.ParticipantDTO;

namespace Event_Management.Domain.Configuration
{
	public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            //Mapper User
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName))
                .ReverseMap();

            //Mapper Event
            CreateMap<Event, EventResponseDto>().ReverseMap();
            CreateMap<EventRequestDto, Event>().ReverseMap();
            CreateMap<PagedList<Event>, PagedList<EventResponseDto>>();

            //Mapper Tag
            CreateMap<TagDTO, Tag>().ReverseMap();

         
            
            CreateMap<Event, EventPreview>().ReverseMap();
            CreateMap<PagedList<Event>, PagedList<EventPreview>>().ReverseMap();

			//Participant
			CreateMap<Participant, ParticipantEventModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
				.ReverseMap();

			CreateMap<PagedList<Participant>, PagedList<ParticipantEventModel>>().ReverseMap();

		}
        
    }
}
