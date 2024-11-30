using App.Dto;
using AutoMapper;
using Domain.Entities;

namespace App.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Event <=> EventDto
        CreateMap<Event, EventDto>()
            .ReverseMap();

        // Attendee <=> AttendeeDto
        CreateMap<Attendee, AttendeeDto>()
            .ReverseMap();
    }
}