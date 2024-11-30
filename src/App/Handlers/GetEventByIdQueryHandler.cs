using App.Dto;
using App.Queries;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace App.Handlers;

public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, EventDto>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public GetEventByIdQueryHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<EventDto> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
    {
        var calendarEvent = await _eventRepository.GetByIdAsync(request.EventId);
        if (calendarEvent == null) 
            throw new KeyNotFoundException("Event not found");

        return _mapper.Map<EventDto>(calendarEvent);
    }
}