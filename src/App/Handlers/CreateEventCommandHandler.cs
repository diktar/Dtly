using App.Commands;
using App.Dto;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace App.Handlers;

public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventDto>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public CreateEventCommandHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<EventDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var calendarEvent = _mapper.Map<Event>(request.EventDto);
        
        await _eventRepository.AddAsync(calendarEvent);

        return _mapper.Map<EventDto>(calendarEvent);
    }
}