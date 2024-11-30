using App.Commands;
using App.Dto;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Handlers;

public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, EventDto>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public UpdateEventCommandHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<EventDto> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var calendarEvent = await _eventRepository.GetByIdAsync(request.EventId);
        if (calendarEvent == null)
            throw new KeyNotFoundException("Event not found");

        // Concurrency check
        if (!calendarEvent.RowVersion.SequenceEqual(request.EventDto.RowVersion))
            throw new DbUpdateConcurrencyException("Concurrency conflict detected. Please reload and try again.");

        // Map updated fields
        _mapper.Map(request.EventDto, calendarEvent);
        await _eventRepository.UpdateAsync(calendarEvent);

        return _mapper.Map<EventDto>(calendarEvent);
    }
}