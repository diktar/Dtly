using App.Dto;
using App.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace App.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public EventService(
        IEventRepository eventRepository,
        IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<EventDto> CreateEventAsync(EventDto eventDto)
    {
        var calendarEvent = _mapper.Map<Event>(eventDto);
        await _eventRepository.AddAsync(calendarEvent);

        return _mapper.Map<EventDto>(calendarEvent);
    }

    public async Task<EventDto> UpdateEventAsync(Guid id, EventDto eventDto)
    {
        var calendarEvent = await _eventRepository.GetByIdAsync(id);
        if (calendarEvent == null) throw new KeyNotFoundException("Event not found");

        _mapper.Map(eventDto, calendarEvent);
        await _eventRepository.UpdateAsync(calendarEvent);

        return _mapper.Map<EventDto>(calendarEvent);
    }

    public async Task DeleteEventAsync(Guid id)
    {
        await _eventRepository.DeleteAsync(id);
    }

    public async Task<List<EventDto>> GetAllEventsAsync()
    {
        var events = await _eventRepository.GetAllAsync();
        return _mapper.Map<List<EventDto>>(events);
    }

    public async Task<EventDto> GetEventByIdAsync(Guid id)
    {
        var calendarEvent = await _eventRepository.GetByIdAsync(id);
        if (calendarEvent == null) throw new KeyNotFoundException("Event not found");

        return _mapper.Map<EventDto>(calendarEvent);
    }
}