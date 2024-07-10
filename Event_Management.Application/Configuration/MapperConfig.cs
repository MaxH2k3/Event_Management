using AutoMapper;
using Event_Management.Application;
using Event_Management.Application.Dto;
using Event_Management.Domain;
using Event_Management.Application.Dto.EventDTO.ResponseDTO;
using Event_Management.Application.Dto.UserDTO.Response;
using Event_Management.Domain.Models.Common;
using Event_Management.Application.Dto.PaymentDTO;
using Event_Management.Domain.Models.EventDTO.ResponseDTO;
using Event_Management.Domain.Models.ParticipantDTO;
using Event_Management.Application.Dto.FeedbackDTO;
using Event_Management.Domain.Entity;
using Event_Management.Application.Dto.UserDTO.Request;
using Event_Management.Application.Dto.SponsorLogoDTO;
using Event_Management.Application.Dto.EventDTO.SponsorDTO;

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

            CreateMap<PagedList<User>, PagedList<UserResponseDto>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items)).ReverseMap();


            //Mapper Event
            CreateMap<Event, EventResponseDto>()
                .ReverseMap();
            CreateMap<Event, EventDetailDto>().ReverseMap();
            CreateMap<EventRequestDto, Event>().ReverseMap();
            CreateMap<PagedList<Event>, PagedList<EventResponseDto>>();

            //Mapper Tag
            CreateMap<TagDto, Tag>().ReverseMap();
            CreateMap<EventTag, Tag>().ReverseMap();

            //Mapper SponsorEvent
            CreateMap<SponsorDto, SponsorEvent>().ReverseMap();

            //Mapper Feedback
            CreateMap<FeedbackDto, Feedback>().ReverseMap();

            CreateMap<Event, EventPreview>().ReverseMap();
            CreateMap<PagedList<Event>, PagedList<EventPreview>>().ReverseMap();

            //Mapper Payment

            CreateMap<Payment, PaymentDto>().ReverseMap();
            //CreateMap<PaymentDto, Payment>()
            //.ForMember(dest => dest.ID, opt => opt.MapFrom(src => Guid.NewGuid().ToString())); // �nh x? Id


            //Participant
            CreateMap<Participant, ParticipantEventModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ReverseMap();
            
            CreateMap<Participant, ParticipantInfo>().ReverseMap();
            CreateMap<PagedList<Participant>, PagedList<ParticipantEventModel>>().ReverseMap();

            CreateMap<Participant, ParticipantModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ReverseMap();
            CreateMap<PagedList<Participant>, PagedList<ParticipantModel>>().ReverseMap();

            //Logo
            CreateMap<Logo, SponsorLogoDto>().ReverseMap();

        }
    }
}
